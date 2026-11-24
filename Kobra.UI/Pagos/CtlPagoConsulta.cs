using Kobra.Datos;
using Kobra.Entidades;
using Kobra.Negocio;

namespace Kobra.UI
{
    /// <summary>
    /// Consulta de pagos: filtros por texto, cliente y estado, rejilla de solo lectura y resumen de totales.
    /// </summary>
    internal sealed class CtlPagoConsulta : UserControl
    {
        private readonly PagoNegocio _negocio = new();
        private readonly ClienteNegocio _clienteNegocio = new();
        private readonly System.Windows.Forms.Timer _timer = new() { Interval = 300 };

        private readonly TarjetaPanel _tarjetaFiltros = new("FILTROS DE BÚSQUEDA");
        private readonly TextBox _txtBuscar = new();
        private readonly ComboBox _cboCliente = new();
        private readonly ComboBox _cboEstado = new();
        private readonly BotonTema _btnBuscar = new("&Buscar", EstiloBoton.Primario);

        private readonly TarjetaPanel _tarjetaLista = new("PAGOS REGISTRADOS");
        private readonly DataGridView _dgvPagos = new();
        private readonly BarraTotales _lblResumen = new();

        private List<Pago> _pagos = [];
        private bool _cargando;

        public CtlPagoConsulta()
        {
            SuspendLayout();
            AutoScaleMode = AutoScaleMode.None;
            Dock = DockStyle.Fill;
            Margin = new Padding(0);

            var raiz = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                BackColor = Color.Transparent,
                Margin = new Padding(0)
            };
            raiz.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            raiz.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            raiz.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            raiz.Controls.Add(CrearFiltros(), 0, 0);
            raiz.Controls.Add(CrearLista(), 0, 1);
            Controls.Add(raiz);

            _timer.Tick += (_, _) =>
            {
                _timer.Stop();
                BusquedaSolicitada?.Invoke(false);
            };
            ResumeLayout(true);
        }

        public event Action<bool>? BusquedaSolicitada;
        public event Action<Pago?>? PagoSeleccionado;
        public event EventHandler? BusquedaEnfocada;
        public event EventHandler? RejillaEnfocada;

        public Button BotonBuscar => _btnBuscar;

