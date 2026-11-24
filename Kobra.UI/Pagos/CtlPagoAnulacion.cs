using Kobra.Datos;
using Kobra.Entidades;
using Kobra.Negocio;

namespace Kobra.UI
{
    /// <summary>
    /// Ficha de anulación lógica de pago: busca el recibo, muestra su impacto en el saldo de la
    /// factura y exige un motivo de hasta 250 caracteres.
    /// </summary>
    internal sealed class CtlPagoAnulacion : UserControl
    {
        private readonly PagoNegocio _negocio = new();
        private readonly ErrorProvider _errores = new() { BlinkStyle = ErrorBlinkStyle.NeverBlink };

        private readonly TarjetaPanel _tarjetaDatos = new("DATOS DEL PAGO A ANULAR");
        private readonly TextBox _txtBuscar = new() { MaxLength = 150 };
        private readonly BotonTema _btnBuscar = new("&Buscar", EstiloBoton.Primario);

        private readonly TextBox _txtNumero = CampoFicha.CrearSoloLectura();
        private readonly TextBox _txtFactura = CampoFicha.CrearSoloLectura();
        private readonly TextBox _txtCliente = CampoFicha.CrearSoloLectura();
        private readonly TextBox _txtFecha = CampoFicha.CrearSoloLectura();
        private readonly TextBox _txtMonto = CampoFicha.CrearSoloLectura(derecha: true);
        private readonly TextBox _txtSaldoActual = CampoFicha.CrearSoloLectura(derecha: true);
        private readonly TextBox _txtSaldoFinal = CampoFicha.CrearSoloLectura(derecha: true);
        private readonly Label _lblEstado = new() { AutoSize = true, Font = Tema.Negrita(9F) };

        private readonly TarjetaMotivo _motivo = new(tabIndex: 2);

