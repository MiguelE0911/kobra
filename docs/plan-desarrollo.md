# Kobra: plan de desarrollo

Sistema de cuentas por cobrar (CxC) de escritorio. Proyecto de Desarrollo de Software IV, Universidad Tecnológica de Panamá. Entrega: 25 de noviembre de 2026.

Este documento reúne el reparto de tareas, las decisiones técnicas y los puntos de extensión para que los tres integrantes trabajen sin pisarse. El contexto general del proyecto está en `CLAUDE.md`.

## 1. Reparto de tareas

### Miguel: Clientes y base del proyecto

- Crear la solución, las capas (Datos, Negocio, Presentación) y la clase de conexión ADO.NET.
- Script SQL de la base: las tablas Cliente, Factura y Pago con llaves y restricciones.
- Formulario Clientes con crear, editar, buscar y desactivar. No permite eliminar un cliente con facturas.
- Formulario principal con el menú que abre los tres módulos.

### Brandon: Facturas

- Acceso a datos y reglas de Factura: número único, fechas de emisión y vencimiento, monto.
- Cálculo automático del saldo y del estado: Pendiente, Parcial, Pagada o Anulada.
- Formulario Facturas con registro, búsqueda por cliente o estado y anulación.
- Validaciones generales de entrada, como campos vacíos, montos y fechas.

### Wilma: Pagos y consultas

- Acceso a datos y reglas de Pago: cada pago va a una factura y no puede superar su saldo.
- Formulario Pagos que muestra las facturas con saldo del cliente elegido.
- Estado de cuenta del cliente y reporte de antigüedad de saldos.
- Datos de prueba.

## 2. Decisiones técnicas

| Tema | Decisión |
| --- | --- |
| Plataforma | .NET 10, Windows Forms (`net10.0-windows`) |
| Base de datos | SQL Server (Express o LocalDB), base `Kobra` |
| Acceso a datos | ADO.NET con `Microsoft.Data.SqlClient`. Sin Entity Framework ni Dapper |
| Cadena de conexión | `Kobra.UI\App.config`, nombre `Kobra`. Nunca en el código |
| Dinero | `DECIMAL(18,2)` en SQL y `decimal` en C#. Nunca `float` ni `double` |
| Consultas | Siempre parametrizadas con `cmd.Parameters.Add(...)` |

### Estructura de la solución

```
kobra.sln
├── Kobra.Entidades   clases simples (Cliente, luego Factura y Pago)
├── Kobra.Datos       ADO.NET: Conexion, DatosException, ClienteDatos, ...
├── Kobra.Negocio     validaciones y reglas: ClienteNegocio, ReglaNegocioException, ...
├── Kobra.UI          formularios Windows Forms (sin SQL ni reglas)
└── Database          scripts SQL
```

Referencias entre proyectos (solo hacia abajo, nunca al revés):

```
Kobra.UI ──► Kobra.Negocio ──► Kobra.Datos ──► Kobra.Entidades
   └────────────────────────────────────────────────▲
```

- `Kobra.UI` no escribe SQL ni reglas de negocio.
- `Kobra.Negocio` no usa controles de interfaz ni SQL.
- `Kobra.Datos` no decide reglas: ejecuta consultas y traduce `SqlException` a `DatosException` con un mensaje que se puede mostrar tal cual.

## 3. Modelo de datos

Script: `Database\01_CrearBaseDatos.sql`. Es idempotente: se puede ejecutar varias veces sin perder datos.

### Cliente

| Columna | Tipo | Notas |
| --- | --- | --- |
| Id | INT IDENTITY | PK |
| Codigo | NVARCHAR(20) | Único, obligatorio |
| Nombre | NVARCHAR(150) | Obligatorio |
| RucCedula | NVARCHAR(30) | Opcional; único solo cuando se informa |
| Telefono, Email, Direccion | NVARCHAR | Opcionales |
| LimiteCredito | DECIMAL(18,2) | `>= 0` |
| PlazoDias | INT | `>= 0`, por defecto 30 |
| Activo | BIT | Por defecto 1 |
| FechaCreacion, FechaModificacion | DATETIME2(0) | |

### Factura

| Columna | Tipo | Notas |
| --- | --- | --- |
| Id | INT IDENTITY | PK |
| Numero | NVARCHAR(30) | Único |
| ClienteId | INT | FK a Cliente |
| FechaEmision, FechaVencimiento | DATE | Vencimiento `>= ` emisión |
| Subtotal, Itbms, Total | DECIMAL(18,2) | `Total = Subtotal + Itbms` y `Total > 0` |
| Anulada, MotivoAnulacion, FechaAnulacion | BIT, NVARCHAR, DATETIME2 | Si está anulada, motivo y fecha son obligatorios |
| FechaCreacion | DATETIME2(0) | |

