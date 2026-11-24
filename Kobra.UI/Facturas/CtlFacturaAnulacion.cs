using System.Globalization;
using Kobra.Datos;
using Kobra.Entidades;
using Kobra.Negocio;

namespace Kobra.UI
{
    /// <summary>
    /// Ficha de anulación lógica de factura: busca por número, muestra datos en solo lectura
    /// y exige un motivo de hasta 250 caracteres.
    /// </summary>
    internal sealed class CtlFacturaAnulacion : UserControl
    {
        private static CultureInfo Cultura => CultureInfo.CurrentCulture;

        private readonly FacturaNegocio _negocio = new();
        private readonly ErrorProvider _errores = new() { BlinkStyle = ErrorBlinkStyle.NeverBlink };

        private readonly TarjetaPanel _tarjetaDatos = new("DATOS DE LA FACTURA A ANULAR");
        private readonly TextBox _txtBuscarNumero = new() { MaxLength = 30, CharacterCasing = CharacterCasing.Upper };
        private readonly BotonTema _btnBuscar = new("&Buscar", EstiloBoton.Primario);

        private readonly TextBox _txtNumero = CrearSoloLectura();
        private readonly TextBox _txtCliente = CrearSoloLectura();
        private readonly TextBox _txtRuc = CrearSoloLectura();
        private readonly TextBox _txtEmision = CrearSoloLectura();
        private readonly TextBox _txtVencimiento = CrearSoloLectura();
        private readonly TextBox _txtSubtotal = CrearSoloLectura(derecha: true);
        private readonly TextBox _txtItbms = CrearSoloLectura(derecha: true);
        private readonly TextBox _txtTotal = CrearSoloLectura(derecha: true);
        private readonly TextBox _txtSaldo = CrearSoloLectura(derecha: true);
        private readonly Label _lblEstado = new() { AutoSize = true, Font = Tema.Negrita(9F) };

        private readonly TarjetaPanel _tarjetaMotivo = new("MOTIVO DE ANULACIÓN (OBLIGATORIO)");
        private readonly TextBox _txtMotivo = new()
        {
            Multiline = true,
            MaxLength = 250,
            ScrollBars = ScrollBars.Vertical,
            BorderStyle = BorderStyle.FixedSingle
        };
        private readonly Label _lblContador = new()
        {
            Text = "0 / 250 caracteres",
            TextAlign = ContentAlignment.MiddleRight,
            Font = Tema.Numeros(8.5F),
            ForeColor = Tema.TextoSecundario
        };

        public CtlFacturaAnulacion()
        {
            SuspendLayout();
            AutoScaleMode = AutoScaleMode.None;
            Dock = DockStyle.Fill;
            Margin = new Padding(0);

            _errores.ContainerControl = this;

            _txtMotivo.Validating += (_, _) =>
            {
                if (FacturaActual is { Anulada: false })
                    _errores.SetError(_txtMotivo,
                        string.IsNullOrWhiteSpace(_txtMotivo.Text) ? "El motivo de anulación es obligatorio." : string.Empty);
            };

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
            raiz.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            raiz.Controls.Add(CrearTarjetaDatos(), 0, 0);
            raiz.Controls.Add(CrearTarjetaMotivo(), 0, 1);

            Controls.Add(raiz);
            ResumeLayout(true);
        }

        public event Action<Factura?>? FacturaCargada;
        public event EventHandler? BusquedaEnfocada;
        public event EventHandler? MotivoEnfocado;

        public Button BotonBuscar => _btnBuscar;
        public Factura? FacturaActual { get; private set; }
        public string Motivo => _txtMotivo.Text.Trim();

        public void EnfocarBusqueda()
        {
            _txtBuscarNumero.Focus();
            _txtBuscarNumero.SelectAll();
        }

