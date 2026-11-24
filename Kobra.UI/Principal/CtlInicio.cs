using System.Globalization;
using Kobra.Datos;
using Kobra.Entidades;
using Kobra.Negocio;

namespace Kobra.UI
{
    public enum Modulo { Clientes, Facturas, Pagos }

    /// <summary>
    /// Panel de inicio: accesos a los módulos, estado de la cartera y clientes con más saldo.
    /// Se arma en código (sin diseñador) y consulta solo agregados, nunca tablas completas.
    /// </summary>
    internal sealed class CtlInicio : UserControl
    {
        // Misma cultura que usan las demás pantallas (la del sistema), para que los montos se lean igual en todas.
        private static CultureInfo Cultura => CultureInfo.CurrentCulture;

        private readonly ResumenNegocio _negocio = new();
        private bool _cargando;

        private readonly Label _lblServidor = new();
        private readonly Label _lblConexion = new();
        private readonly Label _lblFecha = new();
        private readonly Label _lblBaseDatos = new();
        private readonly BotonTema _btnActualizar = new("↻  Actualizar (F5)");

        private readonly Label _lblFacturas = new();
        private readonly Label _lblFacturasDetalle = new();
        private readonly Label _lblClientes = new();
        private readonly Label _lblClientesDetalle = new();
        private readonly Label _lblSaldoTotal = new();
        private readonly Label[] _lblTramos = new Label[5];
        private readonly Label _lblSync = new();
        private readonly Label _lblEstado = new();
        private readonly DataGridView _dgvClientes = new();

        public CtlInicio()
        {
            SuspendLayout();
            AutoScaleMode = AutoScaleMode.None;
            Dock = DockStyle.Fill;
            BackColor = Tema.Superficie;
            Font = Tema.Normal();
            AutoScroll = true;
            AutoScrollMinSize = new Size(880, 620);

            var raiz = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 5,
                Padding = new Padding(10),
                BackColor = Tema.Superficie
            };
            raiz.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            for (var i = 0; i < 4; i++)
                raiz.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            raiz.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            raiz.Controls.Add(CrearBarraHerramientas(), 0, 0);
            raiz.Controls.Add(CrearEncabezado(), 0, 1);
            raiz.Controls.Add(CrearCentro(), 0, 2);
            raiz.Controls.Add(CrearTituloClientes(), 0, 3);
            raiz.Controls.Add(CrearTablaClientes(), 0, 4);

            Controls.Add(raiz);
            ResumeLayout(true);

            var (servidor, baseDatos) = BaseDatos.Destino();
            _lblServidor.Text = $"SERVIDOR: {(servidor.Length > 0 ? servidor : "—")}";
            _lblBaseDatos.Text = $"BASE DE DATOS: {(baseDatos.Length > 0 ? baseDatos : "—")}";
            _lblFecha.Text = $"FECHA DEL SISTEMA: {DateTime.Today:yyyy-MM-dd}";
            MostrarSinDatos("Sin consultar");
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
            _lblEstado.Text = "Actualizando…";

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
            _lblConexion.Text = "SQL CONECTADO";
            _lblConexion.ForeColor = Tema.Primario;

            _lblFacturas.Text = r.FacturasConSaldo.ToString("N0", Cultura);
            _lblFacturasDetalle.Text = r.FacturasVencidas > 0
                ? $"{r.FacturasVencidas:N0} vencidas"
                : "Ninguna vencida";
            _lblFacturasDetalle.ForeColor = r.FacturasVencidas > 0 ? Tema.Secundario : Tema.Primario;

            _lblClientes.Text = r.ClientesActivos.ToString("N0", Cultura);
            _lblClientesDetalle.Text = r.ClientesInactivos > 0
                ? $"{r.ClientesInactivos:N0} inactivos"
                : "Todos activos";

            _lblSaldoTotal.Text = Dinero(r.SaldoTotal);

            var tramos = new[] { r.Corriente, r.Tramo1a30, r.Tramo31a60, r.Tramo61a90, r.TramoMasDe90 };
            for (var i = 0; i < tramos.Length; i++)
                _lblTramos[i].Text = Dinero(tramos[i]);

