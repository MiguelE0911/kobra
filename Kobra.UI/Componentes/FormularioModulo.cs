using Kobra.Negocio;

namespace Kobra.UI
{
    /// <summary>
    /// Base de los formularios de módulo (Clientes, Facturas, Pagos): tema, atajos de teclado comunes,
    /// y mensajes de error y confirmación con el mismo aspecto. Cada módulo decide qué hace cada atajo.
    /// </summary>
    public class FormularioModulo : Form
    {
        /// <summary>Márgenes del cuerpo del módulo, iguales a los de las barras de título y de acciones.</summary>
        internal const int MargenCuerpo = 12;

        public FormularioModulo()
        {
            // Los componentes ya dimensionan en píxeles reales con LogicalToDeviceUnits; si el formulario
            // escalara además por fuente, los tamaños se aplicarían dos veces.
            AutoScaleMode = AutoScaleMode.None;
            Font = Tema.Normal();
            BackColor = Tema.Superficie;
            KeyPreview = true;
        }

        /// <summary>Título de ventana ("Kobra · Clientes") y tamaño mínimo comunes a todos los módulos.</summary>
        protected void ConfigurarVentana(string modulo)
        {
            Text = $"Kobra · {modulo}";
            MinimumSize = new Size(LogicalToDeviceUnits(1020), LogicalToDeviceUnits(680));
            if (ClientSize.Width < MinimumSize.Width || ClientSize.Height < MinimumSize.Height)
                ClientSize = new Size(LogicalToDeviceUnits(1120), LogicalToDeviceUnits(740));
        }

        /// <summary>
        /// Atajos del módulo (F2 buscar, F5 actualizar, Ctrl+S guardar, Ctrl+N nuevo...).
        /// Devuelve true si la tecla se atendió, para que no llegue al control enfocado.
        /// </summary>
        protected virtual bool ProcesarAtajo(Keys tecla) => false;

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (!e.Handled && ProcesarAtajo(e.KeyData))
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        /// <summary>Muestra un error de negocio (aviso) o de datos (error) con el título del módulo.</summary>
        protected void MostrarError(Exception ex)
        {
            var icono = ex is ReglaNegocioException ? MessageBoxIcon.Warning : MessageBoxIcon.Error;
            MessageBox.Show(this, ex.Message, Text, MessageBoxButtons.OK, icono);
        }

        protected void MostrarAdvertencia(string mensaje) =>
            MessageBox.Show(this, mensaje, Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);

        /// <summary>Confirmación temática con "No" como opción por defecto.</summary>
        protected bool Confirmar(string pregunta, string? detalle = null, string? advertencia = null,
            bool peligro = false, string textoSi = "&Sí") =>
            DialogoConfirmacion.Confirmar(this, Text, pregunta, detalle, advertencia, peligro, textoSi);
    }
}
