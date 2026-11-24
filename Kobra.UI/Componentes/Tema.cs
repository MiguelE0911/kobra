using System.Drawing;

namespace Kobra.UI
{
    /// <summary>Paleta y fuentes del diseño "Steel Ledger" para el panel de inicio.</summary>
    internal static class Tema
    {
        public static readonly Color Superficie = ColorTranslator.FromHtml("#F8F9FF");
        public static readonly Color ContenedorBajo = ColorTranslator.FromHtml("#EFF4FF");
        public static readonly Color Contenedor = ColorTranslator.FromHtml("#E5EEFE");
        public static readonly Color ContenedorAlto = ColorTranslator.FromHtml("#E0E9F9");
        public static readonly Color Blanco = Color.White;

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

        public static Font Normal(float puntos = 9F) => new("Segoe UI", puntos, FontStyle.Regular);

        public static Font Negrita(float puntos = 9F) => new("Segoe UI", puntos, FontStyle.Bold);

        public static Font Numeros(float puntos = 9F, FontStyle estilo = FontStyle.Regular) =>
            new("Consolas", puntos, estilo);
    }
}
