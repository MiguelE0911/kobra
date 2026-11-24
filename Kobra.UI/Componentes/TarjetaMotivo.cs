namespace Kobra.UI
{
    /// <summary>Tarjeta "motivo de anulación": caja de texto obligatoria con contador de caracteres.</summary>
    internal sealed class TarjetaMotivo
    {
        public const int Maximo = 250;

        private readonly Label _contador = new()
        {
            TextAlign = ContentAlignment.MiddleRight,
            Font = Tema.Numeros(8.5F),
            ForeColor = Tema.TextoSecundario,
            Dock = DockStyle.Top
        };

        public TarjetaMotivo(int tabIndex)
        {
            Tarjeta = new TarjetaPanel("MOTIVO DE ANULACIÓN (OBLIGATORIO)");
            Caja = new TextBox
            {
                Multiline = true,
                MaxLength = Maximo,
                ScrollBars = ScrollBars.Vertical,
                BorderStyle = BorderStyle.FixedSingle,
                Dock = DockStyle.Top,
                Margin = new Padding(0, 6, 0, 4),
                TabIndex = tabIndex
            };
            Caja.Height = Caja.LogicalToDeviceUnits(74);
            Caja.TextChanged += (_, _) => ActualizarContador();
            Caja.Enter += (_, _) => Enfocado?.Invoke(this, EventArgs.Empty);
            ActualizarContador();

            Tarjeta.MostrarNota("* obligatorio");
            Tarjeta.Cuerpo.RowCount = 2;
            Tarjeta.Cuerpo.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            Tarjeta.Cuerpo.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            Tarjeta.Cuerpo.Controls.Add(Caja, 0, 0);
            Tarjeta.Cuerpo.Controls.Add(_contador, 0, 1);
        }

        public TarjetaPanel Tarjeta { get; }

        public TextBox Caja { get; }

        /// <summary>El usuario entró a la caja de texto (el formulario quita el botón por defecto).</summary>
        public event EventHandler? Enfocado;

        private void ActualizarContador() => _contador.Text = $"{Caja.Text.Length} / {Maximo} caracteres";
    }
}