        public CtlPagoAnulacion()
        {
            SuspendLayout();
            AutoScaleMode = AutoScaleMode.None;
            Dock = DockStyle.Fill;
            Margin = new Padding(0);

            _errores.ContainerControl = this;
            _motivo.Enfocado += (_, _) => MotivoEnfocado?.Invoke(this, EventArgs.Empty);

            _motivo.Caja.Validating += (_, _) =>
            {
                if (PagoActual is { Anulado: false })
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

        public event Action<Pago?>? PagoCargado;
        public event EventHandler? BusquedaEnfocada;
        public event EventHandler? MotivoEnfocado;

        public Button BotonBuscar => _btnBuscar;
        public Pago? PagoActual { get; private set; }
        public string Motivo => _motivo.Caja.Text.Trim();

        public void EnfocarBusqueda()
        {
            _txtBuscar.Focus();
            _txtBuscar.SelectAll();
        }

        public void Cargar(Pago pago)
        {
            PagoActual = pago;
            _errores.Clear();

            _txtBuscar.Text = pago.Id.ToString(Formato.Cultura);
            _txtNumero.Text = pago.Id.ToString(Formato.Cultura);
            _txtFactura.Text = pago.NumeroFactura;
            _txtCliente.Text = pago.ClienteDescripcion;
            _txtFecha.Text = pago.FechaPago.ToString(Formato.Fecha);
            _txtMonto.Text = pago.Monto.Importe();
            _txtSaldoActual.Text = pago.SaldoFactura.Importe();
            _txtSaldoFinal.Text = pago.Anulado
                ? pago.SaldoFactura.Importe()
                : (pago.SaldoFactura + pago.Monto).Importe();

            _lblEstado.ForeColor = pago.Anulado || pago.FacturaAnulada ? Tema.Secundario : Tema.Primario;
            _lblEstado.Text = pago.Anulado ? "Anulado" : pago.FacturaAnulada ? "Factura anulada" : "Activo";

            _motivo.Caja.Text = pago.Anulado ? (pago.MotivoAnulacion ?? string.Empty) : string.Empty;
            _motivo.Caja.ReadOnly = pago.Anulado;

            if (pago.Anulado)
                _tarjetaDatos.MostrarNota("Este pago ya está anulado", esError: true);
            else if (pago.FacturaAnulada)
                _tarjetaDatos.MostrarNota("La factura está anulada", esError: true);
            else
                _tarjetaDatos.MostrarNota("El saldo de la factura aumentará al anular", esError: false);

            PagoCargado?.Invoke(pago);

            if (!pago.Anulado)
                _motivo.Caja.Focus();
        }

        public void Limpiar()
        {
            PagoActual = null;
            _errores.Clear();
            _txtBuscar.Clear();
            _txtNumero.Clear();
            _txtFactura.Clear();
            _txtCliente.Clear();
            _txtFecha.Clear();
            _txtMonto.Clear();
            _txtSaldoActual.Clear();
            _txtSaldoFinal.Clear();
            _lblEstado.Text = "—";
            _motivo.Caja.Clear();
            _motivo.Caja.ReadOnly = false;
            _tarjetaDatos.MostrarNota(string.Empty);
            PagoCargado?.Invoke(null);
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
            barra.Controls.Add(new Label
            {
                Text = "N.º de pago o factura:",
                ForeColor = Tema.TextoSecundario,
                AutoSize = true,
                Margin = new Padding(0, 6, 8, 0)
            });

            _txtBuscar.Width = LogicalToDeviceUnits(220);
            _txtBuscar.BorderStyle = BorderStyle.FixedSingle;
            _txtBuscar.PlaceholderText = "Ej. 12 o F-001";
            _txtBuscar.Margin = new Padding(0, 3, 8, 3);
            _txtBuscar.TabIndex = 0;
            _txtBuscar.Enter += (_, _) => BusquedaEnfocada?.Invoke(this, EventArgs.Empty);

            _btnBuscar.TabIndex = 1;
            _btnBuscar.Click += (_, _) => BuscarPago();
            barra.Controls.AddRange([_txtBuscar, _btnBuscar]);

            _txtSaldoFinal.ForeColor = Tema.Secundario;
            _txtSaldoFinal.Font = Tema.Negrita(9F);
            _lblEstado.Margin = new Padding(0, 4, 0, 0);

            var rejilla = CampoFicha.CrearRejilla(4);
            CampoFicha.Agregar(rejilla, "Número de pago", _txtNumero, 0, 0);
            CampoFicha.Agregar(rejilla, "Saldo actual (B/.)", _txtSaldoActual, 0, 1);
            CampoFicha.Agregar(rejilla, "Factura", _txtFactura, 1, 0);
            CampoFicha.Agregar(rejilla, "Saldo al anular (B/.)", _txtSaldoFinal, 1, 1);
            CampoFicha.Agregar(rejilla, "Cliente", _txtCliente, 2, 0);
            CampoFicha.Agregar(rejilla, "Estado del pago", _lblEstado, 2, 1);
            CampoFicha.Agregar(rejilla, "Fecha de pago", _txtFecha, 3, 0);
            CampoFicha.Agregar(rejilla, "Monto (B/.)", _txtMonto, 3, 1);

            _tarjetaDatos.Cuerpo.RowCount = 2;
            _tarjetaDatos.Cuerpo.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            _tarjetaDatos.Cuerpo.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            _tarjetaDatos.Cuerpo.Controls.Add(barra, 0, 0);
            _tarjetaDatos.Cuerpo.Controls.Add(rejilla, 0, 1);
            return _tarjetaDatos;
        }

        private void BuscarPago()
        {
            _errores.Clear();
            var texto = _txtBuscar.Text.Trim();
            if (texto.Length == 0)
            {
                _errores.SetError(_txtBuscar, "Ingrese el número de pago o de factura.");
                _txtBuscar.Focus();
                return;
            }

            try
            {
                Pago? pago = int.TryParse(texto, out var id)
                    ? _negocio.ObtenerPorId(id)
                    : _negocio.Buscar(texto).FirstOrDefault();

                if (pago is null)
                {
                    Limpiar();
                    _txtBuscar.Text = texto;
                    _tarjetaDatos.MostrarNota($"No existe el pago '{texto}'", esError: true);
                    _txtBuscar.Focus();
                    return;
                }

                Cargar(pago);
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
