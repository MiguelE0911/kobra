using Kobra.Datos;
using Kobra.Entidades;
using Kobra.Negocio;

namespace Kobra.UI
{
    /// <summary>
    /// Búsqueda de clientes (código, nombre o cédula/RUC) y rejilla de resultados.
    /// Consulta siempre con tope (<see cref="ClienteNegocio.MaximoResultados"/>), nunca la tabla completa.
    /// </summary>
    internal sealed class CtlClienteLista : UserControl
    {
        private readonly ClienteNegocio _negocio = new();
        private readonly TarjetaPanel _tarjeta = new("CLIENTES");
        private readonly TextBox _txtBuscar = new();
        private readonly BotonTema _btnBuscar = new("&Buscar");
        private readonly CheckBox _chkSoloActivos = new();
        private readonly DataGridView _dgv = new();
        private readonly System.Windows.Forms.Timer _timer = new() { Interval = 300 };

        private List<Cliente> _clientes = new();
        private bool _cargando;   // evita reaccionar a los cambios de selección hechos por código

        public CtlClienteLista()
        {
            SuspendLayout();
            AutoScaleMode = AutoScaleMode.None;
            Dock = DockStyle.Fill;
            Margin = new Padding(0);

            _tarjeta.AutoSize = false;
            _tarjeta.Cuerpo.AutoSize = false;
            _tarjeta.Cuerpo.RowCount = 2;
            _tarjeta.Cuerpo.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            _tarjeta.Cuerpo.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            _tarjeta.Cuerpo.Controls.Add(CrearBusqueda(), 0, 0);
            _tarjeta.Cuerpo.Controls.Add(CrearRejilla(), 0, 1);
            Controls.Add(_tarjeta);

            _timer.Tick += (_, _) =>
            {
                _timer.Stop();
                BusquedaSolicitada?.Invoke(false);
            };
            ResumeLayout(true);
        }

        /// <summary>Se pide recargar la lista. El parámetro indica si fue una acción explícita (botón Buscar).</summary>
        public event Action<bool>? BusquedaSolicitada;

        /// <summary>El usuario eligió una fila (clic o teclado), no un cambio hecho por código.</summary>
        public event Action<Cliente>? ClienteSeleccionado;

        public event EventHandler? BusquedaEnfocada;
        public event EventHandler? RejillaEnfocada;

        public Button BotonBuscar => _btnBuscar;

        public void EnfocarBusqueda()
        {
            _txtBuscar.Focus();
            _txtBuscar.SelectAll();
        }

        public void QuitarSeleccion()
        {
            _cargando = true;
            try
            {
                _dgv.ClearSelection();
                _dgv.CurrentCell = null;
            }
            finally
            {
                _cargando = false;
            }
        }

        /// <summary>
        /// Recarga según el filtro y deja marcado <paramref name="seleccionarId"/> si aparece en los resultados.
        /// Devuelve el mensaje de error si la base no respondió, o null si todo salió bien.
        /// </summary>
        public string? Cargar(int? seleccionarId)
        {
            _timer.Stop();
            string? error = null;

            try
            {
                _clientes = _negocio.Buscar(_txtBuscar.Text, _chkSoloActivos.Checked);
            }
            catch (DatosException ex)
            {
                _clientes = new List<Cliente>();
                error = ex.Message;
            }

            _cargando = true;
            try
            {
                _dgv.DataSource = _clientes;
                _dgv.ClearSelection();
                _dgv.CurrentCell = null;

                var fila = seleccionarId is int id ? _clientes.FindIndex(c => c.Id == id) : -1;
                if (fila >= 0)
                {
                    _dgv.Rows[fila].Selected = true;
                    _dgv.CurrentCell = _dgv.Rows[fila].Cells["colNombre"];
                }
            }
            finally
            {
                _cargando = false;
            }

            if (error is not null)
                _tarjeta.MostrarNota(error, esError: true);
            else
                _tarjeta.MostrarNota(_clientes.Count >= ClienteNegocio.MaximoResultados
                    ? $"Primeros {ClienteNegocio.MaximoResultados}: afine la búsqueda"
                    : $"{_clientes.Count} cliente(s)");

            return error;
        }

        // ------------------------------------------------------------ construcción

        private Control CrearBusqueda()
        {
            var fila = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                ColumnCount = 4,
                RowCount = 1,
                BackColor = Color.Transparent,
                Margin = new Padding(0, 8, 0, 8)
            };
            fila.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            fila.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            fila.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            fila.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            var etiqueta = new Label
            {
                Text = "Buscar (F2):",
                ForeColor = Tema.TextoSecundario,
                AutoSize = true,
                Anchor = AnchorStyles.Left,
                Margin = new Padding(0, 0, 8, 0)
            };

