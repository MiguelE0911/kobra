namespace Kobra.UI
{
    internal enum EstiloBoton { Primario, Secundario, Peligro }

    /// <summary>Botón plano con la paleta del tema. El estilo indica la importancia de la acción.</summary>
    internal sealed class BotonTema : Button
    {
        private readonly EstiloBoton _estilo;
        private bool _seleccionado;

        public BotonTema(string texto, EstiloBoton estilo = EstiloBoton.Secundario)
        {
            _estilo = estilo;
            Text = texto;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            FlatStyle = FlatStyle.Flat;
            UseVisualStyleBackColor = false;
            Font = Tema.Normal(9F);
            Padding = new Padding(8, 2, 8, 2);
            Margin = new Padding(0);
            MinimumSize = new Size(LogicalToDeviceUnits(96), LogicalToDeviceUnits(30));
            Cursor = Cursors.Hand;
            AplicarColores();
        }

        /// <summary>Resalta el botón como pestaña activa (para los botones que hacen de pestañas).</summary>
        [System.ComponentModel.Browsable(false)]
        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public bool Seleccionado
        {
            get => _seleccionado;
            set
            {
                if (_seleccionado == value)
                    return;
                _seleccionado = value;
                AplicarColores();
            }
        }

        private void AplicarColores()
        {
            FlatAppearance.BorderSize = 1;

            if (!Enabled)
            {
                BackColor = Tema.ContenedorAlto;
                ForeColor = Tema.TextoApagado;
                FlatAppearance.BorderColor = Tema.ContenedorAlto;
                return;
            }

            if (_seleccionado)
            {
                BackColor = Tema.PrimarioContenedor;
                ForeColor = Tema.SobrePrimario;
                FlatAppearance.BorderColor = Tema.PrimarioContenedor;
                FlatAppearance.MouseOverBackColor = ControlPaint.Dark(Tema.PrimarioContenedor, 0.15f);
                FlatAppearance.MouseDownBackColor = ControlPaint.Dark(Tema.PrimarioContenedor, 0.25f);
                return;
            }

            switch (_estilo)
            {
                case EstiloBoton.Primario:
                    BackColor = Tema.PrimarioContenedor;
                    ForeColor = Tema.SobrePrimario;
                    FlatAppearance.BorderColor = Tema.PrimarioContenedor;
                    FlatAppearance.MouseOverBackColor = Tema.Primario;
                    FlatAppearance.MouseDownBackColor = Tema.Primario;
                    break;
                case EstiloBoton.Peligro:
                    BackColor = Tema.Secundario;
                    ForeColor = Tema.SobrePrimario;
                    FlatAppearance.BorderColor = Tema.Secundario;
                    FlatAppearance.MouseOverBackColor = Tema.PeligroOscuro;
                    FlatAppearance.MouseDownBackColor = Tema.PeligroOscuro;
                    break;
                default:
                    BackColor = Tema.Blanco;
                    ForeColor = Tema.Primario;
                    FlatAppearance.BorderColor = Tema.Borde;
                    FlatAppearance.MouseOverBackColor = Tema.Contenedor;
                    FlatAppearance.MouseDownBackColor = Tema.ContenedorAlto;
                    break;
            }
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            AplicarColores();
        }
    }
}