            _dgvClientes.Rows.Clear();
            foreach (var c in r.ClientesPrincipales)
            {
                var uso = c.LimiteCredito > 0 ? (c.Saldo / c.LimiteCredito).ToString("P0", Cultura) : "—";
                var i = _dgvClientes.Rows.Add(c.Codigo, c.Nombre, c.RucCedula ?? "—", c.Telefono ?? "—",
                    c.LimiteCredito, c.Saldo, uso);
                if (c.LimiteCredito > 0 && c.Saldo > c.LimiteCredito)
                    _dgvClientes.Rows[i].Cells[6].Style.ForeColor = Tema.Secundario;
            }

            _dgvClientes.ClearSelection();
            _dgvClientes.CurrentCell = null;

            _lblSync.Text = $"Última actualización: {DateTime.Now:HH:mm:ss}";
            _lblEstado.Text = "Estado: al día";
        }

        private void MostrarSinDatos(string mensaje)
        {
            var sinConexion = mensaje != "Sin consultar";
            _lblConexion.Text = sinConexion ? "SIN CONEXIÓN" : "SIN CONSULTAR";
            _lblConexion.ForeColor = sinConexion ? Tema.Secundario : Tema.TextoSecundario;

            _lblFacturas.Text = "—";
            _lblFacturasDetalle.Text = " ";
            _lblClientes.Text = "—";
            _lblClientesDetalle.Text = " ";
            _lblSaldoTotal.Text = "—";
            foreach (var l in _lblTramos)
                l.Text = "—";
            _dgvClientes.Rows.Clear();

            _lblSync.Text = sinConexion ? $"Último intento: {DateTime.Now:HH:mm:ss}" : " ";
            _lblEstado.Text = sinConexion ? mensaje : " ";
        }

        private static string Dinero(decimal monto) => "B/. " + monto.ToString("N2", Cultura);

        // ------------------------------------------------------------ construcción de la pantalla

        private Control CrearBarraHerramientas()
        {
            var barra = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                ColumnCount = 4,
                RowCount = 1,
                BackColor = Tema.ContenedorBajo,
                Padding = new Padding(6, 4, 6, 4),
                Margin = new Padding(0, 0, 0, 8)
            };
            barra.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            barra.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            barra.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            barra.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            _btnActualizar.Click += async (_, _) => await ActualizarAsync();

            Configurar(_lblServidor, Tema.Numeros(8.5F), Tema.TextoSecundario, ContentAlignment.MiddleRight);
            Configurar(_lblConexion, Tema.Numeros(8.5F, FontStyle.Bold), Tema.TextoSecundario, ContentAlignment.MiddleRight);
            _lblConexion.BackColor = Tema.Contenedor;
            _lblConexion.Padding = new Padding(4, 1, 4, 1);

