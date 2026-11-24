using System.Globalization;
using Kobra.Datos;
using Kobra.Entidades;
using Kobra.Negocio;

namespace Kobra.UI
{
    /// <summary>
    /// Ficha de emisión de factura a crédito: lectura, escritura y validación al salir de cada campo.
    /// </summary>
    internal sealed class CtlFacturaFicha : UserControl
    {
        private static CultureInfo Cultura => CultureInfo.CurrentCulture;

        private static readonly decimal MaximoDecimal = new([276447231, 23027, 0, 131072]);

        private readonly FacturaNegocio _facturaNegocio = new();
        private readonly ClienteNegocio _clienteNegocio = new();
        private readonly TarjetaPanel _tarjeta = new("EMISIÓN DE FACTURA A CRÉDITO");
        private readonly ErrorProvider _errores = new() { BlinkStyle = ErrorBlinkStyle.NeverBlink };

        private readonly TextBox _txtNumero = new() { MaxLength = 30, CharacterCasing = CharacterCasing.Upper };
        private readonly ComboBox _cboCliente = new() { DropDownStyle = ComboBoxStyle.DropDownList };
        private readonly DateTimePicker _dtpEmision = new() { Format = DateTimePickerFormat.Short };
        private readonly DateTimePicker _dtpVencimiento = new() { Format = DateTimePickerFormat.Short };
        private readonly Label _lblPlazo = new() { AutoSize = true, ForeColor = Tema.TextoSecundario };
        private readonly Label _lblCredito = new() { AutoSize = true, ForeColor = Tema.Primario };

        private readonly NumericUpDown _nudSubtotal = CrearNumerico();
        private readonly NumericUpDown _nudItbms = CrearNumerico();
        private readonly CheckBox _chkAutoItbms = new()
        {
            Text = "Calcular 7% automático",
            Checked = true,
            AutoSize = true,
            ForeColor = Tema.TextoSecundario,
            Font = Tema.Normal(8.5F),
            Margin = new Padding(8, 5, 0, 0)
        };
        private readonly TextBox _txtTotal = new()
        {
            ReadOnly = true,
            TabStop = false,
            BorderStyle = BorderStyle.FixedSingle,
            TextAlign = HorizontalAlignment.Right,
            BackColor = Tema.Contenedor,
            ForeColor = Tema.PrimarioContenedor,
            Font = Tema.Negrita(10.5F),
            Text = "0.00"
        };

        private bool _actualizando;

