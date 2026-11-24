using Kobra.Datos;
using Kobra.Entidades;
using Kobra.Negocio;

namespace Kobra.UI
{
    public enum Modulo { Clientes, Facturas, Pagos }

    /// <summary>
    /// Panel de inicio: estado de la cartera, accesos a los módulos, antigüedad de saldos y clientes con más saldo.
    /// Se arma en código (sin diseñador) y consulta solo agregados, nunca tablas completas.
    /// </summary>
    internal sealed class CtlInicio : UserControl
    {
        private const string Pista = "F5 actualizar · Ctrl+1 Clientes · Ctrl+2 Facturas · Ctrl+3 Pagos";
        private const string SinConsultar = "SIN CONSULTAR";

        private readonly ResumenNegocio _negocio = new();
        private bool _cargando;
        private string _estadoConexion = SinConsultar;

        private readonly BarraTituloModulo _barraTitulo = new("KOBRA · CUENTAS POR COBRAR");
        private readonly BarraAcciones _barraAcciones = new(Pista);
        private readonly BotonTema _btnActualizar = new("&Actualizar", EstiloBoton.Primario);

        private readonly Label _lblSaldoTotal = new();
        private readonly Label _lblFacturas = new();
        private readonly Label _lblFacturasDetalle = new();
        private readonly Label _lblClientes = new();
        private readonly Label _lblClientesDetalle = new();
        private readonly Label[] _lblTramos = new Label[5];
        private readonly DataGridView _dgvClientes = new();

        public CtlInicio()
        {
            SuspendLayout();
            AutoScaleMode = AutoScaleMode.None;
            Dock = DockStyle.Fill;
            BackColor = Tema.Superficie;
            Font = Tema.Normal();

            var margen = LogicalToDeviceUnits(FormularioModulo.MargenCuerpo);
            var cuerpo = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                Padding = new Padding(margen),
                BackColor = Color.Transparent,
                TabIndex = 0,
                AutoScroll = true,
                AutoScrollMinSize = new Size(LogicalToDeviceUnits(900), LogicalToDeviceUnits(560))
            };
            cuerpo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            cuerpo.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            cuerpo.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            cuerpo.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            cuerpo.Controls.Add(CrearCartera(), 0, 0);
            cuerpo.Controls.Add(CrearCentro(), 0, 1);
            cuerpo.Controls.Add(CrearClientes(), 0, 2);

            _btnActualizar.Click += async (_, _) => await ActualizarAsync();
            _barraAcciones.Agregar(_btnActualizar);
            _barraAcciones.TabIndex = 1;

            Controls.Add(cuerpo);
            Controls.Add(_barraTitulo);
            Controls.Add(_barraAcciones);
            ResumeLayout(true);

            MostrarSinDatos(null);
        }

        public event Action<Modulo>? ModuloSolicitado;

        public void FocoInicial() => _btnActualizar.Focus();

        // ------------------------------------------------------------ carga de datos

        /// <summary>Consulta el resumen en un hilo aparte para no congelar la ventana.</summary>
        public async Task ActualizarAsync()
        {
            if (_cargando)
                return;

            _cargando = true;
            _btnActualizar.Enabled = false;
            _barraAcciones.MostrarMensaje("Actualizando…", esError: false);

            try
            {
                var resumen = await Task.Run(() => _negocio.Obtener());
                if (IsDisposed)
                    return;
                Mostrar(resumen);
            }
            catch (DatosException ex)
            {
                if (!IsDisposed)
                    MostrarSinDatos(ex.Message);
            }
            catch (Exception)
            {
                // Un panel informativo nunca debe tumbar la aplicación.
                if (!IsDisposed)
                    MostrarSinDatos("No se pudo cargar el resumen.");
            }
            finally
            {
                _cargando = false;
                if (!IsDisposed)
                    _btnActualizar.Enabled = true;
            }
        }

