namespace Kobra.UI
{
    /// <summary>
    /// Tarjeta del panel de inicio que abre un módulo. Se puede usar con ratón o con teclado
    /// (Tab para llegar, Enter o Espacio para abrir) y muestra el foco con un borde claro.
    /// </summary>
    internal sealed class TarjetaModulo : Panel
    {
        private readonly Label _titulo = new();
        private readonly Label _descripcion = new();
        private readonly Label _acceso = new();
        private readonly Label _abrir = new();

        public TarjetaModulo(string titulo, string descripcion, string acceso)
        {
            SetStyle(ControlStyles.Selectable | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
            TabStop = true;
            Cursor = Cursors.Hand;
            BackColor = Tema.PrimarioContenedor;
            Dock = DockStyle.Fill;
            Margin = new Padding(0, 0, 8, 8);
            MinimumSize = new Size(0, 112);

            var diseno = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 3,
                BackColor = Color.Transparent,
                Padding = new Padding(10, 8, 10, 6)
            };
            diseno.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            diseno.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            diseno.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            diseno.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            diseno.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            Configurar(_titulo, titulo, Tema.Negrita(10.5F), Tema.SobrePrimario);
            Configurar(_descripcion, descripcion, Tema.Normal(8.5F), Tema.ContenedorBajo);
            _descripcion.AutoSize = false;   // sin AutoSize el texto largo se ajusta al ancho de la tarjeta
            Configurar(_acceso, acceso, Tema.Numeros(8.5F), Tema.ContenedorBajo);
            Configurar(_abrir, "Abrir formulario →", Tema.Negrita(8.5F), Tema.SobrePrimario);
            _abrir.Font = Tema.Negrita(8.5F, FontStyle.Underline);
            _abrir.TextAlign = ContentAlignment.MiddleRight;

            diseno.Controls.Add(_titulo, 0, 0);
            diseno.SetColumnSpan(_titulo, 2);
            diseno.Controls.Add(_descripcion, 0, 1);
            diseno.SetColumnSpan(_descripcion, 2);
            diseno.Controls.Add(_acceso, 0, 2);
            diseno.Controls.Add(_abrir, 1, 2);
            Controls.Add(diseno);

            // Los rótulos son hijos: reenviamos clic y ratón a la tarjeta para que toda el área responda.
            foreach (Control c in new Control[] { diseno, _titulo, _descripcion, _acceso, _abrir })
            {
                c.Click += (_, _) => Abrir();
                c.MouseEnter += (_, _) => Resaltar(true);
                c.MouseLeave += (_, _) => Resaltar(false);
                c.Cursor = Cursors.Hand;
            }
        }

        /// <summary>Se dispara al hacer clic, o con Enter o Espacio con la tarjeta enfocada.</summary>
        public event EventHandler? Abierta;

        private void Abrir()
        {
            Focus();
            Abierta?.Invoke(this, EventArgs.Empty);
        }

        private static void Configurar(Label l, string texto, Font fuente, Color color)
        {
            l.Text = texto;
            l.Font = fuente;
            l.ForeColor = color;
            l.BackColor = Color.Transparent;
            l.AutoSize = true;
            l.Dock = DockStyle.Fill;
            l.Margin = new Padding(0, 0, 0, 2);
        }

        private void Resaltar(bool encima) =>
            BackColor = encima || Focused ? Tema.Primario : Tema.PrimarioContenedor;

        protected override bool IsInputKey(Keys keyData) =>
            keyData is Keys.Enter or Keys.Space || base.IsInputKey(keyData);

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.KeyCode is Keys.Enter or Keys.Space)
            {
                e.Handled = true;
                Abierta?.Invoke(this, EventArgs.Empty);
            }
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            Resaltar(false);
            Invalidate();
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            Resaltar(false);
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            using (var pincel = new SolidBrush(BackColor))
                e.Graphics.FillRectangle(pincel, ClientRectangle);

            if (Focused)
            {
                using var lapiz = new Pen(Tema.SobrePrimarioContenedor, 2);
                e.Graphics.DrawRectangle(lapiz, 1, 1, Width - 3, Height - 3);
            }
        }
    }
}
