using Kobra.Datos;
using Kobra.Entidades;
using Kobra.Negocio;

namespace Kobra.UI
{
    /// <summary>
    /// Módulo de Facturas: orquesta Consulta y Saldos, Nueva Factura y Anulación.
    /// Las reglas viven en FacturaNegocio y cada vista en su UserControl.
    /// </summary>
    public partial class FrmFacturas : FormularioModulo
    {
        private enum Vista { Consulta, Registro, Anulacion }

        private readonly FacturaNegocio _negocio = new();

        private readonly BarraTituloModulo _barraTitulo = new("FACTURAS");
        private readonly BarraAcciones _barraAcciones = new("F2 buscar · F5 actualizar · Ctrl+N nueva · Ctrl+S guardar · Esc cerrar");

        private readonly BotonTema _btnTabConsulta = new("Consulta y Saldos");
        private readonly BotonTema _btnTabRegistro = new("Nueva Factura");
        private readonly BotonTema _btnTabAnulacion = new("Anular Factura");

        private readonly BotonTema _btnNueva = new("&Nueva");
        private readonly BotonTema _btnGuardar = new("&Guardar", EstiloBoton.Primario);
        private readonly BotonTema _btnIrAnular = new("&Anular", EstiloBoton.Peligro) { Enabled = false };
        private readonly BotonTema _btnConfirmarAnular = new("&Anular factura", EstiloBoton.Peligro) { Enabled = false };
        private readonly BotonTema _btnLimpiar = new("&Limpiar");
        private readonly BotonTema _btnCerrar = new("&Cerrar");

        private readonly CtlFacturaConsulta _consulta = new();
        private readonly CtlFacturaFicha _ficha = new();
        private readonly CtlFacturaAnulacion _anulacion = new();

        private Vista _vistaActual = Vista.Consulta;
        private Factura? _seleccionada;

        public FrmFacturas()
        {
            InitializeComponent();
            ConstruirInterfaz();
            ConectarEventos();
        }

        // construcción
        private void ConstruirInterfaz()
        {
            Text = "Kobra · Facturas";
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
            barraVistas.Controls.AddRange([_btnTabConsulta, _btnTabRegistro, _btnTabAnulacion]);

            _barraAcciones.Agregar(_btnNueva, _btnGuardar, _btnIrAnular, _btnConfirmarAnular, _btnLimpiar, _btnCerrar);

            var cuerpo = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(12),
                BackColor = Color.Transparent
            };
            cuerpo.Controls.Add(_consulta);
            cuerpo.Controls.Add(_ficha);
            cuerpo.Controls.Add(_anulacion);

            Controls.Add(cuerpo);
            Controls.Add(barraVistas);
            Controls.Add(_barraTitulo);
            Controls.Add(_barraAcciones);

            CancelButton = _btnCerrar;
        }

        private void ConectarEventos()
        {
            Load += FrmFacturas_Load;

            _btnTabConsulta.Click += (_, _) => CambiarVista(Vista.Consulta);
            _btnTabRegistro.Click += (_, _) => CambiarVista(Vista.Registro);
            _btnTabAnulacion.Click += (_, _) =>
            {
                CambiarVista(Vista.Anulacion);
                if (_seleccionada is not null)
                    _anulacion.Cargar(_seleccionada);
            };

            _btnNueva.Click += (_, _) =>
            {
                CambiarVista(Vista.Registro);
                _ficha.Limpiar();
                _ficha.EnfocarPrimero();
            };
            _btnGuardar.Click += (_, _) => Guardar();
            _btnIrAnular.Click += (_, _) =>
            {
                CambiarVista(Vista.Anulacion);
                if (_seleccionada is not null)
                    _anulacion.Cargar(_seleccionada);
            };
            _btnConfirmarAnular.Click += (_, _) => Anular();
            _btnLimpiar.Click += (_, _) => LimpiarVistaActual();
            _btnCerrar.Click += (_, _) => Close();

            // Eventos de CtlFacturaConsulta
            _consulta.BusquedaSolicitada += explicita => CargarConsulta(mostrarMensaje: explicita);
            _consulta.FacturaSeleccionada += f =>
            {
                _seleccionada = f;
                _btnIrAnular.Enabled = f is { Anulada: false, TotalPagado: 0 };
            };
            _consulta.BusquedaEnfocada += (_, _) => AcceptButton = _consulta.BotonBuscar;
            _consulta.RejillaEnfocada += (_, _) => AcceptButton = null;

            // Eventos de CtlFacturaFicha
            _ficha.FichaEnfocada += (_, _) => AcceptButton = _btnGuardar;

            // Eventos de CtlFacturaAnulacion
            _anulacion.FacturaCargada += f =>
                _btnConfirmarAnular.Enabled = f is { Anulada: false, TotalPagado: 0 };
            _anulacion.BusquedaEnfocada += (_, _) => AcceptButton = _anulacion.BotonBuscar;
            _anulacion.MotivoEnfocado += (_, _) => AcceptButton = null;
        }

