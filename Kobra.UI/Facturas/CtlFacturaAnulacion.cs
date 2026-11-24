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
        private readonly FacturaNegocio _negocio = new();
        private readonly ErrorProvider _errores = new() { BlinkStyle = ErrorBlinkStyle.NeverBlink };

        private readonly TarjetaPanel _tarjetaDatos = new("DATOS DE LA FACTURA A ANULAR");
        private readonly TextBox _txtBuscarNumero = new() { MaxLength = 30, CharacterCasing = CharacterCasing.Upper };
        private readonly BotonTema _btnBuscar = new("&Buscar", EstiloBoton.Primario);

        private readonly TextBox _txtNumero = CampoFicha.CrearSoloLectura();
        private readonly TextBox _txtCliente = CampoFicha.CrearSoloLectura();
        private readonly TextBox _txtRuc = CampoFicha.CrearSoloLectura();
        private readonly TextBox _txtEmision = CampoFicha.CrearSoloLectura();
        private readonly TextBox _txtVencimiento = CampoFicha.CrearSoloLectura();
        private readonly TextBox _txtSubtotal = CampoFicha.CrearSoloLectura(derecha: true);
        private readonly TextBox _txtItbms = CampoFicha.CrearSoloLectura(derecha: true);
        private readonly TextBox _txtTotal = CampoFicha.CrearSoloLectura(derecha: true);
        private readonly TextBox _txtSaldo = CampoFicha.CrearSoloLectura(derecha: true);
        private readonly Label _lblEstado = new() { AutoSize = true, Font = Tema.Negrita(9F) };

        private readonly TarjetaMotivo _motivo = new(tabIndex: 2);

        public CtlFacturaAnulacion()
        {
            SuspendLayout();
            AutoScaleMode = AutoScaleMode.None;
            Dock = DockStyle.Fill;
            Margin = new Padding(0);

            _errores.ContainerControl = this;
            _motivo.Enfocado += (_, _) => MotivoEnfocado?.Invoke(this, EventArgs.Empty);

            _motivo.Caja.Validating += (_, _) =>
            {
                if (FacturaActual is { Anulada: false })
                    _errores.SetError(_motivo.Caja,
                        string.IsNullOrWhiteSpace(_motivo.Caja.Text) ? Mensajes.MotivoObligatorio : string.Empty);
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
            raiz.Controls.Add(_motivo.Tarjeta, 0, 1);

            Controls.Add(raiz);
            ResumeLayout(true);
        }

        public event Action<Factura?>? FacturaCargada;
        public event EventHandler? BusquedaEnfocada;
        public event EventHandler? MotivoEnfocado;

        public Button BotonBuscar => _btnBuscar;
        public Factura? FacturaActual { get; private set; }
        public string Motivo => _motivo.Caja.Text.Trim();

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
            _txtEmision.Text = f.FechaEmision.ToString(Formato.Fecha);
            _txtVencimiento.Text = f.FechaVencimiento.ToString(Formato.Fecha);
            _txtSubtotal.Text = f.Subtotal.Importe();
            _txtItbms.Text = f.Itbms.Importe();
            _txtTotal.Text = f.Total.Importe();
            _txtSaldo.Text = f.Saldo.Importe();

            _lblEstado.ForeColor = f.Anulada || f.EstaVencida ? Tema.Secundario : Tema.Primario;
            _lblEstado.Text = f.EstaVencida ? $"{f.Estado} · Vencida ({f.DiasVencidos} d)" : f.Estado;

            _motivo.Caja.Text = f.Anulada ? (f.MotivoAnulacion ?? string.Empty) : string.Empty;
            _motivo.Caja.ReadOnly = f.Anulada;

            if (f.Anulada)
                _tarjetaDatos.MostrarNota("Ya se encuentra anulada", esError: true);
            else if (f.TotalPagado > 0)
                _tarjetaDatos.MostrarNota("Tiene pagos activos; anúlelos primero", esError: true);
            else
                _tarjetaDatos.MostrarNota("Lista para anular", esError: false);

            FacturaCargada?.Invoke(f);

            if (!f.Anulada && f.TotalPagado == 0)
                _motivo.Caja.Focus();
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
            _motivo.Caja.Clear();
            _motivo.Caja.ReadOnly = false;
            _tarjetaDatos.MostrarNota(string.Empty);
            FacturaCargada?.Invoke(null);
        }

        public void MarcarErrorMotivo(string mensaje)
        {
            _errores.SetError(_motivo.Caja, mensaje);
            _motivo.Caja.Focus();
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _errores.Dispose();
            base.Dispose(disposing);
        }
    }
}