using System.Globalization;
using Kobra.Datos;
using Kobra.Entidades;
using Kobra.Negocio;

namespace Kobra.UI
{
    /// <summary>
    /// Consulta de facturas y saldos: filtros, grilla principal, pagos asociados y balance.
    /// </summary>
    internal sealed class CtlFacturaConsulta : UserControl
    {
        private static CultureInfo Cultura => CultureInfo.CurrentCulture;

        private readonly FacturaNegocio _negocio = new();
        private readonly ClienteNegocio _clienteNegocio = new();
        private readonly System.Windows.Forms.Timer _timer = new() { Interval = 300 };

        private readonly TarjetaPanel _tarjetaFiltros = new("FILTROS DE BÚSQUEDA");
        private readonly TextBox _txtBuscar = new();
        private readonly ComboBox _cboCliente = new();
        private readonly ComboBox _cboEstado = new();
        private readonly CheckBox _chkSoloVencidas = new();
        private readonly BotonTema _btnBuscar = new("&Buscar", EstiloBoton.Primario);

        private readonly TarjetaPanel _tarjetaLista = new("FACTURAS Y SALDOS");
        private readonly DataGridView _dgvFacturas = new();
        private readonly Label _lblResumenCartera = new();

        private readonly TarjetaPanel _tarjetaPagos = new("PAGOS ASOCIADOS");
        private readonly DataGridView _dgvPagos = new();

        private readonly TarjetaPanel _tarjetaBalance = new("BALANCE DE FACTURA");
        private readonly Label _lblBalTotal = new();
        private readonly Label _lblBalPagado = new();
        private readonly Label _lblBalSaldo = new();
        private readonly Label _lblBalEstado = new();

        private List<Factura> _facturas = [];
        private bool _cargando;

        public CtlFacturaConsulta()
        {
            SuspendLayout();
            AutoScaleMode = AutoScaleMode.None;
            Dock = DockStyle.Fill;
            Margin = new Padding(0);

            var raiz = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                BackColor = Color.Transparent,
                Margin = new Padding(0)
            };
            raiz.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            raiz.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            raiz.RowStyles.Add(new RowStyle(SizeType.Percent, 64));
            raiz.RowStyles.Add(new RowStyle(SizeType.Percent, 36));

            raiz.Controls.Add(CrearFiltros(), 0, 0);
            raiz.Controls.Add(CrearListaPrincipal(), 0, 1);
            raiz.Controls.Add(CrearDetalleInferior(), 0, 2);

            Controls.Add(raiz);

            _timer.Tick += (_, _) =>
            {
                _timer.Stop();
                BusquedaSolicitada?.Invoke(false);
            };
            ResumeLayout(true);
        }

        public event Action<bool>? BusquedaSolicitada;
        public event Action<Factura?>? FacturaSeleccionada;
        public event EventHandler? BusquedaEnfocada;
        public event EventHandler? RejillaEnfocada;

        public Button BotonBuscar => _btnBuscar;

        public void EnfocarBusqueda()
        {
            _txtBuscar.Focus();
            _txtBuscar.SelectAll();
        }

        public void CargarClientesFiltro()
        {
            _cargando = true;
            try
            {
                var seleccionado = (_cboCliente.SelectedItem as ItemCliente)?.Id ?? 0;
                var todos = _clienteNegocio.Buscar(null, soloActivos: false);

                var items = new List<ItemCliente> { new(0, "Todos los clientes") };
                items.AddRange(todos.Select(c => new ItemCliente(c.Id, $"{c.Codigo} - {c.Nombre}")));

                _cboCliente.DataSource = items;
                _cboCliente.DisplayMember = nameof(ItemCliente.Texto);
                _cboCliente.ValueMember = nameof(ItemCliente.Id);
                _cboCliente.SelectedValue = seleccionado;
            }
            catch (DatosException)
            {
               
            }
            finally
            {
                _cargando = false;
            }
        }

        public void LimpiarFiltros()
        {
            _cargando = true;
            try
            {
                _txtBuscar.Clear();
                if (_cboCliente.Items.Count > 0)
                    _cboCliente.SelectedIndex = 0;
                _cboEstado.SelectedIndex = 0;
                _chkSoloVencidas.Checked = false;
            }
            finally
            {
                _cargando = false;
            }
        }

