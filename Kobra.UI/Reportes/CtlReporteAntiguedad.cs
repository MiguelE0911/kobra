using System.Globalization;
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
        private static CultureInfo Cultura => CultureInfo.CurrentCulture;

        private readonly ReporteNegocio _negocio = new();
        private readonly ClienteNegocio _clienteNegocio = new();

        private readonly TarjetaPanel _tarjetaFiltros = new("ANTIGÜEDAD DE SALDOS");
        private readonly ComboBox _cboCliente = new() { DropDownStyle = ComboBoxStyle.DropDownList };
        private readonly BotonTema _btnGenerar = new("&Generar", EstiloBoton.Primario);
        private readonly Label _lblResumen = new()
        {
            AutoSize = true,
            Font = Tema.Numeros(8.5F),
            ForeColor = Tema.TextoSecundario,
            Margin = new Padding(12, 8, 0, 0)
        };

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
                var seleccionado = (_cboCliente.SelectedItem as ItemCliente)?.Id ?? 0;
                var clientes = _clienteNegocio.Buscar(null, soloActivos: false);
                var items = new List<ItemCliente> { new(0, "Todos los clientes") };
                items.AddRange(clientes.Select(c => new ItemCliente(c.Id, $"{c.Codigo} - {c.Nombre}")));

                _cboCliente.DataSource = items;
                _cboCliente.DisplayMember = nameof(ItemCliente.Texto);
                _cboCliente.ValueMember = nameof(ItemCliente.Id);
                _cboCliente.SelectedValue = seleccionado;
            }
            catch (DatosException ex)
            {
                _tarjetaFiltros.MostrarNota(ex.Message, esError: true);
            }
        }

        public void Generar()
        {
            var clienteId = (_cboCliente.SelectedItem as ItemCliente)?.Id;
            if (clienteId == 0)
                clienteId = null;

            try
            {
                var reporte = _negocio.ObtenerAntiguedad(clienteId);
                _dgvTramos.DataSource = reporte.Tramos;
                _dgvTramos.ClearSelection();
                _dgvTramos.CurrentCell = null;
                _dgvDetalle.DataSource = null;

                _lblResumen.Text =
                    $"Saldo total: B/. {reporte.SaldoTotal.ToString("N2", Cultura)}   ·   " +
                    $"{reporte.Facturas} factura(s) con saldo";
                _tarjetaTramos.MostrarNota("Seleccione un tramo para ver el detalle");
                _tarjetaFiltros.MostrarNota(string.Empty);
            }
            catch (DatosException ex)
            {
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
            EstiloGrid.AgregarColumna(_dgvTramos, "colSaldo", "Saldo (B/.)", 30, derecha: true, formato: "N2", negrita: true, propiedad: nameof(TramoAntiguedad.Saldo));
            EstiloGrid.AgregarColumna(_dgvTramos, "colPorcentaje", "% del saldo", 30, derecha: true, formato: "N2", propiedad: nameof(TramoAntiguedad.Porcentaje));

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
            EstiloGrid.AgregarColumna(_dgvDetalle, "colVenc", "Vencimiento", 18, formato: "yyyy-MM-dd", propiedad: nameof(LineaAntiguedad.FechaVencimiento));
            EstiloGrid.AgregarColumna(_dgvDetalle, "colSaldo", "Saldo (B/.)", 18, derecha: true, formato: "N2", propiedad: nameof(LineaAntiguedad.Saldo));
            EstiloGrid.AgregarColumna(_dgvDetalle, "colDias", "Días venc.", 12, derecha: true, propiedad: nameof(LineaAntiguedad.DiasVencidos));

            _tarjetaDetalle.Cuerpo.Controls.Add(_dgvDetalle, 0, 0);
            return _tarjetaDetalle;
        }

        private void DgvTramos_CambioSeleccion(object? sender, EventArgs e)
        {
            if (_dgvTramos.CurrentRow?.DataBoundItem is not TramoAntiguedad tramo)
                return;

            _dgvDetalle.DataSource = tramo.Detalle;
            _dgvDetalle.ClearSelection();
            _tarjetaDetalle.MostrarNota($"{tramo.Nombre} · {tramo.Facturas} factura(s)");
        }

        private sealed record ItemCliente(int Id, string Texto);
    }
}