        /// <summary>Número de factura del pago indicado, ya cargado en la rejilla.</summary>
        public string ObtenerNumeroFactura(int pagoId) =>
            _pagos.FirstOrDefault(p => p.Id == pagoId)?.NumeroFactura ?? string.Empty;

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
                ItemCliente.Llenar(_cboCliente, _clienteNegocio.Buscar(null, soloActivos: false), "Todos los clientes");
            }
            catch (DatosException ex)
            {
                _tarjetaFiltros.MostrarNota(ex.Message, esError: true);
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
            }
            finally
            {
                _cargando = false;
            }
        }

        /// <summary>Recarga la rejilla. Devuelve el mensaje de error, o null si todo salió bien.</summary>
        public string? Cargar(int? seleccionarId)
        {
            _timer.Stop();
            string? error = null;

            var clienteId = (_cboCliente.SelectedItem as ItemCliente)?.Id;
            if (clienteId == 0)
                clienteId = null;

            bool? anulado = _cboEstado.SelectedItem?.ToString() switch
            {
                "Activos" => false,
                "Anulados" => true,
                _ => null
            };

            try
            {
                _pagos = _negocio.Buscar(_txtBuscar.Text, clienteId, anulado);
            }
            catch (DatosException ex)
            {
                _pagos = [];
                error = ex.Message;
            }

            _cargando = true;
            Pago? seleccionado = null;
            try
            {
                _dgvPagos.DataSource = _pagos;
                _dgvPagos.ClearSelection();
                _dgvPagos.CurrentCell = null;

                var fila = seleccionarId is int id ? _pagos.FindIndex(p => p.Id == id) : -1;
                if (fila < 0 && _pagos.Count > 0)
                    fila = 0;

                if (fila >= 0)
                {
                    _dgvPagos.Rows[fila].Selected = true;
                    _dgvPagos.CurrentCell = _dgvPagos.Rows[fila].Cells["colNumero"];
                    seleccionado = _pagos[fila];
                }
            }
            finally
            {
                _cargando = false;
            }

            ActualizarResumen();

            if (error is not null)
                _tarjetaLista.MostrarNota(error, esError: true);
            else
                _tarjetaLista.MostrarNota(_pagos.Count >= PagoNegocio.MaximoResultados
                    ? $"Primeros {PagoNegocio.MaximoResultados}: afine la búsqueda"
                    : $"{_pagos.Count} pago(s)");

            PagoSeleccionado?.Invoke(seleccionado);
            return error;
        }

        // construcción

        private Control CrearFiltros()
        {
            _tarjetaFiltros.Margin = new Padding(0, 0, 0, 10);

            var fila = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                ColumnCount = 7,
                RowCount = 1,
                BackColor = Color.Transparent,
                Margin = new Padding(0, 6, 0, 2)
            };
            fila.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            fila.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));
            fila.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            fila.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 36));
            fila.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            fila.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 24));
            fila.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            _txtBuscar.MaxLength = 150;
            _txtBuscar.PlaceholderText = "N.º pago, factura o cliente";
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
            _cboEstado.Items.AddRange(["Todos", "Activos", "Anulados"]);
            _cboEstado.SelectedIndex = 0;
            _cboEstado.SelectedIndexChanged += (_, _) =>
            {
                if (!_cargando)
                    BusquedaSolicitada?.Invoke(false);
            };
            _cboEstado.Enter += (_, _) => BusquedaEnfocada?.Invoke(this, EventArgs.Empty);

            _btnBuscar.TabIndex = 3;
            _btnBuscar.Margin = new Padding(0);
            _btnBuscar.Click += (_, _) =>
            {
                _timer.Stop();
                BusquedaSolicitada?.Invoke(true);
            };

            fila.Controls.Add(CampoFicha.CrearEtiquetaFiltro("Buscar (F2):"), 0, 0);
            fila.Controls.Add(_txtBuscar, 1, 0);
            fila.Controls.Add(CampoFicha.CrearEtiquetaFiltro("Cliente:"), 2, 0);
            fila.Controls.Add(_cboCliente, 3, 0);
            fila.Controls.Add(CampoFicha.CrearEtiquetaFiltro("Estado:"), 4, 0);
            fila.Controls.Add(_cboEstado, 5, 0);
            fila.Controls.Add(_btnBuscar, 6, 0);

            _tarjetaFiltros.Cuerpo.Controls.Add(fila, 0, 0);
            return _tarjetaFiltros;
        }

        private Control CrearLista()
        {
            _tarjetaLista.AutoSize = false;
            _tarjetaLista.Cuerpo.AutoSize = false;
            _tarjetaLista.Cuerpo.RowCount = 2;
            _tarjetaLista.Cuerpo.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            _tarjetaLista.Cuerpo.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            EstiloGrid.Aplicar(_dgvPagos);
            _dgvPagos.Margin = new Padding(0, 6, 0, 0);
            _dgvPagos.TabIndex = 4;

            EstiloGrid.AgregarColumna(_dgvPagos, "colNumero", "N.º pago", 10, negrita: true, propiedad: nameof(Pago.Id));
            EstiloGrid.AgregarColumna(_dgvPagos, "colFactura", "Factura", 14, propiedad: nameof(Pago.NumeroFactura));
            EstiloGrid.AgregarColumna(_dgvPagos, "colCliente", "Cliente", 26, propiedad: nameof(Pago.ClienteDescripcion));
            EstiloGrid.AgregarColumna(_dgvPagos, "colFecha", "Fecha pago", 13, formato: Formato.Fecha, propiedad: nameof(Pago.FechaPago));
            EstiloGrid.AgregarColumna(_dgvPagos, "colMonto", "Monto (B/.)", 13, derecha: true, formato: Formato.Monto, negrita: true, propiedad: nameof(Pago.Monto));
            EstiloGrid.AgregarColumna(_dgvPagos, "colEstado", "Estado", 11, propiedad: nameof(Pago.EstadoTexto));
            EstiloGrid.AgregarColumna(_dgvPagos, "colSaldo", "Saldo factura (B/.)", 13, derecha: true, formato: Formato.Monto, propiedad: nameof(Pago.SaldoFactura));
            EstiloGrid.AgregarColumna(_dgvPagos, "colMotivo", "Motivo anulación", 20, propiedad: nameof(Pago.MotivoAnulacion));

            _dgvPagos.CellFormatting += (_, e) =>
            {
                if (e.RowIndex < 0 || _dgvPagos.Rows[e.RowIndex].DataBoundItem is not Pago pago)
                    return;
                if (pago.Anulado || pago.FacturaAnulada)
                    e.CellStyle!.ForeColor = Tema.TextoApagado;
            };
            _dgvPagos.Enter += (_, _) => RejillaEnfocada?.Invoke(this, EventArgs.Empty);
            _dgvPagos.SelectionChanged += DgvPagos_CambioSeleccion;
            _dgvPagos.CurrentCellChanged += DgvPagos_CambioSeleccion;


            _tarjetaLista.Cuerpo.Controls.Add(_dgvPagos, 0, 0);
            _tarjetaLista.Cuerpo.Controls.Add(_lblResumen, 0, 1);
            return _tarjetaLista;
        }

        // eventos y helpers

        private void DgvPagos_CambioSeleccion(object? sender, EventArgs e)
        {
            if (_cargando || _dgvPagos.CurrentRow?.DataBoundItem is not Pago pago)
                return;
            PagoSeleccionado?.Invoke(pago);
        }

        private void ActualizarResumen()
        {
            var activos = _pagos.Where(p => !p.Anulado).ToList();
            var anulados = _pagos.Where(p => p.Anulado).ToList();

            _lblResumen.Text =
                $"Cobrado activo: {activos.Sum(p => p.Monto).Dinero()}   ·   " +
                $"Anulado: {anulados.Sum(p => p.Monto).Dinero()}   ·   " +
                $"{activos.Count} activo(s) / {anulados.Count} anulado(s)";
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _timer.Dispose();
            base.Dispose(disposing);
        }

    }
}
