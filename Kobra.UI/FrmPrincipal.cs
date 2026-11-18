using Kobra.Negocio;

namespace Kobra.UI
{
    public partial class FrmPrincipal : Form
    {
        public FrmPrincipal()
        {
            InitializeComponent();
        }

        private void FrmPrincipal_Shown(object? sender, EventArgs e)
        {
            // Aviso temprano: la aplicación sigue abierta aunque no haya base de datos.
            var error = BaseDatos.ProbarConexion();
            if (error is not null)
                MessageBox.Show(this, error, "Sin conexión a la base de datos",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void mnuClientes_Click(object? sender, EventArgs e) => AbrirModulo<FrmClientes>();

        private void mnuFacturas_Click(object? sender, EventArgs e) => AbrirModulo<FrmFacturas>();

        private void mnuPagos_Click(object? sender, EventArgs e) => AbrirModulo<FrmPagos>();

        private void mnuSalir_Click(object? sender, EventArgs e) => Close();

        /// <summary>Abre el módulo como hijo MDI; si ya está abierto, lo trae al frente.</summary>
        private void AbrirModulo<T>() where T : Form, new()
        {
            var abierto = MdiChildren.OfType<T>().FirstOrDefault();
            if (abierto is not null)
            {
                abierto.Activate();
                return;
            }

            var formulario = new T { MdiParent = this };
            formulario.Show();
        }
    }
}
