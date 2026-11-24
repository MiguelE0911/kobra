namespace Kobra.UI
{
    partial class FrmPrincipal
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            menuPrincipal = new MenuStrip();
            mnuInicio = new ToolStripMenuItem();
            mnuClientes = new ToolStripMenuItem();
            mnuFacturas = new ToolStripMenuItem();
            mnuPagos = new ToolStripMenuItem();
            mnuSalir = new ToolStripMenuItem();
            barraEstado = new StatusStrip();
            lblEstadoListo = new ToolStripStatusLabel();
            lblEstadoBd = new ToolStripStatusLabel();
            lblEstadoServidor = new ToolStripStatusLabel();
            lblEstadoVersion = new ToolStripStatusLabel();
            menuPrincipal.SuspendLayout();
            barraEstado.SuspendLayout();
            SuspendLayout();
            //
            // menuPrincipal
            //
            menuPrincipal.Items.AddRange(new ToolStripItem[] { mnuInicio, mnuClientes, mnuFacturas, mnuPagos, mnuSalir });
            menuPrincipal.Location = new Point(0, 0);
            menuPrincipal.Name = "menuPrincipal";
            menuPrincipal.Size = new Size(1100, 24);
            menuPrincipal.TabIndex = 0;
            //
            // mnuInicio
            //
            mnuInicio.Name = "mnuInicio";
            mnuInicio.ShortcutKeys = Keys.Control | Keys.D0;
            mnuInicio.Size = new Size(48, 20);
            mnuInicio.Text = "&Inicio";
            mnuInicio.Click += mnuInicio_Click;
            //
            // mnuClientes
            //
            mnuClientes.Name = "mnuClientes";
            mnuClientes.ShortcutKeys = Keys.Control | Keys.D1;
            mnuClientes.Size = new Size(61, 20);
            mnuClientes.Text = "&Clientes";
            mnuClientes.Click += mnuClientes_Click;
            //
            // mnuFacturas
            //
            mnuFacturas.Name = "mnuFacturas";
            mnuFacturas.ShortcutKeys = Keys.Control | Keys.D2;
            mnuFacturas.Size = new Size(61, 20);
            mnuFacturas.Text = "&Facturas";
            mnuFacturas.Click += mnuFacturas_Click;
            //
            // mnuPagos
            //
            mnuPagos.Name = "mnuPagos";
            mnuPagos.ShortcutKeys = Keys.Control | Keys.D3;
            mnuPagos.Size = new Size(48, 20);
            mnuPagos.Text = "&Pagos";
            mnuPagos.Click += mnuPagos_Click;
            //
            // mnuSalir
            //
            mnuSalir.Name = "mnuSalir";
            mnuSalir.Size = new Size(41, 20);
            mnuSalir.Text = "&Salir";
            mnuSalir.Click += mnuSalir_Click;
            //
            // barraEstado
            //
            barraEstado.Items.AddRange(new ToolStripItem[] { lblEstadoListo, lblEstadoBd, lblEstadoServidor, lblEstadoVersion });
            barraEstado.Name = "barraEstado";
            barraEstado.SizingGrip = false;
            barraEstado.TabIndex = 1;
            //
            // lblEstadoListo
            //
            lblEstadoListo.Name = "lblEstadoListo";
            lblEstadoListo.Text = "● Listo";
            //
            // lblEstadoBd
            //
            lblEstadoBd.Name = "lblEstadoBd";
            //
            // lblEstadoServidor
            //
            lblEstadoServidor.Name = "lblEstadoServidor";
            lblEstadoServidor.Spring = true;
            lblEstadoServidor.TextAlign = ContentAlignment.MiddleLeft;
            //
            // lblEstadoVersion
            //
            lblEstadoVersion.Name = "lblEstadoVersion";
            //
            // FrmPrincipal
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1100, 700);
            Controls.Add(barraEstado);
            Controls.Add(menuPrincipal);
            IsMdiContainer = true;
            MainMenuStrip = menuPrincipal;
            Name = "FrmPrincipal";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Kobra · Cuentas por cobrar";
            WindowState = FormWindowState.Maximized;
            KeyPreview = true;
            KeyDown += FrmPrincipal_KeyDown;
            Shown += FrmPrincipal_Shown;
            menuPrincipal.ResumeLayout(false);
            menuPrincipal.PerformLayout();
            barraEstado.ResumeLayout(false);
            barraEstado.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuPrincipal;
        private ToolStripMenuItem mnuInicio;
        private ToolStripMenuItem mnuClientes;
        private ToolStripMenuItem mnuFacturas;
        private ToolStripMenuItem mnuPagos;
        private ToolStripMenuItem mnuSalir;
        private StatusStrip barraEstado;
        private ToolStripStatusLabel lblEstadoListo;
        private ToolStripStatusLabel lblEstadoBd;
        private ToolStripStatusLabel lblEstadoServidor;
        private ToolStripStatusLabel lblEstadoVersion;
    }
}
