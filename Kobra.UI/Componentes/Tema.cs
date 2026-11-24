using System.Drawing;

namespace Kobra.UI
{
    /// <summary>Paleta y fuentes del diseño "Steel Ledger" compartidas por todas las ventanas.</summary>
    internal static class Tema
    {
        public static readonly Color Superficie = ColorTranslator.FromHtml("#F8F9FF");
        public static readonly Color ContenedorBajo = ColorTranslator.FromHtml("#EFF4FF");
        public static readonly Color Contenedor = ColorTranslator.FromHtml("#E5EEFE");
        public static readonly Color ContenedorAlto = ColorTranslator.FromHtml("#E0E9F9");
        public static readonly Color Blanco = Color.White;
        /// <summary>Texto sobre fondos oscuros (barras, cabeceras de rejilla y botones rellenos).</summary>
        public static readonly Color SobrePrimario = Color.White;

        public static readonly Color Primario = ColorTranslator.FromHtml("#00375E");
        public static readonly Color PrimarioContenedor = ColorTranslator.FromHtml("#1F4E79");
        public static readonly Color SobrePrimarioContenedor = ColorTranslator.FromHtml("#95BFF1");

        public static readonly Color Secundario = ColorTranslator.FromHtml("#B12A37");
        public static readonly Color SecundarioContenedor = ColorTranslator.FromHtml("#FF656C");
        public static readonly Color TerciarioContenedor = ColorTranslator.FromHtml("#6A4300");

        public static readonly Color ErrorContenedor = ColorTranslator.FromHtml("#FFDAD6");
        public static readonly Color PeligroOscuro = ColorTranslator.FromHtml("#8F0E22");
        public static readonly Color TextoApagado = ColorTranslator.FromHtml("#72777F");

        public static readonly Color Texto = ColorTranslator.FromHtml("#131C27");
        public static readonly Color TextoSecundario = ColorTranslator.FromHtml("#42474F");
        public static readonly Color Borde = ColorTranslator.FromHtml("#C2C7D0");

        // Las fuentes se comparten: ningún control las libera, así que se crean una sola vez.
        private static readonly Dictionary<(string, float, FontStyle), Font> _fuentes = new();

        private static Font Fuente(string familia, float puntos, FontStyle estilo)
        {
            lock (_fuentes)
            {
                if (!_fuentes.TryGetValue((familia, puntos, estilo), out var fuente))
                    _fuentes[(familia, puntos, estilo)] = fuente = new Font(familia, puntos, estilo);
                return fuente;
            }
        }

        public static Font Normal(float puntos = 9F, FontStyle estilo = FontStyle.Regular) =>
            Fuente("Segoe UI", puntos, estilo);

        public static Font Negrita(float puntos = 9F, FontStyle extra = FontStyle.Regular) =>
            Fuente("Segoe UI", puntos, FontStyle.Bold | extra);

        public static Font Numeros(float puntos = 9F, FontStyle estilo = FontStyle.Regular) =>
            Fuente("Consolas", puntos, estilo);
    }
}
