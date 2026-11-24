using System.Reflection;
using Kobra.Negocio;

namespace Kobra.UI
{
    public partial class FrmPrincipal : Form
    {
        private readonly CtlInicio _inicio = new();

        public FrmPrincipal()
        {
            InitializeComponent();
            AplicarTema();

            // El panel de inicio cubre el área MDI mientras no haya módulos abiertos.
            _inicio.ModuloSolicitado += AbrirModulo;
            Controls.Add(_inicio);
            _inicio.BringToFront();
        }

        private void AplicarTema()
        {
            BackColor = Tema.Superficie;

            menuPrincipal.Renderer = new ToolStripProfessionalRenderer(new ColoresMenu());
            menuPrincipal.BackColor = Tema.ContenedorBajo;
            menuPrincipal.ForeColor = Tema.TextoSecundario;
            menuPrincipal.Font = Tema.Normal(9.5F);
            menuPrincipal.Padding = new Padding(6, 4, 6, 4);

            barraEstado.Renderer = new ToolStripProfessionalRenderer(new ColoresMenu());
            barraEstado.BackColor = Tema.ContenedorAlto;
            barraEstado.ForeColor = Tema.TextoSecundario;
            barraEstado.Font = Tema.Numeros(8.5F);
            foreach (var item in new[] { lblEstadoBd, lblEstadoServidor })
                item.Padding = new Padding(0, 0, 14, 0);

            var (servidor, baseDatos) = BaseDatos.Destino();
            lblEstadoBd.Text = $"BD: {(baseDatos.Length > 0 ? baseDatos : "—")}";
            lblEstadoServidor.Text = $"SERVIDOR: {(servidor.Length > 0 ? servidor : "—")}";
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            lblEstadoVersion.Text = version is null ? string.Empty : $"v{version.Major}.{version.Minor}.{version.Build}";
        }

        private async void FrmPrincipal_Shown(object? sender, EventArgs e)
        {
            // Aviso temprano: la aplicación sigue abierta aunque no haya base de datos.
            var error = await Task.Run(BaseDatos.ProbarConexion);
            if (IsDisposed)
                return;

            if (error is not null)
                MessageBox.Show(this, error, "Sin conexión a la base de datos",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);

            await _inicio.ActualizarAsync();
            _inicio.FocoInicial();
        }

        private void FrmPrincipal_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5 && _inicio.Visible)
            {
                e.Handled = true;
                _ = _inicio.ActualizarAsync();
            }
        }

        private void mnuInicio_Click(object? sender, EventArgs e)
        {
            // Cierra los módulos abiertos; si alguno cancela el cierre, se queda donde está.
            foreach (var hijo in MdiChildren.ToList())
                hijo.Close();
            MostrarInicioSiCorresponde();
        }

        private void mnuClientes_Click(object? sender, EventArgs e) => AbrirModulo(Modulo.Clientes);

        private void mnuFacturas_Click(object? sender, EventArgs e) => AbrirModulo(Modulo.Facturas);

        private void mnuPagos_Click(object? sender, EventArgs e) => AbrirModulo(Modulo.Pagos);

        private void mnuSalir_Click(object? sender, EventArgs e) => Close();

        private void AbrirModulo(Modulo modulo)
        {
            switch (modulo)
            {
                case Modulo.Clientes: AbrirModulo<FrmClientes>(); break;
                case Modulo.Facturas: AbrirModulo<FrmFacturas>(); break;
                case Modulo.Pagos: AbrirModulo<FrmPagos>(); break;
            }
        }

        /// <summary>Abre el módulo como hijo MDI; si ya está abierto, lo trae al frente.</summary>
        private void AbrirModulo<T>() where T : Form, new()
        {
            var abierto = MdiChildren.OfType<T>().FirstOrDefault();
            if (abierto is not null)
            {
                abierto.Activate();
                return;
            }

            _inicio.Visible = false;
            var formulario = new T { MdiParent = this };
            // Al cerrarse, el hijo todavía figura en MdiChildren: se revisa cuando termine el cierre.
            formulario.FormClosed += (_, _) => BeginInvoke(MostrarInicioSiCorresponde);
            formulario.Show();
        }

        private void MostrarInicioSiCorresponde()
        {
            if (IsDisposed || MdiChildren.Any(f => !f.IsDisposed))
                return;

            _inicio.Visible = true;
            _inicio.BringToFront();
            _ = _inicio.ActualizarAsync();   // los módulos pudieron cambiar saldos
        }

        /// <summary>Colores del menú y la barra de estado con la paleta del tema.</summary>
        private sealed class ColoresMenu : ProfessionalColorTable
        {
            public ColoresMenu() => UseSystemColors = false;

            public override Color MenuItemSelected => Tema.ContenedorAlto;
            public override Color MenuItemSelectedGradientBegin => Tema.ContenedorAlto;
            public override Color MenuItemSelectedGradientEnd => Tema.ContenedorAlto;
            public override Color MenuItemBorder => Tema.ContenedorAlto;
            public override Color MenuItemPressedGradientBegin => Tema.ContenedorAlto;
            public override Color MenuItemPressedGradientEnd => Tema.ContenedorAlto;
            public override Color MenuBorder => Tema.Borde;
            public override Color ToolStripDropDownBackground => Tema.Superficie;
            public override Color ImageMarginGradientBegin => Tema.Superficie;
            public override Color ImageMarginGradientMiddle => Tema.Superficie;
            public override Color ImageMarginGradientEnd => Tema.Superficie;
            public override Color MenuStripGradientBegin => Tema.ContenedorBajo;
            public override Color MenuStripGradientEnd => Tema.ContenedorBajo;
            public override Color StatusStripGradientBegin => Tema.ContenedorAlto;
            public override Color StatusStripGradientEnd => Tema.ContenedorAlto;
        }
    }
}