        public string? Cargar(int? seleccionarId)
        {
            _timer.Stop();
            string? error = null;

            var clienteId = (_cboCliente.SelectedItem as ItemCliente)?.Id;
            if (clienteId == 0)
                clienteId = null;

            try
            {
                _facturas = _negocio.Buscar(
                    _txtBuscar.Text,
                    clienteId,
                    _cboEstado.SelectedItem?.ToString(),
                    _chkSoloVencidas.Checked);
            }
            catch (DatosException ex)
            {
                _facturas = [];
                error = ex.Message;
            }

            _cargando = true;
            Factura? seleccionada = null;
            try
            {
                _dgvFacturas.DataSource = _facturas;
                _dgvFacturas.ClearSelection();
                _dgvFacturas.CurrentCell = null;

                var fila = seleccionarId is int id ? _facturas.FindIndex(f => f.Id == id) : -1;
                if (fila < 0 && _facturas.Count > 0)
                    fila = 0;

                if (fila >= 0)
                {
                    _dgvFacturas.Rows[fila].Selected = true;
                    _dgvFacturas.CurrentCell = _dgvFacturas.Rows[fila].Cells["colNumero"];
                    seleccionada = _facturas[fila];
                }
            }
            finally
            {
                _cargando = false;
            }

            ActualizarResumenCartera();

            if (error is not null)
                _tarjetaLista.MostrarNota(error, esError: true);
            else
                _tarjetaLista.MostrarNota(_facturas.Count >= FacturaNegocio.MaximoResultados
                    ? $"Primeras {FacturaNegocio.MaximoResultados}: afine la búsqueda"
                    : $"{_facturas.Count} factura(s)");

            MostrarDetalleFactura(seleccionada);
            FacturaSeleccionada?.Invoke(seleccionada);
            return error;
        }

        // construcción

