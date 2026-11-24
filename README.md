# Kobra

Sistema de **cuentas por cobrar** para escritorio (Windows). Registra clientes, facturas a crédito y pagos, controla el límite de crédito de cada cliente y muestra los saldos y la antigüedad de la cartera para cobrar a tiempo.

Proyecto académico de la Universidad Tecnológica de Panamá, Desarrollo de Software IV, Parcial 3. Está pensado para usarse de verdad, no como una demostración: todo se puede operar con el teclado, las consultas son ligeras y los saldos se calculan siempre a partir de los pagos registrados.

## Qué hace

- **Clientes:** registrar, consultar, actualizar, desactivar y reactivar. Cada cliente tiene su límite y su plazo de crédito. Un cliente sin facturas se puede eliminar; si tiene documentos solo se desactiva.
- **Facturas:** facturas a crédito con fecha de vencimiento, ITBMS (7 %), total y saldo. Antes de guardar se comprueba que el cliente esté activo y que la factura no supere su límite de crédito. Se anulan con motivo; no se borran.
- **Pagos:** pagos totales o parciales aplicados a una factura. Un pago no puede superar el saldo de la factura. Se anulan con motivo, y el saldo de la factura vuelve a subir.
- **Reportes:** estado de cuenta por cliente y antigüedad de saldos (corriente, 1-30, 31-60, 61-90 y más de 90 días). Están dentro del módulo de Pagos, en la pestaña "Consultas y reportes".
- **Inicio:** saldo total pendiente, facturas con saldo y vencidas, clientes activos, distribución por antigüedad y los clientes con mayor saldo.

## Guía rápida de instalación

Necesita **Windows**, el **SDK de .NET 10** y **SQL Server Express** con la instancia `SQLEXPRESS` y autenticación de Windows. Si usa otra instancia (por ejemplo LocalDB), cambie `Server=` en `Kobra.UI\App.config`.

```powershell
# 1. Clonar el repositorio
git clone https://github.com/MiguelE0911/kobra.git
cd kobra

# 2. Crear la base de datos (se puede repetir sin perder datos)
sqlcmd -S 'localhost\SQLEXPRESS' -E -C -b -f 65001 -i Database\01_CrearBaseDatos.sql

# 3. (Opcional) Cargar datos de prueba para una demostración
sqlcmd -S 'localhost\SQLEXPRESS' -E -C -b -I -f 65001 -i Database\02_datos_prueba.sql

# 4. Ejecutar
dotnet run --project Kobra.UI
```

También puede abrir `kobra.sln` en Visual Studio y ejecutar el proyecto `Kobra.UI`.

Notas:

- Si `sqlcmd` no se reconoce, no está en el PATH. La guía [docs/base-de-datos.md](docs/base-de-datos.md) explica cómo encontrarlo, además de cómo instalar SQL Server y resolver los errores más frecuentes.
- `-C` es obligatorio (confía en el certificado del servidor) y `-f 65001` evita que se dañen las tildes.
- La cadena de conexión está en `Kobra.UI\App.config`. Nunca se escriben contraseñas en ella: usa la autenticación de Windows.
- Si la base no está disponible, la aplicación se abre igualmente y muestra el aviso en el inicio; no se cierra.

### Datos de prueba

`Database\02_datos_prueba.sql` carga 60 clientes (`DEMO001` a `DEMO060`) y unas 96 facturas con sus pagos. Cubre los cinco tramos de antigüedad, los cuatro estados de factura, pagos y facturas anuladas, clientes inactivos, un cliente cerca de su límite y otro por encima, plazos de 0 y 60 días, y facturas con varios pagos. Se puede ejecutar todas las veces que se quiera: solo reemplaza lo que empieza con `DEMO` y no toca los datos propios. Las fechas son relativas al día de ejecución.

## Cómo se usa

La ventana principal tiene un menú (**Inicio, Clientes, Facturas, Pagos, Salir**) y un panel de inicio. Cada módulo se abre como ventana hija y se puede tener uno de cada tipo a la vez; al cerrarlos, el inicio se actualiza con los saldos nuevos.

Un recorrido típico de demostración:

1. **Clientes:** buscar por código, nombre o cédula/RUC, o crear uno nuevo con su límite y plazo.
2. **Facturas:** pestaña *Nueva Factura*: elegir el cliente, el subtotal y el vencimiento (se sugiere según el plazo del cliente). El ITBMS se calcula solo. Si el total más el saldo del cliente supera su límite, la factura se rechaza.
3. **Pagos:** pestaña *Registrar Pago*: elegir el cliente, la factura con saldo y el monto. El estado de la factura pasa a `Parcial` o `Pagada`.
4. **Anulaciones:** desde *Anular Factura* o *Anular Pago*, con el motivo obligatorio. Una factura con pagos activos no se puede anular hasta anular sus pagos.
5. **Reportes:** en Pagos, pestaña *Consultas y reportes*, para el estado de cuenta de un cliente y la antigüedad de saldos.

