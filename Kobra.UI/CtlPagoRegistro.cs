using System.Globalization;
using Kobra.Datos;
using Kobra.Entidades;
using Kobra.Negocio;

namespace Kobra.UI
{
    /// <summary>
    /// Registro de pago: elige un cliente, muestra sus facturas con saldo y captura fecha y monto.
    /// El monto se limita al saldo de la factura seleccionada.
    /// </summary>
    internal sealed class CtlPagoRegistro : UserControl
    {
        private static CultureInfo Cultura => CultureInfo.CurrentCulture;
        private static readonly decimal MaximoDecimal = new([276447231, 23027, 0, 131072]);

        private readonly PagoNegocio _negocio = new();
        private readonly ClienteNegocio _clienteNegocio = new();
        private readonly ErrorProvider _errores = new() { BlinkStyle = ErrorBlinkStyle.NeverBlink };

        private readonly TarjetaPanel _tarjetaFacturas = new("FACTURAS CON SALDO DEL CLIENTE");
        private readonly ComboBox _cboCliente = new() { DropDownStyle = ComboBoxStyle.DropDownList };
        private readonly DataGridView _dgvFacturas = new();

        private readonly TarjetaPanel _tarjetaPago = new("DATOS DEL PAGO");
        private readonly TextBox _txtFactura = CrearSoloLectura();
        private readonly Label _lblSaldo = new()
        {
            Text = "B/. 0.00",
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleRight,
            Font = Tema.Numeros(12F, FontStyle.Bold),
            ForeColor = Tema.Secundario
        };
        private readonly DateTimePicker _dtpFecha = new() { Format = DateTimePickerFormat.Short };
        private readonly NumericUpDown _nudMonto = new()
        {
            DecimalPlaces = 2,
            ThousandsSeparator = true,
            TextAlign = HorizontalAlignment.Right,
            Font = Tema.Numeros(9.5F),
            Maximum = MaximoDecimal,
            Minimum = 0
        };

        private List<Factura> _facturas = [];
        private bool _cargando;

        public CtlPagoRegistro()
        {
            SuspendLayout();
            AutoScaleMode = AutoScaleMode.None;
            Dock = DockStyle.Fill;
            Margin = new Padding(0);

            _errores.ContainerControl = this;

            var raiz = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                BackColor = Color.Transparent,
                Margin = new Padding(0)
            };
            raiz.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            raiz.RowStyles.Add(new RowStyle(SizeType.Percent, 62));
            raiz.RowStyles.Add(new RowStyle(SizeType.Percent, 38));

            raiz.Controls.Add(CrearTarjetaFacturas(), 0, 0);
            raiz.Controls.Add(CrearTarjetaPago(), 0, 1);
            Controls.Add(raiz);

            ResumeLayout(true);
        }

        public event EventHandler? FichaEnfocada;

        public Factura? FacturaActual { get; private set; }

        public void EnfocarPrimero() => _cboCliente.Focus();

        public void CargarClientesActivos()
        {
            _cargando = true;
            try
            {
                var actual = (_cboCliente.SelectedItem as ItemCliente)?.Id ?? 0;
                var activos = _clienteNegocio.Buscar(null, soloActivos: true);

                var items = new List<ItemCliente> { new(0, "Seleccione un cliente") };
                items.AddRange(activos.Select(c => new ItemCliente(c.Id, $"{c.Codigo} - {c.Nombre}")));

                _cboCliente.DataSource = items;
                _cboCliente.DisplayMember = nameof(ItemCliente.Texto);
                _cboCliente.ValueMember = nameof(ItemCliente.Id);
                _cboCliente.SelectedValue = actual;
            }
            catch (DatosException ex)
            {
                _tarjetaFacturas.MostrarNota(ex.Message, esError: true);
            }
            finally
            {
                _cargando = false;
            }
        }

        public Pago Leer() => new()
        {
            FacturaId = FacturaActual?.Id ?? 0,
            FechaPago = _dtpFecha.Value.Date,
            Monto = _nudMonto.Value
        };

