using Kobra.Datos;
using Kobra.Entidades;
using Kobra.Negocio;

namespace Kobra.UI
{
    /// <summary>
    /// Reporte de antigüedad de saldos: cinco tramos con su detalle de facturas, con filtro opcional por cliente.
    /// </summary>
    internal sealed class CtlReporteAntiguedad : UserControl
    {
        private readonly ReporteNegocio _negocio = new();

        // Tramo cuyo detalle se ve abajo; evita reenlazar cuando llegan los dos eventos de selección.
        private TramoAntiguedad? _tramoMostrado;

        private readonly TarjetaPanel _tarjetaFiltros = new("ANTIGÜEDAD DE SALDOS");
        private readonly ComboBox _cboCliente = new()
        {
            DropDownStyle = ComboBoxStyle.DropDown,
            AutoCompleteMode = AutoCompleteMode.SuggestAppend,
            AutoCompleteSource = AutoCompleteSource.ListItems
        };
        private readonly BotonTema _btnGenerar = new("&Generar", EstiloBoton.Primario);
        private readonly BarraTotales _lblResumen = new();

        private readonly TarjetaPanel _tarjetaTramos = new("TRAMOS DE ANTIGÜEDAD");
        private readonly DataGridView _dgvTramos = new();

        private readonly TarjetaPanel _tarjetaDetalle = new("DETALLE DEL TRAMO");
        private readonly DataGridView _dgvDetalle = new();

        public CtlReporteAntiguedad()
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
            raiz.RowStyles.Add(new RowStyle(SizeType.Percent, 38));
            raiz.RowStyles.Add(new RowStyle(SizeType.Percent, 62));