            _txtBuscar.MaxLength = 150;
            _txtBuscar.PlaceholderText = "Código, nombre o cédula/RUC";
            _txtBuscar.BorderStyle = BorderStyle.FixedSingle;
            _txtBuscar.Dock = DockStyle.Fill;
            _txtBuscar.Margin = new Padding(0, 3, 8, 3);
            _txtBuscar.TabIndex = 0;
            _txtBuscar.TextChanged += (_, _) =>
            {
                // Espera a que el usuario deje de escribir para no consultar en cada tecla.
                _timer.Stop();
                _timer.Start();
            };
            _txtBuscar.Enter += (_, _) => BusquedaEnfocada?.Invoke(this, EventArgs.Empty);

            _btnBuscar.TabIndex = 1;
            _btnBuscar.Margin = new Padding(0, 0, 12, 0);
            _btnBuscar.Click += (_, _) =>
            {
                _timer.Stop();
                BusquedaSolicitada?.Invoke(true);
            };

            _chkSoloActivos.Text = "Solo activos";
            _chkSoloActivos.Checked = true;
            _chkSoloActivos.AutoSize = true;
            _chkSoloActivos.Anchor = AnchorStyles.Left;
            _chkSoloActivos.Margin = new Padding(0);
            _chkSoloActivos.TabIndex = 2;
            _chkSoloActivos.CheckedChanged += (_, _) => BusquedaSolicitada?.Invoke(false);
            _chkSoloActivos.Enter += (_, _) => BusquedaEnfocada?.Invoke(this, EventArgs.Empty);

            fila.Controls.Add(etiqueta, 0, 0);
            fila.Controls.Add(_txtBuscar, 1, 0);
            fila.Controls.Add(_btnBuscar, 2, 0);
            fila.Controls.Add(_chkSoloActivos, 3, 0);
            return fila;
        }

        private Control CrearRejilla()
        {
            EstiloGrid.Aplicar(_dgv);
            _dgv.Margin = new Padding(0);
            _dgv.TabIndex = 3;

            EstiloGrid.AgregarColumna(_dgv, "colCodigo", "Código", 11, negrita: true, propiedad: "Codigo");
            EstiloGrid.AgregarColumna(_dgv, "colNombre", "Nombre", 34, propiedad: "Nombre");
            EstiloGrid.AgregarColumna(_dgv, "colRucCedula", "Cédula / RUC", 15, propiedad: "RucCedula");
            EstiloGrid.AgregarColumna(_dgv, "colTelefono", "Teléfono", 14, propiedad: "Telefono");
            EstiloGrid.AgregarColumna(_dgv, "colLimite", "Límite de crédito", 15, derecha: true, formato: "N2",
                propiedad: "LimiteCredito");
            EstiloGrid.AgregarColumna(_dgv, "colPlazo", "Plazo", 8, derecha: true, propiedad: "PlazoDias");
            var activo = EstiloGrid.AgregarColumna(_dgv, "colActivo", "Activo", 8, propiedad: "Activo");
            activo.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            activo.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            _dgv.CellFormatting += Rejilla_CellFormatting;
            _dgv.Enter += (_, _) => RejillaEnfocada?.Invoke(this, EventArgs.Empty);

            // Se engancha a SelectionChanged y a CurrentCellChanged porque, según cómo se seleccione la
            // fila (clic, teclado o código), solo uno de los dos eventos llega con la fila actual ya fijada.
            _dgv.SelectionChanged += Rejilla_CambioSeleccion;
            _dgv.CurrentCellChanged += Rejilla_CambioSeleccion;
            return _dgv;
        }

        private void Rejilla_CambioSeleccion(object? sender, EventArgs e)
        {
            if (_cargando || _dgv.CurrentRow?.DataBoundItem is not Cliente cliente)
                return;
            ClienteSeleccionado?.Invoke(cliente);
        }

        private void Rejilla_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || _dgv.Rows[e.RowIndex].DataBoundItem is not Cliente c)
                return;

            if (!c.Activo)
                e.CellStyle!.ForeColor = Tema.TextoApagado;

            // La columna Activo se muestra como etiqueta Sí / No en lugar de True / False.
            if (_dgv.Columns[e.ColumnIndex].Name == "colActivo")
            {
                e.Value = c.Activo ? "Sí" : "No";
                e.FormattingApplied = true;
                if (!c.Activo)
                {
                    e.CellStyle!.BackColor = Tema.ErrorContenedor;
                    e.CellStyle.ForeColor = Tema.Secundario;
                    e.CellStyle.Font = Tema.Negrita(9F);
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _timer.Dispose();
            base.Dispose(disposing);
        }
    }
}