Las búsquedas muestran como máximo 50 resultados; si hay más, aparece el aviso "Primeros 50: afine la búsqueda".

### Reglas de negocio

- Una factura vence en `fecha de emisión + plazo en días` del cliente.
- Antes de guardar: `saldo del cliente + total de la factura ≤ límite de crédito`. No se factura a un cliente inactivo.
- El saldo de una factura es su total menos los pagos activos, y nunca es negativo. El estado (`Pendiente`, `Parcial`, `Pagada`, `Anulada`) se calcula a partir del saldo; no se escribe a mano.
- Los documentos no se borran: se anulan con motivo (hasta 250 caracteres) y fecha.
- Los importes se manejan con `DECIMAL(18,2)` en SQL y `decimal` en C#. Moneda: balboa (B/.). Impuesto: ITBMS al 7 %.

## Atajos de teclado

### Ventana principal

| Atajo | Acción |
| --- | --- |
| `Ctrl+0` | Ir al inicio (cierra los módulos abiertos) |
| `Ctrl+1` | Abrir Clientes |
| `Ctrl+2` | Abrir Facturas |
| `Ctrl+3` | Abrir Pagos |
| `F5` | Actualizar el panel de inicio |
| `Alt` + letra subrayada | Opciones del menú (`Alt+I`, `Alt+C`, `Alt+F`, `Alt+P`, `Alt+S`) |
| `Tab` / `Enter` o `Espacio` | Moverse entre las tarjetas de módulo y abrir la enfocada |

### Dentro de un módulo

| Atajo | Acción |
| --- | --- |
| `F2` | Ir al campo de búsqueda (en Reportes, al botón Generar) |
| `F5` | Actualizar la consulta (en Reportes, regenera ambos) |
| `Ctrl+N` | Nuevo cliente, nueva factura o nuevo pago |
| `Ctrl+S` | Guardar (en Facturas y Pagos, solo desde la pestaña de registro) |
| `Esc` | Cerrar el módulo |
| `Enter` | En la búsqueda ejecuta *Buscar*; en una ficha, *Guardar* |
| `Alt` + letra subrayada | Botones de la barra inferior: `Nuevo`, `Guardar`, `Anular`, `Limpiar`, `Cerrar`, `Buscar`, `Generar` |

## Arquitectura

Cuatro proyectos en capas dentro de `kobra.sln`; cada capa solo conoce a la de abajo.

| Proyecto | Responsabilidad |
| --- | --- |
| `Kobra.UI` | Formularios Windows Forms. Sin SQL ni reglas de negocio |
| `Kobra.Negocio` | Validaciones y reglas: límite de crédito, aplicación de pagos, estados |
| `Kobra.Datos` | Acceso a datos con ADO.NET (`SqlConnection`, `SqlCommand`, `SqlDataReader`), consultas parametrizadas |
| `Kobra.Entidades` | Clases simples compartidas (`Cliente`, `Factura`, `Pago`, reportes) |

No se usa Entity Framework ni Dapper. El saldo y el estado de cada factura se leen de la vista `vw_FacturaSaldo`.

```
kobra/
├── Database/            Scripts SQL: 01_CrearBaseDatos.sql, 02_datos_prueba.sql
├── docs/                base-de-datos.md y plan-desarrollo.md
├── Kobra.UI/
│   ├── Principal/       Ventana principal y panel de inicio
│   ├── Clientes/        Módulo de clientes
│   ├── Facturas/        Módulo de facturas
│   ├── Pagos/           Módulo de pagos
│   ├── Reportes/        Estado de cuenta y antigüedad de saldos
│   └── Componentes/     Tema, formato y controles compartidos
├── Kobra.Negocio/
├── Kobra.Datos/
└── Kobra.Entidades/
```

## Alcance actual

Incluido: clientes, facturas, pagos, anulaciones con motivo, estado de cuenta, antigüedad de saldos y panel de inicio.

Todavía **no** incluido: inicio de sesión y roles, notas de crédito y débito, bitácora de usuario, gestiones de cobranza, exportación a Excel o PDF, intereses por mora. Tampoco la factura electrónica de la DGI, varias monedas ni la integración contable.

## Documentación

- [docs/base-de-datos.md](docs/base-de-datos.md): instalar SQL Server, conectarse a la base y resolver errores.
- [docs/plan-desarrollo.md](docs/plan-desarrollo.md): reparto de tareas, modelo de datos y supuestos del equipo.

## Licencia

MIT. Véase [LICENSE](LICENSE).
