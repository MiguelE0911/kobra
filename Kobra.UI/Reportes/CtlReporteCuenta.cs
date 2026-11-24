using Kobra.Datos;
using Kobra.Entidades;
using Kobra.Negocio;

namespace Kobra.UI
{
    /// <summary>
    /// Estado de cuenta del cliente: facturas con saldo, pagos de la factura seleccionada y totales.
    /// </summary>
    internal sealed class CtlReporteCuenta : UserControl
    {
        private readonly ReporteNegocio _negocio = new();
        private readonly ClienteNegocio _clienteNegocio = new();

        private readonly TarjetaPanel _tarjetaFiltros = new("ESTADO DE CUENTA DEL CLIENTE");
        private readonly ComboBox _cboCliente = new() { DropDownStyle = ComboBoxStyle.DropDownList };
        private readonly BotonTema _btnGenerar = new("&Generar", EstiloBoton.Primario);
        private readonly Label _lblCliente = new() { AutoSize = true, Font = Tema.Negrita(9.5F), ForeColor = Tema.Primario };
        private readonly Label _lblLimite = new() { AutoSize = true, Font = Tema.Normal(8.5F), ForeColor = Tema.TextoSecundario };

        private readonly TarjetaPanel _tarjetaFacturas = new("FACTURAS DEL CLIENTE");
        private readonly DataGridView _dgvFacturas = new();
        private readonly BarraTotales _lblTotales = new();

        private readonly TarjetaPanel _tarjetaPagos = new("PAGOS DE LA FACTURA SELECCIONADA");
        private readonly DataGridView _dgvPagos = new();

        public CtlReporteCuenta()
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
            raiz.RowStyles.Add(new RowStyle(SizeType.Percent, 62));
            raiz.RowStyles.Add(new RowStyle(SizeType.Percent, 38));

            raiz.Controls.Add(CrearFiltros(), 0, 0);
            raiz.Controls.Add(CrearFacturas(), 0, 1);
            raiz.Controls.Add(CrearPagos(), 0, 2);
            Controls.Add(raiz);

            ResumeLayout(true);
        }

        public event EventHandler? GeneracionEnfocada;

        public Button BotonGenerar => _btnGenerar;

        /// <summary>Deja los filtros y las rejillas sin datos, sin consultar la base.</summary>
        public void Limpiar()
        {
            if (_cboCliente.Items.Count > 0)
                _cboCliente.SelectedIndex = 0;
            LimpiarDetalle();
            _tarjetaFiltros.MostrarNota(string.Empty);
        }

        public void EnfocarGenerar()
        {
            _cboCliente.Focus();
            GeneracionEnfocada?.Invoke(this, EventArgs.Empty);
        }

        public void CargarClientes()
        {
            try
            {
                ItemCliente.Llenar(_cboCliente, _clienteNegocio.Buscar(null, soloActivos: false), "Seleccione un cliente");
            }
            catch (DatosException ex)
            {
                _tarjetaFiltros.MostrarNota(ex.Message, esError: true);
            }
        }

        public void Generar()
        {
            var clienteId = (_cboCliente.SelectedItem as ItemCliente)?.Id ?? 0;

            try
            {
                var cuenta = _negocio.ObtenerEstadoCuenta(clienteId);
                if (cuenta is null)
                {
                    LimpiarDetalle();
                    _tarjetaFiltros.MostrarNota("El cliente ya no existe.", esError: true);
                    return;
                }

                _lblCliente.Text = cuenta.ClienteDescripcion +
                    (string.IsNullOrWhiteSpace(cuenta.ClienteRucCedula) ? string.Empty : $"  ·  RUC: {cuenta.ClienteRucCedula}");
                _lblLimite.Text = $"Límite de crédito: {cuenta.LimiteCredito.Dinero()}" +
                    (cuenta.Activo ? string.Empty : "  ·  Cliente inactivo");

                _dgvFacturas.DataSource = cuenta.Facturas;
                _lblTotales.Text =
                    $"Facturado: {cuenta.TotalFacturado.Dinero()}   ·   " +
                    $"Cobrado: {cuenta.TotalCobrado.Dinero()}   ·   " +
                    $"Saldo: {cuenta.Saldo.Dinero()}";

                _dgvFacturas.ClearSelection();
                _dgvFacturas.CurrentCell = null;
                _dgvPagos.DataSource = null;
                _tarjetaPagos.MostrarNota("Seleccione una factura");
                _tarjetaFiltros.MostrarNota($"{cuenta.Facturas.Count} factura(s)");
            }
            catch (ReglaNegocioException ex)
            {
                LimpiarDetalle();
                _tarjetaFiltros.MostrarNota(ex.Message, esError: true);
            }
            catch (DatosException ex)
            {
                LimpiarDetalle();
                _tarjetaFiltros.MostrarNota(ex.Message, esError: true);
            }
        }