La factura **no guarda saldo ni estado**.

### Pago

| Columna | Tipo | Notas |
| --- | --- | --- |
| Id | INT IDENTITY | PK |
| FacturaId | INT | FK a Factura |
| FechaPago | DATE | |
| Monto | DECIMAL(18,2) | `> 0` |
| Anulado, MotivoAnulacion, FechaAnulacion | BIT, NVARCHAR, DATETIME2 | Si está anulado, motivo y fecha son obligatorios |
| FechaCreacion | DATETIME2(0) | |

### Índices

`Cliente(Nombre)`, `Cliente(RucCedula)` filtrado, `Factura(ClienteId)`, `Factura(FechaVencimiento)`, `Pago(FacturaId)`. El código del cliente y el número de factura ya tienen índice por su restricción `UNIQUE`.

### Vista `vw_FacturaSaldo`: fuente única del saldo

El saldo y el estado se **calculan**, no se escriben a mano. Brandon y Wilma deben leer de esta vista y no repetir la fórmula.

| Columna | Significado |
| --- | --- |
| `Id`, `Numero`, `ClienteId`, `FechaEmision`, `FechaVencimiento`, `Total` | Datos de la factura |
| `TotalPagado` | Suma de pagos no anulados |
| `Saldo` | `Total - TotalPagado` (0 si la factura está anulada). Nunca negativo si se respeta la regla de pagos |
| `Estado` | `Anulada`, `Pagada` (saldo 0), `Parcial` (hay pagos y queda saldo) o `Pendiente` |
| `EstaVencida` | 1 si no está anulada, tiene saldo y su vencimiento ya pasó |
| `DiasVencidos` | Días desde el vencimiento (0 si no está vencida). Sirve para los tramos de antigüedad |

**"Vencida" no es un estado**: es un indicador. Así una factura puede ser a la vez `Parcial` y vencida. Para filtrar por "Vencida" en pantalla, usar `EstaVencida = 1`.

## 4. Reglas de negocio de Clientes (implementadas)

- Código y nombre obligatorios. Código de máximo 20 caracteres, nombre de 150.
- Límite de crédito y plazo no pueden ser negativos.
- Correo con formato válido (si se informa).
- El código no se repite. Se valida en la capa de negocio y además lo garantiza el índice único.
- Un cliente **nuevo** queda activo.
- Un cliente con facturas **no se elimina**: se desactiva. La regla se comprueba en `ClienteNegocio.Eliminar` y además el `DELETE` incluye `NOT EXISTS` sobre las facturas, por si alguien factura justo en ese momento.
- Un cliente inactivo no aparece por defecto en la búsqueda (hay un filtro "Solo activos"). **La regla "no se factura a un cliente inactivo" la aplica el módulo de Facturas** (Brandon).
- La búsqueda trae como máximo 50 filas ordenadas por nombre y filtra por código, nombre o cédula/RUC.

## 5. Estado de los módulos y etapas de Pagos

Estado al cierre: etapas A a F implementadas en la rama `feature/pagos`.

### Etapa A — punto de partida tras la integración

- Clientes y Facturas ya están implementados. `FrmPagos` sigue siendo el esqueleto conectado al menú (`Ctrl+D3`); los reportes no tienen ítem propio en `FrmPrincipal`.
- Las tablas `Factura` y `Pago` y la vista `vw_FacturaSaldo` ya están en `Database\01_CrearBaseDatos.sql`.
- Existen los controles reutilizables en `Kobra.UI\Componentes` (`FormularioModulo`, `BarraTituloModulo`, `BarraAcciones`, `BotonTema`, `TarjetaPanel`, `CampoFicha`, `EstiloGrid`, `DialogoConfirmacion`) y la paleta en `Tema.cs`.
- El panel de inicio (`CtlInicio`) ya muestra la antigüedad global de la cartera en cinco tramos. Esa lógica pertenece a Miguel y no se modifica.
- Una sola instancia de cada módulo: si ya está abierto, el menú lo trae al frente.

### Etapa B — patrón a copiar

| Capa | Archivo modelo | Qué copiar |
| --- | --- | --- |
| Entidades | `Kobra.Entidades\Factura.cs` | Propiedades de la tabla más los campos calculados que vienen de `vw_FacturaSaldo` |
| Datos | `Kobra.Datos\FacturaDatos.cs` | `using` en conexión, comando y lector; abrir la conexión lo más tarde posible; parámetros tipados; `catch (SqlException)` que lanza `DatosException` |
| Negocio | `Kobra.Negocio\FacturaNegocio.cs` | `Normalizar`, `Validar`, `Error(mensaje, campo)`, inyección de constructores para pruebas |
| UI | `Kobra.UI\FrmFacturas.cs` | Pseudo-pestañas con `BotonTema` y `UserControl` visibles, atajos F2, F5, Ctrl+N, Ctrl+S y Esc, y captura de `ReglaNegocioException` y `DatosException` sin cerrar la aplicación |