        private void FrmFacturas_Load(object? sender, EventArgs e)
        {
            _consulta.CargarClientesFiltro();
            _ficha.CargarClientesActivos();
            _ficha.Limpiar();
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
                    return true;
                case Keys.F5:
                    _consulta.CargarClientesFiltro();
                    _ficha.CargarClientesActivos();
                    CargarConsulta(mostrarMensaje: true);
                    return true;
                case Keys.Control | Keys.N:
                    CambiarVista(Vista.Registro);
                    _ficha.Limpiar();
                    _ficha.EnfocarPrimero();
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

        private void CambiarVista(Vista vista)
        {
            _vistaActual = vista;

            _consulta.Visible = vista == Vista.Consulta;
            _ficha.Visible = vista == Vista.Registro;
            _anulacion.Visible = vista == Vista.Anulacion;

            MarcarTab(_btnTabConsulta, vista == Vista.Consulta);
            MarcarTab(_btnTabRegistro, vista == Vista.Registro);
            MarcarTab(_btnTabAnulacion, vista == Vista.Anulacion);

            _btnNueva.Visible = vista == Vista.Consulta;
            _btnIrAnular.Visible = vista == Vista.Consulta;
            _btnGuardar.Visible = vista == Vista.Registro;
            _btnConfirmarAnular.Visible = vista == Vista.Anulacion;

            AcceptButton = vista switch
            {
                Vista.Consulta => _consulta.BotonBuscar,
                Vista.Registro => _btnGuardar,
                Vista.Anulacion => _anulacion.BotonBuscar,
                _ => null
            };

            _barraTitulo.Info = vista switch
            {
                Vista.Consulta => "CONSULTA Y SALDOS",
                Vista.Registro => "MODO: NUEVA FACTURA",
                Vista.Anulacion => "ANULACIÓN DE FACTURA",
                _ => string.Empty
            };

            _barraAcciones.RestablecerPista();

            if (vista == Vista.Registro)
                _ficha.EnfocarPrimero();
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
            var mantener = seleccionarId ?? _seleccionada?.Id;
            var error = _consulta.Cargar(mantener);
            if (error is not null && mostrarMensaje)
                MostrarAdvertencia(error);
        }

        private void Guardar()
        {
            _ficha.LimpiarErrores();
            var factura = _ficha.Leer();

            try
            {
                var nuevoId = _negocio.Registrar(factura);
                _ficha.Limpiar();
                CambiarVista(Vista.Consulta);
                CargarConsulta(mostrarMensaje: false, seleccionarId: nuevoId);
                _barraAcciones.MostrarMensaje($"Factura {factura.Numero.Trim()} guardada.", esError: false);
            }
            catch (ReglaNegocioException ex)
            {
                _ficha.MarcarCampo(ex.Campo, ex.Message);
                MostrarError(ex);
            }
            catch (DatosException ex)
            {
                MostrarError(ex);
            }
        }

        private void Anular()
        {
            var factura = _anulacion.FacturaActual;
            if (factura is null)
                return;

            var motivo = _anulacion.Motivo;
            if (motivo.Length == 0)
            {
                _anulacion.MarcarErrorMotivo("El motivo de anulación es obligatorio.");
                MostrarAdvertencia("Debe indicar el motivo de anulación.");
                return;
            }

            var confirmado = Confirmar(
                $"¿Anular definitivamente la factura {factura.Numero}?",
                $"Cliente: {factura.ClienteDescripcion}  ·  Total: B/. {factura.Total:N2}",
                "Esta acción es irreversible y dejará el saldo de la factura en B/. 0.00.",
                peligro: true,
                textoSi: "&Anular");

            if (!confirmado)
                return;

            try
            {
                _negocio.Anular(factura.Id, motivo);
                _anulacion.Limpiar();
                CambiarVista(Vista.Consulta);
                CargarConsulta(mostrarMensaje: false, seleccionarId: factura.Id);
                _barraAcciones.MostrarMensaje($"Factura {factura.Numero} anulada.", esError: false);
            }
            catch (ReglaNegocioException ex)
            {
                if (ex.Campo == nameof(Factura.MotivoAnulacion))
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
                    _ficha.Limpiar();
                    _ficha.EnfocarPrimero();
                    break;
                case Vista.Anulacion:
                    _anulacion.Limpiar();
                    _anulacion.EnfocarBusqueda();
                    break;
            }
            _barraAcciones.RestablecerPista();
        }
    }
}