            raiz.Controls.Add(CrearFiltros(), 0, 0);
            raiz.Controls.Add(CrearTramos(), 0, 1);
            raiz.Controls.Add(CrearDetalle(), 0, 2);
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
            _tramoMostrado = null;
            _dgvTramos.DataSource = null;
            _dgvDetalle.DataSource = null;
            _lblResumen.Text = string.Empty;
            _tarjetaFiltros.MostrarNota(string.Empty);
            _tarjetaTramos.MostrarNota(string.Empty);
            _tarjetaDetalle.MostrarNota(string.Empty);
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
                ItemCliente.Llenar(_cboCliente, _negocio.ListarClientesConSaldo(), "Todos los clientes");
            }
            catch (DatosException ex)
            {
                _tarjetaFiltros.MostrarNota(ex.Message, esError: true);
            }
        }

        public void Generar()
        {
            // Con el combo editable, SelectedItem puede quedar con el cliente anterior si se escribió otro texto.
            var elegido = _cboCliente.SelectedItem as ItemCliente;
            if (_cboCliente.Items.Count > 0 && (elegido is null || elegido.Texto != _cboCliente.Text))
            {
                _tarjetaFiltros.MostrarNota("Elija un cliente de la lista.", esError: true);
                return;
            }

            int? clienteId = elegido is null || elegido.Id == 0 ? null : elegido.Id;

            try
            {
                var reporte = _negocio.ObtenerAntiguedad(clienteId);

                // El primer tramo queda seleccionado al enlazar y su detalle se carga en el evento de selección.
                _tramoMostrado = null;
                _dgvDetalle.DataSource = null;
                _dgvTramos.DataSource = reporte.Tramos;
                MostrarDetalleSeleccionado();

                _lblResumen.Text =
                    $"Saldo total: {reporte.SaldoTotal.Dinero()}   ·   " +
                    $"{reporte.Facturas} factura(s) con saldo";
                _tarjetaTramos.MostrarNota(string.Empty);
                _tarjetaFiltros.MostrarNota(string.Empty);
            }
            catch (DatosException ex)
            {
                _tramoMostrado = null;
                _dgvTramos.DataSource = null;
                _dgvDetalle.DataSource = null;
                _lblResumen.Text = string.Empty;
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

            _tarjetaFiltros.Cuerpo.Controls.Add(fila, 0, 0);
            _tarjetaFiltros.Cuerpo.Controls.Add(_lblResumen, 0, 1);
            return _tarjetaFiltros;
        }

        private Control CrearTramos()
        {
            _tarjetaTramos.AutoSize = false;
            _tarjetaTramos.Margin = new Padding(0, 0, 10, 0);
            _tarjetaTramos.Cuerpo.AutoSize = false;
            _tarjetaTramos.Cuerpo.RowCount = 1;
            _tarjetaTramos.Cuerpo.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            EstiloGrid.Aplicar(_dgvTramos);
            _dgvTramos.Margin = new Padding(0, 6, 0, 0);
            EstiloGrid.AgregarColumna(_dgvTramos, "colTramo", "Tramo", 24, negrita: true, propiedad: nameof(TramoAntiguedad.Nombre));
            EstiloGrid.AgregarColumna(_dgvTramos, "colFacturas", "Facturas", 16, derecha: true, propiedad: nameof(TramoAntiguedad.Facturas));
            EstiloGrid.AgregarColumna(_dgvTramos, "colSaldo", "Saldo (B/.)", 30, derecha: true, formato: Formato.Monto, negrita: true, propiedad: nameof(TramoAntiguedad.Saldo));
            EstiloGrid.AgregarColumna(_dgvTramos, "colPorcentaje", "% del saldo", 30, derecha: true, formato: Formato.Porcentaje, propiedad: nameof(TramoAntiguedad.Porcentaje));

            _dgvTramos.SelectionChanged += DgvTramos_CambioSeleccion;
            _dgvTramos.CurrentCellChanged += DgvTramos_CambioSeleccion;

            _tarjetaTramos.Cuerpo.Controls.Add(_dgvTramos, 0, 0);
            return _tarjetaTramos;
        }

        private Control CrearDetalle()
        {
            _tarjetaDetalle.AutoSize = false;
            _tarjetaDetalle.Cuerpo.AutoSize = false;
            _tarjetaDetalle.Cuerpo.RowCount = 1;
            _tarjetaDetalle.Cuerpo.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            EstiloGrid.Aplicar(_dgvDetalle);
            _dgvDetalle.Margin = new Padding(0, 6, 0, 0);
            EstiloGrid.AgregarColumna(_dgvDetalle, "colFactura", "Factura", 18, negrita: true, propiedad: nameof(LineaAntiguedad.Numero));
            EstiloGrid.AgregarColumna(_dgvDetalle, "colCliente", "Cliente", 34, propiedad: nameof(LineaAntiguedad.ClienteDescripcion));
            EstiloGrid.AgregarColumna(_dgvDetalle, "colVenc", "Vencimiento", 18, formato: Formato.Fecha, propiedad: nameof(LineaAntiguedad.FechaVencimiento));
            EstiloGrid.AgregarColumna(_dgvDetalle, "colSaldo", "Saldo (B/.)", 18, derecha: true, formato: Formato.Monto, propiedad: nameof(LineaAntiguedad.Saldo));
            EstiloGrid.AgregarColumna(_dgvDetalle, "colDias", "Días venc.", 12, derecha: true, propiedad: nameof(LineaAntiguedad.DiasVencidos));

            _tarjetaDetalle.Cuerpo.Controls.Add(_dgvDetalle, 0, 0);
            return _tarjetaDetalle;
        }

        private void DgvTramos_CambioSeleccion(object? sender, EventArgs e) => MostrarDetalleSeleccionado();

        private void MostrarDetalleSeleccionado()
        {
            var tramo = EstiloGrid.Seleccionado<TramoAntiguedad>(_dgvTramos);
            if (tramo is null || ReferenceEquals(tramo, _tramoMostrado))
                return;

            _tramoMostrado = tramo;
            _dgvDetalle.DataSource = tramo.Detalle;
            _dgvDetalle.ClearSelection();
            _tarjetaDetalle.MostrarNota($"{tramo.Nombre} · {tramo.Facturas} factura(s)");
        }

    }
}
