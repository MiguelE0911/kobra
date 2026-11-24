using Kobra.Datos;
using Kobra.Entidades;
using Kobra.Negocio;

namespace Kobra.UI
{
    /// <summary>
    /// Módulo de Clientes: registrar, consultar, actualizar, desactivar y eliminar. Solo orquesta los
    /// componentes (lista, ficha, barras); las reglas viven en ClienteNegocio.
    /// </summary>
    public partial class FrmClientes : FormularioModulo
    {
        private readonly ClienteNegocio _negocio = new();

        private int _clienteId;          // 0 = cliente nuevo
        private bool _activo = true;

        public FrmClientes()
        {
            InitializeComponent();
        }

        // ------------------------------------------------------------ eventos del formulario

        private void FrmClientes_Load(object? sender, EventArgs e)
        {
            PrepararNuevo();
            CargarLista(mostrarMensaje: true);
            lista.EnfocarBusqueda();   // lo más frecuente es buscar un cliente existente
        }

        protected override bool ProcesarAtajo(Keys tecla)
        {
            switch (tecla)
            {
                case Keys.F2:
                    lista.EnfocarBusqueda();
                    return true;
                case Keys.F5:
                    CargarLista(mostrarMensaje: true);
                    return true;
                case Keys.Control | Keys.S:
                    Guardar();
                    return true;
                case Keys.Control | Keys.N:
                    PrepararNuevo();
                    return true;
                default:
                    return false;
            }
        }

        private void lista_BusquedaSolicitada(bool explicita) => CargarLista(mostrarMensaje: explicita);

        private void lista_ClienteSeleccionado(Cliente cliente)
        {
            // Evita recargar (y pisar lo que se está editando) cuando ya es el cliente mostrado.
            if (cliente.Id != _clienteId)
                CargarEnFormulario(cliente);
        }

        // ------------------------------------------------------------ botones

        private void btnNuevo_Click(object? sender, EventArgs e) => PrepararNuevo();

        private void btnGuardar_Click(object? sender, EventArgs e) => Guardar();

        private void btnCerrar_Click(object? sender, EventArgs e) => Close();

        private void btnDesactivar_Click(object? sender, EventArgs e)
        {
            if (_clienteId == 0)
                return;

            var desactivar = _activo;
            var confirmado = Confirmar(
                $"¿Desea {(desactivar ? "desactivar" : "reactivar")} a este cliente?",
                $"Código: {ficha.Codigo}  ·  Nombre: {ficha.Nombre}",
                desactivar ? "No se le podrá facturar mientras esté inactivo." : null);
            if (!confirmado)
                return;

            try
            {
                if (desactivar)
                    _negocio.Desactivar(_clienteId);
                else
                    _negocio.Reactivar(_clienteId);

                _activo = !_activo;
                ficha.MostrarActivo(_activo);
                ActualizarEstado();
                CargarLista(mostrarMensaje: false);
                barraAcciones.MostrarMensaje(_activo ? "Cliente reactivado." : "Cliente desactivado.", esError: false);
            }
            catch (Exception ex) when (ex is ReglaNegocioException or DatosException)
            {
                MostrarError(ex);
            }
        }

        private void btnEliminar_Click(object? sender, EventArgs e)
        {
            if (_clienteId == 0)
                return;

            var confirmado = Confirmar(
                "¿Eliminar definitivamente a este cliente?",
                $"Código: {ficha.Codigo}  ·  Nombre: {ficha.Nombre}",
                "Esta acción no se puede deshacer. Si el cliente tiene facturas, no se eliminará.",
                peligro: true, textoSi: "&Eliminar");
            if (!confirmado)
                return;

            try
            {
                _negocio.Eliminar(_clienteId);
                PrepararNuevo();
                CargarLista(mostrarMensaje: false);
                barraAcciones.MostrarMensaje("Cliente eliminado.", esError: false);
            }
            catch (Exception ex) when (ex is ReglaNegocioException or DatosException)
            {
                MostrarError(ex);
            }
        }

        // ------------------------------------------------------------ operaciones

        /// <summary>
        /// Recarga la lista según el filtro. <paramref name="seleccionarId"/> deja marcado ese cliente;
        /// si no se indica, se conserva el que se está editando.
        /// </summary>
        private void CargarLista(bool mostrarMensaje, int? seleccionarId = null)
        {
            var mantener = seleccionarId ?? (_clienteId > 0 ? _clienteId : (int?)null);
            var error = lista.Cargar(mantener);

            if (error is not null && mostrarMensaje)
                MostrarAdvertencia(error);
        }

        private void Guardar()
        {
            ficha.LimpiarErrores();
            var cliente = ficha.Leer(_clienteId, _activo);

            try
            {
                if (cliente.Id == 0)
                {
                    cliente.Id = _negocio.Registrar(cliente);
                    _clienteId = cliente.Id;
                    ActualizarEstado();
                }
                else
                {
                    _negocio.Actualizar(cliente);
                }

                CargarLista(mostrarMensaje: false, seleccionarId: cliente.Id);
                // Recarga la ficha desde la base para mostrar las fechas reales de creación y modificación.
                if (_negocio.ObtenerPorId(cliente.Id) is { } guardado)
                    ficha.Cargar(guardado);
                barraAcciones.MostrarMensaje("Cliente guardado.", esError: false);
            }
            catch (ReglaNegocioException ex)
            {
                ficha.MarcarCampo(ex.Campo, ex.Message);
                MostrarError(ex);
            }
            catch (DatosException ex)
            {
                MostrarError(ex);
            }
        }

        private void PrepararNuevo()
        {
            _clienteId = 0;
            _activo = true;
            ficha.Limpiar();
            lista.QuitarSeleccion();
            ActualizarEstado();
            barraAcciones.RestablecerPista();
            ficha.EnfocarPrimero();
        }

        private void CargarEnFormulario(Cliente c)
        {
            _clienteId = c.Id;
            _activo = c.Activo;
            ficha.Cargar(c);
            ActualizarEstado();
        }

        /// <summary>Refleja en la barra de título y en los botones si se edita un cliente o se crea uno nuevo.</summary>
        private void ActualizarEstado()
        {
            var existe = _clienteId > 0;
            btnDesactivar.Enabled = existe;
            btnEliminar.Enabled = existe;
            btnDesactivar.Text = _activo ? "&Desactivar" : "&Reactivar";
            barraTitulo.Info = existe ? $"MODO: EDICIÓN · ID {_clienteId}" : "MODO: NUEVO CLIENTE";
        }
    }
}