        public void Limpiar()
        {
            LimpiarErrores();
            _cargando = true;
            try
            {
                if (_cboCliente.Items.Count > 0)
                    _cboCliente.SelectedIndex = 0;
                _dtpFecha.Value = DateTime.Today;
                _nudMonto.Value = 0;
            }
            finally
            {
                _cargando = false;
            }

            CargarFacturas();
            MostrarFactura(null);
        }

        public void LimpiarErrores() => _errores.Clear();

        public void MarcarCampo(string? campo, string mensaje)
        {
            Control? control = campo switch
            {
                nameof(Pago.FacturaId) => _dgvFacturas,
                nameof(Pago.FechaPago) => _dtpFecha,
                nameof(Pago.Monto) => _nudMonto,
                _ => null
            };

            if (control is null)
                return;

            _errores.SetError(control, mensaje);
            control.Focus();
        }

        // construcción

        private Control CrearTarjetaFacturas()
        {
            _tarjetaFacturas.AutoSize = false;
            _tarjetaFacturas.Margin = new Padding(0, 0, 0, 10);
            _tarjetaFacturas.Cuerpo.AutoSize = false;
            _tarjetaFacturas.Cuerpo.RowCount = 2;
            _tarjetaFacturas.Cuerpo.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            _tarjetaFacturas.Cuerpo.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            var barra = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                ColumnCount = 2,
                RowCount = 1,
                Margin = new Padding(0),
                BackColor = Color.Transparent
            };
            barra.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            barra.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            _cboCliente.Dock = DockStyle.Fill;
            _cboCliente.Width = LogicalToDeviceUnits(320);
            _cboCliente.Margin = new Padding(8, 6, 0, 6);
            _cboCliente.TabIndex = 0;
            _cboCliente.SelectedIndexChanged += (_, _) =>
            {
                if (!_cargando)
                {
                    CargarFacturas();
                    MostrarFactura(null);
                }
            };
            _cboCliente.Enter += (_, _) => FichaEnfocada?.Invoke(this, EventArgs.Empty);

            barra.Controls.Add(new Label
            {
                Text = "Cliente:",
                ForeColor = Tema.TextoSecundario,
                AutoSize = true,
                Margin = new Padding(0, 10, 8, 0)
            }, 0, 0);
            barra.Controls.Add(_cboCliente, 1, 0);

            EstiloGrid.Aplicar(_dgvFacturas);
            _dgvFacturas.Margin = new Padding(0, 6, 0, 0);
            _dgvFacturas.TabIndex = 1;
            EstiloGrid.AgregarColumna(_dgvFacturas, "colNumero", "Factura", 15, negrita: true, propiedad: nameof(Factura.Numero));
            EstiloGrid.AgregarColumna(_dgvFacturas, "colEmision", "Emisión", 13, formato: "yyyy-MM-dd", propiedad: nameof(Factura.FechaEmision));
            EstiloGrid.AgregarColumna(_dgvFacturas, "colVenc", "Vencimiento", 13, formato: "yyyy-MM-dd", propiedad: nameof(Factura.FechaVencimiento));
            EstiloGrid.AgregarColumna(_dgvFacturas, "colTotal", "Total", 13, derecha: true, formato: "N2", propiedad: nameof(Factura.Total));
            EstiloGrid.AgregarColumna(_dgvFacturas, "colPagado", "Pagado", 13, derecha: true, formato: "N2", propiedad: nameof(Factura.TotalPagado));
            EstiloGrid.AgregarColumna(_dgvFacturas, "colSaldo", "Saldo", 16, derecha: true, formato: "N2", negrita: true, propiedad: nameof(Factura.Saldo));
            EstiloGrid.AgregarColumna(_dgvFacturas, "colEstado", "Estado", 12, propiedad: nameof(Factura.Estado));
            EstiloGrid.AgregarColumna(_dgvFacturas, "colVencida", "Vencida", 11, propiedad: nameof(Factura.TextoVencida));