        private Control CrearFiltros()
        {
            _tarjetaFiltros.Margin = new Padding(0, 0, 0, 8);

            var fila = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                ColumnCount = 8,
                RowCount = 1,
                BackColor = Color.Transparent,
                Margin = new Padding(0, 6, 0, 2)
            };
            fila.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            fila.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 32));
            fila.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            fila.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 38));
            fila.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            fila.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));
            fila.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            fila.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            _txtBuscar.MaxLength = 150;
            _txtBuscar.PlaceholderText = "Número o cliente";
            _txtBuscar.BorderStyle = BorderStyle.FixedSingle;
            _txtBuscar.Dock = DockStyle.Fill;
            _txtBuscar.Margin = new Padding(0, 3, 10, 3);
            _txtBuscar.TabIndex = 0;
            _txtBuscar.TextChanged += (_, _) =>
            {
                _timer.Stop();
                _timer.Start();
            };
            _txtBuscar.Enter += (_, _) => BusquedaEnfocada?.Invoke(this, EventArgs.Empty);

            _cboCliente.DropDownStyle = ComboBoxStyle.DropDownList;
            _cboCliente.Dock = DockStyle.Fill;
            _cboCliente.Margin = new Padding(0, 3, 10, 3);
            _cboCliente.TabIndex = 1;
            _cboCliente.SelectedIndexChanged += (_, _) =>
            {
                if (!_cargando)
                    BusquedaSolicitada?.Invoke(false);
            };
            _cboCliente.Enter += (_, _) => BusquedaEnfocada?.Invoke(this, EventArgs.Empty);

            _cboEstado.DropDownStyle = ComboBoxStyle.DropDownList;
            _cboEstado.Dock = DockStyle.Fill;
            _cboEstado.Margin = new Padding(0, 3, 10, 3);
            _cboEstado.TabIndex = 2;
            _cboEstado.Items.AddRange(["Todas", "Pendiente", "Parcial", "Pagada", "Anulada"]);
            _cboEstado.SelectedIndex = 0;
            _cboEstado.SelectedIndexChanged += (_, _) =>
            {
                if (!_cargando)
                    BusquedaSolicitada?.Invoke(false);
            };
            _cboEstado.Enter += (_, _) => BusquedaEnfocada?.Invoke(this, EventArgs.Empty);

            _chkSoloVencidas.Text = "Solo vencidas";
            _chkSoloVencidas.ForeColor = Tema.Secundario;
            _chkSoloVencidas.Font = Tema.Negrita(8.5F);
            _chkSoloVencidas.AutoSize = true;
            _chkSoloVencidas.Anchor = AnchorStyles.Left;
            _chkSoloVencidas.Margin = new Padding(0, 0, 10, 0);
            _chkSoloVencidas.TabIndex = 3;
            _chkSoloVencidas.CheckedChanged += (_, _) =>
            {
                if (!_cargando)
                    BusquedaSolicitada?.Invoke(false);
            };
            _chkSoloVencidas.Enter += (_, _) => BusquedaEnfocada?.Invoke(this, EventArgs.Empty);

            _btnBuscar.TabIndex = 4;
            _btnBuscar.Margin = new Padding(0);
            _btnBuscar.Click += (_, _) =>
            {
                _timer.Stop();
                BusquedaSolicitada?.Invoke(true);
            };

            fila.Controls.Add(CrearEtiqueta("Buscar (F2):"), 0, 0);
            fila.Controls.Add(_txtBuscar, 1, 0);
            fila.Controls.Add(CrearEtiqueta("Cliente:"), 2, 0);
            fila.Controls.Add(_cboCliente, 3, 0);
            fila.Controls.Add(CrearEtiqueta("Estado:"), 4, 0);
            fila.Controls.Add(_cboEstado, 5, 0);
            fila.Controls.Add(_chkSoloVencidas, 6, 0);
            fila.Controls.Add(_btnBuscar, 7, 0);

            _tarjetaFiltros.Cuerpo.Controls.Add(fila, 0, 0);
            return _tarjetaFiltros;
        }

        private Control CrearListaPrincipal()
        {
            _tarjetaLista.AutoSize = false;
            _tarjetaLista.Margin = new Padding(0, 0, 0, 8);
            _tarjetaLista.Cuerpo.AutoSize = false;
            _tarjetaLista.Cuerpo.RowCount = 2;
            _tarjetaLista.Cuerpo.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            _tarjetaLista.Cuerpo.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            EstiloGrid.Aplicar(_dgvFacturas);
            _dgvFacturas.Margin = new Padding(0, 6, 0, 0);
            _dgvFacturas.TabIndex = 5;

            EstiloGrid.AgregarColumna(_dgvFacturas, "colNumero", "Número", 12, negrita: true, propiedad: nameof(Factura.Numero));
            EstiloGrid.AgregarColumna(_dgvFacturas, "colCliente", "Cliente", 24, propiedad: nameof(Factura.ClienteDescripcion));
            EstiloGrid.AgregarColumna(_dgvFacturas, "colEmision", "Emisión", 10, formato: "yyyy-MM-dd", propiedad: nameof(Factura.FechaEmision));
            EstiloGrid.AgregarColumna(_dgvFacturas, "colVenc", "Vencimiento", 10, formato: "yyyy-MM-dd", propiedad: nameof(Factura.FechaVencimiento));
            EstiloGrid.AgregarColumna(_dgvFacturas, "colTotal", "Total", 10, derecha: true, formato: "N2", propiedad: nameof(Factura.Total));
            EstiloGrid.AgregarColumna(_dgvFacturas, "colPagado", "Pagado", 10, derecha: true, formato: "N2", propiedad: nameof(Factura.TotalPagado));
            EstiloGrid.AgregarColumna(_dgvFacturas, "colSaldo", "Saldo", 10, derecha: true, formato: "N2", negrita: true, propiedad: nameof(Factura.Saldo));
            EstiloGrid.AgregarColumna(_dgvFacturas, "colEstado", "Estado", 9, propiedad: nameof(Factura.Estado));
            EstiloGrid.AgregarColumna(_dgvFacturas, "colDias", "Días venc.", 7, derecha: true, propiedad: nameof(Factura.DiasVencidos));
            EstiloGrid.AgregarColumna(_dgvFacturas, "colVencida", "Vencida", 9, propiedad: nameof(Factura.TextoVencida));

            _dgvFacturas.CellFormatting += DgvFacturas_CellFormatting;
            _dgvFacturas.Enter += (_, _) => RejillaEnfocada?.Invoke(this, EventArgs.Empty);
            _dgvFacturas.SelectionChanged += DgvFacturas_CambioSeleccion;
            _dgvFacturas.CurrentCellChanged += DgvFacturas_CambioSeleccion;

            _lblResumenCartera.Dock = DockStyle.Fill;
            _lblResumenCartera.AutoSize = false;
            _lblResumenCartera.Height = LogicalToDeviceUnits(24);
            _lblResumenCartera.TextAlign = ContentAlignment.MiddleRight;
            _lblResumenCartera.Font = Tema.Numeros(8.5F);
            _lblResumenCartera.ForeColor = Tema.Texto;
            _lblResumenCartera.BackColor = Tema.ContenedorBajo;
            _lblResumenCartera.Padding = new Padding(8, 0, 8, 0);
            _lblResumenCartera.Margin = new Padding(0, 4, 0, 0);

            _tarjetaLista.Cuerpo.Controls.Add(_dgvFacturas, 0, 0);
            _tarjetaLista.Cuerpo.Controls.Add(_lblResumenCartera, 0, 1);
            return _tarjetaLista;
        }

        private Control CrearDetalleInferior()
        {
            var panel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                BackColor = Color.Transparent,
                Margin = new Padding(0)
            };
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 66));
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 34));
            panel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            _tarjetaPagos.AutoSize = false;
            _tarjetaPagos.Margin = new Padding(0, 0, 8, 0);
            _tarjetaPagos.Cuerpo.AutoSize = false;
            _tarjetaPagos.Cuerpo.RowCount = 1;
            _tarjetaPagos.Cuerpo.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            EstiloGrid.Aplicar(_dgvPagos);
            _dgvPagos.Margin = new Padding(0, 6, 0, 0);
            _dgvPagos.TabStop = false;
            EstiloGrid.AgregarColumna(_dgvPagos, "colPagoId", "ID Pago", 14, negrita: true, propiedad: nameof(PagoFactura.Id));
            EstiloGrid.AgregarColumna(_dgvPagos, "colPagoFecha", "Fecha pago", 20, formato: "yyyy-MM-dd", propiedad: nameof(PagoFactura.FechaPago));
            EstiloGrid.AgregarColumna(_dgvPagos, "colPagoMonto", "Monto", 22, derecha: true, formato: "N2", negrita: true, propiedad: nameof(PagoFactura.Monto));
            EstiloGrid.AgregarColumna(_dgvPagos, "colPagoEstado", "Estado", 18, propiedad: nameof(PagoFactura.EstadoTexto));
            EstiloGrid.AgregarColumna(_dgvPagos, "colPagoReg", "Registrado", 26, formato: "yyyy-MM-dd HH:mm", propiedad: nameof(PagoFactura.FechaCreacion));
            _tarjetaPagos.Cuerpo.Controls.Add(_dgvPagos, 0, 0);

            _tarjetaBalance.AutoSize = false;
            _tarjetaBalance.Cuerpo.AutoSize = false;
            _tarjetaBalance.Cuerpo.RowCount = 1;
            _tarjetaBalance.Cuerpo.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            var tabla = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 4,
                Margin = new Padding(0, 6, 0, 0)
            };
            tabla.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 52));
            tabla.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 48));
            for (var i = 0; i < 4; i++)
                tabla.RowStyles.Add(new RowStyle(SizeType.Percent, 25));

            ConfigurarValor(_lblBalTotal, Tema.Texto, false);
            ConfigurarValor(_lblBalPagado, Tema.PrimarioContenedor, false);
            ConfigurarValor(_lblBalSaldo, Tema.Secundario, true);
            ConfigurarValor(_lblBalEstado, Tema.Primario, true);

            tabla.Controls.Add(CrearEtiquetaBalance("Total factura:"), 0, 0);
            tabla.Controls.Add(_lblBalTotal, 1, 0);
            tabla.Controls.Add(CrearEtiquetaBalance("Total pagado:"), 0, 1);
            tabla.Controls.Add(_lblBalPagado, 1, 1);
            tabla.Controls.Add(CrearEtiquetaBalance("Saldo pendiente:"), 0, 2);
            tabla.Controls.Add(_lblBalSaldo, 1, 2);
            tabla.Controls.Add(CrearEtiquetaBalance("Condición:"), 0, 3);
            tabla.Controls.Add(_lblBalEstado, 1, 3);

            _tarjetaBalance.Cuerpo.Controls.Add(tabla, 0, 0);

            panel.Controls.Add(_tarjetaPagos, 0, 0);
            panel.Controls.Add(_tarjetaBalance, 1, 0);
            return panel;
        }

        // eventos y helpers

        private void DgvFacturas_CambioSeleccion(object? sender, EventArgs e)
        {
            if (_cargando || _dgvFacturas.CurrentRow?.DataBoundItem is not Factura factura)
                return;

            MostrarDetalleFactura(factura);
            FacturaSeleccionada?.Invoke(factura);
        }

        private void DgvFacturas_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || _dgvFacturas.Rows[e.RowIndex].DataBoundItem is not Factura f)
                return;

            if (f.Anulada)
                e.CellStyle!.ForeColor = Tema.TextoApagado;
            else if (f.EstaVencida)
                e.CellStyle!.ForeColor = Tema.Secundario;
        }

        private void ActualizarResumenCartera()
        {
            var activas = _facturas.Where(f => !f.Anulada).ToList();
            var facturado = activas.Sum(f => f.Total);
            var cobrado = activas.Sum(f => f.TotalPagado);
            var saldo = activas.Sum(f => f.Saldo);

            _lblResumenCartera.Text =
                $"Facturado activo: B/. {facturado.ToString("N2", Cultura)}   ·   " +
                $"Cobrado: B/. {cobrado.ToString("N2", Cultura)}   ·   " +
                $"Saldo pendiente: B/. {saldo.ToString("N2", Cultura)}";
        }

        private void MostrarDetalleFactura(Factura? f)
        {
            if (f is null)
            {
                _dgvPagos.DataSource = null;
                _tarjetaPagos.MostrarNota(string.Empty);
                _tarjetaBalance.MostrarNota(string.Empty);
                _lblBalTotal.Text = "B/. 0.00";
                _lblBalPagado.Text = "B/. 0.00";
                _lblBalSaldo.Text = "B/. 0.00";
                _lblBalEstado.Text = "—";
                return;
            }

            _tarjetaBalance.MostrarNota(f.Numero);
            _lblBalTotal.Text = $"B/. {f.Total.ToString("N2", Cultura)}";
            _lblBalPagado.Text = $"B/. {f.TotalPagado.ToString("N2", Cultura)}";
            _lblBalSaldo.Text = $"B/. {f.Saldo.ToString("N2", Cultura)}";
            _lblBalEstado.ForeColor = f.Anulada || f.EstaVencida ? Tema.Secundario : Tema.Primario;
            _lblBalEstado.Text = f.EstaVencida ? $"{f.Estado} ({f.DiasVencidos} d)" : f.Estado;

            try
            {
                var pagos = _negocio.ListarPagosDeFactura(f.Id);
                _dgvPagos.DataSource = pagos;
                _dgvPagos.ClearSelection();
                _tarjetaPagos.MostrarNota($"{pagos.Count} recibo(s)");
            }
            catch (DatosException ex)
            {
                _tarjetaPagos.MostrarNota(ex.Message, esError: true);
            }
        }

        private static Label CrearEtiqueta(string texto) => new()
        {
            Text = texto,
            ForeColor = Tema.TextoSecundario,
            AutoSize = true,
            Anchor = AnchorStyles.Left,
            Margin = new Padding(0, 0, 6, 0)
        };

        private static Label CrearEtiquetaBalance(string texto) => new()
        {
            Text = texto,
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleLeft,
            ForeColor = Tema.TextoSecundario,
            Font = Tema.Normal(9F)
        };

        private static void ConfigurarValor(Label lbl, Color color, bool negrita)
        {
            lbl.Text = "B/. 0.00";
            lbl.Dock = DockStyle.Fill;
            lbl.TextAlign = ContentAlignment.MiddleRight;
            lbl.ForeColor = color;
            lbl.Font = negrita ? Tema.Negrita(9.5F) : Tema.Numeros(9F);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _timer.Dispose();
            base.Dispose(disposing);
        }

        private sealed record ItemCliente(int Id, string Texto);
    }
}