        public void Cargar(Factura f)
        {
            FacturaActual = f;
            _errores.Clear();

            _txtBuscarNumero.Text = f.Numero;
            _txtNumero.Text = f.Numero;
            _txtCliente.Text = f.ClienteDescripcion;
            _txtRuc.Text = string.IsNullOrWhiteSpace(f.ClienteRucCedula) ? "—" : f.ClienteRucCedula;
            _txtEmision.Text = f.FechaEmision.ToString("yyyy-MM-dd");
            _txtVencimiento.Text = f.FechaVencimiento.ToString("yyyy-MM-dd");
            _txtSubtotal.Text = f.Subtotal.ToString("N2", Cultura);
            _txtItbms.Text = f.Itbms.ToString("N2", Cultura);
            _txtTotal.Text = f.Total.ToString("N2", Cultura);
            _txtSaldo.Text = f.Saldo.ToString("N2", Cultura);

            _lblEstado.ForeColor = f.Anulada || f.EstaVencida ? Tema.Secundario : Tema.Primario;
            _lblEstado.Text = f.EstaVencida ? $"{f.Estado} · Vencida ({f.DiasVencidos} d)" : f.Estado;

            _txtMotivo.Text = f.Anulada ? (f.MotivoAnulacion ?? string.Empty) : string.Empty;
            _txtMotivo.ReadOnly = f.Anulada;

            if (f.Anulada)
                _tarjetaDatos.MostrarNota("Ya se encuentra anulada", esError: true);
            else if (f.TotalPagado > 0)
                _tarjetaDatos.MostrarNota("Tiene pagos activos; anúlelos primero", esError: true);
            else
                _tarjetaDatos.MostrarNota("Lista para anular", esError: false);

            FacturaCargada?.Invoke(f);

            if (!f.Anulada && f.TotalPagado == 0)
                _txtMotivo.Focus();
        }

        public void Limpiar()
        {
            FacturaActual = null;
            _errores.Clear();
            _txtBuscarNumero.Clear();
            _txtNumero.Clear();
            _txtCliente.Clear();
            _txtRuc.Clear();
            _txtEmision.Clear();
            _txtVencimiento.Clear();
            _txtSubtotal.Clear();
            _txtItbms.Clear();
            _txtTotal.Clear();
            _txtSaldo.Clear();
            _lblEstado.Text = "—";
            _txtMotivo.Clear();
            _txtMotivo.ReadOnly = false;
            _tarjetaDatos.MostrarNota(string.Empty);
            FacturaCargada?.Invoke(null);
        }

        public void MarcarErrorMotivo(string mensaje)
        {
            _errores.SetError(_txtMotivo, mensaje);
            _txtMotivo.Focus();
        }

        // construcción

        private Control CrearTarjetaDatos()
        {
            _tarjetaDatos.Margin = new Padding(0, 0, 0, 10);

            var barra = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                Margin = new Padding(0, 6, 0, 6)
            };

            var lbl = new Label
            {
                Text = "Número de factura:",
                ForeColor = Tema.TextoSecundario,
                AutoSize = true,
                Margin = new Padding(0, 6, 8, 0)
            };

            _txtBuscarNumero.Width = LogicalToDeviceUnits(200);
            _txtBuscarNumero.BorderStyle = BorderStyle.FixedSingle;
            _txtBuscarNumero.PlaceholderText = "Ej. F-001";
            _txtBuscarNumero.Margin = new Padding(0, 3, 8, 3);
            _txtBuscarNumero.TabIndex = 0;
            _txtBuscarNumero.Enter += (_, _) => BusquedaEnfocada?.Invoke(this, EventArgs.Empty);

            _btnBuscar.TabIndex = 1;
            _btnBuscar.Click += (_, _) => BuscarPorNumero();

            barra.Controls.AddRange([lbl, _txtBuscarNumero, _btnBuscar]);

            _txtSaldo.ForeColor = Tema.Secundario;
            _txtSaldo.Font = Tema.Negrita(9F);
            _lblEstado.Margin = new Padding(0, 4, 0, 0);

