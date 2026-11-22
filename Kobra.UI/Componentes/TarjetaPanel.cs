namespace Kobra.UI
{
    /// <summary>
    /// Tarjeta blanca con título subrayado y una nota opcional a la derecha.
    /// El contenido se agrega en <see cref="Cuerpo"/>.
    /// </summary>
    internal sealed class TarjetaPanel : TableLayoutPanel
    {
        private readonly Label _lblNota = new();

        public TarjetaPanel(string titulo)
        {
            Dock = DockStyle.Fill;
            AutoSize = true;
            ColumnCount = 1;
            RowCount = 2;
            BackColor = Tema.Blanco;
            Padding = new Padding(10);
            Margin = new Padding(0);
            ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            RowStyles.Add(new RowStyle(SizeType.AutoSize));
            RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            var cabecera = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                ColumnCount = 2,
                RowCount = 1,
                BackColor = Color.Transparent,
                Margin = new Padding(0),
                Padding = new Padding(0, 0, 0, 4)
            };
            cabecera.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            cabecera.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            cabecera.Paint += (_, e) =>
            {
                using var lapiz = new Pen(Tema.ContenedorAlto);
                e.Graphics.DrawLine(lapiz, 0, cabecera.Height - 1, cabecera.Width, cabecera.Height - 1);
            };

            cabecera.Controls.Add(new Label
            {
                Text = titulo,
                Font = Tema.Negrita(9.5F),
                ForeColor = Tema.Primario,
                AutoSize = true,
                Margin = new Padding(0)
            }, 0, 0);

            _lblNota.Font = Tema.Numeros(8.5F);
            _lblNota.ForeColor = Tema.TextoSecundario;
            _lblNota.AutoSize = true;
            _lblNota.Anchor = AnchorStyles.Right;
            _lblNota.Margin = new Padding(0);
            cabecera.Controls.Add(_lblNota, 1, 0);

            Cuerpo = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                ColumnCount = 1,
                BackColor = Color.Transparent,
                Margin = new Padding(0)
            };
            Cuerpo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            Controls.Add(cabecera, 0, 0);
            Controls.Add(Cuerpo, 0, 1);
        }

        /// <summary>Panel interior (una columna) donde se agrega el contenido de la tarjeta.</summary>
        public TableLayoutPanel Cuerpo { get; }

        /// <summary>Texto corto a la derecha del título (conteos, avisos). En rojo si es un error.</summary>
        public void MostrarNota(string texto, bool esError = false)
        {
            _lblNota.ForeColor = esError ? Tema.Secundario : Tema.TextoSecundario;
            _lblNota.Text = texto;
        }
    }
}
