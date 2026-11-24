namespace Kobra.UI
{
    /// <summary>Franja azul con el título del módulo a la izquierda y datos de contexto (modo, ID) a la derecha.</summary>
    internal sealed class BarraTituloModulo : TableLayoutPanel
    {
        private readonly Label _titulo = new();
        private readonly Label _info = new();

        public BarraTituloModulo(string titulo)
        {
            Dock = DockStyle.Top;
            AutoSize = true;
            ColumnCount = 2;
            RowCount = 1;
            BackColor = Tema.PrimarioContenedor;
            Padding = new Padding(12, 7, 12, 7);
            Margin = new Padding(0);
            ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            _titulo.Text = titulo;
            _titulo.Font = Tema.Negrita(10F);
            _titulo.ForeColor = Tema.SobrePrimario;
            _titulo.AutoSize = true;
            _titulo.Margin = new Padding(0);

            _info.Font = Tema.Numeros(8.5F);
            _info.ForeColor = Tema.SobrePrimarioContenedor;
            _info.AutoSize = true;
            _info.Anchor = AnchorStyles.Right;
            _info.Margin = new Padding(0);

            Controls.Add(_titulo, 0, 0);
            Controls.Add(_info, 1, 0);
        }

        [System.ComponentModel.Browsable(false)]
        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public string Info
        {
            get => _info.Text;
            set => _info.Text = value;
        }
    }
}