        private void Mostrar(ResumenCartera r)
        {
            ActualizarTitulo("SQL CONECTADO");

            _lblSaldoTotal.Text = r.SaldoTotal.Dinero();

            _lblFacturas.Text = r.FacturasConSaldo.ToString("N0", Formato.Cultura);
            _lblFacturasDetalle.Text = r.FacturasVencidas > 0
                ? $"{r.FacturasVencidas:N0} vencidas"
                : "Ninguna vencida";
            _lblFacturasDetalle.ForeColor = r.FacturasVencidas > 0 ? Tema.Secundario : Tema.TextoSecundario;

            _lblClientes.Text = r.ClientesActivos.ToString("N0", Formato.Cultura);
            _lblClientesDetalle.Text = r.ClientesInactivos > 0
                ? $"{r.ClientesInactivos:N0} inactivos"
                : "Todos activos";

            var tramos = new[] { r.Corriente, r.Tramo1a30, r.Tramo31a60, r.Tramo61a90, r.TramoMasDe90 };
            for (var i = 0; i < tramos.Length; i++)
                _lblTramos[i].Text = tramos[i].Dinero();

            _dgvClientes.Rows.Clear();
            foreach (var c in r.ClientesPrincipales)
            {
                var uso = c.LimiteCredito > 0 ? (c.Saldo / c.LimiteCredito).ToString("P0", Formato.Cultura) : "—";
                var i = _dgvClientes.Rows.Add(c.Codigo, c.Nombre, c.RucCedula ?? "—", c.Telefono ?? "—",
                    c.LimiteCredito, c.Saldo, uso);
                if (c.LimiteCredito > 0 && c.Saldo > c.LimiteCredito)
                    _dgvClientes.Rows[i].Cells[6].Style.ForeColor = Tema.Secundario;
            }

            _dgvClientes.ClearSelection();
            _dgvClientes.CurrentCell = null;

            _barraAcciones.MostrarMensaje($"Actualizado {DateTime.Now:HH:mm:ss} · {Pista}", esError: false);
        }

        /// <summary>Muestra el panel vacío. Sin mensaje es el estado inicial; con mensaje, un fallo de conexión.</summary>
        private void MostrarSinDatos(string? mensaje)
        {
            ActualizarTitulo(mensaje is null ? SinConsultar : "SIN CONEXIÓN");

            _lblSaldoTotal.Text = "—";
            _lblFacturas.Text = "—";
            _lblFacturasDetalle.Text = " ";
            _lblClientes.Text = "—";
            _lblClientesDetalle.Text = " ";
            foreach (var l in _lblTramos)
                l.Text = "—";
            _dgvClientes.Rows.Clear();

            if (mensaje is null)
                _barraAcciones.RestablecerPista();
            else
                _barraAcciones.MostrarMensaje($"Último intento {DateTime.Now:HH:mm:ss}: {mensaje}", esError: true);
        }

        private void ActualizarTitulo(string estadoConexion)
        {
            _estadoConexion = estadoConexion;
            _barraTitulo.Info = $"{DateTime.Today.ToString(Formato.Fecha)} · {_estadoConexion}";
        }

        // ------------------------------------------------------------ construcción de la pantalla