            barra.Controls.Add(_btnActualizar, 0, 0);
            barra.Controls.Add(new Label { AutoSize = true, Margin = new Padding(0) }, 1, 0);
            barra.Controls.Add(_lblServidor, 2, 0);
            barra.Controls.Add(_lblConexion, 3, 0);
            return barra;
        }

        private Control CrearEncabezado()
        {
            var tarjeta = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                ColumnCount = 3,
                RowCount = 2,
                BackColor = Tema.ContenedorBajo,
                Padding = new Padding(10),
                Margin = new Padding(0, 0, 0, 8)
            };
            tarjeta.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            tarjeta.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            tarjeta.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            var logo = new Label
            {
                Text = "K",
                Font = new Font("Segoe UI", 20F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Tema.Primario,
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(LogicalToDeviceUnits(46), LogicalToDeviceUnits(46)),
                Margin = new Padding(0, 0, 10, 0)
            };

            var titulo = new Label
            {
                Text = "KOBRA – SISTEMA DE CUENTAS POR COBRAR",
                Font = Tema.Negrita(12F),
                ForeColor = Tema.Primario,
                AutoSize = true,
                Margin = new Padding(0)
            };
            var subtitulo = new Label
            {
                Text = "Clientes, facturas a crédito y pagos • Saldos y antigüedad de la cartera",
                Font = Tema.Normal(9F),
                ForeColor = Tema.TextoSecundario,
                AutoSize = true,
                Margin = new Padding(0)
            };

            Configurar(_lblFecha, Tema.Numeros(8.5F), Tema.TextoSecundario, ContentAlignment.MiddleRight);
            Configurar(_lblBaseDatos, Tema.Numeros(8.5F), Tema.Primario, ContentAlignment.MiddleRight);

            tarjeta.Controls.Add(logo, 0, 0);
            tarjeta.SetRowSpan(logo, 2);
            tarjeta.Controls.Add(titulo, 1, 0);
            tarjeta.Controls.Add(subtitulo, 1, 1);
            tarjeta.Controls.Add(_lblFecha, 2, 0);
            tarjeta.Controls.Add(_lblBaseDatos, 2, 1);
            return tarjeta;
        }

        private Control CrearCentro()
        {
            var centro = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                ColumnCount = 2,
                RowCount = 1,
                BackColor = Tema.Superficie,
                Margin = new Padding(0, 0, 0, 8)
            };
            centro.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 58));
            centro.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 42));
            centro.Controls.Add(CrearModulos(), 0, 0);
            centro.Controls.Add(CrearCartera(), 1, 0);
            return centro;
        }

        private Control CrearModulos()
        {
            var grupo = new TarjetaPanel("MÓDULOS DEL SISTEMA") { Margin = new Padding(0, 0, 8, 0) };
            var cuerpo = grupo.Cuerpo;

            var rejilla = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                ColumnCount = 2,
                RowCount = 2,
                BackColor = Color.Transparent,
                Margin = new Padding(0, 8, 0, 0)
            };
            rejilla.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            rejilla.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            rejilla.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            rejilla.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            rejilla.AutoSize = true;

            var clientes = new TarjetaModulo("1. Gestión de Clientes",
                "Registro, consulta, actualización e inactivación de clientes, con su límite y plazo de crédito.",
                "ACCESO: Ctrl+1");
            var facturas = new TarjetaModulo("2. Facturación",
                "Facturas a crédito con vencimiento, ITBMS, total y saldo. Valida el límite de crédito.",
                "ACCESO: Ctrl+2");
            var pagos = new TarjetaModulo("3. Recepción de Pagos",
                "Pagos totales o parciales aplicados a una o varias facturas del cliente.",
                "ACCESO: Ctrl+3");

            clientes.Abierta += (_, _) => ModuloSolicitado?.Invoke(Modulo.Clientes);
            facturas.Abierta += (_, _) => ModuloSolicitado?.Invoke(Modulo.Facturas);
            pagos.Abierta += (_, _) => ModuloSolicitado?.Invoke(Modulo.Pagos);

            rejilla.Controls.Add(clientes, 0, 0);
            rejilla.Controls.Add(facturas, 1, 0);
            rejilla.Controls.Add(pagos, 0, 1);

            var ayuda = new Label
            {
                Text = "⌨  Use Tab para moverse entre módulos y Enter para abrirlos, o las teclas de acceso del menú.",
                Font = Tema.Normal(8.5F),
                ForeColor = Tema.TextoSecundario,
                BackColor = Tema.Contenedor,
                AutoSize = false,
                Dock = DockStyle.Top,
                Padding = new Padding(6),
                Height = LogicalToDeviceUnits(40),
                Margin = new Padding(0, 4, 0, 0)
            };

            cuerpo.RowCount = 2;
            cuerpo.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            cuerpo.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            cuerpo.Controls.Add(rejilla, 0, 0);
            cuerpo.Controls.Add(ayuda, 0, 1);
            return grupo;
        }

        private Control CrearCartera()
        {
            var grupo = new TarjetaPanel("ESTADO DE LA CARTERA");
            var cuerpo = grupo.Cuerpo;

            // Dos indicadores
            var indicadores = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                ColumnCount = 2,
                RowCount = 1,
                BackColor = Color.Transparent,
                Margin = new Padding(0, 8, 0, 8)
            };
            indicadores.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            indicadores.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            indicadores.Controls.Add(CrearIndicador("FACTURAS CON SALDO", _lblFacturas, "documentos", _lblFacturasDetalle), 0, 0);
            indicadores.Controls.Add(CrearIndicador("CLIENTES ACTIVOS", _lblClientes, "registrados", _lblClientesDetalle), 1, 0);

            // Saldo total
            var saldo = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                ColumnCount = 1,
                RowCount = 3,
                BackColor = Tema.Primario,
                Padding = new Padding(10, 8, 10, 8),
                Margin = new Padding(0, 0, 0, 8)
            };
            var etiquetaSaldo = new Label
            {
                Text = "SALDO TOTAL PENDIENTE",
                Font = Tema.Normal(8.5F),
                ForeColor = Tema.SobrePrimarioContenedor,
                AutoSize = true,
                Margin = new Padding(0)
            };
            Configurar(_lblSaldoTotal, new Font("Segoe UI", 18F, FontStyle.Bold), Color.White, ContentAlignment.MiddleLeft);
            _lblSaldoTotal.Margin = new Padding(0, 2, 0, 2);
            var notaSaldo = new Label
            {
                Text = "Facturas no anuladas, tras aplicar pagos.",
                Font = Tema.Normal(8.5F),
                ForeColor = Tema.SobrePrimarioContenedor,
                AutoSize = true,
                Margin = new Padding(0)
            };
            saldo.Controls.Add(etiquetaSaldo, 0, 0);
            saldo.Controls.Add(_lblSaldoTotal, 0, 1);
            saldo.Controls.Add(notaSaldo, 0, 2);

            // Antigüedad
            var antiguedad = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                ColumnCount = 3,
                RowCount = 6,
                BackColor = Tema.ContenedorBajo,
                Padding = new Padding(6),
                Margin = new Padding(0, 0, 0, 6)
            };
            antiguedad.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            antiguedad.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            antiguedad.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            antiguedad.Controls.Add(new Label
            {
                Text = "Distribución por vencimiento",
                Font = Tema.Negrita(9F),
                ForeColor = Tema.Primario,
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 4)
            }, 0, 0);
            antiguedad.SetColumnSpan(antiguedad.GetControlFromPosition(0, 0)!, 3);

            var nombres = new[] { "Corriente (no vencido)", "1 a 30 días", "31 a 60 días", "61 a 90 días", "Más de 90 días" };
            var colores = new[] { Tema.Primario, Tema.PrimarioContenedor, Tema.TerciarioContenedor, Tema.Secundario, Tema.SecundarioContenedor };
            for (var i = 0; i < 5; i++)
            {
                var punto = new Label
                {
                    Text = "●",
                    ForeColor = colores[i],
                    AutoSize = true,
                    Margin = new Padding(2, 2, 4, 2)
                };
                var nombre = new Label
                {
                    Text = nombres[i],
                    Font = Tema.Normal(9F),
                    ForeColor = Tema.Texto,
                    AutoSize = true,
                    Margin = new Padding(0, 2, 0, 2)
                };
                _lblTramos[i] = new Label
                {
                    Font = Tema.Numeros(9F, FontStyle.Bold),
                    ForeColor = i >= 3 ? Tema.Secundario : Tema.Texto,
                    AutoSize = true,
                    Anchor = AnchorStyles.Right,
                    Margin = new Padding(0, 2, 2, 2)
                };
                antiguedad.Controls.Add(punto, 0, i + 1);
                antiguedad.Controls.Add(nombre, 1, i + 1);
                antiguedad.Controls.Add(_lblTramos[i], 2, i + 1);
            }

            // Pie
            var pie = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                ColumnCount = 2,
                RowCount = 1,
                BackColor = Color.Transparent,
                Margin = new Padding(0)
            };
            pie.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            pie.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            Configurar(_lblSync, Tema.Numeros(8.5F), Tema.TextoSecundario, ContentAlignment.MiddleLeft);
            Configurar(_lblEstado, Tema.Numeros(8.5F), Tema.Primario, ContentAlignment.MiddleRight);
            pie.Controls.Add(_lblSync, 0, 0);
            pie.Controls.Add(_lblEstado, 1, 0);

            cuerpo.RowCount = 4;
            for (var i = 0; i < 4; i++)
                cuerpo.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            cuerpo.Controls.Add(indicadores, 0, 0);
            cuerpo.Controls.Add(saldo, 0, 1);
            cuerpo.Controls.Add(antiguedad, 0, 2);
            cuerpo.Controls.Add(pie, 0, 3);
            return grupo;
        }

        private Control CrearIndicador(string titulo, Label valor, string unidad, Label detalle)
        {
            var panel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                ColumnCount = 2,
                RowCount = 3,
                BackColor = Tema.ContenedorBajo,
                Padding = new Padding(8),
                Margin = new Padding(0, 0, 4, 0)
            };
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            var lblTitulo = new Label
            {
                Text = titulo,
                Font = Tema.Normal(8.5F),
                ForeColor = Tema.TextoSecundario,
                AutoSize = true,
                Margin = new Padding(0)
            };
            Configurar(valor, Tema.Negrita(16F), Tema.Primario, ContentAlignment.MiddleLeft);
            var lblUnidad = new Label
            {
                Text = unidad,
                Font = Tema.Normal(8.5F),
                ForeColor = Tema.TextoSecundario,
                AutoSize = true,
                Anchor = AnchorStyles.Left | AnchorStyles.Bottom,
                Margin = new Padding(4, 0, 0, 4)
            };
            Configurar(detalle, Tema.Numeros(8.5F), Tema.Primario, ContentAlignment.MiddleLeft);

            panel.Controls.Add(lblTitulo, 0, 0);
            panel.SetColumnSpan(lblTitulo, 2);
            panel.Controls.Add(valor, 0, 1);
            panel.Controls.Add(lblUnidad, 1, 1);
            panel.Controls.Add(detalle, 0, 2);
            panel.SetColumnSpan(detalle, 2);
            return panel;
        }

        private Control CrearTituloClientes()
        {
            var fila = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                ColumnCount = 2,
                RowCount = 1,
                BackColor = Tema.Blanco,
                Padding = new Padding(10, 8, 10, 4),
                Margin = new Padding(0)
            };
            fila.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            fila.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            fila.Controls.Add(new Label
            {
                Text = "CLIENTES ACTIVOS CON MAYOR SALDO",
                Font = Tema.Negrita(9.5F),
                ForeColor = Tema.Primario,
                AutoSize = true,
                Margin = new Padding(0)
            }, 0, 0);
            fila.Controls.Add(new Label
            {
                Text = $"Primeros {ResumenNegocio.MaximoClientes} · para buscar, use Clientes (Ctrl+1)",
                Font = Tema.Numeros(8.5F),
                ForeColor = Tema.TextoSecundario,
                AutoSize = true,
                Margin = new Padding(0)
            }, 1, 0);
            return fila;
        }

        private Control CrearTablaClientes()
        {
            var g = _dgvClientes;
            EstiloGrid.Aplicar(g);

            EstiloGrid.AgregarColumna(g, "Codigo", "Código", 11, negrita: true);
            EstiloGrid.AgregarColumna(g, "Nombre", "Nombre del cliente", 34);
            EstiloGrid.AgregarColumna(g, "RucCedula", "Cédula / RUC", 15);
            EstiloGrid.AgregarColumna(g, "Telefono", "Teléfono", 14);
            EstiloGrid.AgregarColumna(g, "Limite", "Límite de crédito", 15, derecha: true, formato: "N2");
            EstiloGrid.AgregarColumna(g, "Saldo", "Saldo pendiente", 15, derecha: true, formato: "N2", negrita: true);
            EstiloGrid.AgregarColumna(g, "Uso", "Uso del límite", 11, derecha: true);

            var contenedor = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Tema.Blanco,
                Padding = new Padding(10, 0, 10, 10),
                Margin = new Padding(0),
                MinimumSize = new Size(0, LogicalToDeviceUnits(120))
            };
            contenedor.Controls.Add(g);
            return contenedor;
        }

        private static void Configurar(Label l, Font fuente, Color color, ContentAlignment alineacion)
        {
            l.Font = fuente;
            l.ForeColor = color;
            l.AutoSize = true;
            l.TextAlign = alineacion;
            l.Margin = new Padding(0);
            if (alineacion == ContentAlignment.MiddleRight)
                l.Anchor = AnchorStyles.Right;
        }
    }
}
