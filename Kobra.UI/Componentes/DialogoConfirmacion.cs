namespace Kobra.UI
{
    /// <summary>
    /// Confirmación Sí/No con el tema de la aplicación. El foco y Enter quedan en "No": una acción
    /// destructiva nunca se confirma por accidente.
    /// </summary>
    internal sealed class DialogoConfirmacion : Form
    {
        private readonly BotonTema _btnNo = new("&No");

        private DialogoConfirmacion(string titulo, string pregunta, string? detalle, string? advertencia,
            bool peligro, string textoSi)
        {
            AutoScaleMode = AutoScaleMode.None;
            Font = Tema.Normal();
            BackColor = Tema.Blanco;
            Text = titulo;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;

            var ancho = LogicalToDeviceUnits(380);

            var raiz = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                ColumnCount = 1,
                RowCount = 2,
                Margin = new Padding(0)
            };

            var cuerpo = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                ColumnCount = 2,
                RowCount = 1,
                Padding = new Padding(16, 16, 16, 12),
                Margin = new Padding(0)
            };
            var icono = new Label
            {
                Text = peligro ? "!" : "?",
                Font = Tema.Negrita(18F),
                ForeColor = peligro ? Tema.Secundario : Tema.Primario,
                BackColor = peligro ? Tema.ErrorContenedor : Tema.Contenedor,
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(LogicalToDeviceUnits(44), LogicalToDeviceUnits(44)),
                Margin = new Padding(0, 0, 14, 0)
            };

            var texto = new FlowLayoutPanel
            {
                AutoSize = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Margin = new Padding(0)
            };
            texto.Controls.Add(CrearTexto(pregunta, Tema.Negrita(10F), Tema.Texto, ancho, 0));
            if (!string.IsNullOrWhiteSpace(detalle))
                texto.Controls.Add(CrearTexto(detalle, Tema.Normal(9F), Tema.TextoSecundario, ancho, 6));
            if (!string.IsNullOrWhiteSpace(advertencia))
                texto.Controls.Add(CrearTexto(advertencia, Tema.Normal(8.5F), Tema.Secundario, ancho, 8));

            cuerpo.Controls.Add(icono, 0, 0);
            cuerpo.Controls.Add(texto, 1, 0);

            var btnSi = new BotonTema(textoSi, peligro ? EstiloBoton.Peligro : EstiloBoton.Primario)
            {
                DialogResult = DialogResult.Yes
            };
            _btnNo.DialogResult = DialogResult.No;

            var pie = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                FlowDirection = FlowDirection.RightToLeft,
                BackColor = Tema.ContenedorBajo,
                Padding = new Padding(12, 8, 12, 8),
                Margin = new Padding(0)
            };
            // Con RightToLeft el primero que se agrega queda a la derecha: "No" a la derecha, "Sí" a su izquierda.
            _btnNo.Margin = new Padding(6, 0, 0, 0);
            pie.Controls.Add(_btnNo);
            pie.Controls.Add(btnSi);

            raiz.Controls.Add(cuerpo, 0, 0);
            raiz.Controls.Add(pie, 0, 1);
            Controls.Add(raiz);

            AcceptButton = _btnNo;
            CancelButton = _btnNo;
            Shown += (_, _) => _btnNo.Focus();
        }

        private static Label CrearTexto(string texto, Font fuente, Color color, int anchoMaximo, int margenSuperior) =>
            new()
            {
                Text = texto,
                Font = fuente,
                ForeColor = color,
                AutoSize = true,
                MaximumSize = new Size(anchoMaximo, 0),
                Margin = new Padding(0, margenSuperior, 0, 0)
            };

        /// <summary>Devuelve true solo si el usuario elige "Sí".</summary>
        public static bool Confirmar(IWin32Window? propietario, string titulo, string pregunta, string? detalle = null,
            string? advertencia = null, bool peligro = false, string textoSi = "&Sí")
        {
            using var dialogo = new DialogoConfirmacion(titulo, pregunta, detalle, advertencia, peligro, textoSi);
            return dialogo.ShowDialog(propietario) == DialogResult.Yes;
        }
    }
}
