using Kobra.Datos;
using Kobra.Entidades;
using Kobra.Negocio;

namespace Kobra.UI
{
    public partial class FrmClientes : Form
    {
        private readonly ClienteNegocio _negocio = new();

        private List<Cliente> _clientes = new();
        private int _clienteId;          // 0 = cliente nuevo
        private bool _activo = true;
        private bool _cargando;          // evita reaccionar a los cambios de selección hechos por código

        public FrmClientes()
        {
            InitializeComponent();
        }

        // ------------------------------------------------------------ eventos del formulario

        private void FrmClientes_Load(object? sender, EventArgs e)
        {
            PrepararNuevo();
            CargarLista(mostrarMensaje: true);
            ActiveControl = txtBuscar;   // lo más frecuente es buscar un cliente existente
        }

        private void FrmClientes_KeyDown(object? sender, KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.F2:
                    txtBuscar.Focus();
                    txtBuscar.SelectAll();
                    break;
                case Keys.F5:
                    CargarLista(mostrarMensaje: true);
                    break;
                case Keys.Control | Keys.S:
                    Guardar();
                    break;
                case Keys.Control | Keys.N:
                    PrepararNuevo();
                    break;
                default:
                    return;
            }
            e.Handled = true;
            e.SuppressKeyPress = true;
        }

        // Enter busca si se está en la búsqueda, no hace nada en la lista y guarda en los datos.
        private void EnterBusqueda(object? sender, EventArgs e) => AcceptButton = btnBuscar;

        private void EnterLista(object? sender, EventArgs e) => AcceptButton = null;

        private void EnterDatos(object? sender, EventArgs e) => AcceptButton = btnGuardar;

        // ------------------------------------------------------------ búsqueda y lista

        private void txtBuscar_TextChanged(object? sender, EventArgs e)
        {
            // Espera a que el usuario deje de escribir para no consultar en cada tecla.
            timerBusqueda.Stop();
            timerBusqueda.Start();
        }

        private void timerBusqueda_Tick(object? sender, EventArgs e)
        {
            timerBusqueda.Stop();
            CargarLista(mostrarMensaje: false);
        }

        private void btnBuscar_Click(object? sender, EventArgs e)
        {
            timerBusqueda.Stop();
            CargarLista(mostrarMensaje: true);
        }

        private void chkSoloActivos_CheckedChanged(object? sender, EventArgs e) =>
            CargarLista(mostrarMensaje: false);

        /// <summary>
        /// Recarga la lista según el filtro. <paramref name="seleccionarId"/> deja marcado ese cliente;
        /// si no se indica, se conserva el que se está editando.
        /// </summary>
        private void CargarLista(bool mostrarMensaje, int? seleccionarId = null)
        {
            timerBusqueda.Stop();
            string? error = null;

            try
            {
                _clientes = _negocio.Buscar(txtBuscar.Text, chkSoloActivos.Checked);
            }
            catch (DatosException ex)
            {
                _clientes = new List<Cliente>();
                error = ex.Message;
            }

            var idMantener = seleccionarId ?? (_clienteId > 0 ? _clienteId : (int?)null);

            _cargando = true;
            try
            {
                dgvClientes.DataSource = _clientes;
                dgvClientes.ClearSelection();
                dgvClientes.CurrentCell = null;

                var fila = idMantener is int id ? _clientes.FindIndex(c => c.Id == id) : -1;
                if (fila >= 0)
                {
                    dgvClientes.Rows[fila].Selected = true;
                    dgvClientes.CurrentCell = dgvClientes.Rows[fila].Cells[colNombre.Index];
                }
            }
            finally
            {
                _cargando = false;
            }

            if (error is not null)
            {
                MostrarMensajeEstado(error, esError: true);
                if (mostrarMensaje)
                    MessageBox.Show(this, error, "Clientes", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            MostrarMensajeEstado(_clientes.Count >= ClienteNegocio.MaximoResultados
                ? $"Mostrando los primeros {ClienteNegocio.MaximoResultados}. Afine la búsqueda."
                : $"{_clientes.Count} cliente(s)", esError: false);
        }

        private void dgvClientes_SelectionChanged(object? sender, EventArgs e)
        {
            // Se engancha a SelectionChanged y a CurrentCellChanged porque, según cómo se seleccione la
            // fila (clic, teclado o código), solo uno de los dos eventos llega con la fila actual ya fijada.
            if (_cargando || dgvClientes.CurrentRow?.DataBoundItem is not Cliente cliente)
                return;

            // Evita recargar (y pisar lo que se está editando) cuando ya es el cliente mostrado.
            if (cliente.Id == _clienteId)
                return;

            CargarEnFormulario(cliente);
        }

        private void dgvClientes_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvClientes.Rows[e.RowIndex].DataBoundItem is Cliente { Activo: false })
                e.CellStyle!.ForeColor = SystemColors.GrayText;
        }

