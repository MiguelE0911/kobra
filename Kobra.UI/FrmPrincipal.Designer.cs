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
            mnuClientes = new ToolStripMenuItem();
            mnuFacturas = new ToolStripMenuItem();
            mnuPagos = new ToolStripMenuItem();
            mnuSalir = new ToolStripMenuItem();
            menuPrincipal.SuspendLayout();
            SuspendLayout();
            //
            // menuPrincipal
            //
            menuPrincipal.Items.AddRange(new ToolStripItem[] { mnuClientes, mnuFacturas, mnuPagos, mnuSalir });
            menuPrincipal.Location = new Point(0, 0);
            menuPrincipal.Name = "menuPrincipal";
            menuPrincipal.Size = new Size(1100, 24);
            menuPrincipal.TabIndex = 0;
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
            // FrmPrincipal
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1100, 700);
            Controls.Add(menuPrincipal);
            IsMdiContainer = true;
            MainMenuStrip = menuPrincipal;
            Name = "FrmPrincipal";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Kobra - Cuentas por cobrar";
            WindowState = FormWindowState.Maximized;
            Shown += FrmPrincipal_Shown;
            menuPrincipal.ResumeLayout(false);
            menuPrincipal.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuPrincipal;
        private ToolStripMenuItem mnuClientes;
        private ToolStripMenuItem mnuFacturas;
        private ToolStripMenuItem mnuPagos;
        private ToolStripMenuItem mnuSalir;
    }
}
