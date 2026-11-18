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

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            DataGridViewCellStyle estiloMonto = new DataGridViewCellStyle();
            lblBuscar = new Label();
            txtBuscar = new TextBox();
            btnBuscar = new Button();
            chkSoloActivos = new CheckBox();
            lblResultados = new Label();
            dgvClientes = new DataGridView();
            colCodigo = new DataGridViewTextBoxColumn();
            colNombre = new DataGridViewTextBoxColumn();
            colRucCedula = new DataGridViewTextBoxColumn();
            colTelefono = new DataGridViewTextBoxColumn();
            colLimiteCredito = new DataGridViewTextBoxColumn();
            colPlazoDias = new DataGridViewTextBoxColumn();
            colActivo = new DataGridViewCheckBoxColumn();
            grpDatos = new GroupBox();
            lblCodigo = new Label();
            txtCodigo = new TextBox();
            lblNombre = new Label();
            txtNombre = new TextBox();
            lblRucCedula = new Label();
            txtRucCedula = new TextBox();
            lblTelefono = new Label();
            txtTelefono = new TextBox();
            lblEmail = new Label();
            txtEmail = new TextBox();
            lblLimiteCredito = new Label();
            nudLimiteCredito = new NumericUpDown();
            lblPlazoDias = new Label();
            nudPlazoDias = new NumericUpDown();
            lblDireccion = new Label();
            txtDireccion = new TextBox();
            chkActivo = new CheckBox();
            btnNuevo = new Button();
            btnGuardar = new Button();
            btnDesactivar = new Button();
            btnEliminar = new Button();
            btnCerrar = new Button();
            errorProvider = new ErrorProvider(components);
            timerBusqueda = new System.Windows.Forms.Timer(components);
            ((System.ComponentModel.ISupportInitialize)dgvClientes).BeginInit();
            grpDatos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudLimiteCredito).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudPlazoDias).BeginInit();
            ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
            SuspendLayout();
            //
            // lblBuscar
            //
            lblBuscar.AutoSize = true;
            lblBuscar.Location = new Point(12, 16);
            lblBuscar.Name = "lblBuscar";
            lblBuscar.Size = new Size(77, 15);
            lblBuscar.Text = "Buscar (F2):";
            //
            // txtBuscar
            //
            txtBuscar.Location = new Point(98, 12);
            txtBuscar.MaxLength = 150;
            txtBuscar.Name = "txtBuscar";
            txtBuscar.PlaceholderText = "Código, nombre o cédula/RUC";
            txtBuscar.Size = new Size(300, 23);
            txtBuscar.TabIndex = 0;
            txtBuscar.TextChanged += txtBuscar_TextChanged;
            txtBuscar.Enter += EnterBusqueda;
            //
            // btnBuscar
            //
            btnBuscar.Location = new Point(404, 11);
            btnBuscar.Name = "btnBuscar";
            btnBuscar.Size = new Size(80, 25);
            btnBuscar.TabIndex = 1;
            btnBuscar.Text = "Buscar";
            btnBuscar.UseVisualStyleBackColor = true;
            btnBuscar.Click += btnBuscar_Click;
            //
            // chkSoloActivos
            //
            chkSoloActivos.AutoSize = true;
            chkSoloActivos.Checked = true;
            chkSoloActivos.CheckState = CheckState.Checked;
            chkSoloActivos.Location = new Point(500, 14);
            chkSoloActivos.Name = "chkSoloActivos";
            chkSoloActivos.Size = new Size(94, 19);
            chkSoloActivos.TabIndex = 2;
            chkSoloActivos.Text = "Solo activos";
            chkSoloActivos.UseVisualStyleBackColor = true;
            chkSoloActivos.CheckedChanged += chkSoloActivos_CheckedChanged;
            chkSoloActivos.Enter += EnterBusqueda;
            //
            // lblResultados
            //
            lblResultados.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblResultados.AutoEllipsis = true;
            lblResultados.Location = new Point(610, 16);
            lblResultados.Name = "lblResultados";
            lblResultados.Size = new Size(302, 15);
            lblResultados.TextAlign = ContentAlignment.MiddleRight;
            //
            // dgvClientes
            //
            dgvClientes.AllowUserToAddRows = false;
            dgvClientes.AllowUserToDeleteRows = false;
            dgvClientes.AllowUserToResizeRows = false;
            dgvClientes.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvClientes.AutoGenerateColumns = false;
            dgvClientes.BackgroundColor = SystemColors.Window;
            dgvClientes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvClientes.Columns.AddRange(new DataGridViewColumn[] { colCodigo, colNombre, colRucCedula, colTelefono, colLimiteCredito, colPlazoDias, colActivo });
            dgvClientes.Location = new Point(12, 45);
            dgvClientes.MultiSelect = false;
            dgvClientes.Name = "dgvClientes";
            dgvClientes.ReadOnly = true;
            dgvClientes.RowHeadersVisible = false;
            dgvClientes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvClientes.Size = new Size(900, 235);
            dgvClientes.TabIndex = 3;
            dgvClientes.CellFormatting += dgvClientes_CellFormatting;
            dgvClientes.Enter += EnterLista;
            dgvClientes.SelectionChanged += dgvClientes_SelectionChanged;
            dgvClientes.CurrentCellChanged += dgvClientes_SelectionChanged;
            //
            // colCodigo
            //
            colCodigo.DataPropertyName = "Codigo";
            colCodigo.HeaderText = "Código";
            colCodigo.Name = "colCodigo";
            colCodigo.ReadOnly = true;
            colCodigo.Width = 90;
            //
            // colNombre
            //
            colNombre.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colNombre.DataPropertyName = "Nombre";
            colNombre.HeaderText = "Nombre";
            colNombre.Name = "colNombre";
            colNombre.ReadOnly = true;
            //
            // colRucCedula
            //
            colRucCedula.DataPropertyName = "RucCedula";
            colRucCedula.HeaderText = "Cédula / RUC";
            colRucCedula.Name = "colRucCedula";
            colRucCedula.ReadOnly = true;
            colRucCedula.Width = 130;
            //
            // colTelefono
            //
            colTelefono.DataPropertyName = "Telefono";
            colTelefono.HeaderText = "Teléfono";
            colTelefono.Name = "colTelefono";
            colTelefono.ReadOnly = true;
            colTelefono.Width = 110;
            //
            // colLimiteCredito
            //
            estiloMonto.Alignment = DataGridViewContentAlignment.MiddleRight;
            estiloMonto.Format = "N2";
            colLimiteCredito.DefaultCellStyle = estiloMonto;
            colLimiteCredito.DataPropertyName = "LimiteCredito";
            colLimiteCredito.HeaderText = "Límite de crédito";
            colLimiteCredito.Name = "colLimiteCredito";
            colLimiteCredito.ReadOnly = true;
            colLimiteCredito.Width = 120;
            //
            // colPlazoDias
            //
            colPlazoDias.DataPropertyName = "PlazoDias";
            colPlazoDias.HeaderText = "Plazo (días)";
            colPlazoDias.Name = "colPlazoDias";
            colPlazoDias.ReadOnly = true;
            colPlazoDias.Width = 80;
            //
            // colActivo
            //
            colActivo.DataPropertyName = "Activo";
            colActivo.HeaderText = "Activo";
            colActivo.Name = "colActivo";
            colActivo.ReadOnly = true;
            colActivo.Width = 55;
            //
            // grpDatos
            //
            grpDatos.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            grpDatos.Controls.Add(lblCodigo);
            grpDatos.Controls.Add(txtCodigo);
            grpDatos.Controls.Add(lblNombre);
            grpDatos.Controls.Add(txtNombre);
            grpDatos.Controls.Add(lblRucCedula);
            grpDatos.Controls.Add(txtRucCedula);
            grpDatos.Controls.Add(lblTelefono);
            grpDatos.Controls.Add(txtTelefono);
            grpDatos.Controls.Add(lblEmail);
            grpDatos.Controls.Add(txtEmail);
            grpDatos.Controls.Add(lblLimiteCredito);
            grpDatos.Controls.Add(nudLimiteCredito);
            grpDatos.Controls.Add(lblPlazoDias);
            grpDatos.Controls.Add(nudPlazoDias);
            grpDatos.Controls.Add(lblDireccion);
            grpDatos.Controls.Add(txtDireccion);
            grpDatos.Controls.Add(chkActivo);
            grpDatos.Location = new Point(12, 290);
            grpDatos.Name = "grpDatos";
            grpDatos.Size = new Size(900, 215);
            grpDatos.TabIndex = 4;
            grpDatos.TabStop = false;
            grpDatos.Text = "Datos del cliente";
            grpDatos.Enter += EnterDatos;
            //
            // lblCodigo
            //
            lblCodigo.AutoSize = true;
            lblCodigo.Location = new Point(15, 31);
            lblCodigo.Name = "lblCodigo";
            lblCodigo.Text = "Código *";
            //
            // txtCodigo
            //
            txtCodigo.Location = new Point(125, 28);
            txtCodigo.MaxLength = 20;
            txtCodigo.Name = "txtCodigo";
            txtCodigo.Size = new Size(140, 23);
            txtCodigo.TabIndex = 0;
            txtCodigo.Validating += txtCodigo_Validating;
            //
            // lblNombre
            //
            lblNombre.AutoSize = true;
            lblNombre.Location = new Point(15, 63);
            lblNombre.Name = "lblNombre";
            lblNombre.Text = "Nombre *";
            //
            // txtNombre
            //
            txtNombre.Location = new Point(125, 60);
            txtNombre.MaxLength = 150;
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(300, 23);
            txtNombre.TabIndex = 1;
            txtNombre.Validating += txtNombre_Validating;
            //
            // lblRucCedula
            //
            lblRucCedula.AutoSize = true;
            lblRucCedula.Location = new Point(15, 95);
            lblRucCedula.Name = "lblRucCedula";
            lblRucCedula.Text = "Cédula / RUC";
            //
            // txtRucCedula
            //
            txtRucCedula.Location = new Point(125, 92);
            txtRucCedula.MaxLength = 30;
            txtRucCedula.Name = "txtRucCedula";
            txtRucCedula.Size = new Size(200, 23);
            txtRucCedula.TabIndex = 2;
            //
            // lblTelefono
            //
            lblTelefono.AutoSize = true;
            lblTelefono.Location = new Point(15, 127);
            lblTelefono.Name = "lblTelefono";
            lblTelefono.Text = "Teléfono";
            //
            // txtTelefono
            //
            txtTelefono.Location = new Point(125, 124);
            txtTelefono.MaxLength = 30;
            txtTelefono.Name = "txtTelefono";
            txtTelefono.Size = new Size(200, 23);
            txtTelefono.TabIndex = 3;
            //
            // lblEmail
            //
            lblEmail.AutoSize = true;
            lblEmail.Location = new Point(15, 159);
            lblEmail.Name = "lblEmail";
            lblEmail.Text = "Correo";
            //
            // txtEmail
            //
            txtEmail.Location = new Point(125, 156);
            txtEmail.MaxLength = 150;
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(300, 23);
            txtEmail.TabIndex = 4;
            txtEmail.Validating += txtEmail_Validating;
            //
            // lblLimiteCredito
            //
            lblLimiteCredito.AutoSize = true;
            lblLimiteCredito.Location = new Point(480, 31);
            lblLimiteCredito.Name = "lblLimiteCredito";
            lblLimiteCredito.Text = "Límite de crédito";
            //
            // nudLimiteCredito
            //
            nudLimiteCredito.DecimalPlaces = 2;
            nudLimiteCredito.Location = new Point(600, 28);
            nudLimiteCredito.Maximum = new decimal(new int[] { 276447231, 23027, 0, 131072 });
            nudLimiteCredito.Name = "nudLimiteCredito";
            nudLimiteCredito.Size = new Size(150, 23);
            nudLimiteCredito.TabIndex = 5;
            nudLimiteCredito.TextAlign = HorizontalAlignment.Right;
            nudLimiteCredito.ThousandsSeparator = true;
            //
            // lblPlazoDias
            //
            lblPlazoDias.AutoSize = true;
            lblPlazoDias.Location = new Point(480, 63);
            lblPlazoDias.Name = "lblPlazoDias";
            lblPlazoDias.Text = "Plazo (días)";
            //
            // nudPlazoDias
            //
            nudPlazoDias.Location = new Point(600, 60);
            nudPlazoDias.Maximum = new decimal(new int[] { 3650, 0, 0, 0 });
            nudPlazoDias.Name = "nudPlazoDias";
            nudPlazoDias.Size = new Size(80, 23);
            nudPlazoDias.TabIndex = 6;
            nudPlazoDias.TextAlign = HorizontalAlignment.Right;
            nudPlazoDias.Value = new decimal(new int[] { 30, 0, 0, 0 });
            //
            // lblDireccion
            //
            lblDireccion.AutoSize = true;
            lblDireccion.Location = new Point(480, 95);
            lblDireccion.Name = "lblDireccion";
            lblDireccion.Text = "Dirección";
            //
            // txtDireccion
            //
            txtDireccion.Location = new Point(600, 92);
            txtDireccion.MaxLength = 250;
            txtDireccion.Multiline = true;
            txtDireccion.Name = "txtDireccion";
            txtDireccion.ScrollBars = ScrollBars.Vertical;
            txtDireccion.Size = new Size(280, 55);
            txtDireccion.TabIndex = 7;
            //
            // chkActivo
            //
            chkActivo.AutoSize = true;
            chkActivo.Checked = true;
            chkActivo.CheckState = CheckState.Checked;
            chkActivo.Enabled = false;
            chkActivo.Location = new Point(600, 160);
            chkActivo.Name = "chkActivo";
            chkActivo.Size = new Size(60, 19);
            chkActivo.TabIndex = 8;
            chkActivo.TabStop = false;
            chkActivo.Text = "Activo";
            chkActivo.UseVisualStyleBackColor = true;
            //
            // btnNuevo
            //
            btnNuevo.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnNuevo.Location = new Point(12, 515);
            btnNuevo.Name = "btnNuevo";
            btnNuevo.Size = new Size(100, 32);
            btnNuevo.TabIndex = 5;
            btnNuevo.Text = "&Nuevo";
            btnNuevo.UseVisualStyleBackColor = true;
            btnNuevo.Click += btnNuevo_Click;
            //
            // btnGuardar
            //
            btnGuardar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnGuardar.Location = new Point(120, 515);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(100, 32);
            btnGuardar.TabIndex = 6;
            btnGuardar.Text = "&Guardar";
            btnGuardar.UseVisualStyleBackColor = true;
            btnGuardar.Click += btnGuardar_Click;
            //
            // btnDesactivar
            //
            btnDesactivar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnDesactivar.Location = new Point(228, 515);
            btnDesactivar.Name = "btnDesactivar";
            btnDesactivar.Size = new Size(100, 32);
            btnDesactivar.TabIndex = 7;
            btnDesactivar.Text = "&Desactivar";
            btnDesactivar.UseVisualStyleBackColor = true;
            btnDesactivar.Click += btnDesactivar_Click;
            //
            // btnEliminar
            //
            btnEliminar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnEliminar.Location = new Point(336, 515);
            btnEliminar.Name = "btnEliminar";
            btnEliminar.Size = new Size(100, 32);
            btnEliminar.TabIndex = 8;
            btnEliminar.Text = "&Eliminar";
            btnEliminar.UseVisualStyleBackColor = true;
            btnEliminar.Click += btnEliminar_Click;
            //
            // btnCerrar
            //
            btnCerrar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCerrar.DialogResult = DialogResult.Cancel;
            btnCerrar.Location = new Point(812, 515);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(100, 32);
            btnCerrar.TabIndex = 9;
            btnCerrar.Text = "&Cerrar (Esc)";
            btnCerrar.UseVisualStyleBackColor = true;
            btnCerrar.Click += btnCerrar_Click;
            //
            // errorProvider
            //
            errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            errorProvider.ContainerControl = this;
            //
            // timerBusqueda
            //
            timerBusqueda.Interval = 300;
            timerBusqueda.Tick += timerBusqueda_Tick;
            //
            // FrmClientes
            //
            AcceptButton = btnGuardar;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCerrar;
            ClientSize = new Size(924, 560);
            Controls.Add(lblBuscar);
            Controls.Add(txtBuscar);
            Controls.Add(btnBuscar);
            Controls.Add(chkSoloActivos);
            Controls.Add(lblResultados);
            Controls.Add(dgvClientes);
            Controls.Add(grpDatos);
            Controls.Add(btnNuevo);
            Controls.Add(btnGuardar);
            Controls.Add(btnDesactivar);
            Controls.Add(btnEliminar);
            Controls.Add(btnCerrar);
            KeyPreview = true;
            MinimumSize = new Size(940, 599);
            Name = "FrmClientes";
            Text = "Clientes";
            Load += FrmClientes_Load;
            KeyDown += FrmClientes_KeyDown;
            ((System.ComponentModel.ISupportInitialize)dgvClientes).EndInit();
            grpDatos.ResumeLayout(false);
            grpDatos.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudLimiteCredito).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudPlazoDias).EndInit();
            ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblBuscar;
        private TextBox txtBuscar;
        private Button btnBuscar;
        private CheckBox chkSoloActivos;
        private Label lblResultados;
        private DataGridView dgvClientes;
        private DataGridViewTextBoxColumn colCodigo;
        private DataGridViewTextBoxColumn colNombre;
        private DataGridViewTextBoxColumn colRucCedula;
        private DataGridViewTextBoxColumn colTelefono;
        private DataGridViewTextBoxColumn colLimiteCredito;
        private DataGridViewTextBoxColumn colPlazoDias;
        private DataGridViewCheckBoxColumn colActivo;
        private GroupBox grpDatos;
        private Label lblCodigo;
        private TextBox txtCodigo;
        private Label lblNombre;
        private TextBox txtNombre;
        private Label lblRucCedula;
        private TextBox txtRucCedula;
        private Label lblTelefono;
        private TextBox txtTelefono;
        private Label lblEmail;
        private TextBox txtEmail;
        private Label lblLimiteCredito;
        private NumericUpDown nudLimiteCredito;
        private Label lblPlazoDias;
        private NumericUpDown nudPlazoDias;
        private Label lblDireccion;
        private TextBox txtDireccion;
        private CheckBox chkActivo;
        private Button btnNuevo;
        private Button btnGuardar;
        private Button btnDesactivar;
        private Button btnEliminar;
        private Button btnCerrar;
        private ErrorProvider errorProvider;
        private System.Windows.Forms.Timer timerBusqueda;
    }
}