        // ------------------------------------------------------------ botones

        private void btnNuevo_Click(object? sender, EventArgs e) => PrepararNuevo();

        private void btnGuardar_Click(object? sender, EventArgs e) => Guardar();

        private void btnCerrar_Click(object? sender, EventArgs e) => Close();

        private void btnDesactivar_Click(object? sender, EventArgs e)
        {
            if (_clienteId == 0)
                return;

            var accion = _activo ? "desactivar" : "reactivar";
            var detalle = _activo ? "\nNo se le podrá facturar mientras esté inactivo." : string.Empty;
            var respuesta = MessageBox.Show(this,
                $"¿Desea {accion} al cliente \"{txtNombre.Text.Trim()}\"?{detalle}",
                "Clientes", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (respuesta != DialogResult.Yes)
                return;

            try
            {
                if (_activo)
                    _negocio.Desactivar(_clienteId);
                else
                    _negocio.Reactivar(_clienteId);

                _activo = !_activo;
                chkActivo.Checked = _activo;
                ActualizarBotones();
                CargarLista(mostrarMensaje: false);
                MostrarMensajeEstado(_activo ? "Cliente reactivado." : "Cliente desactivado.", esError: false);
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

            var respuesta = MessageBox.Show(this,
                $"¿Eliminar definitivamente al cliente \"{txtNombre.Text.Trim()}\"?\n" +
                "Esta acción no se puede deshacer. Si el cliente tiene facturas, no se eliminará.",
                "Clientes", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (respuesta != DialogResult.Yes)
                return;

            try
            {
                _negocio.Eliminar(_clienteId);
                PrepararNuevo();
                CargarLista(mostrarMensaje: false);
                MostrarMensajeEstado("Cliente eliminado.", esError: false);
            }
            catch (Exception ex) when (ex is ReglaNegocioException or DatosException)
            {
                MostrarError(ex);
            }
        }

        // ------------------------------------------------------------ guardar

        private void Guardar()
        {
            errorProvider.Clear();
            var cliente = LeerFormulario();

            try
            {
                if (cliente.Id == 0)
                {
                    cliente.Id = _negocio.Registrar(cliente);
                    _clienteId = cliente.Id;
                    ActualizarBotones();
                }
                else
                {
                    _negocio.Actualizar(cliente);
                }

                CargarLista(mostrarMensaje: false, seleccionarId: cliente.Id);
                MostrarMensajeEstado("Cliente guardado.", esError: false);
            }
            catch (ReglaNegocioException ex)
            {
                MarcarCampo(ex.Campo, ex.Message);
                MostrarError(ex);
            }
            catch (DatosException ex)
            {
                MostrarError(ex);
            }
        }

        // ------------------------------------------------------------ validación al salir del campo

        private void txtCodigo_Validating(object? sender, System.ComponentModel.CancelEventArgs e) =>
            errorProvider.SetError(txtCodigo,
                string.IsNullOrWhiteSpace(txtCodigo.Text) ? "El código es obligatorio." : string.Empty);

        private void txtNombre_Validating(object? sender, System.ComponentModel.CancelEventArgs e) =>
            errorProvider.SetError(txtNombre,
                string.IsNullOrWhiteSpace(txtNombre.Text) ? "El nombre es obligatorio." : string.Empty);

        private void txtEmail_Validating(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            var correo = txtEmail.Text.Trim();
            var invalido = correo.Length > 0 && !ClienteNegocio.EmailValido(correo);
            errorProvider.SetError(txtEmail, invalido ? "El correo electrónico no tiene un formato válido." : string.Empty);
        }

        // ------------------------------------------------------------ apoyo

        private void PrepararNuevo()
        {
            _clienteId = 0;
            _activo = true;
            errorProvider.Clear();

            txtCodigo.Clear();
            txtNombre.Clear();
            txtRucCedula.Clear();
            txtTelefono.Clear();
            txtEmail.Clear();
            txtDireccion.Clear();
            nudLimiteCredito.Value = 0;
            nudPlazoDias.Value = 30;
            chkActivo.Checked = true;

            _cargando = true;
            try
            {
                dgvClientes.ClearSelection();
                dgvClientes.CurrentCell = null;
            }
            finally
            {
                _cargando = false;
            }

            ActualizarBotones();
            txtCodigo.Focus();
        }

        private void CargarEnFormulario(Cliente c)
        {
            errorProvider.Clear();
            _clienteId = c.Id;
            _activo = c.Activo;

            txtCodigo.Text = c.Codigo;
            txtNombre.Text = c.Nombre;
            txtRucCedula.Text = c.RucCedula ?? string.Empty;
            txtTelefono.Text = c.Telefono ?? string.Empty;
            txtEmail.Text = c.Email ?? string.Empty;
            txtDireccion.Text = c.Direccion ?? string.Empty;
            nudLimiteCredito.Value = Math.Min(c.LimiteCredito, nudLimiteCredito.Maximum);
            nudPlazoDias.Value = Math.Min(c.PlazoDias, nudPlazoDias.Maximum);
            chkActivo.Checked = c.Activo;

            ActualizarBotones();
        }

        private Cliente LeerFormulario() => new()
        {
            Id = _clienteId,
            Codigo = txtCodigo.Text,
            Nombre = txtNombre.Text,
            RucCedula = txtRucCedula.Text,
            Telefono = txtTelefono.Text,
            Email = txtEmail.Text,
            Direccion = txtDireccion.Text,
            LimiteCredito = nudLimiteCredito.Value,
            PlazoDias = (int)nudPlazoDias.Value,
            Activo = _activo
        };

        private void ActualizarBotones()
        {
            var existe = _clienteId > 0;
            btnDesactivar.Enabled = existe;
            btnEliminar.Enabled = existe;
            btnDesactivar.Text = _activo ? "&Desactivar" : "&Reactivar";
        }

        private void MarcarCampo(string? campo, string mensaje)
        {
            Control? control = campo switch
            {
                nameof(Cliente.Codigo) => txtCodigo,
                nameof(Cliente.Nombre) => txtNombre,
                nameof(Cliente.RucCedula) => txtRucCedula,
                nameof(Cliente.Telefono) => txtTelefono,
                nameof(Cliente.Email) => txtEmail,
                nameof(Cliente.Direccion) => txtDireccion,
                nameof(Cliente.LimiteCredito) => nudLimiteCredito,
                nameof(Cliente.PlazoDias) => nudPlazoDias,
                _ => null
            };

            if (control is null)
                return;

            errorProvider.SetError(control, mensaje);
            control.Focus();
        }

        private void MostrarError(Exception ex)
        {
            var icono = ex is ReglaNegocioException ? MessageBoxIcon.Warning : MessageBoxIcon.Error;
            MessageBox.Show(this, ex.Message, "Clientes", MessageBoxButtons.OK, icono);
        }

        private void MostrarMensajeEstado(string texto, bool esError)
        {
            lblResultados.ForeColor = esError ? Color.Firebrick : SystemColors.ControlText;
            lblResultados.Text = texto;
        }
    }
}
