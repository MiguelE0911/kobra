namespace Kobra.UI
{
    /// <summary>
    /// Rejilla de fichas: dos pares "etiqueta + control" por fila (4 columnas). La etiqueta va alineada a la
    /// derecha, pegada a su control, y los controles se estiran para aprovechar el ancho.
    /// </summary>
    internal static class CampoFicha
    {
        /// <summary>Caja de texto de solo lectura con el fondo de los datos calculados; los importes van a la derecha.</summary>
        public static TextBox CrearSoloLectura(bool derecha = false) => new()
        {
            ReadOnly = true,
            TabStop = false,
            BorderStyle = BorderStyle.FixedSingle,
            BackColor = Tema.Contenedor,
            ForeColor = Tema.TextoSecundario,
            TextAlign = derecha ? HorizontalAlignment.Right : HorizontalAlignment.Left,
            Font = derecha ? Tema.Numeros(9F) : Tema.Normal(9F)
        };

        /// <summary>Etiqueta de una fila de filtros ("Buscar (F2):", "Cliente:").</summary>
        public static Label CrearEtiquetaFiltro(string texto) => new()
        {
            Text = texto,
            ForeColor = Tema.TextoSecundario,
            AutoSize = true,
            Anchor = AnchorStyles.Left,
            Margin = new Padding(0, 0, 6, 0)
        };

        public static TableLayoutPanel CrearRejilla(int filas)
        {
            var t = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                ColumnCount = 4,
                RowCount = filas,
                BackColor = Color.Transparent,
                Margin = new Padding(0, 8, 0, 0)
            };
            t.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            t.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            t.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            t.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            for (var i = 0; i < filas; i++)
                t.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            return t;
        }

        /// <summary>
        /// Agrega un campo. <paramref name="par"/> es 0 (izquierda) o 1 (derecha); <paramref name="filasOcupadas"/>
        /// permite que un control alto, como una dirección, ocupe varias filas.
        /// </summary>
        public static void Agregar(TableLayoutPanel t, string texto, Control control, int fila, int par,
            bool obligatorio = false, int filasOcupadas = 1)
        {
            var etiqueta = new Label
            {
                Text = obligatorio ? texto + " *" : texto,
                ForeColor = Tema.TextoSecundario,
                AutoSize = true,
                Dock = DockStyle.Fill,
                TextAlign = filasOcupadas > 1 ? ContentAlignment.TopRight : ContentAlignment.MiddleRight,
                // El margen izquierdo de la columna derecha deja sitio al icono de error de la columna izquierda.
                Margin = new Padding(par == 0 ? 0 : 30, 4, 8, 4)
            };

            if (control is TextBox caja)
                caja.BorderStyle = BorderStyle.FixedSingle;

            control.Dock = DockStyle.Fill;
            control.Margin = new Padding(0, 4, 0, 4);

            t.Controls.Add(etiqueta, par * 2, fila);
            t.Controls.Add(control, par * 2 + 1, fila);
            if (filasOcupadas > 1)
            {
                t.SetRowSpan(etiqueta, filasOcupadas);
                t.SetRowSpan(control, filasOcupadas);
            }
        }
    }
}
