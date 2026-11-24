namespace Kobra.UI
{
    /// <summary>Textos que se repiten en varias ventanas.</summary>
    internal static class Mensajes
    {
        public const string MotivoObligatorio = "El motivo de anulación es obligatorio.";
        public const string MotivoFaltante = "Debe indicar el motivo de anulación.";

        /// <summary>Pista de teclas de la barra de acciones; <paramref name="nuevo"/> concuerda con el género del módulo.</summary>
        public static string PistaAtajos(string nuevo) =>
            $"F2 buscar · F5 actualizar · Ctrl+N {nuevo} · Ctrl+S guardar · Esc cerrar";
    }
}
