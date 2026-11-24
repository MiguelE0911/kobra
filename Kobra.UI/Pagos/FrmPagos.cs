using Kobra.Datos;
using Kobra.Entidades;
using Kobra.Negocio;

namespace Kobra.UI
{
    /// <summary>
    /// Módulo de Pagos: orquesta consulta, registro, anulación y reportes. Las reglas viven en
    /// PagoNegocio y ReporteNegocio; cada vista es un UserControl que se alterna por visibilidad.
    /// </summary>
    public partial class FrmPagos : FormularioModulo
    {
        private enum Vista { Consulta, Registro, Anulacion, Reportes }

        private readonly PagoNegocio _negocio = new();

        private readonly BarraTituloModulo _barraTitulo = new("PAGOS");
        private readonly BarraAcciones _barraAcciones =
            new("F2 buscar · F5 actualizar · Ctrl+N nuevo · Ctrl+S guardar · Esc cerrar");

        private readonly BotonTema _btnTabConsulta = new("Consulta de Pagos");
        private readonly BotonTema _btnTabRegistro = new("Registrar Pago");
        private readonly BotonTema _btnTabAnulacion = new("Anular Pago");
        private readonly BotonTema _btnTabReportes = new("Consultas y reportes");

        private readonly BotonTema _btnNuevo = new("&Nuevo");
        private readonly BotonTema _btnGuardar = new("&Guardar", EstiloBoton.Primario);
        private readonly BotonTema _btnIrAnular = new("&Anular", EstiloBoton.Peligro) { Enabled = false };
        private readonly BotonTema _btnConfirmarAnular = new("&Anular pago", EstiloBoton.Peligro) { Enabled = false };
        private readonly BotonTema _btnLimpiar = new("&Limpiar");
        private readonly BotonTema _btnCerrar = new("&Cerrar");

        private readonly CtlPagoConsulta _consulta = new();
        private readonly CtlPagoRegistro _registro = new();
        private readonly CtlPagoAnulacion _anulacion = new();
        private readonly CtlReporteCuenta _reporteCuenta = new();
        private readonly CtlReporteAntiguedad _reporteAntiguedad = new();
        private readonly TableLayoutPanel _panelReportes = new();

        private Vista _vistaActual = Vista.Consulta;
        private Pago? _seleccionado;

        public FrmPagos()
        {
            InitializeComponent();
            ConstruirInterfaz();
            ConectarEventos();
        }

        // construcción

        private void ConstruirInterfaz()
        {
            Text = "Kobra · Pagos";
            MinimumSize = new Size(LogicalToDeviceUnits(1020), LogicalToDeviceUnits(680));
            if (ClientSize.Width < MinimumSize.Width || ClientSize.Height < MinimumSize.Height)
                ClientSize = new Size(LogicalToDeviceUnits(1120), LogicalToDeviceUnits(740));

            var barraVistas = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                BackColor = Tema.ContenedorBajo,
                Padding = new Padding(12, 6, 12, 6),
                Margin = new Padding(0)
            };
            _btnTabConsulta.Margin = new Padding(0, 0, 6, 0);
            _btnTabRegistro.Margin = new Padding(0, 0, 6, 0);
            _btnTabAnulacion.Margin = new Padding(0, 0, 6, 0);
            barraVistas.Controls.AddRange([_btnTabConsulta, _btnTabRegistro, _btnTabAnulacion, _btnTabReportes]);

            _barraAcciones.Agregar(_btnNuevo, _btnGuardar, _btnIrAnular, _btnConfirmarAnular, _btnLimpiar, _btnCerrar);

            _panelReportes.Dock = DockStyle.Fill;
            _panelReportes.ColumnCount = 2;
            _panelReportes.RowCount = 1;
            _panelReportes.BackColor = Color.Transparent;
            _panelReportes.Margin = new Padding(0);
            _panelReportes.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            _panelReportes.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            _panelReportes.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            _reporteCuenta.Margin = new Padding(0, 0, 6, 0);
            _reporteAntiguedad.Margin = new Padding(6, 0, 0, 0);
            _panelReportes.Controls.Add(_reporteCuenta, 0, 0);
            _panelReportes.Controls.Add(_reporteAntiguedad, 1, 0);

            var cuerpo = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(12),
                BackColor = Color.Transparent
            };
            cuerpo.Controls.Add(_consulta);
            cuerpo.Controls.Add(_registro);
            cuerpo.Controls.Add(_anulacion);
            cuerpo.Controls.Add(_panelReportes);

            Controls.Add(cuerpo);
            Controls.Add(barraVistas);
            Controls.Add(_barraTitulo);
            Controls.Add(_barraAcciones);