        public CtlFacturaFicha()
        {
            SuspendLayout();
            AutoScaleMode = AutoScaleMode.None;
            Dock = DockStyle.Fill;
            Margin = new Padding(0);
            AutoSize = true;

            _errores.ContainerControl = this;

            // Validación al salir de cada campo
            _txtNumero.Validating += (_, _) => _errores.SetError(_txtNumero,
                string.IsNullOrWhiteSpace(_txtNumero.Text) ? "El número de factura es obligatorio." : string.Empty);

            _cboCliente.Validating += (_, _) => _errores.SetError(_cboCliente,
                _cboCliente.SelectedItem is not ItemCliente ? "Seleccione un cliente activo." : string.Empty);

            _dtpVencimiento.Validating += (_, _) => _errores.SetError(_dtpVencimiento,
                _dtpVencimiento.Value.Date < _dtpEmision.Value.Date
                    ? "La fecha de vencimiento no puede ser anterior a la emisión."
                    : string.Empty);

            _nudSubtotal.Validating += (_, _) => _errores.SetError(_nudSubtotal,
                (_nudSubtotal.Value + _nudItbms.Value) <= 0
                    ? "El total de la factura debe ser mayor que 0."
                    : string.Empty);

            var orden = new Control[] { _txtNumero, _cboCliente, _dtpEmision, _dtpVencimiento, _nudSubtotal, _chkAutoItbms, _nudItbms };
            for (var i = 0; i < orden.Length; i++)
                orden[i].TabIndex = i;

            _lblPlazo.Font = Tema.Normal(8.5F);
            _lblPlazo.Margin = new Padding(0, 4, 0, 0);
            _lblCredito.Font = Tema.Numeros(8.5F);
            _lblCredito.Margin = new Padding(0, 4, 0, 0);

            var panelItbms = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                ColumnCount = 2,
                RowCount = 1,
                Margin = new Padding(0)
            };
            panelItbms.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 52));
            panelItbms.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 48));
            _nudItbms.Dock = DockStyle.Fill;
            _nudItbms.Margin = new Padding(0);
            panelItbms.Controls.Add(_nudItbms, 0, 0);
            panelItbms.Controls.Add(_chkAutoItbms, 1, 0);

            var rejilla = CampoFicha.CrearRejilla(5);
            CampoFicha.Agregar(rejilla, "Número de factura", _txtNumero, 0, 0, obligatorio: true);
            CampoFicha.Agregar(rejilla, "Cliente destino", _cboCliente, 0, 1, obligatorio: true);
            CampoFicha.Agregar(rejilla, "Fecha de emisión", _dtpEmision, 1, 0, obligatorio: true);
            CampoFicha.Agregar(rejilla, "Fecha vencimiento", _dtpVencimiento, 1, 1, obligatorio: true);
            CampoFicha.Agregar(rejilla, "Condición / Plazo", _lblPlazo, 2, 0);
            CampoFicha.Agregar(rejilla, "Crédito disponible", _lblCredito, 2, 1);
            CampoFicha.Agregar(rejilla, "Subtotal neto (B/.)", _nudSubtotal, 3, 0, obligatorio: true);
            CampoFicha.Agregar(rejilla, "ITBMS 7% (B/.)", panelItbms, 3, 1);
            CampoFicha.Agregar(rejilla, "TOTAL FACTURA (B/.)", _txtTotal, 4, 1);

            _tarjeta.MostrarNota("* obligatorio");
            _tarjeta.Cuerpo.Controls.Add(rejilla, 0, 0);
            _tarjeta.Margin = new Padding(0);
            Controls.Add(_tarjeta);

            ConectarEventos();
            ResumeLayout(true);
        }

        public event EventHandler? FichaEnfocada;

        public void EnfocarPrimero() => _txtNumero.Focus();

        public void CargarClientesActivos()
        {
            try
            {
                var actual = (_cboCliente.SelectedItem as ItemCliente)?.Id;
                var activos = _clienteNegocio.Buscar(null, soloActivos: true);

                _cboCliente.DataSource = activos
                    .Select(c => new ItemCliente(c.Id, $"{c.Codigo} - {c.Nombre}", c))
                    .ToList();
                _cboCliente.DisplayMember = nameof(ItemCliente.Texto);
                _cboCliente.ValueMember = nameof(ItemCliente.Id);

                if (actual is int id)
                    _cboCliente.SelectedValue = id;
            }
            catch (DatosException ex)
            {
                _tarjeta.MostrarNota(ex.Message, esError: true);
            }
        }

        public Factura Leer() => new()
        {
            Numero = _txtNumero.Text,
            ClienteId = (_cboCliente.SelectedItem as ItemCliente)?.Id ?? 0,
            FechaEmision = _dtpEmision.Value.Date,
            FechaVencimiento = _dtpVencimiento.Value.Date,
            Subtotal = _nudSubtotal.Value,
            Itbms = _nudItbms.Value,
            Total = _nudSubtotal.Value + _nudItbms.Value
        };

        public void Limpiar()
        {
            LimpiarErrores();
            _actualizando = true;
            try
            {
                _txtNumero.Clear();
                _dtpEmision.Value = DateTime.Today;
                _chkAutoItbms.Checked = true;
                _nudItbms.Enabled = false;
                _nudSubtotal.Value = 0;
                _nudItbms.Value = 0;
                _txtTotal.Text = 0m.ToString("N2", Cultura);

                if (_cboCliente.Items.Count > 0 && _cboCliente.SelectedIndex < 0)
                    _cboCliente.SelectedIndex = 0;
            }
            finally
            {
                _actualizando = false;
            }

            ActualizarClienteSeleccionado();
        }

        public void LimpiarErrores() => _errores.Clear();

        public void MarcarCampo(string? campo, string mensaje)
        {
            Control? control = campo switch
            {
                nameof(Factura.Numero) => _txtNumero,
                nameof(Factura.ClienteId) => _cboCliente,
                nameof(Factura.FechaEmision) => _dtpEmision,
                nameof(Factura.FechaVencimiento) => _dtpVencimiento,
                nameof(Factura.Subtotal) => _nudSubtotal,
                nameof(Factura.Itbms) => _nudItbms,
                _ => null
            };

            if (control is null)
                return;

            _errores.SetError(control, mensaje);
            control.Focus();
        }

        // privados

        private void ConectarEventos()
        {
            _tarjeta.Enter += (_, _) => FichaEnfocada?.Invoke(this, EventArgs.Empty);

            _cboCliente.SelectedIndexChanged += (_, _) =>
            {
                if (!_actualizando)
                    ActualizarClienteSeleccionado();
            };

            _dtpEmision.ValueChanged += (_, _) =>
            {
                if (!_actualizando)
                    RecalcularVencimiento();
            };

            _nudSubtotal.ValueChanged += (_, _) => RecalcularTotales();
            _nudItbms.ValueChanged += (_, _) => RecalcularTotales();
            _chkAutoItbms.CheckedChanged += (_, _) =>
            {
                _nudItbms.Enabled = !_chkAutoItbms.Checked;
                RecalcularTotales();
            };
        }

        private void ActualizarClienteSeleccionado()
        {
            if ((_cboCliente.SelectedItem as ItemCliente)?.Cliente is not { } cliente)
            {
                _lblPlazo.Text = "—";
                _lblCredito.Text = "Sin cliente activo";
                return;
            }

            RecalcularVencimiento();
            _lblPlazo.Text = $"Crédito a {cliente.PlazoDias} día(s)";

            try
            {
                var saldo = _facturaNegocio.ObtenerSaldoCliente(cliente.Id);
                var disponible = cliente.LimiteCredito - saldo;
                _lblCredito.ForeColor = disponible <= 0 ? Tema.Secundario : Tema.Primario;
                _lblCredito.Text =
                    $"Límite: B/. {cliente.LimiteCredito.ToString("N2", Cultura)} · " +
                    $"Saldo: B/. {saldo.ToString("N2", Cultura)} · " +
                    $"Disp.: B/. {disponible.ToString("N2", Cultura)}";
            }
            catch (DatosException)
            {
                _lblCredito.Text = $"Límite: B/. {cliente.LimiteCredito.ToString("N2", Cultura)}";
            }
        }

        private void RecalcularVencimiento()
        {
            if ((_cboCliente.SelectedItem as ItemCliente)?.Cliente is not { } cliente)
                return;

            _actualizando = true;
            try
            {
                _dtpVencimiento.Value = _dtpEmision.Value.Date.AddDays(cliente.PlazoDias);
            }
            finally
            {
                _actualizando = false;
            }
        }

        private void RecalcularTotales()
        {
            if (_actualizando)
                return;

            _actualizando = true;
            try
            {
                if (_chkAutoItbms.Checked)
                    _nudItbms.Value = Math.Round(_nudSubtotal.Value * 0.07m, 2, MidpointRounding.AwayFromZero);

                var total = _nudSubtotal.Value + _nudItbms.Value;
                _txtTotal.Text = total.ToString("N2", Cultura);
            }
            finally
            {
                _actualizando = false;
            }
        }

        private static NumericUpDown CrearNumerico() => new()
        {
            DecimalPlaces = 2,
            ThousandsSeparator = true,
            TextAlign = HorizontalAlignment.Right,
            Font = Tema.Numeros(9.5F),
            Maximum = MaximoDecimal
        };

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _errores.Dispose();
            base.Dispose(disposing);
        }

        private sealed record ItemCliente(int Id, string Texto, Cliente Cliente);
    }
}