namespace Kobra.UI
{
    partial class FrmClientes
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        // Composición del módulo: el detalle de cada parte está en los componentes
        // (CtlClienteLista, CtlClienteFicha, BarraTituloModulo, BarraAcciones).
        private void InitializeComponent()
        {
            barraTitulo = new BarraTituloModulo("CLIENTES");
            lista = new CtlClienteLista();
            ficha = new CtlClienteFicha();
            barraAcciones = new BarraAcciones(Mensajes.PistaAtajos("nuevo"));
            btnNuevo = new BotonTema("&Nuevo");
            btnGuardar = new BotonTema("&Guardar", EstiloBoton.Primario);
            btnDesactivar = new BotonTema("&Desactivar");
            btnEliminar = new BotonTema("&Eliminar", EstiloBoton.Peligro);
            btnCerrar = new BotonTema("&Cerrar");
            var cuerpo = new TableLayoutPanel();
            cuerpo.SuspendLayout();
            SuspendLayout();
            //
            // cuerpo: lista (ocupa el espacio sobrante) y ficha (alto propio)
            //
            cuerpo.BackColor = Color.Transparent;
            cuerpo.ColumnCount = 1;
            cuerpo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            cuerpo.Dock = DockStyle.Fill;
            cuerpo.Padding = new Padding(MargenCuerpo);
            cuerpo.RowCount = 2;
            cuerpo.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            cuerpo.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            cuerpo.TabIndex = 0;
            cuerpo.Controls.Add(lista, 0, 0);
            cuerpo.Controls.Add(ficha, 0, 1);
            //
            // lista
            //
            lista.Name = "lista";
            lista.TabIndex = 0;
            lista.BusquedaSolicitada += lista_BusquedaSolicitada;
            lista.ClienteSeleccionado += lista_ClienteSeleccionado;
            lista.BusquedaEnfocada += (_, _) => AcceptButton = lista.BotonBuscar;
            lista.RejillaEnfocada += (_, _) => AcceptButton = null;
            //
            // ficha
            //
            ficha.Name = "ficha";
            ficha.TabIndex = 1;
            ficha.FichaEnfocada += (_, _) => AcceptButton = btnGuardar;
            //
            // botones
            //
            btnNuevo.Name = "btnNuevo";
            btnNuevo.TabIndex = 0;
            btnNuevo.Click += btnNuevo_Click;
            btnGuardar.Name = "btnGuardar";
            btnGuardar.TabIndex = 1;
            btnGuardar.Click += btnGuardar_Click;
            btnDesactivar.Name = "btnDesactivar";
            btnDesactivar.TabIndex = 2;
            btnDesactivar.Click += btnDesactivar_Click;
            btnEliminar.Name = "btnEliminar";
            btnEliminar.TabIndex = 3;
            btnEliminar.Click += btnEliminar_Click;
            btnCerrar.DialogResult = DialogResult.Cancel;
            btnCerrar.Name = "btnCerrar";
            btnCerrar.TabIndex = 4;
            btnCerrar.Click += btnCerrar_Click;
            //
            // barraAcciones
            //
            barraAcciones.Agregar(btnNuevo, btnGuardar, btnDesactivar, btnEliminar, btnCerrar);
            barraAcciones.Name = "barraAcciones";
            barraAcciones.TabIndex = 1;
            //
            // barraTitulo
            //
            barraTitulo.Name = "barraTitulo";
            barraTitulo.TabIndex = 2;
            //
            // FrmClientes
            //
            AcceptButton = btnGuardar;
            CancelButton = btnCerrar;
            Controls.Add(cuerpo);
            Controls.Add(barraAcciones);
            Controls.Add(barraTitulo);
            ConfigurarVentana("Clientes");
            Name = "FrmClientes";
            Load += FrmClientes_Load;
            cuerpo.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private BarraTituloModulo barraTitulo;
        private CtlClienteLista lista;
        private CtlClienteFicha ficha;
        private BarraAcciones barraAcciones;
        private BotonTema btnNuevo;
        private BotonTema btnGuardar;
        private BotonTema btnDesactivar;
        private BotonTema btnEliminar;
        private BotonTema btnCerrar;
    }
}