            _dgvFacturas.CellFormatting += (_, e) =>
            {
                if (e.RowIndex < 0 || _dgvFacturas.Rows[e.RowIndex].DataBoundItem is not Factura factura)
                    return;
                if (factura.EstaVencida)
                    e.CellStyle!.ForeColor = Tema.Secundario;
            };
            _dgvFacturas.SelectionChanged += DgvFacturas_CambioSeleccion;
            _dgvFacturas.CurrentCellChanged += DgvFacturas_CambioSeleccion;

            _tarjetaFacturas.Cuerpo.Controls.Add(barra, 0, 0);
            _tarjetaFacturas.Cuerpo.Controls.Add(_dgvFacturas, 0, 1);
            return _tarjetaFacturas;
        }

        private Control CrearTarjetaPago()
        {
            _tarjetaPago.AutoSize = false;
            _tarjetaPago.Cuerpo.AutoSize = false;

            var rejilla = CampoFicha.CrearRejilla(2);
            CampoFicha.Agregar(rejilla, "Factura seleccionada", _txtFactura, 0, 0);
            CampoFicha.Agregar(rejilla, "Saldo disponible (B/.)", _lblSaldo, 0, 1);
            CampoFicha.Agregar(rejilla, "Fecha de pago", _dtpFecha, 1, 0, obligatorio: true);
            CampoFicha.Agregar(rejilla, "Monto a abonar (B/.)", _nudMonto, 1, 1, obligatorio: true);

            _dtpFecha.TabIndex = 2;
            _nudMonto.TabIndex = 3;
            _tarjetaPago.MostrarNota("* obligatorio · el monto no puede superar el saldo");
            _tarjetaPago.Cuerpo.Controls.Add(rejilla, 0, 0);
            return _tarjetaPago;
        }

        // eventos y helpers

        private void CargarFacturas()
        {
            var clienteId = (_cboCliente.SelectedItem as ItemCliente)?.Id ?? 0;
            try
            {
                _facturas = _negocio.ObtenerFacturasConSaldo(clienteId);
                _tarjetaFacturas.MostrarNota(clienteId == 0
                    ? "Seleccione un cliente"
                    : $"{_facturas.Count} factura(s) con saldo");
            }
            catch (DatosException ex)
            {
                _facturas = [];
                _tarjetaFacturas.MostrarNota(ex.Message, esError: true);
            }

            _cargando = true;
            try
            {
                _dgvFacturas.DataSource = _facturas;
                _dgvFacturas.ClearSelection();
                _dgvFacturas.CurrentCell = null;
            }
            finally
            {
                _cargando = false;
            }
        }

        private void DgvFacturas_CambioSeleccion(object? sender, EventArgs e)
        {
            if (_cargando || _dgvFacturas.CurrentRow?.DataBoundItem is not Factura factura)
                return;
            MostrarFactura(factura);
        }

        private void MostrarFactura(Factura? factura)
        {
            FacturaActual = factura;
            _errores.Clear();

            if (factura is null)
            {
                _txtFactura.Clear();
                _lblSaldo.Text = "B/. 0.00";
                _nudMonto.Value = 0;
                return;
            }

            _txtFactura.Text = factura.Numero;
            _lblSaldo.Text = $"B/. {factura.Saldo.ToString("N2", Cultura)}";
            _nudMonto.Maximum = Math.Min(MaximoDecimal, Math.Max(0.01m, factura.Saldo));
            _nudMonto.Value = Math.Min(factura.Saldo, _nudMonto.Maximum);
        }

        private static TextBox CrearSoloLectura() => new()
        {
            ReadOnly = true,
            TabStop = false,
            BorderStyle = BorderStyle.FixedSingle,
            BackColor = Tema.Contenedor,
            ForeColor = Tema.TextoSecundario,
            Font = Tema.Normal(9F)
        };

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _errores.Dispose();
            base.Dispose(disposing);
        }

        private sealed record ItemCliente(int Id, string Texto);
    }
}