            CancelButton = _btnCerrar;
        }

        private void ConectarEventos()
        {
            Load += FrmPagos_Load;

            _btnTabConsulta.Click += (_, _) => CambiarVista(Vista.Consulta);
            _btnTabRegistro.Click += (_, _) =>
            {
                CambiarVista(Vista.Registro);
                _registro.EnfocarPrimero();
            };
            _btnTabAnulacion.Click += (_, _) =>
            {
                CambiarVista(Vista.Anulacion);
                if (_seleccionado is not null)
                    _anulacion.Cargar(_seleccionado);
            };
            _btnTabReportes.Click += (_, _) => CambiarVista(Vista.Reportes);

            _btnNuevo.Click += (_, _) =>
            {
                CambiarVista(Vista.Registro);
                _registro.Limpiar();
                _registro.EnfocarPrimero();
            };
            _btnGuardar.Click += (_, _) => Guardar();
            _btnIrAnular.Click += (_, _) =>
            {
                CambiarVista(Vista.Anulacion);
                if (_seleccionado is not null)
                    _anulacion.Cargar(_seleccionado);
            };
            _btnConfirmarAnular.Click += (_, _) => Anular();
            _btnLimpiar.Click += (_, _) => LimpiarVistaActual();
            _btnCerrar.Click += (_, _) => Close();

            // Eventos de CtlPagoConsulta
            _consulta.BusquedaSolicitada += explicita => CargarConsulta(mostrarMensaje: explicita);
            _consulta.PagoSeleccionado += pago =>
            {
                _seleccionado = pago;
                _btnIrAnular.Enabled = pago is { Anulado: false, FacturaAnulada: false };
            };
            _consulta.BusquedaEnfocada += (_, _) => AcceptButton = _consulta.BotonBuscar;
            _consulta.RejillaEnfocada += (_, _) => AcceptButton = null;

            // Eventos de CtlPagoRegistro
            _registro.FichaEnfocada += (_, _) => AcceptButton = _btnGuardar;

            // Eventos de CtlPagoAnulacion
            _anulacion.PagoCargado += pago =>
                _btnConfirmarAnular.Enabled = pago is { Anulado: false, FacturaAnulada: false };
            _anulacion.BusquedaEnfocada += (_, _) => AcceptButton = _anulacion.BotonBuscar;
            _anulacion.MotivoEnfocado += (_, _) => AcceptButton = null;

            // Eventos de los reportes
            _reporteCuenta.GeneracionEnfocada += (_, _) => AcceptButton = _reporteCuenta.BotonGenerar;
            _reporteAntiguedad.GeneracionEnfocada += (_, _) => AcceptButton = _reporteAntiguedad.BotonGenerar;
        }

        private void FrmPagos_Load(object? sender, EventArgs e)
        {
            _consulta.CargarClientesFiltro();
            _registro.CargarClientesActivos();
            _registro.Limpiar();
            _reporteCuenta.CargarClientes();
            _reporteAntiguedad.CargarClientes();
            CambiarVista(Vista.Consulta);
            CargarConsulta(mostrarMensaje: true);
            _consulta.EnfocarBusqueda();
        }

        protected override bool ProcesarAtajo(Keys tecla)
        {
            switch (tecla)
            {
                case Keys.F2:
                    if (_vistaActual == Vista.Consulta)
                        _consulta.EnfocarBusqueda();
                    else if (_vistaActual == Vista.Anulacion)
                        _anulacion.EnfocarBusqueda();
                    else if (_vistaActual == Vista.Reportes)
                        _reporteCuenta.EnfocarGenerar();
                    return true;

                case Keys.F5:
                    RecargarFiltros();
                    CargarConsulta(mostrarMensaje: true);
                    if (_vistaActual == Vista.Reportes)
                    {
                        _reporteCuenta.Generar();
                        _reporteAntiguedad.Generar();
                    }
                    return true;

                case Keys.Control | Keys.N:
                    CambiarVista(Vista.Registro);
                    _registro.Limpiar();
                    _registro.EnfocarPrimero();
                    return true;

                case Keys.Control | Keys.S:
                    if (_vistaActual == Vista.Registro)
                        Guardar();
                    return true;

                default:
                    return false;
            }
        }

        // operaciones

        private void RecargarFiltros()
        {
            _consulta.CargarClientesFiltro();
            _registro.CargarClientesActivos();
            _reporteCuenta.CargarClientes();
            _reporteAntiguedad.CargarClientes();
        }

        private void CambiarVista(Vista vista)
        {
            _vistaActual = vista;

            _consulta.Visible = vista == Vista.Consulta;
            _registro.Visible = vista == Vista.Registro;
            _anulacion.Visible = vista == Vista.Anulacion;
            _panelReportes.Visible = vista == Vista.Reportes;

            MarcarTab(_btnTabConsulta, vista == Vista.Consulta);
            MarcarTab(_btnTabRegistro, vista == Vista.Registro);
            MarcarTab(_btnTabAnulacion, vista == Vista.Anulacion);
            MarcarTab(_btnTabReportes, vista == Vista.Reportes);

            _btnNuevo.Visible = vista == Vista.Consulta;
            _btnIrAnular.Visible = vista == Vista.Consulta;
            _btnGuardar.Visible = vista == Vista.Registro;
            _btnConfirmarAnular.Visible = vista == Vista.Anulacion;

            AcceptButton = vista switch
            {
                Vista.Consulta => _consulta.BotonBuscar,
                Vista.Registro => _btnGuardar,
                Vista.Anulacion => _anulacion.BotonBuscar,
                Vista.Reportes => _reporteCuenta.BotonGenerar,
                _ => null
            };

            _barraTitulo.Info = vista switch
            {
                Vista.Consulta => "CONSULTA DE PAGOS",
                Vista.Registro => "MODO: NUEVO PAGO",
                Vista.Anulacion => "ANULACIÓN DE PAGO",
                Vista.Reportes => "ESTADO DE CUENTA Y ANTIGÜEDAD",
                _ => string.Empty
            };

            _barraAcciones.RestablecerPista();

            if (vista == Vista.Registro)
                _registro.EnfocarPrimero();
            else if (vista == Vista.Anulacion)
                _anulacion.EnfocarBusqueda();
        }

        private static void MarcarTab(BotonTema btn, bool activo)
        {
            if (activo)
            {
                btn.BackColor = Tema.PrimarioContenedor;
                btn.ForeColor = Color.White;
                btn.FlatAppearance.BorderColor = Tema.PrimarioContenedor;
                btn.FlatAppearance.MouseOverBackColor = ControlPaint.Dark(Tema.PrimarioContenedor, 0.15f);
                btn.FlatAppearance.MouseDownBackColor = ControlPaint.Dark(Tema.PrimarioContenedor, 0.25f);
            }
            else
            {
                btn.BackColor = Tema.Blanco;
                btn.ForeColor = Tema.Primario;
                btn.FlatAppearance.BorderColor = Tema.Borde;
                btn.FlatAppearance.MouseOverBackColor = Tema.Contenedor;
                btn.FlatAppearance.MouseDownBackColor = ControlPaint.Dark(Tema.Contenedor, 0.06f);
            }
        }

        private void CargarConsulta(bool mostrarMensaje, int? seleccionarId = null)
        {
            var mantener = seleccionarId ?? _seleccionado?.Id;
            var error = _consulta.Cargar(mantener);
            if (error is not null && mostrarMensaje)
                MostrarAdvertencia(error);
        }

        private void Guardar()
        {
            _registro.LimpiarErrores();
            var pago = _registro.Leer();

            try
            {
                var nuevoId = _negocio.Registrar(pago);
                _registro.Limpiar();
                CambiarVista(Vista.Consulta);
                CargarConsulta(mostrarMensaje: false, seleccionarId: nuevoId);
                var numeroFactura = _consulta.ObtenerNumeroFactura(nuevoId);
                var mensaje = string.IsNullOrEmpty(numeroFactura)
                    ? $"Pago n.º {nuevoId} registrado."
                    : $"Pago registrado en la factura {numeroFactura}.";
                _barraAcciones.MostrarMensaje(mensaje, esError: false);
            }
            catch (ReglaNegocioException ex)
            {
                _registro.MarcarCampo(ex.Campo, ex.Message);
                MostrarError(ex);
            }
            catch (DatosException ex)
            {
                MostrarError(ex);
            }
        }

        private void Anular()
        {
            var pago = _anulacion.PagoActual;
            if (pago is null)
                return;

            var motivo = _anulacion.Motivo;
            if (motivo.Length == 0)
            {
                _anulacion.MarcarErrorMotivo("El motivo de anulación es obligatorio.");
                MostrarAdvertencia("Debe indicar el motivo de anulación.");
                return;
            }

            var confirmado = Confirmar(
                $"¿Anular definitivamente el pago n.º {pago.Id}?",
                $"Factura: {pago.NumeroFactura}  ·  Cliente: {pago.ClienteDescripcion}  ·  Monto: B/. {pago.Monto:N2}",
                "El saldo de la factura volverá a aumentar. Esta acción es irreversible.",
                peligro: true,
                textoSi: "&Anular");

            if (!confirmado)
                return;

            try
            {
                _negocio.Anular(pago.Id, motivo);
                _anulacion.Limpiar();
                CambiarVista(Vista.Consulta);
                CargarConsulta(mostrarMensaje: false, seleccionarId: pago.Id);
                _barraAcciones.MostrarMensaje($"Pago n.º {pago.Id} anulado.", esError: false);
            }
            catch (ReglaNegocioException ex)
            {
                if (ex.Campo == nameof(Pago.MotivoAnulacion))
                    _anulacion.MarcarErrorMotivo(ex.Message);
                MostrarError(ex);
            }
            catch (DatosException ex)
            {
                MostrarError(ex);
            }
        }

        private void LimpiarVistaActual()
        {
            switch (_vistaActual)
            {
                case Vista.Consulta:
                    _consulta.LimpiarFiltros();
                    CargarConsulta(mostrarMensaje: false);
                    break;
                case Vista.Registro:
                    _registro.Limpiar();
                    _registro.EnfocarPrimero();
                    break;
                case Vista.Anulacion:
                    _anulacion.Limpiar();
                    _anulacion.EnfocarBusqueda();
                    break;
                case Vista.Reportes:
                    _reporteCuenta.Limpiar();
                    _reporteAntiguedad.Limpiar();
                    break;
            }
            _barraAcciones.RestablecerPista();
        }
    }
}
