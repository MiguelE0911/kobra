using Kobra.Entidades;
using Kobra.Negocio;

namespace Kobra.UI
{
    /// <summary>
    /// Ficha de datos de un cliente: lectura y escritura de los campos, y validación al salir de cada campo.
    /// No sabe de bases de datos ni de botones; el formulario decide cuándo guardar.
    /// </summary>
    internal sealed class CtlClienteFicha : UserControl
    {
        private readonly TarjetaPanel _tarjeta = new("INFORMACIÓN DEL CLIENTE");
        private readonly ErrorProvider _errores = new() { BlinkStyle = ErrorBlinkStyle.NeverBlink };

        private readonly TextBox _txtCodigo = new() { MaxLength = 20 };
        private readonly TextBox _txtNombre = new() { MaxLength = 150 };
        private readonly TextBox _txtRucCedula = new() { MaxLength = 30 };
        private readonly TextBox _txtTelefono = new() { MaxLength = 30 };
        private readonly TextBox _txtEmail = new() { MaxLength = 150 };
        private readonly TextBox _txtDireccion = new()
        {
            MaxLength = 250,
            Multiline = true,
            ScrollBars = ScrollBars.Vertical
        };
        private readonly NumericUpDown _nudLimite = new()
        {
            DecimalPlaces = 2,
            ThousandsSeparator = true,
            TextAlign = HorizontalAlignment.Right,
            Font = Tema.Numeros(9.5F),
            // Tope de DECIMAL(18,2).
            Maximum = Formato.MaximoMonto
        };
        private readonly NumericUpDown _nudPlazo = new()
        {
            Maximum = 3650,
            Value = 30,
            TextAlign = HorizontalAlignment.Right,
            Font = Tema.Numeros(9.5F)
        };
        private readonly CheckBox _chkActivo = new() { Text = "Cliente activo", AutoSize = true, Enabled = false, TabStop = false };
        private readonly TextBox _txtCreacion = CampoFicha.CrearSoloLectura();
        private readonly TextBox _txtModificacion = CampoFicha.CrearSoloLectura();

        public CtlClienteFicha()
        {
            SuspendLayout();
            AutoScaleMode = AutoScaleMode.None;
            Dock = DockStyle.Fill;
            Margin = new Padding(0, 8, 0, 0);
            AutoSize = true;

            _errores.ContainerControl = this;
            _txtCodigo.Validating += (_, _) => _errores.SetError(_txtCodigo,
                string.IsNullOrWhiteSpace(_txtCodigo.Text) ? "El código es obligatorio." : string.Empty);
            _txtNombre.Validating += (_, _) => _errores.SetError(_txtNombre,
                string.IsNullOrWhiteSpace(_txtNombre.Text) ? "El nombre es obligatorio." : string.Empty);
            _txtEmail.Validating += (_, _) =>
            {
                var correo = _txtEmail.Text.Trim();
                var invalido = correo.Length > 0 && !ClienteNegocio.EmailValido(correo);
                _errores.SetError(_txtEmail, invalido ? "El correo electrónico no tiene un formato válido." : string.Empty);
            };

            // Orden de Tab: columna izquierda de arriba abajo y luego la derecha.
            var orden = new Control[] { _txtCodigo, _txtNombre, _txtRucCedula, _txtTelefono, _txtEmail,
                                        _nudLimite, _nudPlazo, _txtDireccion };
            for (var i = 0; i < orden.Length; i++)
                orden[i].TabIndex = i;

            var rejilla = CampoFicha.CrearRejilla(6);
            CampoFicha.Agregar(rejilla, "Código", _txtCodigo, 0, 0, obligatorio: true);
            CampoFicha.Agregar(rejilla, "Nombre", _txtNombre, 1, 0, obligatorio: true);
            CampoFicha.Agregar(rejilla, "Cédula / RUC", _txtRucCedula, 2, 0);
            CampoFicha.Agregar(rejilla, "Teléfono", _txtTelefono, 3, 0);
            CampoFicha.Agregar(rejilla, "Correo", _txtEmail, 4, 0);
            CampoFicha.Agregar(rejilla, "Creado", _txtCreacion, 5, 0);

            CampoFicha.Agregar(rejilla, "Límite de crédito (B/.)", _nudLimite, 0, 1);
            CampoFicha.Agregar(rejilla, "Plazo (días)", _nudPlazo, 1, 1);
            CampoFicha.Agregar(rejilla, "Dirección", _txtDireccion, 2, 1, filasOcupadas: 2);
            CampoFicha.Agregar(rejilla, "Estado", _chkActivo, 4, 1);
            CampoFicha.Agregar(rejilla, "Modificado", _txtModificacion, 5, 1);

            _tarjeta.MostrarNota("* obligatorio");
            _tarjeta.Cuerpo.Controls.Add(rejilla, 0, 0);
            _tarjeta.Margin = new Padding(0);
            Controls.Add(_tarjeta);

            // Enter en esta ficha convierte "Guardar" en el botón por defecto del formulario.
            _tarjeta.Enter += (_, _) => FichaEnfocada?.Invoke(this, EventArgs.Empty);
            ResumeLayout(true);
        }