### Etapa C — acceso a datos y reglas de Pago

- `Pago.cs` refleja la tabla `dbo.Pago` y trae campos de consulta de la factura y el cliente. `PagoFactura` se mantiene como lectura de solo los pagos asociados a una factura.
- `PagoDatos` lee el saldo siempre de `dbo.vw_FacturaSaldo`. `Insertar` usa `SqlTransaction` y `UPDLOCK, HOLDLOCK` sobre `dbo.Factura` para que dos pagos concurrentes no superen el saldo; devuelve `null` si la factura desapareció, está anulada o el monto ya no cabe.
- `Anular` repite la condición `Anulado = 0` dentro del `UPDATE`, igual que la anulación de Factura: es una defensa contra cambios ocurridos entre la lectura y la escritura.
- `PagoNegocio` valida factura activa, fecha no futura, `Monto > 0` y `Monto <= Saldo`. El motivo de anulación es obligatorio y de máximo 250 caracteres. Los mensajes llevan `nameof(...)` para que la UI marque el control.

### Etapa D — formulario de Pagos (implementada)

- `FrmPagos` hereda `FormularioModulo` y copia las pseudo-pestañas de `FrmFacturas`: un `FlowLayoutPanel` de `BotonTema` y un `Panel` con `UserControl`s alternados por `.Visible`.
- Vistas: consulta de pagos, registro de pago, anulación de pago y consultas/reportes.
- `CtlPagoConsulta`: filtros por texto, cliente y estado, rejilla de solo lectura y resumen de lo cobrado y anulado.
- `CtlPagoRegistro`: al elegir un cliente activo se cargan sus facturas con saldo ordenadas por vencimiento. El monto se precarga con el saldo y su máximo queda limitado a ese valor.
- `CtlPagoAnulacion`: búsqueda por número de pago o factura, saldo actual, saldo al anular y motivo obligatorio de hasta 250 caracteres.
- `PagoNegocio.ObtenerFacturasConSaldo` reutiliza `FacturaDatos.Buscar` y `vw_FacturaSaldo`; no se creó un DTO paralelo al de Factura.
- No se agregan ítems al menú de `FrmPrincipal`: el módulo sigue abriéndose con `Ctrl+D3`.

### Etapa E — consultas y reportes (implementada)

- `Reporte.cs` agrupa los DTOs `EstadoCuenta`, `LineaEstadoCuenta`, `AntiguedadSaldos`, `TramoAntiguedad` y `LineaAntiguedad`.
- `ReporteDatos` consulta `dbo.Cliente`, `dbo.Factura` y `dbo.vw_FacturaSaldo` siempre con parámetros; el saldo no se recalcula en C#.
- `ReporteNegocio` valida la selección de cliente y agrupa en los cinco tramos de `ResumenDatos` (Corriente, 1-30, 31-60, 61-90 y más de 90) sin editar los archivos de Miguel. Los porcentajes se calculan sobre el saldo total del filtro.
- `CtlReporteCuenta`: cabecera del cliente, facturas, totales y pagos de la factura seleccionada.
- `CtlReporteAntiguedad`: filtro opcional por cliente, rejilla de tramos y detalle de facturas al elegir un tramo.
- Ambos reportes viven como cuarta vista de `FrmPagos`, sin formularios nuevos.

### Etapa F — datos de prueba y cierre (implementada)

- `Database\02_datos_prueba.sql` se ejecuta después del `01`, es idempotente y conserva `DEMO001` a `DEMO004`.
- Cubre factura pagada, parcial, pendiente corriente, vencida en cada uno de los cinco tramos, anulada, pago anulado y cliente inactivo con saldo.
- Verificación realizada: `dotnet build kobra.sln --no-restore -v minimal` termina con 0 advertencias y 0 errores. Los importes, saldos y límites de crédito de los datos demo se validaron por separado.
- Pendiente en la máquina de cada integrante: ejecutar el script SQL y hacer el recorrido manual registrar factura → registrar pago → ver pagos asociados → anular pago → comprobar que el saldo aumenta → anular la factura (después de anular sus pagos).

### Decisiones confirmadas para Pagos

- Los atajos son los del código ya implementado (`F2` buscar, `F5` actualizar, `Ctrl+N` nuevo, `Ctrl+S` guardar, `Esc` cerrar). Se descartan los de la maqueta (`F1`, `F3`, `F9`).
- No se toca `FrmPrincipal` ni `ResumenDatos`/`ResumenCartera`.
- Los nombres de los clientes demo se mantienen por ahora.
- Los reportes viven dentro de `FrmPagos`, sin formularios nuevos en el menú.
- Todo lo pendiente de Pagos se entrega en una sola rama (`feature/pagos`) y un único pull request final a `main`, aparte del PR de la Etapa 1 ya mergeado.

