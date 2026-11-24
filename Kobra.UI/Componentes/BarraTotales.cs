namespace Kobra.UI
{
    /// <summary>Barra de resumen bajo una rejilla: totales en Consolas, alineados a la derecha.</summary>
    internal sealed class BarraTotales : Label
    {
        public BarraTotales()
        {
            Dock = DockStyle.Fill;
            AutoSize = false;
            Height = LogicalToDeviceUnits(24);
            TextAlign = ContentAlignment.MiddleRight;
            Font = Tema.Numeros(8.5F);
            ForeColor = Tema.Texto;
            BackColor = Tema.ContenedorBajo;
            Padding = new Padding(8, 0, 8, 0);
            Margin = new Padding(0, 4, 0, 0);
        }
    }
}