        public event EventHandler? FichaEnfocada;

        public string Codigo => _txtCodigo.Text.Trim();

        public string Nombre => _txtNombre.Text.Trim();

        public void EnfocarPrimero() => _txtCodigo.Focus();

        public Cliente Leer(int id, bool activo) => new()
        {
            Id = id,
            Codigo = _txtCodigo.Text,
            Nombre = _txtNombre.Text,
            RucCedula = _txtRucCedula.Text,
            Telefono = _txtTelefono.Text,
            Email = _txtEmail.Text,
            Direccion = _txtDireccion.Text,
            LimiteCredito = _nudLimite.Value,
            PlazoDias = (int)_nudPlazo.Value,
            Activo = activo
        };

        public void Cargar(Cliente c)
        {
            LimpiarErrores();
            _txtCodigo.Text = c.Codigo;
            _txtNombre.Text = c.Nombre;
            _txtRucCedula.Text = c.RucCedula ?? string.Empty;
            _txtTelefono.Text = c.Telefono ?? string.Empty;
            _txtEmail.Text = c.Email ?? string.Empty;
            _txtDireccion.Text = c.Direccion ?? string.Empty;
            _nudLimite.Value = Math.Min(c.LimiteCredito, _nudLimite.Maximum);
            _nudPlazo.Value = Math.Min(c.PlazoDias, _nudPlazo.Maximum);
            _chkActivo.Checked = c.Activo;
            _txtCreacion.Text = c.FechaCreacion.ToString(Formato.FechaHora);
            _txtModificacion.Text = c.FechaModificacion?.ToString(Formato.FechaHora) ?? "—";
        }

        /// <summary>Deja la ficha lista para un cliente nuevo (activo, plazo de 30 días).</summary>
        public void Limpiar()
        {
            LimpiarErrores();
            _txtCodigo.Clear();
            _txtNombre.Clear();
            _txtRucCedula.Clear();
            _txtTelefono.Clear();
            _txtEmail.Clear();
            _txtDireccion.Clear();
            _nudLimite.Value = 0;
            _nudPlazo.Value = 30;
            _chkActivo.Checked = true;
            _txtCreacion.Text = "Al guardar";
            _txtModificacion.Text = "—";
        }

        public void MostrarActivo(bool activo) => _chkActivo.Checked = activo;

        public void LimpiarErrores() => _errores.Clear();

        /// <summary>Marca el campo que causó el error de negocio y le da el foco.</summary>
        public void MarcarCampo(string? campo, string mensaje)
        {
            Control? control = campo switch
            {
                nameof(Cliente.Codigo) => _txtCodigo,
                nameof(Cliente.Nombre) => _txtNombre,
                nameof(Cliente.RucCedula) => _txtRucCedula,
                nameof(Cliente.Telefono) => _txtTelefono,
                nameof(Cliente.Email) => _txtEmail,
                nameof(Cliente.Direccion) => _txtDireccion,
                nameof(Cliente.LimiteCredito) => _nudLimite,
                nameof(Cliente.PlazoDias) => _nudPlazo,
                _ => null
            };

            if (control is null)
                return;

            _errores.SetError(control, mensaje);
            control.Focus();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _errores.Dispose();
            base.Dispose(disposing);
        }
    }
}