        // construcción

        private Control CrearFiltros()
        {
            _tarjetaFiltros.Margin = new Padding(0, 0, 0, 10);
            _tarjetaFiltros.Cuerpo.RowCount = 2;
            _tarjetaFiltros.Cuerpo.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            _tarjetaFiltros.Cuerpo.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            var fila = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                ColumnCount = 3,
                RowCount = 1,
                BackColor = Color.Transparent,
                Margin = new Padding(0, 6, 0, 0)
            };
            fila.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            fila.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            fila.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            _cboCliente.Dock = DockStyle.Fill;
            _cboCliente.Margin = new Padding(0, 3, 10, 3);
            _cboCliente.TabIndex = 0;
            _cboCliente.Enter += (_, _) => GeneracionEnfocada?.Invoke(this, EventArgs.Empty);

            _btnGenerar.TabIndex = 1;
            _btnGenerar.Click += (_, _) => Generar();

            fila.Controls.Add(new Label
            {
                Text = "Cliente:",
                ForeColor = Tema.TextoSecundario,
                AutoSize = true,
                Margin = new Padding(0, 8, 8, 0)
            }, 0, 0);
            fila.Controls.Add(_cboCliente, 1, 0);
            fila.Controls.Add(_btnGenerar, 2, 0);

            var cabecera = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                ColumnCount = 2,
                RowCount = 1,
                BackColor = Color.Transparent,
                Margin = new Padding(0, 6, 0, 0)
            };
            cabecera.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60));
            cabecera.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));
            _lblCliente.Margin = new Padding(0, 6, 0, 0);
            _lblLimite.Margin = new Padding(0, 6, 0, 0);
            _lblLimite.TextAlign = ContentAlignment.MiddleRight;
            cabecera.Controls.Add(_lblCliente, 0, 0);
            cabecera.Controls.Add(_lblLimite, 1, 0);

            _tarjetaFiltros.Cuerpo.Controls.Add(fila, 0, 0);
            _tarjetaFiltros.Cuerpo.Controls.Add(cabecera, 0, 1);
            return _tarjetaFiltros;
        }

        private Control CrearFacturas()
        {
            _tarjetaFacturas.AutoSize = false;
            _tarjetaFacturas.Margin = new Padding(0, 0, 0, 10);
            _tarjetaFacturas.Cuerpo.AutoSize = false;
            _tarjetaFacturas.Cuerpo.RowCount = 2;
            _tarjetaFacturas.Cuerpo.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            _tarjetaFacturas.Cuerpo.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            EstiloGrid.Aplicar(_dgvFacturas);
            _dgvFacturas.Margin = new Padding(0, 6, 0, 0);
            EstiloGrid.AgregarColumna(_dgvFacturas, "colNumero", "Factura", 14, negrita: true, propiedad: nameof(LineaEstadoCuenta.Numero));
            EstiloGrid.AgregarColumna(_dgvFacturas, "colEmision", "Emisión", 12, formato: Formato.Fecha, propiedad: nameof(LineaEstadoCuenta.FechaEmision));
            EstiloGrid.AgregarColumna(_dgvFacturas, "colVenc", "Vencimiento", 12, formato: Formato.Fecha, propiedad: nameof(LineaEstadoCuenta.FechaVencimiento));
            EstiloGrid.AgregarColumna(_dgvFacturas, "colTotal", "Total (B/.)", 13, derecha: true, formato: Formato.Monto, propiedad: nameof(LineaEstadoCuenta.Total));
            EstiloGrid.AgregarColumna(_dgvFacturas, "colPagado", "Pagado (B/.)", 13, derecha: true, formato: Formato.Monto, propiedad: nameof(LineaEstadoCuenta.TotalPagado));
            EstiloGrid.AgregarColumna(_dgvFacturas, "colSaldo", "Saldo (B/.)", 14, derecha: true, formato: Formato.Monto, negrita: true, propiedad: nameof(LineaEstadoCuenta.Saldo));
            EstiloGrid.AgregarColumna(_dgvFacturas, "colEstado", "Estado", 11, propiedad: nameof(LineaEstadoCuenta.Estado));
            EstiloGrid.AgregarColumna(_dgvFacturas, "colVencida", "Vencida", 11, propiedad: nameof(LineaEstadoCuenta.TextoVencida));

            _dgvFacturas.SelectionChanged += DgvFacturas_CambioSeleccion;
            _dgvFacturas.CurrentCellChanged += DgvFacturas_CambioSeleccion;
            _dgvFacturas.CellFormatting += (_, e) =>
            {
                if (e.RowIndex < 0 || _dgvFacturas.Rows[e.RowIndex].DataBoundItem is not LineaEstadoCuenta linea)
                    return;
                if (linea.EstaVencida)
                    e.CellStyle!.ForeColor = Tema.Secundario;
            };


            _tarjetaFacturas.Cuerpo.Controls.Add(_dgvFacturas, 0, 0);
            _tarjetaFacturas.Cuerpo.Controls.Add(_lblTotales, 0, 1);
            return _tarjetaFacturas;
        }

        private Control CrearPagos()
        {
            _tarjetaPagos.AutoSize = false;
            _tarjetaPagos.Cuerpo.AutoSize = false;
            _tarjetaPagos.Cuerpo.RowCount = 1;
            _tarjetaPagos.Cuerpo.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            EstiloGrid.Aplicar(_dgvPagos);
            _dgvPagos.Margin = new Padding(0, 6, 0, 0);
            EstiloGrid.AgregarColumna(_dgvPagos, "colNumero", "N.º pago", 12, negrita: true, propiedad: nameof(Pago.Id));
            EstiloGrid.AgregarColumna(_dgvPagos, "colFecha", "Fecha pago", 18, formato: Formato.Fecha, propiedad: nameof(Pago.FechaPago));
            EstiloGrid.AgregarColumna(_dgvPagos, "colMonto", "Monto (B/.)", 16, derecha: true, formato: Formato.Monto, propiedad: nameof(Pago.Monto));
            EstiloGrid.AgregarColumna(_dgvPagos, "colEstado", "Estado", 14, propiedad: nameof(Pago.EstadoTexto));
            EstiloGrid.AgregarColumna(_dgvPagos, "colMotivo", "Motivo anulación", 40, propiedad: nameof(Pago.MotivoAnulacion));

            _tarjetaPagos.Cuerpo.Controls.Add(_dgvPagos, 0, 0);
            return _tarjetaPagos;
        }

        // eventos y helpers

        private void DgvFacturas_CambioSeleccion(object? sender, EventArgs e)
        {
            if (_dgvFacturas.CurrentRow?.DataBoundItem is not LineaEstadoCuenta linea)
                return;

            try
            {
                var pagos = _negocio.ListarPagosDeFactura(linea.FacturaId);
                _dgvPagos.DataSource = pagos;
                _dgvPagos.ClearSelection();
                _tarjetaPagos.MostrarNota($"Factura {linea.Numero} · {pagos.Count} pago(s)");
            }
            catch (DatosException ex)
            {
                _tarjetaPagos.MostrarNota(ex.Message, esError: true);
            }
        }

        private void LimpiarDetalle()
        {
            _lblCliente.Text = "—";
            _lblLimite.Text = string.Empty;
            _lblTotales.Text = string.Empty;
            _dgvFacturas.DataSource = null;
            _dgvPagos.DataSource = null;
            _tarjetaPagos.MostrarNota(string.Empty);
        }

    }
}
