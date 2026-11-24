using System.Globalization;

namespace Kobra.UI
{
    /// <summary>Formatos de montos y fechas compartidos por todas las ventanas.</summary>
    internal static class Formato
    {
        /// <summary>Formato de columna para importes (dos decimales, separador de miles).</summary>
        public const string Monto = "N2";

        public const string Porcentaje = "0.00'%'";
        public const string Fecha = "yyyy-MM-dd";
        public const string FechaHora = "yyyy-MM-dd HH:mm";

        public static CultureInfo Cultura => CultureInfo.CurrentCulture;

        /// <summary>Tope de una columna DECIMAL(18,2).</summary>
        public static readonly decimal MaximoMonto = new([276447231, 23027, 0, 131072]);

        /// <summary>Importe sin moneda: "1,234.50".</summary>
        public static string Importe(this decimal monto) => monto.ToString(Monto, Cultura);

        /// <summary>Importe con moneda: "B/. 1,234.50".</summary>
        public static string Dinero(this decimal monto) => "B/. " + monto.Importe();
    }
}