            var rejilla = CampoFicha.CrearRejilla(5);
            CampoFicha.Agregar(rejilla, "Número", _txtNumero, 0, 0);
            CampoFicha.Agregar(rejilla, "Subtotal (B/.)", _txtSubtotal, 0, 1);
            CampoFicha.Agregar(rejilla, "Cliente", _txtCliente, 1, 0);
            CampoFicha.Agregar(rejilla, "ITBMS (B/.)", _txtItbms, 1, 1);
            CampoFicha.Agregar(rejilla, "Cédula / RUC", _txtRuc, 2, 0);
            CampoFicha.Agregar(rejilla, "Monto total (B/.)", _txtTotal, 2, 1);
            CampoFicha.Agregar(rejilla, "Fecha emisión", _txtEmision, 3, 0);
            CampoFicha.Agregar(rejilla, "Saldo pendiente (B/.)", _txtSaldo, 3, 1);
            CampoFicha.Agregar(rejilla, "Fecha vencimiento", _txtVencimiento, 4, 0);
            CampoFicha.Agregar(rejilla, "Estado actual", _lblEstado, 4, 1);

            _tarjetaDatos.Cuerpo.RowCount = 2;
            _tarjetaDatos.Cuerpo.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            _tarjetaDatos.Cuerpo.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            _tarjetaDatos.Cuerpo.Controls.Add(barra, 0, 0);
            _tarjetaDatos.Cuerpo.Controls.Add(rejilla, 0, 1);
            return _tarjetaDatos;
        }

        private Control CrearTarjetaMotivo()
        {
            _tarjetaMotivo.MostrarNota("* obligatorio");
            _txtMotivo.Dock = DockStyle.Top;
            _txtMotivo.Height = LogicalToDeviceUnits(74);
            _txtMotivo.Margin = new Padding(0, 6, 0, 4);
            _txtMotivo.TabIndex = 2;
            _txtMotivo.TextChanged += (_, _) =>
                _lblContador.Text = $"{_txtMotivo.Text.Length} / 250 caracteres";
            _txtMotivo.Enter += (_, _) => MotivoEnfocado?.Invoke(this, EventArgs.Empty);

            _lblContador.Dock = DockStyle.Top;

            _tarjetaMotivo.Cuerpo.RowCount = 2;
            _tarjetaMotivo.Cuerpo.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            _tarjetaMotivo.Cuerpo.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            _tarjetaMotivo.Cuerpo.Controls.Add(_txtMotivo, 0, 0);
            _tarjetaMotivo.Cuerpo.Controls.Add(_lblContador, 0, 1);
            return _tarjetaMotivo;
        }

        private void BuscarPorNumero()
        {
            _errores.Clear();
            var numero = _txtBuscarNumero.Text.Trim();
            if (numero.Length == 0)
            {
                _errores.SetError(_txtBuscarNumero, "Ingrese el número de factura.");
                _txtBuscarNumero.Focus();
                return;
            }

            try
            {
                var factura = _negocio.ObtenerPorNumero(numero);
                if (factura is null)
                {
                    Limpiar();
                    _txtBuscarNumero.Text = numero;
                    _tarjetaDatos.MostrarNota($"No existe la factura '{numero}'", esError: true);
                    _txtBuscarNumero.Focus();
                    return;
                }
                Cargar(factura);
            }
            catch (DatosException ex)
            {
                _tarjetaDatos.MostrarNota(ex.Message, esError: true);
            }
        }

        private static TextBox CrearSoloLectura(bool derecha = false) => new()
        {
            ReadOnly = true,
            TabStop = false,
            BorderStyle = BorderStyle.FixedSingle,
            BackColor = Tema.Contenedor,
            ForeColor = Tema.TextoSecundario,
            TextAlign = derecha ? HorizontalAlignment.Right : HorizontalAlignment.Left,
            Font = derecha ? Tema.Numeros(9F) : Tema.Normal(9F)
        };

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _errores.Dispose();
            base.Dispose(disposing);
        }
    }
}