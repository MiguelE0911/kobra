namespace Kobra.UI
{
    /// <summary>
    /// Base de los módulos que alternan vistas con pestañas (Facturas, Pagos): arma la barra de pestañas,
    /// el cuerpo con los márgenes comunes y la disposición de las barras.
    /// </summary>
    public class FormularioConVistas : FormularioModulo
    {
        private BotonTema[] _pestanas = [];

        /// <summary>
        /// Coloca título, pestañas, vistas y barra de acciones. Las vistas se alternan por visibilidad.
        /// </summary>
        private protected void ArmarMarco(BarraTituloModulo titulo, BarraAcciones acciones, BotonTema cerrar,
            BotonTema[] pestanas, params Control[] vistas)
        {
            _pestanas = pestanas;

            var barraVistas = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                BackColor = Tema.ContenedorBajo,
                Padding = new Padding(MargenCuerpo, 6, MargenCuerpo, 6),
                Margin = new Padding(0)
            };
            foreach (var pestana in pestanas)
            {
                pestana.Margin = new Padding(0, 0, 6, 0);
                barraVistas.Controls.Add(pestana);
            }

            var cuerpo = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(MargenCuerpo),
                BackColor = Color.Transparent
            };
            cuerpo.Controls.AddRange(vistas);

            Controls.Add(cuerpo);
            Controls.Add(barraVistas);
            Controls.Add(titulo);
            Controls.Add(acciones);

            CancelButton = cerrar;
        }

        /// <summary>Resalta la pestaña de la vista activa; <paramref name="activa"/> es su posición.</summary>
        private protected void MarcarPestana(int activa)
        {
            for (var i = 0; i < _pestanas.Length; i++)
                _pestanas[i].Seleccionado = i == activa;
        }
    }
}