## 6. Preparar el entorno

Cada integrante tiene **su propia base local**. La guía completa, con los pasos del instalador, cómo encontrar el servidor, conectarse y resolver errores, está en **[`docs/base-de-datos.md`](base-de-datos.md)**. Resumen:

1. Instalar **SQL Server Express** con la instancia `SQLEXPRESS`, autenticación de Windows y tu usuario agregado como administrador (o LocalDB).
2. Ejecutar `Database\01_CrearBaseDatos.sql` (con `sqlcmd` hace falta la opción `-C`; ver la guía).
3. Ajustar `Server=` en `Kobra.UI\App.config` solo si tu instancia no es `localhost\SQLEXPRESS`, y evitar subir ese cambio (la guía explica cómo).
4. Abrir `kobra.sln` desde **Archivo, Abrir, Proyecto/Solución** (no con "Abrir carpeta", porque en ese modo no funciona el diseñador de formularios).
5. Compilar con `dotnet build kobra.sln` o con F6 en Visual Studio.

Si la base no está disponible, la aplicación **no se cierra**: avisa con un mensaje claro y en Clientes el error queda visible en rojo junto a la búsqueda.

## 7. Flujo de trabajo con git

- Una rama por persona y tarea: `feature/clientes-base` (Miguel), `feature/facturas` (Brandon), `feature/pagos` (Wilma). Se crean desde `main` actualizada.
- Commits pequeños con mensaje en español que expliquen el cambio.
- Se recomienda no hacer push directo a `main`: abrir un pull request y que otra persona lo revise (el equipo puede decidir otra cosa).
- La rama `feature/clientes-base` debe integrarse primero, porque trae la estructura de proyectos. Brandon y Wilma deben crear sus ramas después de esa integración.
- No se suben `bin/`, `obj/`, `.vs/` ni `*.user` (ya están en `.gitignore`).

## 8. Supuestos pendientes de confirmar con el equipo

1. **Un pago va a una sola factura**, como dice el reparto. `CLAUDE.md` contempla aplicar un pago a varias facturas; si se quiere, se agregaría una tabla de aplicaciones en un script nuevo.
2. **"Vencida" se calcula** (columnas `EstaVencida` y `DiasVencidos` de la vista) y no es un estado guardado. Los estados guardados o derivados son Pendiente, Parcial, Pagada y Anulada.
3. **Inicio de sesión y roles**: nadie lo tiene asignado. Por eso las tablas no guardan todavía el usuario que crea o anula cada documento.
4. **El inicio de sesión** ¿cuenta como uno de los tres formularios funcionales o es adicional? Está pendiente en `CLAUDE.md`.
5. **Estado de las pruebas (18 de noviembre de 2026, SQL Server 2025 Express)**: el script se ejecutó desde cero y por segunda vez sin errores, y se repara si una ejecución quedó a medias. Se comprobaron las 20 restricciones (todas rechazan lo que deben) y la vista `vw_FacturaSaldo` con 7 casos de saldo y estado. La capa de datos y negocio de Clientes y el formulario `FrmClientes` se probaron contra la base real con pruebas automáticas temporales (no están en el repositorio). **Falta que una persona haga la prueba manual de la sección 9 con el mouse y el teclado**, sobre todo el aspecto visual y la comodidad de uso.
6. **Probar en otro equipo**: todo se verificó en una sola máquina. Brandon y Wilma deben ejecutar el script en la suya y avisar si algo falla.

## 9. Prueba manual de Clientes (Fase 7)

Con la base creada y la cadena de conexión ajustada:

1. **Crear**: código `C001`, nombre `Cliente de prueba`, límite 1,000.00, plazo 30. Debe aparecer en la lista.
2. **Buscar**: escribir parte del nombre. La lista se filtra sola. Probar también por código.
3. **Editar**: cambiar el teléfono y guardar con Ctrl+S.
4. **Campos vacíos**: guardar sin código o sin nombre. Debe marcar el campo y mostrar el mensaje.
5. **Datos incorrectos**: correo `abc` (debe rechazar). Código repetido (debe rechazar).
6. **Desactivar y reactivar**: el cliente desaparece de la lista con "Solo activos" marcado.
7. **Eliminar sin facturas**: debe borrarlo.
8. **Eliminar con facturas**: insertar una factura de ese cliente desde SQL y probar. Debe bloquearlo y sugerir desactivar.
9. **Conexión fallida**: poner un servidor inexistente en `App.config`. La aplicación debe avisar sin cerrarse.
10. **Restricciones SQL**: desde SQL, intentar un pago con monto 0 o negativo, una factura sin cliente, y una factura cuyo vencimiento sea anterior a la emisión. Todas deben fallar.