        private Control CrearCartera()
        {
            var tarjeta = new TarjetaPanel("ESTADO DE LA CARTERA") { Margin = new Padding(0, 0, 0, 10) };

            var fila = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                ColumnCount = 3,
                RowCount = 1,
                BackColor = Color.Transparent,
                Margin = new Padding(0, 6, 0, 2)
            };
            for (var i = 0; i < 3; i++)
                fila.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F / 3));

            fila.Controls.Add(CrearIndicador("SALDO TOTAL PENDIENTE", _lblSaldoTotal, null), 0, 0);
            fila.Controls.Add(CrearIndicador("FACTURAS CON SALDO", _lblFacturas, _lblFacturasDetalle), 1, 0);
            fila.Controls.Add(CrearIndicador("CLIENTES ACTIVOS", _lblClientes, _lblClientesDetalle), 2, 0);

            tarjeta.Cuerpo.Controls.Add(fila, 0, 0);
            return tarjeta;
        }

        private static Control CrearIndicador(string titulo, Label valor, Label? detalle)
        {
            var panel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                ColumnCount = 1,
                RowCount = 3,
                BackColor = Color.Transparent,
                Margin = new Padding(0, 0, 12, 0)
            };
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            var lblTitulo = new Label
            {
                Text = titulo,
                Font = Tema.Normal(8.5F),
                ForeColor = Tema.TextoSecundario,
                AutoSize = true,
                Margin = new Padding(0)
            };
            Configurar(valor, Tema.Negrita(18F), Tema.Primario);
            valor.Margin = new Padding(0, 2, 0, 0);

            detalle ??= new Label { Text = " " };
            Configurar(detalle, Tema.Normal(8.5F), Tema.TextoSecundario);

            panel.Controls.Add(lblTitulo, 0, 0);
            panel.Controls.Add(valor, 0, 1);
            panel.Controls.Add(detalle, 0, 2);
            return panel;
        }

        private Control CrearCentro()
        {
            var centro = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                ColumnCount = 2,
                RowCount = 1,
                BackColor = Color.Transparent,
                Margin = new Padding(0, 0, 0, 10)
            };
            centro.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55));
            centro.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45));
            centro.Controls.Add(CrearModulos(), 0, 0);
            centro.Controls.Add(CrearAntiguedad(), 1, 0);
            return centro;
        }

        private Control CrearModulos()
        {
            var tarjeta = new TarjetaPanel("MÓDULOS") { Margin = new Padding(0, 0, 10, 0) };

            var lista = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                ColumnCount = 1,
                RowCount = 3,
                BackColor = Color.Transparent,
                Margin = new Padding(0, 8, 0, 0)
            };
            lista.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            var modulos = new (string Titulo, string Descripcion, string Acceso, Modulo Modulo)[]
            {
                ("Gestión de Clientes", "Alta, consulta, edición e inactivación, con límite y plazo de crédito.", "Ctrl+1", Modulo.Clientes),
                ("Facturación", "Facturas a crédito con vencimiento, ITBMS y saldo. Valida el límite de crédito.", "Ctrl+2", Modulo.Facturas),
                ("Recepción de Pagos", "Pagos totales o parciales aplicados a las facturas del cliente.", "Ctrl+3", Modulo.Pagos)
            };

            for (var i = 0; i < modulos.Length; i++)
            {
                var (titulo, descripcion, acceso, modulo) = modulos[i];
                var fila = new TarjetaModulo(titulo, descripcion, acceso) { TabIndex = i };
                fila.Abierta += (_, _) => ModuloSolicitado?.Invoke(modulo);
                lista.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                lista.Controls.Add(fila, 0, i);
            }

            tarjeta.Cuerpo.Controls.Add(lista, 0, 0);
            return tarjeta;
        }

        private Control CrearAntiguedad()
        {
            var tarjeta = new TarjetaPanel("ANTIGÜEDAD DE SALDOS");

            var tabla = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                ColumnCount = 3,
                RowCount = 5,
                BackColor = Color.Transparent,
                Margin = new Padding(0, 8, 0, 0)
            };
            tabla.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            tabla.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            tabla.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            // Azul para lo que no ha vencido, grises para el vencimiento reciente y rojos para el avanzado.
            var nombres = new[] { "Corriente (no vencido)", "1 a 30 días", "31 a 60 días", "61 a 90 días", "Más de 90 días" };
            var colores = new[] { Tema.Primario, Tema.TextoApagado, Tema.TextoSecundario, Tema.Secundario, Tema.PeligroOscuro };
            for (var i = 0; i < 5; i++)
            {
                tabla.RowStyles.Add(new RowStyle(SizeType.AutoSize));

                var punto = new Label
                {
                    Text = "●",
                    ForeColor = colores[i],
                    AutoSize = true,
                    Anchor = AnchorStyles.Left,
                    Margin = new Padding(0, 6, 8, 6)
                };
                var nombre = new Label
                {
                    Text = nombres[i],
                    ForeColor = Tema.Texto,
                    AutoSize = true,
                    Anchor = AnchorStyles.Left,
                    Margin = new Padding(0, 6, 8, 6)
                };
                _lblTramos[i] = new Label
                {
                    Font = Tema.Numeros(9F, FontStyle.Bold),
                    ForeColor = i >= 3 ? Tema.Secundario : Tema.Texto,
                    AutoSize = true,
                    Anchor = AnchorStyles.Right,
                    Margin = new Padding(0, 6, 0, 6)
                };
                tabla.Controls.Add(punto, 0, i);
                tabla.Controls.Add(nombre, 1, i);
                tabla.Controls.Add(_lblTramos[i], 2, i);
            }

            tarjeta.Cuerpo.Controls.Add(tabla, 0, 0);
            return tarjeta;
        }

        private Control CrearClientes()
        {
            var tarjeta = new TarjetaPanel("CLIENTES ACTIVOS CON MAYOR SALDO")
            {
                AutoSize = false,
                MinimumSize = new Size(0, LogicalToDeviceUnits(140))
            };
            tarjeta.MostrarNota($"Primeros {ResumenNegocio.MaximoClientes}");
            tarjeta.Cuerpo.AutoSize = false;
            tarjeta.Cuerpo.RowCount = 1;
            tarjeta.Cuerpo.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            var g = _dgvClientes;
            EstiloGrid.Aplicar(g);
            g.Margin = new Padding(0, 6, 0, 0);

            EstiloGrid.AgregarColumna(g, "Codigo", "Código", 11, negrita: true);
            EstiloGrid.AgregarColumna(g, "Nombre", "Nombre del cliente", 34);
            EstiloGrid.AgregarColumna(g, "RucCedula", "Cédula / RUC", 15);
            EstiloGrid.AgregarColumna(g, "Telefono", "Teléfono", 14);
            EstiloGrid.AgregarColumna(g, "Limite", "Límite (B/.)", 15, derecha: true, formato: Formato.Monto);
            EstiloGrid.AgregarColumna(g, "Saldo", "Saldo pendiente (B/.)", 15, derecha: true, formato: Formato.Monto, negrita: true);
            EstiloGrid.AgregarColumna(g, "Uso", "Uso del límite", 11, derecha: true);

            tarjeta.Cuerpo.Controls.Add(g, 0, 0);
            return tarjeta;
        }

        private static void Configurar(Label l, Font fuente, Color color)
        {
            l.Font = fuente;
            l.ForeColor = color;
            l.AutoSize = true;
            l.Margin = new Padding(0);
        }
    }
}
