namespace Kobra.UI
{
    /// <summary>
    /// Barra inferior del módulo: a la izquierda una pista de teclas (o el último mensaje de resultado)
    /// y a la derecha los botones de acción.
    /// </summary>
    internal sealed class BarraAcciones : TableLayoutPanel
    {
        private readonly Label _mensaje = new();
        private readonly FlowLayoutPanel _botones = new();
        private readonly string _pista;

        public BarraAcciones(string pista)
        {
            _pista = pista;
            Dock = DockStyle.Bottom;
            AutoSize = true;
            ColumnCount = 2;
            RowCount = 1;
            BackColor = Tema.ContenedorBajo;
            Padding = new Padding(12, 8, 12, 8);
            Margin = new Padding(0);
            ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            _mensaje.AutoSize = false;
            _mensaje.AutoEllipsis = true;
            _mensaje.Dock = DockStyle.Fill;
            _mensaje.TextAlign = ContentAlignment.MiddleLeft;
            _mensaje.Font = Tema.Normal(8.5F);
            _mensaje.Margin = new Padding(0, 0, 12, 0);

            _botones.AutoSize = true;
            _botones.WrapContents = false;
            _botones.FlowDirection = FlowDirection.LeftToRight;
            _botones.Anchor = AnchorStyles.Right;
            _botones.Margin = new Padding(0);

            Controls.Add(_mensaje, 0, 0);
            Controls.Add(_botones, 1, 0);
            RestablecerPista();
        }

        /// <summary>Agrega botones de izquierda a derecha, con separación entre ellos.</summary>
        public void Agregar(params Button[] botones)
        {
            foreach (var b in botones)
            {
                b.Margin = new Padding(_botones.Controls.Count == 0 ? 0 : 6, 0, 0, 0);
                _botones.Controls.Add(b);
            }
        }

        public void MostrarMensaje(string texto, bool esError)
        {
            _mensaje.ForeColor = esError ? Tema.Secundario : Tema.Primario;
            _mensaje.Text = texto;
        }

        public void RestablecerPista()
        {
            _mensaje.ForeColor = Tema.TextoSecundario;
            _mensaje.Text = _pista;
        }
    }
}
