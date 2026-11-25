### UNIVERSIDAD TECNOLÓGICA DE PANAMÁ
### CENTRO REGIONAL DE CHIRIQUÍ
### FACULTAD DE INGENIERÍA DE SISTEMAS COMPUTACIONALES

**CARRERA:** Licenciatura en Desarrollo y Gestión de Software

**PARCIAL #3:** Sistema empresarial: Cuentas por Cobrar (Kobra) — Código fuente

**ASIGNATURA:** Desarrollo de Software IV

**DOCENTE:** Marlenis Pimentel

**GRUPO:** 2GS221

**ESTUDIANTES:**

- Miguel Aparicio 4-836-632
- Wilma De Gracia 4-832-2462
- Brandon Arango 4-841-1242

**II SEMESTRE 2026**

**FECHA DE ENTREGA:** 25/11/2026

---

## CONTENIDO

- [Introducción](#introduccion)
- [1. Base de datos (SQL Server)](#1-base-de-datos-sql-server)
  - [Database/01_CrearBaseDatos.sql](#database01_crearbasedatossql)
  - [Database/02_datos_prueba.sql](#database02_datos_pruebasql)
- [2. Capa de entidades (Kobra.Entidades)](#2-capa-de-entidades-kobraentidades)
  - [Kobra.Entidades/Cliente.cs](#kobraentidadesclientecs)
  - [Kobra.Entidades/Factura.cs](#kobraentidadesfacturacs)
  - [Kobra.Entidades/Pago.cs](#kobraentidadespagocs)
  - [Kobra.Entidades/Reporte.cs](#kobraentidadesreportecs)
  - [Kobra.Entidades/ResumenCartera.cs](#kobraentidadesresumencarteracs)
- [3. Capa de acceso a datos (Kobra.Datos)](#3-capa-de-acceso-a-datos-kobradatos)
  - [Kobra.Datos/ClienteDatos.cs](#kobradatosclientedatoscs)
  - [Kobra.Datos/Conexion.cs](#kobradatosconexioncs)
  - [Kobra.Datos/DatosException.cs](#kobradatosdatosexceptioncs)
  - [Kobra.Datos/FacturaDatos.cs](#kobradatosfacturadatoscs)
  - [Kobra.Datos/PagoDatos.cs](#kobradatospagodatoscs)
  - [Kobra.Datos/ReporteDatos.cs](#kobradatosreportedatoscs)
  - [Kobra.Datos/ResumenDatos.cs](#kobradatosresumendatoscs)
- [4. Capa de negocio (Kobra.Negocio)](#4-capa-de-negocio-kobranegocio)
  - [Kobra.Negocio/BaseDatos.cs](#kobranegociobasedatoscs)
  - [Kobra.Negocio/ClienteNegocio.cs](#kobranegocioclientenegociocs)
  - [Kobra.Negocio/FacturaNegocio.cs](#kobranegociofacturanegociocs)
  - [Kobra.Negocio/PagoNegocio.cs](#kobranegociopagonegociocs)
  - [Kobra.Negocio/ReglaNegocioException.cs](#kobranegocioreglanegocioexceptioncs)
  - [Kobra.Negocio/ReporteNegocio.cs](#kobranegocioreportenegociocs)
  - [Kobra.Negocio/ResumenNegocio.cs](#kobranegocioresumennegociocs)
- [5. Capa de presentación (Kobra.UI)](#5-capa-de-presentacion-kobraui)
  - [Punto de entrada](#punto-de-entrada)
    - [Kobra.UI/Program.cs](#kobrauiprogramcs)
  - [Ventana principal y panel de inicio](#ventana-principal-y-panel-de-inicio)
    - [Kobra.UI/Principal/CtlInicio.cs](#kobrauiprincipalctliniciocs)
    - [Kobra.UI/Principal/FrmPrincipal.cs](#kobrauiprincipalfrmprincipalcs)
    - [Kobra.UI/Principal/FrmPrincipal.Designer.cs](#kobrauiprincipalfrmprincipaldesignercs)
    - [Kobra.UI/Principal/TarjetaModulo.cs](#kobrauiprincipaltarjetamodulocs)
  - [Módulo de Clientes](#modulo-de-clientes)
    - [Kobra.UI/Clientes/CtlClienteFicha.cs](#kobrauiclientesctlclientefichacs)
    - [Kobra.UI/Clientes/CtlClienteLista.cs](#kobrauiclientesctlclientelistacs)
    - [Kobra.UI/Clientes/FrmClientes.cs](#kobrauiclientesfrmclientescs)
    - [Kobra.UI/Clientes/FrmClientes.Designer.cs](#kobrauiclientesfrmclientesdesignercs)
  - [Módulo de Facturas](#modulo-de-facturas)
    - [Kobra.UI/Facturas/CtlFacturaAnulacion.cs](#kobrauifacturasctlfacturaanulacioncs)
    - [Kobra.UI/Facturas/CtlFacturaConsulta.cs](#kobrauifacturasctlfacturaconsultacs)
    - [Kobra.UI/Facturas/CtlFacturaFicha.cs](#kobrauifacturasctlfacturafichacs)
    - [Kobra.UI/Facturas/FrmFacturas.cs](#kobrauifacturasfrmfacturascs)
    - [Kobra.UI/Facturas/FrmFacturas.Designer.cs](#kobrauifacturasfrmfacturasdesignercs)
  - [Módulo de Pagos](#modulo-de-pagos)
    - [Kobra.UI/Pagos/CtlPagoAnulacion.cs](#kobrauipagosctlpagoanulacioncs)
    - [Kobra.UI/Pagos/CtlPagoConsulta.cs](#kobrauipagosctlpagoconsultacs)
    - [Kobra.UI/Pagos/CtlPagoRegistro.cs](#kobrauipagosctlpagoregistrocs)
    - [Kobra.UI/Pagos/FrmPagos.cs](#kobrauipagosfrmpagoscs)
    - [Kobra.UI/Pagos/FrmPagos.Designer.cs](#kobrauipagosfrmpagosdesignercs)
  - [Reportes](#reportes)
    - [Kobra.UI/Reportes/CtlReporteAntiguedad.cs](#kobrauireportesctlreporteantiguedadcs)
    - [Kobra.UI/Reportes/CtlReporteCuenta.cs](#kobrauireportesctlreportecuentacs)
  - [Componentes compartidos](#componentes-compartidos)
    - [Kobra.UI/Componentes/BarraAcciones.cs](#kobrauicomponentesbarraaccionescs)
    - [Kobra.UI/Componentes/BarraTituloModulo.cs](#kobrauicomponentesbarratitulomodulocs)
    - [Kobra.UI/Componentes/BarraTotales.cs](#kobrauicomponentesbarratotalescs)
    - [Kobra.UI/Componentes/BotonTema.cs](#kobrauicomponentesbotontemacs)
    - [Kobra.UI/Componentes/CampoFicha.cs](#kobrauicomponentescampofichacs)
    - [Kobra.UI/Componentes/DialogoConfirmacion.cs](#kobrauicomponentesdialogoconfirmacioncs)
    - [Kobra.UI/Componentes/EstiloGrid.cs](#kobrauicomponentesestilogridcs)
    - [Kobra.UI/Componentes/Formato.cs](#kobrauicomponentesformatocs)
    - [Kobra.UI/Componentes/FormularioConVistas.cs](#kobrauicomponentesformularioconvistascs)
    - [Kobra.UI/Componentes/FormularioModulo.cs](#kobrauicomponentesformulariomodulocs)
    - [Kobra.UI/Componentes/ItemCliente.cs](#kobrauicomponentesitemclientecs)
    - [Kobra.UI/Componentes/Mensajes.cs](#kobrauicomponentesmensajescs)
    - [Kobra.UI/Componentes/TarjetaMotivo.cs](#kobrauicomponentestarjetamotivocs)
    - [Kobra.UI/Componentes/TarjetaPanel.cs](#kobrauicomponentestarjetapanelcs)
    - [Kobra.UI/Componentes/Tema.cs](#kobrauicomponentestemacs)

---

<a id="introduccion"></a>

## Introducción

Este documento reúne el código fuente completo de **Kobra**, el sistema de cuentas por cobrar desarrollado en C# con Windows Forms y SQL Server, con acceso a datos por ADO.NET sobre .NET 10.

El sistema está dividido en capas, cada una en su propio proyecto de la solución. Cada capa solo conoce a la que tiene debajo:

```text
Kobra.UI  →  Kobra.Negocio  →  Kobra.Datos  →  Kobra.Entidades
```

| Carpeta | Contenido | Archivos | Líneas |
|---|---|---:|---:|
| `Database/` | Scripts SQL: creación de tablas, claves, índices y vista de saldos; datos de prueba | 2 | 485 |
| `Kobra.Entidades` | Clases simples compartidas por todas las capas | 5 | 188 |
| `Kobra.Datos` | Acceso a SQL Server con ADO.NET (consultas parametrizadas, transacciones) | 7 | 1,097 |
| `Kobra.Negocio` | Validaciones y reglas de negocio | 7 | 472 |
| `Kobra.UI` | Formularios y controles de Windows Forms | 36 | 5,302 |
| **Total** | | **57** | **7,544** |

Se incluyen todos los archivos `.cs` y `.sql` del proyecto, agrupados por capa en el orden de dependencia. Los archivos de proyecto (`.csproj`), la configuración (`App.config`) y los recursos (`.resx`) no se incluyen. Para mantener la lectura ágil se omitieron los comentarios, salvo una breve descripción de cada clase y de sus miembros públicos.

---

<a id="1-base-de-datos-sql-server"></a>

## 1. Base de datos (SQL Server)

<a id="database01_crearbasedatossql"></a>

### Database/01_CrearBaseDatos.sql

```sql
/*
  Kobra - Script de creación de la base de datos
  Tablas: Cliente, Factura, Pago. Vista: vw_FacturaSaldo.

  Es idempotente: se puede ejecutar varias veces sin perder datos.

  Cómo ejecutarlo:
    - SSMS / Azure Data Studio: abrir el archivo y ejecutar (F5).
    - Línea de comandos (instancia SQL Express):
        sqlcmd -S .\SQLEXPRESS -E -f 65001 -i Database\01_CrearBaseDatos.sql
    - LocalDB:
        sqlcmd -S "(localdb)\MSSQLLocalDB" -E -f 65001 -i Database\01_CrearBaseDatos.sql
*/

SET QUOTED_IDENTIFIER ON;
SET ANSI_NULLS ON;
GO

IF DB_ID(N'Kobra') IS NULL
    CREATE DATABASE Kobra;
GO

USE Kobra;
GO

/* ---------------------------------------------------------------- Cliente */
IF OBJECT_ID(N'dbo.Cliente', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Cliente
    (
        Id                  INT IDENTITY(1,1)  NOT NULL,
        Codigo              NVARCHAR(20)       NOT NULL,
        Nombre              NVARCHAR(150)      NOT NULL,
        RucCedula           NVARCHAR(30)       NULL,
        Telefono            NVARCHAR(30)       NULL,
        Email               NVARCHAR(150)      NULL,
        Direccion           NVARCHAR(250)      NULL,
        LimiteCredito       DECIMAL(18,2)      NOT NULL CONSTRAINT DF_Cliente_LimiteCredito DEFAULT (0),
        PlazoDias           INT                NOT NULL CONSTRAINT DF_Cliente_PlazoDias DEFAULT (30),
        Activo              BIT                NOT NULL CONSTRAINT DF_Cliente_Activo DEFAULT (1),
        FechaCreacion       DATETIME2(0)       NOT NULL CONSTRAINT DF_Cliente_FechaCreacion DEFAULT (SYSDATETIME()),
        FechaModificacion   DATETIME2(0)       NULL,

        CONSTRAINT PK_Cliente PRIMARY KEY CLUSTERED (Id),
        CONSTRAINT UQ_Cliente_Codigo UNIQUE (Codigo),
        CONSTRAINT CK_Cliente_LimiteCredito CHECK (LimiteCredito >= 0),
        CONSTRAINT CK_Cliente_PlazoDias CHECK (PlazoDias >= 0),
        CONSTRAINT CK_Cliente_Codigo CHECK (LEN(LTRIM(RTRIM(Codigo))) > 0),
        CONSTRAINT CK_Cliente_Nombre CHECK (LEN(LTRIM(RTRIM(Nombre))) > 0)
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Cliente_Nombre' AND object_id = OBJECT_ID(N'dbo.Cliente'))
    CREATE NONCLUSTERED INDEX IX_Cliente_Nombre ON dbo.Cliente (Nombre);
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'UX_Cliente_RucCedula' AND object_id = OBJECT_ID(N'dbo.Cliente'))
    CREATE UNIQUE NONCLUSTERED INDEX UX_Cliente_RucCedula
        ON dbo.Cliente (RucCedula) WHERE RucCedula IS NOT NULL;
GO

/* ---------------------------------------------------------------- Factura */
IF OBJECT_ID(N'dbo.Factura', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Factura
    (
        Id                  INT IDENTITY(1,1)  NOT NULL,
        Numero              NVARCHAR(30)       NOT NULL,
        ClienteId           INT                NOT NULL,
        FechaEmision        DATE               NOT NULL,
        FechaVencimiento    DATE               NOT NULL,
        Subtotal            DECIMAL(18,2)      NOT NULL,
        Itbms               DECIMAL(18,2)      NOT NULL CONSTRAINT DF_Factura_Itbms DEFAULT (0),
        Total               DECIMAL(18,2)      NOT NULL,
        Anulada             BIT                NOT NULL CONSTRAINT DF_Factura_Anulada DEFAULT (0),
        MotivoAnulacion     NVARCHAR(250)      NULL,
        FechaAnulacion      DATETIME2(0)       NULL,
        FechaCreacion       DATETIME2(0)       NOT NULL CONSTRAINT DF_Factura_FechaCreacion DEFAULT (SYSDATETIME()),

        CONSTRAINT PK_Factura PRIMARY KEY CLUSTERED (Id),
        CONSTRAINT UQ_Factura_Numero UNIQUE (Numero),
        CONSTRAINT FK_Factura_Cliente FOREIGN KEY (ClienteId) REFERENCES dbo.Cliente (Id),
        CONSTRAINT CK_Factura_Fechas CHECK (FechaVencimiento >= FechaEmision),
        CONSTRAINT CK_Factura_Montos CHECK (Subtotal >= 0 AND Itbms >= 0 AND Total > 0 AND Total = Subtotal + Itbms),
        CONSTRAINT CK_Factura_Anulacion CHECK (Anulada = 0 OR (MotivoAnulacion IS NOT NULL AND FechaAnulacion IS NOT NULL))
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Factura_ClienteId' AND object_id = OBJECT_ID(N'dbo.Factura'))
    CREATE NONCLUSTERED INDEX IX_Factura_ClienteId ON dbo.Factura (ClienteId);
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Factura_FechaVencimiento' AND object_id = OBJECT_ID(N'dbo.Factura'))
    CREATE NONCLUSTERED INDEX IX_Factura_FechaVencimiento ON dbo.Factura (FechaVencimiento);
GO

/* ------------------------------------------------------------------- Pago */
IF OBJECT_ID(N'dbo.Pago', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Pago
    (
        Id                  INT IDENTITY(1,1)  NOT NULL,
        FacturaId           INT                NOT NULL,
        FechaPago           DATE               NOT NULL,
        Monto               DECIMAL(18,2)      NOT NULL,
        Anulado             BIT                NOT NULL CONSTRAINT DF_Pago_Anulado DEFAULT (0),
        MotivoAnulacion     NVARCHAR(250)      NULL,
        FechaAnulacion      DATETIME2(0)       NULL,
        FechaCreacion       DATETIME2(0)       NOT NULL CONSTRAINT DF_Pago_FechaCreacion DEFAULT (SYSDATETIME()),

        CONSTRAINT PK_Pago PRIMARY KEY CLUSTERED (Id),
        CONSTRAINT FK_Pago_Factura FOREIGN KEY (FacturaId) REFERENCES dbo.Factura (Id),
        CONSTRAINT CK_Pago_Monto CHECK (Monto > 0),
        CONSTRAINT CK_Pago_Anulacion CHECK (Anulado = 0 OR (MotivoAnulacion IS NOT NULL AND FechaAnulacion IS NOT NULL))
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Pago_FacturaId' AND object_id = OBJECT_ID(N'dbo.Pago'))
    CREATE NONCLUSTERED INDEX IX_Pago_FacturaId ON dbo.Pago (FacturaId);
GO

/* ------------------------------------------------------- vw_FacturaSaldo
   Fuente única del saldo y el estado de cada factura.
   Estado: Anulada, Pagada (saldo = 0), Parcial (hay pagos y queda saldo) o Pendiente.
   Vencida no es un estado: es un indicador calculado (EstaVencida / DiasVencidos).
*/
CREATE OR ALTER VIEW dbo.vw_FacturaSaldo
AS
SELECT
    f.Id,
    f.Numero,
    f.ClienteId,
    f.FechaEmision,
    f.FechaVencimiento,
    f.Total,
    ISNULL(p.TotalPagado, 0)                                   AS TotalPagado,
    CASE WHEN f.Anulada = 1 THEN 0
         ELSE f.Total - ISNULL(p.TotalPagado, 0) END           AS Saldo,
    CASE WHEN f.Anulada = 1                          THEN N'Anulada'
         WHEN f.Total - ISNULL(p.TotalPagado, 0) <= 0 THEN N'Pagada'
         WHEN ISNULL(p.TotalPagado, 0) > 0           THEN N'Parcial'
         ELSE N'Pendiente' END                                  AS Estado,
    CAST(CASE WHEN f.Anulada = 0
                   AND f.Total - ISNULL(p.TotalPagado, 0) > 0
                   AND f.FechaVencimiento < CAST(SYSDATETIME() AS DATE)
              THEN 1 ELSE 0 END AS BIT)                         AS EstaVencida,
    CASE WHEN f.Anulada = 0
              AND f.Total - ISNULL(p.TotalPagado, 0) > 0
              AND f.FechaVencimiento < CAST(SYSDATETIME() AS DATE)
         THEN DATEDIFF(DAY, f.FechaVencimiento, CAST(SYSDATETIME() AS DATE))
         ELSE 0 END                                             AS DiasVencidos
FROM dbo.Factura AS f
OUTER APPLY
(
    SELECT SUM(pg.Monto) AS TotalPagado
    FROM dbo.Pago AS pg
    WHERE pg.FacturaId = f.Id AND pg.Anulado = 0
) AS p;
GO
```

<a id="database02_datos_pruebasql"></a>

### Database/02_datos_prueba.sql

```sql
/*
  Kobra - Datos de prueba (fixture) para la demostración de Clientes, Facturas,
  Pagos y reportes. Es el archivo que usa todo el equipo.

  Ejecutar DESPUÉS de 01_CrearBaseDatos.sql, desde la raíz del repositorio:

      sqlcmd -S .\SQLEXPRESS -E -C -b -I -f 65001 -i Database\02_datos_prueba.sql

  Las opciones importan:
    -I        activa QUOTED_IDENTIFIER. Sin ella el MERGE sobre dbo.Cliente falla
              (error 1934) por el índice filtrado de RucCedula.
    -f 65001  el archivo es UTF-8 sin BOM; sin ella "Compañía" sale dañado.
    -b        si algo falla, sqlcmd termina con código de salida distinto de cero.

  Es idempotente: se puede ejecutar varias veces. Primero retira las facturas
  F-DEMO-% (con sus pagos) y luego crea o actualiza los clientes DEMO%. No toca
  los datos que cada integrante haya registrado con otros códigos o números.

  Los datos son deterministas (sin RAND): todos los integrantes ven lo mismo.
  Las fechas son relativas al día de ejecución, así que la antigüedad de saldos
  es válida en cualquier fecha. El ITBMS de las facturas nuevas es el 7 % redondeado
  como lo hace la pantalla de facturas; F-DEMO-001..008 conservan sus cifras
  originales.

  Contenido:
    DEMO001-DEMO004  Casos básicos (parcial vencida, pagada, anulada, inactivo con
                     saldo de más de 90 días). F-DEMO-001..008.
    DEMO005-DEMO012  Casos con nombre propio: cerca del límite, sin facturas,
                     sobre el límite, plazo 60, contado, inactivo sin saldo,
                     redondeo de ITBMS y pagos múltiples. F-DEMO-009..024.
    DEMO013-DEMO060  48 clientes generados con 1 o 2 facturas cada uno, para que
                     la búsqueda supere el tope de 50 resultados y el top 8 del
                     inicio tenga de dónde elegir. F-DEMO-Bnnn.

  Cobertura: los cinco tramos de antigüedad, los cuatro estados de factura
  (Pendiente, Parcial, Pagada, Anulada), pagos anulados, clientes inactivos
  y clientes por encima del límite de crédito.
*/

USE Kobra;
GO

SET QUOTED_IDENTIFIER ON;
SET ANSI_NULLS ON;
GO

SET NOCOUNT ON;
SET XACT_ABORT ON;
GO

BEGIN TRY
    BEGIN TRANSACTION;

    /* ------------------------------------------ 1. Limpieza de datos demo */
    DELETE p
      FROM dbo.Pago AS p
      INNER JOIN dbo.Factura AS f ON f.Id = p.FacturaId
     WHERE f.Numero LIKE N'F-DEMO-%';

    DELETE FROM dbo.Factura
     WHERE Numero LIKE N'F-DEMO-%';

    /* ------------------------------------------------ 2. Clientes DEMO001-060 */
    ;WITH Curados (Codigo, Nombre, RucCedula, Telefono, Email, Direccion, LimiteCredito, PlazoDias, Activo)
    AS
    (
        SELECT * FROM (VALUES
            (N'DEMO001', N'Compañía Demo, S.A.',        N'155000001001', N'507-2200-1101', N'cobros@demosa.example',       N'Ciudad de Panamá', 50000.00, 30, 1),
            (N'DEMO002', N'Distribuidora Demo',         N'155000002002', N'507-2200-1102', N'cartera@distdemo.example',    N'Colón',            40000.00, 30, 1),
            (N'DEMO003', N'Servicios Demo',             N'155000003003', N'507-2200-1103', N'facturacion@servdemo.example', N'David',            30000.00, 15, 1),
            (N'DEMO004', N'Cliente Inactivo Demo',      N'155000004004', N'507-2200-1104', N'contacto@demo4.example',      N'Chitré',           20000.00, 30, 0),
            (N'DEMO005', N'Comercial Límite Justo',     N'155000005005', N'507-2200-1105', N'compras@limitejusto.example', N'Santiago',          5000.00, 30, 1),
            (N'DEMO006', N'Cliente Sin Movimientos',    N'155000006006', N'507-2200-1106', N'info@sinmov.example',         N'Las Tablas',       10000.00, 30, 1),
            (N'DEMO007', N'Importadora Sobre el Límite',N'155000007007', N'507-2200-1107', N'pagos@sobrelimite.example',   N'Penonomé',          3000.00, 30, 1),
            (N'DEMO008', N'Mayorista Plazo 60',         N'155000008008', N'507-2200-1108', N'tesoreria@mayorista60.example',N'Aguadulce',        25000.00, 60, 1),
            (N'DEMO009', N'Tienda de Contado',          N'155000009009', N'507-2200-1109', N'caja@contado.example',        N'La Chorrera',       8000.00,  0, 1),
            (N'DEMO010', N'Inactivo Sin Saldo',         N'155000010010', N'507-2200-1110', N'archivo@inactivo.example',    N'Bocas del Toro',   15000.00, 30, 0),
            (N'DEMO011', N'Redondeo ITBMS, S.A.',       N'155000011011', N'507-2200-1111', N'contabilidad@redondeo.example',N'Ciudad de Panamá', 20000.00, 30, 1),
            (N'DEMO012', N'Pagos Múltiples, S.A.',      N'155000012012', N'507-2200-1112', N'cobros@multiples.example',    N'Colón',            30000.00, 30, 1)
        ) AS v (Codigo, Nombre, RucCedula, Telefono, Email, Direccion, LimiteCredito, PlazoDias, Activo)
    ),
    Numeros (n) AS
    (
        SELECT TOP (48) CAST(12 + ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS INT) FROM sys.all_objects
    ),
    Demo AS
    (
        SELECT Codigo, Nombre, RucCedula, Telefono, Email, Direccion,
               CAST(LimiteCredito AS DECIMAL(18,2)) AS LimiteCredito,
               CAST(PlazoDias AS INT) AS PlazoDias, CAST(Activo AS BIT) AS Activo
          FROM Curados
        UNION ALL
        SELECT N'DEMO' + RIGHT(CONCAT(N'000', n), 3),
               N'Cliente Demo ' + RIGHT(CONCAT(N'000', n), 3),
               N'1550' + RIGHT(CONCAT(N'00000000', n), 8),
               N'507-2300-' + RIGHT(CONCAT(N'0000', n), 4),
               N'cliente' + CAST(n AS NVARCHAR(3)) + N'@demo.example',
               CHOOSE(n % 6 + 1, N'Ciudad de Panamá', N'Colón', N'David', N'Santiago', N'Chitré', N'La Chorrera'),
               CAST(10000 + (n % 5) * 5000 AS DECIMAL(18,2)),
               CHOOSE(n % 3 + 1, 15, 30, 45),
               CAST(CASE WHEN n % 9 = 0 THEN 0 ELSE 1 END AS BIT)
          FROM Numeros
    )
    MERGE dbo.Cliente AS dest
    USING Demo AS src ON dest.Codigo = src.Codigo
    WHEN MATCHED THEN
        UPDATE SET Nombre = src.Nombre,
                   RucCedula = src.RucCedula,
                   Telefono = src.Telefono,
                   Email = src.Email,
                   Direccion = src.Direccion,
                   LimiteCredito = src.LimiteCredito,
                   PlazoDias = src.PlazoDias,
                   Activo = src.Activo,
                   FechaModificacion = SYSDATETIME()
    WHEN NOT MATCHED THEN
        INSERT (Codigo, Nombre, RucCedula, Telefono, Email, Direccion, LimiteCredito, PlazoDias, Activo)
        VALUES (src.Codigo, src.Nombre, src.RucCedula, src.Telefono, src.Email, src.Direccion,
                src.LimiteCredito, src.PlazoDias, src.Activo);

    /* --------------------------------------------------------- 3. Facturas */
    -- Emi y Ven son días respecto a hoy (negativo = pasado). Itbms NULL = 7 % redondeado.
    -- N es el índice del cliente generado; NULL en las facturas con nombre propio.
    DECLARE @Fac TABLE
    (
        Numero   NVARCHAR(30)  NOT NULL,
        Codigo   NVARCHAR(20)  NOT NULL,
        Emi      INT           NOT NULL,
        Ven      INT           NOT NULL,
        Subtotal DECIMAL(18,2) NOT NULL,
        Itbms    DECIMAL(18,2) NULL,
        Anulada  BIT           NOT NULL,
        Motivo   NVARCHAR(250) NULL,
        AnulDias INT           NULL,
        N        INT           NULL
    );

    INSERT INTO @Fac (Numero, Codigo, Emi, Ven, Subtotal, Itbms, Anulada, Motivo, AnulDias)
    VALUES
        -- DEMO001-004: casos básicos (cifras originales)
        (N'F-DEMO-001', N'DEMO001',  -40,  -10, 1121.50,  78.50, 0, NULL, NULL),
        (N'F-DEMO-002', N'DEMO001',   -5,   25, 2336.45, 163.55, 0, NULL, NULL),
        (N'F-DEMO-003', N'DEMO001',  -70,  -40, 2803.74, 196.26, 0, NULL, NULL),
        (N'F-DEMO-004', N'DEMO002', -100,  -70, 1682.24, 117.76, 0, NULL, NULL),
        (N'F-DEMO-005', N'DEMO002', -105,  -75, 4205.61, 294.39, 0, NULL, NULL),
        (N'F-DEMO-006', N'DEMO003',  -20,   10,  887.85,  62.15, 0, NULL, NULL),
        (N'F-DEMO-007', N'DEMO004', -150, -120, 2056.07, 143.93, 0, NULL, NULL),
        (N'F-DEMO-008', N'DEMO003',  -60,  -30, 1028.04,  71.96, 1, N'Factura de prueba anulada', -29),
        -- DEMO005: cerca del límite (saldo 4 815,00 de 5 000,00)
        (N'F-DEMO-009', N'DEMO005',  -12,   18, 2800.00, NULL, 0, NULL, NULL),
        (N'F-DEMO-010', N'DEMO005',  -45,  -15, 1700.00, NULL, 0, NULL, NULL),
        -- DEMO007: saldo por encima del límite (4 494,00 sobre 3 000,00)
        (N'F-DEMO-011', N'DEMO007',  -50,  -20, 2000.00, NULL, 0, NULL, NULL),
        (N'F-DEMO-012', N'DEMO007',  -80,  -50, 2200.00, NULL, 0, NULL, NULL),
        -- DEMO008: plazo de 60 días
        (N'F-DEMO-013', N'DEMO008',  -30,   30, 6000.00, NULL, 0, NULL, NULL),
        (N'F-DEMO-014', N'DEMO008',  -75,  -15, 3500.00, NULL, 0, NULL, NULL),
        -- DEMO009: contado (vence el mismo día)
        (N'F-DEMO-015', N'DEMO009',    0,    0,  450.00, NULL, 0, NULL, NULL),
        (N'F-DEMO-016', N'DEMO009',   -3,   -3,  800.00, NULL, 0, NULL, NULL),
        -- DEMO010: inactivo, todo pagado
        (N'F-DEMO-017', N'DEMO010',  -90,  -60, 5000.00, NULL, 0, NULL, NULL),
        (N'F-DEMO-018', N'DEMO010',  -40,  -10, 1000.00, NULL, 0, NULL, NULL),
        -- DEMO011: redondeo de ITBMS (78,505 -> 78,51; 143,9249 -> 143,92; 6,9951 -> 7,00)
        (N'F-DEMO-019', N'DEMO011',  -10,   20, 1121.50, NULL, 0, NULL, NULL),
        (N'F-DEMO-020', N'DEMO011',  -35,   -5,  333.33, NULL, 0, NULL, NULL),
        (N'F-DEMO-021', N'DEMO011',  -65,  -35, 2056.07, NULL, 0, NULL, NULL),
        (N'F-DEMO-022', N'DEMO011', -100,  -70,   99.93, NULL, 0, NULL, NULL),
        -- DEMO012: varios pagos
        (N'F-DEMO-023', N'DEMO012',  -60,  -30, 4000.00, NULL, 0, NULL, NULL),
        (N'F-DEMO-024', N'DEMO012',  -25,    5, 3000.00, NULL, 0, NULL, NULL);

    -- DEMO013-060: una factura A por cliente y una B en los pares. El plazo es el del cliente.
    ;WITH Numeros (n) AS
    (
        SELECT TOP (48) CAST(12 + ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS INT) FROM sys.all_objects
    )
    INSERT INTO @Fac (Numero, Codigo, Emi, Ven, Subtotal, Itbms, Anulada, Motivo, AnulDias, N)
    SELECT N'F-DEMO-B' + RIGHT(CONCAT(N'000', n), 3) + N'A',
           N'DEMO' + RIGHT(CONCAT(N'000', n), 3),
           -((n * 11) % 120) - 2,
           -((n * 11) % 120) - 2 + CHOOSE(n % 3 + 1, 15, 30, 45),
           CAST(300 + (n * 137) % 2700 + (n % 100) / 100.0 AS DECIMAL(18,2)),
           NULL, 0, NULL, NULL, n
      FROM Numeros;

    ;WITH Numeros (n) AS
    (
        SELECT TOP (48) CAST(12 + ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS INT) FROM sys.all_objects
    )
    INSERT INTO @Fac (Numero, Codigo, Emi, Ven, Subtotal, Itbms, Anulada, Motivo, AnulDias, N)
    SELECT N'F-DEMO-B' + RIGHT(CONCAT(N'000', n), 3) + N'B',
           N'DEMO' + RIGHT(CONCAT(N'000', n), 3),
           -((n * 5) % 60) - 1,
           -((n * 5) % 60) - 1 + CHOOSE(n % 3 + 1, 15, 30, 45),
           CAST(200 + (n * 89) % 1500 + (n % 100) / 100.0 AS DECIMAL(18,2)),
           NULL, 0, NULL, NULL, n
      FROM Numeros
     WHERE n % 2 = 0;

    UPDATE @Fac SET Itbms = ROUND(Subtotal * 0.07, 2) WHERE Itbms IS NULL;

    INSERT INTO dbo.Factura
        (Numero, ClienteId, FechaEmision, FechaVencimiento, Subtotal, Itbms, Total,
         Anulada, MotivoAnulacion, FechaAnulacion)
    SELECT f.Numero, c.Id,
           DATEADD(DAY, f.Emi, CAST(GETDATE() AS DATE)),
           DATEADD(DAY, f.Ven, CAST(GETDATE() AS DATE)),
           f.Subtotal, f.Itbms, f.Subtotal + f.Itbms,
           f.Anulada, f.Motivo,
           CASE WHEN f.Anulada = 1 THEN DATEADD(DAY, f.AnulDias, CAST(GETDATE() AS DATETIME2(0))) END
      FROM @Fac AS f
      INNER JOIN dbo.Cliente AS c ON c.Codigo = f.Codigo;

    /* --------------------------------------------- 4. Pagos activos/anulados */
    -- Dias es respecto a hoy. Un pago nunca supera el saldo de su factura.
    DECLARE @Pag TABLE
    (
        Numero   NVARCHAR(30)  NOT NULL,
        Dias     INT           NOT NULL,
        Monto    DECIMAL(18,2) NOT NULL,
        Anulado  BIT           NOT NULL,
        Motivo   NVARCHAR(250) NULL,
        AnulDias INT           NULL
    );

    INSERT INTO @Pag (Numero, Dias, Monto, Anulado, Motivo, AnulDias)
    VALUES
        (N'F-DEMO-001',   -8,  400.00, 0, NULL, NULL),
        (N'F-DEMO-001',   -6,  200.00, 1, N'Depósito rechazado por el banco', -5),
        (N'F-DEMO-003',  -35, 1000.00, 0, NULL, NULL),
        (N'F-DEMO-003',  -30,  500.00, 1, N'Pago duplicado registrado por el cliente', -28),
        (N'F-DEMO-004',  -65, 1800.00, 0, NULL, NULL),
        (N'F-DEMO-005', -100, 1500.00, 0, NULL, NULL),
        -- F-DEMO-013: abono parcial
        (N'F-DEMO-013',  -10, 2000.00, 0, NULL, NULL),
        -- F-DEMO-017 y 018: pagadas
        (N'F-DEMO-017',  -80, 3000.00, 0, NULL, NULL),
        (N'F-DEMO-017',  -70, 2350.00, 0, NULL, NULL),
        (N'F-DEMO-018',  -30, 1070.00, 0, NULL, NULL),
        -- F-DEMO-023: dos pagos activos y uno anulado; queda pagada (4 280,00)
        (N'F-DEMO-023',  -50, 2500.00, 0, NULL, NULL),
        (N'F-DEMO-023',  -45,  500.00, 1, N'Pago aplicado por error a otra factura', -44),
        (N'F-DEMO-023',  -40, 1780.00, 0, NULL, NULL),
        -- F-DEMO-024: tres abonos; queda parcial (saldo 910,00)
        (N'F-DEMO-024',  -20, 1000.00, 0, NULL, NULL),
        (N'F-DEMO-024',  -12,  800.00, 0, NULL, NULL),
        (N'F-DEMO-024',   -4,  500.00, 0, NULL, NULL);

    -- Clientes generados: pagan completo los múltiplos de 5 y abonan el 40 % los múltiplos de 3.
    INSERT INTO @Pag (Numero, Dias, Monto, Anulado, Motivo, AnulDias)
    SELECT f.Numero, f.Emi + 1,
           CAST(CASE WHEN f.N % 5 = 0 THEN f.Subtotal + f.Itbms
                     ELSE ROUND((f.Subtotal + f.Itbms) * 0.4, 2) END AS DECIMAL(18,2)),
           0, NULL, NULL
      FROM @Fac AS f
     WHERE f.N IS NOT NULL AND RIGHT(f.Numero, 1) = N'A' AND (f.N % 5 = 0 OR f.N % 3 = 0);

    INSERT INTO dbo.Pago (FacturaId, FechaPago, Monto, Anulado, MotivoAnulacion, FechaAnulacion)
    SELECT fa.Id, DATEADD(DAY, p.Dias, CAST(GETDATE() AS DATE)), p.Monto, p.Anulado, p.Motivo,
           CASE WHEN p.Anulado = 1 THEN DATEADD(DAY, p.AnulDias, CAST(GETDATE() AS DATETIME2(0))) END
      FROM @Pag AS p
      INNER JOIN dbo.Factura AS fa ON fa.Numero = p.Numero;

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF XACT_STATE() <> 0
        ROLLBACK TRANSACTION;

    DECLARE @Error nvarchar(4000) = ERROR_MESSAGE();
    RAISERROR(N'No se pudieron cargar los datos de prueba: %s', 16, 1, @Error);
END CATCH;
GO

/* --------------------------------------------------------- Verificación */
-- Falla (con -b, sqlcmd devuelve código distinto de cero) si el fixture no cubre lo prometido.
IF EXISTS (SELECT 1 FROM dbo.vw_FacturaSaldo WHERE Saldo < 0)
    RAISERROR(N'Verificación: hay facturas con saldo negativo.', 16, 1);

IF (SELECT COUNT(DISTINCT CASE WHEN DiasVencidos = 0 THEN 0 WHEN DiasVencidos <= 30 THEN 1
                               WHEN DiasVencidos <= 60 THEN 2 WHEN DiasVencidos <= 90 THEN 3 ELSE 4 END)
      FROM dbo.vw_FacturaSaldo
     WHERE Numero LIKE N'F-DEMO-%' AND Saldo > 0) <> 5
    RAISERROR(N'Verificación: faltan facturas con saldo en alguno de los 5 tramos de antigüedad.', 16, 1);

IF (SELECT COUNT(DISTINCT Estado) FROM dbo.vw_FacturaSaldo WHERE Numero LIKE N'F-DEMO-%') <> 4
    RAISERROR(N'Verificación: faltan estados de factura (Pendiente, Parcial, Pagada, Anulada).', 16, 1);

IF (SELECT COUNT(*) FROM dbo.Cliente WHERE Codigo LIKE N'DEMO%') <> 60
    RAISERROR(N'Verificación: se esperaban 60 clientes DEMO.', 16, 1);
GO

-- Resumen por tramo de antigüedad (todas las facturas con saldo de la base)
SELECT
    ISNULL(SUM(CASE WHEN DiasVencidos = 0 THEN Saldo END), 0) AS Corriente,
    ISNULL(SUM(CASE WHEN DiasVencidos BETWEEN 1 AND 30 THEN Saldo END), 0) AS Tramo1a30,
    ISNULL(SUM(CASE WHEN DiasVencidos BETWEEN 31 AND 60 THEN Saldo END), 0) AS Tramo31a60,
    ISNULL(SUM(CASE WHEN DiasVencidos BETWEEN 61 AND 90 THEN Saldo END), 0) AS Tramo61a90,
    ISNULL(SUM(CASE WHEN DiasVencidos > 90 THEN Saldo END), 0) AS TramoMasDe90,
    ISNULL(SUM(Saldo), 0) AS SaldoTotal
  FROM dbo.vw_FacturaSaldo
 WHERE Saldo > 0;

-- Facturas demo por estado
SELECT Estado, COUNT(*) AS Facturas, ISNULL(SUM(Saldo), 0) AS Saldo
  FROM dbo.vw_FacturaSaldo
 WHERE Numero LIKE N'F-DEMO-%'
 GROUP BY Estado
 ORDER BY Estado;

-- Clientes con nombre propio (DEMO001-DEMO012)
SELECT c.Codigo, c.Nombre, c.Activo, c.LimiteCredito,
       COUNT(v.Id) AS Facturas,
       ISNULL(SUM(v.Total), 0) AS Facturado,
       ISNULL(SUM(v.TotalPagado), 0) AS Cobrado,
       ISNULL(SUM(v.Saldo), 0) AS Saldo
  FROM dbo.Cliente AS c
  LEFT JOIN dbo.vw_FacturaSaldo AS v ON v.ClienteId = c.Id
 WHERE c.Codigo LIKE N'DEMO0[01][0-9]' AND c.Codigo <= N'DEMO012'
 GROUP BY c.Codigo, c.Nombre, c.Activo, c.LimiteCredito
 ORDER BY c.Codigo;
GO
```

---

<a id="2-capa-de-entidades-kobraentidades"></a>

## 2. Capa de entidades (Kobra.Entidades)

<a id="kobraentidadesclientecs"></a>

### Kobra.Entidades/Cliente.cs

```csharp
namespace Kobra.Entidades;

public class Cliente
{
    public int Id { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string? RucCedula { get; set; }
    public string? Telefono { get; set; }
    public string? Email { get; set; }
    public string? Direccion { get; set; }
    public decimal LimiteCredito { get; set; }
    public int PlazoDias { get; set; } = 30;
    public bool Activo { get; set; } = true;
    public DateTime FechaCreacion { get; set; }
    public DateTime? FechaModificacion { get; set; }
}
```

<a id="kobraentidadesfacturacs"></a>

### Kobra.Entidades/Factura.cs

```csharp
namespace Kobra.Entidades;

public class Factura
{
    public int Id { get; set; }
    public string Numero { get; set; } = string.Empty;
    public int ClienteId { get; set; }
    public DateTime FechaEmision { get; set; } = DateTime.Today;
    public DateTime FechaVencimiento { get; set; } = DateTime.Today.AddDays(30);
    public decimal Subtotal { get; set; }
    public decimal Itbms { get; set; }
    public decimal Total { get; set; }
    public bool Anulada { get; set; }
    public string? MotivoAnulacion { get; set; }
    public DateTime? FechaAnulacion { get; set; }
    public DateTime FechaCreacion { get; set; }

    public string ClienteCodigo { get; set; } = string.Empty;
    public string ClienteNombre { get; set; } = string.Empty;
    public string? ClienteRucCedula { get; set; }
    public decimal TotalPagado { get; set; }
    public decimal Saldo { get; set; }
    public string Estado { get; set; } = "Pendiente";
    public bool EstaVencida { get; set; }
    public int DiasVencidos { get; set; }

    public string ClienteDescripcion =>
        string.IsNullOrEmpty(ClienteCodigo) ? ClienteNombre : $"{ClienteCodigo} - {ClienteNombre}";

    public string TextoVencida =>
        Anulada ? "No (Anulada)" : EstaVencida ? $"Sí ({DiasVencidos} d)" : "No";
}

/// <summary>Representación de solo lectura de los pagos asociados a una factura en consulta</summary>
public class PagoFactura
{
    public int Id { get; set; }
    public DateTime FechaPago { get; set; }
    public decimal Monto { get; set; }
    public bool Anulado { get; set; }
    public string? MotivoAnulacion { get; set; }
    public DateTime FechaCreacion { get; set; }

    public string EstadoTexto => Anulado ? "Anulado" : "Activo";
}
```

<a id="kobraentidadespagocs"></a>

### Kobra.Entidades/Pago.cs

```csharp
namespace Kobra.Entidades;

/// <summary>Pago aplicado a una sola factura</summary>
public class Pago
{
    public int Id { get; set; }
    public int FacturaId { get; set; }
    public DateTime FechaPago { get; set; } = DateTime.Today;
    public decimal Monto { get; set; }
    public bool Anulado { get; set; }
    public string? MotivoAnulacion { get; set; }
    public DateTime? FechaAnulacion { get; set; }
    public DateTime FechaCreacion { get; set; }

    public string NumeroFactura { get; set; } = string.Empty;
    public int ClienteId { get; set; }
    public string ClienteCodigo { get; set; } = string.Empty;
    public string ClienteNombre { get; set; } = string.Empty;
    public decimal SaldoFactura { get; set; }
    public bool FacturaAnulada { get; set; }

    public string EstadoTexto => Anulado ? "Anulado" : "Activo";

    public string ClienteDescripcion =>
        string.IsNullOrEmpty(ClienteCodigo) ? ClienteNombre : $"{ClienteCodigo} - {ClienteNombre}";
}
```

<a id="kobraentidadesreportecs"></a>

### Kobra.Entidades/Reporte.cs

```csharp
namespace Kobra.Entidades;

/// <summary>Estado de cuenta de un cliente: cabecera, facturas y totales</summary>
public class EstadoCuenta
{
    public int ClienteId { get; set; }
    public string ClienteCodigo { get; set; } = string.Empty;
    public string ClienteNombre { get; set; } = string.Empty;
    public string? ClienteRucCedula { get; set; }
    public decimal LimiteCredito { get; set; }
    public bool Activo { get; set; }
    public decimal TotalFacturado { get; set; }
    public decimal TotalCobrado { get; set; }
    public decimal Saldo { get; set; }

    public List<LineaEstadoCuenta> Facturas { get; } = [];

    public string ClienteDescripcion =>
        string.IsNullOrEmpty(ClienteCodigo) ? ClienteNombre : $"{ClienteCodigo} - {ClienteNombre}";
}

/// <summary>Una factura dentro del estado de cuenta del cliente</summary>
public class LineaEstadoCuenta
{
    public int FacturaId { get; set; }
    public string Numero { get; set; } = string.Empty;
    public DateTime FechaEmision { get; set; }
    public DateTime FechaVencimiento { get; set; }
    public decimal Total { get; set; }
    public decimal TotalPagado { get; set; }
    public decimal Saldo { get; set; }
    public string Estado { get; set; } = string.Empty;
    public bool EstaVencida { get; set; }
    public int DiasVencidos { get; set; }

    public string TextoVencida =>
        EstaVencida ? $"Sí ({DiasVencidos} d)" : "No";
}

/// <summary>Reporte de antigüedad de saldos: cinco tramos y el detalle de facturas de cada uno</summary>
public class AntiguedadSaldos
{
    public decimal SaldoTotal { get; set; }
    public int Facturas { get; set; }

    public List<TramoAntiguedad> Tramos { get; } = [];
}

/// <summary>Un tramo de antigüedad (Corriente, 1-30, 31-60, 61-90 o más de 90)</summary>
public class TramoAntiguedad
{
    public string Nombre { get; set; } = string.Empty;
    public int Orden { get; set; }
    public int Facturas { get; set; }
    public decimal Saldo { get; set; }
    public decimal Porcentaje { get; set; }

    public List<LineaAntiguedad> Detalle { get; } = [];
}

/// <summary>Una factura dentro del detalle de un tramo de antigüedad</summary>
public class LineaAntiguedad
{
    public int FacturaId { get; set; }
    public string Numero { get; set; } = string.Empty;
    public string ClienteDescripcion { get; set; } = string.Empty;
    public DateTime FechaVencimiento { get; set; }
    public decimal Saldo { get; set; }
    public int DiasVencidos { get; set; }
}
```

<a id="kobraentidadesresumencarteracs"></a>

### Kobra.Entidades/ResumenCartera.cs

```csharp
namespace Kobra.Entidades;

/// <summary>Cliente con su saldo pendiente, para el panel de inicio</summary>
public class ClienteSaldo
{
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string? RucCedula { get; set; }
    public string? Telefono { get; set; }
    public decimal LimiteCredito { get; set; }
    public decimal Saldo { get; set; }
}

/// <summary>Totales de la cartera que muestra el panel de inicio</summary>
public class ResumenCartera
{
    public decimal SaldoTotal { get; set; }
    public int FacturasConSaldo { get; set; }
    public int FacturasVencidas { get; set; }
    public int ClientesActivos { get; set; }
    public int ClientesInactivos { get; set; }

    public decimal Corriente { get; set; }
    public decimal Tramo1a30 { get; set; }
    public decimal Tramo31a60 { get; set; }
    public decimal Tramo61a90 { get; set; }
    public decimal TramoMasDe90 { get; set; }

    public List<ClienteSaldo> ClientesPrincipales { get; set; } = new();
}
```

---

<a id="3-capa-de-acceso-a-datos-kobradatos"></a>

## 3. Capa de acceso a datos (Kobra.Datos)

<a id="kobradatosclientedatoscs"></a>

### Kobra.Datos/ClienteDatos.cs

```csharp
using System.Data;
using Kobra.Entidades;
using Microsoft.Data.SqlClient;

namespace Kobra.Datos;

/// <summary>Acceso a datos de Cliente con ADO.NET</summary>
public class ClienteDatos
{
    private const string Columnas =
        "Id, Codigo, Nombre, RucCedula, Telefono, Email, Direccion, LimiteCredito, PlazoDias, Activo, " +
        "FechaCreacion, FechaModificacion";

    /// <summary>Inserta el cliente y devuelve su Id</summary>
    public int Insertar(Cliente cliente)
    {
        const string sql = @"
            INSERT INTO dbo.Cliente (Codigo, Nombre, RucCedula, Telefono, Email, Direccion, LimiteCredito, PlazoDias, Activo)
            OUTPUT INSERTED.Id
            VALUES (@Codigo, @Nombre, @RucCedula, @Telefono, @Email, @Direccion, @LimiteCredito, @PlazoDias, @Activo);";

        try
        {
            using var cn = Conexion.Crear();
            using var cmd = new SqlCommand(sql, cn);
            AgregarParametros(cmd, cliente);
            cn.Open();
            return (int)cmd.ExecuteScalar()!;
        }
        catch (SqlException ex)
        {
            throw Traducir(ex);
        }
    }

    /// <summary>Actualiza el cliente</summary>
    public bool Actualizar(Cliente cliente)
    {
        const string sql = @"
            UPDATE dbo.Cliente
               SET Codigo = @Codigo, Nombre = @Nombre, RucCedula = @RucCedula, Telefono = @Telefono,
                   Email = @Email, Direccion = @Direccion, LimiteCredito = @LimiteCredito,
                   PlazoDias = @PlazoDias, Activo = @Activo, FechaModificacion = SYSDATETIME()
             WHERE Id = @Id;";

        try
        {
            using var cn = Conexion.Crear();
            using var cmd = new SqlCommand(sql, cn);
            AgregarParametros(cmd, cliente);
            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = cliente.Id;
            cn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }
        catch (SqlException ex)
        {
            throw Traducir(ex);
        }
    }

    public bool CambiarEstado(int id, bool activo)
    {
        const string sql =
            "UPDATE dbo.Cliente SET Activo = @Activo, FechaModificacion = SYSDATETIME() WHERE Id = @Id;";

        try
        {
            using var cn = Conexion.Crear();
            using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.Add("@Activo", SqlDbType.Bit).Value = activo;
            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;
            cn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }
        catch (SqlException ex)
        {
            throw Traducir(ex);
        }
    }

    public Cliente? ObtenerPorId(int id)
    {
        var sql = $"SELECT {Columnas} FROM dbo.Cliente WHERE Id = @Id;";

        try
        {
            using var cn = Conexion.Crear();
            using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;
            cn.Open();
            using var rd = cmd.ExecuteReader();
            return rd.Read() ? Mapear(rd) : null;
        }
        catch (SqlException ex)
        {
            throw Traducir(ex);
        }
    }

    /// <summary>Busca por código o nombre (contiene)</summary>
    public List<Cliente> Buscar(string? texto, bool soloActivos, int maximo = 50)
    {
        var sql = $@"
            SELECT TOP (@Maximo) {Columnas}
              FROM dbo.Cliente
             WHERE (@SoloActivos = 0 OR Activo = 1)
               AND (@Texto IS NULL OR Codigo LIKE @Patron ESCAPE '\' OR Nombre LIKE @Patron ESCAPE '\'
                                   OR RucCedula LIKE @Patron ESCAPE '\')
             ORDER BY Nombre;";

        var texto2 = string.IsNullOrWhiteSpace(texto) ? null : texto.Trim();
        var lista = new List<Cliente>();

        try
        {
            using var cn = Conexion.Crear();
            using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.Add("@Maximo", SqlDbType.Int).Value = maximo;
            cmd.Parameters.Add("@SoloActivos", SqlDbType.Bit).Value = soloActivos;
            cmd.Parameters.Add("@Texto", SqlDbType.NVarChar, 150).Value = (object?)texto2 ?? DBNull.Value;
            cmd.Parameters.Add("@Patron", SqlDbType.NVarChar, 160).Value =
                texto2 is null ? DBNull.Value : "%" + EscaparLike(texto2) + "%";
            cn.Open();
            using var rd = cmd.ExecuteReader();
            while (rd.Read())
                lista.Add(Mapear(rd));
            return lista;
        }
        catch (SqlException ex)
        {
            throw Traducir(ex);
        }
    }

    /// <summary>¿Existe otro cliente con ese código? excluirId es el cliente que se está editando</summary>
    public bool ExisteCodigo(string codigo, int excluirId = 0)
    {
        const string sql = "SELECT CASE WHEN EXISTS (SELECT 1 FROM dbo.Cliente WHERE Codigo = @Codigo AND Id <> @Id) THEN 1 ELSE 0 END;";
        return EjecutarExiste(sql, cmd =>
        {
            cmd.Parameters.Add("@Codigo", SqlDbType.NVarChar, 20).Value = codigo;
            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = excluirId;
        });
    }

    public bool TieneFacturas(int id)
    {
        const string sql = "SELECT CASE WHEN EXISTS (SELECT 1 FROM dbo.Factura WHERE ClienteId = @Id) THEN 1 ELSE 0 END;";
        return EjecutarExiste(sql, cmd => cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id);
    }

    /// <summary>Borra el cliente solo si no tiene facturas</summary>
    public bool Eliminar(int id)
    {
        const string sql = @"
            DELETE FROM dbo.Cliente
             WHERE Id = @Id
               AND NOT EXISTS (SELECT 1 FROM dbo.Factura WHERE ClienteId = @Id);";

        try
        {
            using var cn = Conexion.Crear();
            using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;
            cn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }
        catch (SqlException ex)
        {
            throw Traducir(ex);
        }
    }

    private static bool EjecutarExiste(string sql, Action<SqlCommand> parametros)
    {
        try
        {
            using var cn = Conexion.Crear();
            using var cmd = new SqlCommand(sql, cn);
            parametros(cmd);
            cn.Open();
            return (int)cmd.ExecuteScalar()! == 1;
        }
        catch (SqlException ex)
        {
            throw Traducir(ex);
        }
    }

    private static void AgregarParametros(SqlCommand cmd, Cliente c)
    {
        cmd.Parameters.Add("@Codigo", SqlDbType.NVarChar, 20).Value = c.Codigo;
        cmd.Parameters.Add("@Nombre", SqlDbType.NVarChar, 150).Value = c.Nombre;
        cmd.Parameters.Add("@RucCedula", SqlDbType.NVarChar, 30).Value = (object?)c.RucCedula ?? DBNull.Value;
        cmd.Parameters.Add("@Telefono", SqlDbType.NVarChar, 30).Value = (object?)c.Telefono ?? DBNull.Value;
        cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 150).Value = (object?)c.Email ?? DBNull.Value;
        cmd.Parameters.Add("@Direccion", SqlDbType.NVarChar, 250).Value = (object?)c.Direccion ?? DBNull.Value;

        var limite = cmd.Parameters.Add("@LimiteCredito", SqlDbType.Decimal);
        limite.Precision = 18;
        limite.Scale = 2;
        limite.Value = c.LimiteCredito;

        cmd.Parameters.Add("@PlazoDias", SqlDbType.Int).Value = c.PlazoDias;
        cmd.Parameters.Add("@Activo", SqlDbType.Bit).Value = c.Activo;
    }

    private static Cliente Mapear(SqlDataReader rd) => new()
    {
        Id = rd.GetInt32(rd.GetOrdinal("Id")),
        Codigo = rd.GetString(rd.GetOrdinal("Codigo")),
        Nombre = rd.GetString(rd.GetOrdinal("Nombre")),
        RucCedula = LeerTexto(rd, "RucCedula"),
        Telefono = LeerTexto(rd, "Telefono"),
        Email = LeerTexto(rd, "Email"),
        Direccion = LeerTexto(rd, "Direccion"),
        LimiteCredito = rd.GetDecimal(rd.GetOrdinal("LimiteCredito")),
        PlazoDias = rd.GetInt32(rd.GetOrdinal("PlazoDias")),
        Activo = rd.GetBoolean(rd.GetOrdinal("Activo")),
        FechaCreacion = rd.GetDateTime(rd.GetOrdinal("FechaCreacion")),
        FechaModificacion = rd.IsDBNull(rd.GetOrdinal("FechaModificacion"))
            ? null
            : rd.GetDateTime(rd.GetOrdinal("FechaModificacion"))
    };

    private static string? LeerTexto(SqlDataReader rd, string columna)
    {
        var i = rd.GetOrdinal(columna);
        return rd.IsDBNull(i) ? null : rd.GetString(i);
    }

    private static string EscaparLike(string texto) =>
        texto.Replace(@"\", @"\\").Replace("%", @"\%").Replace("_", @"\_").Replace("[", @"\[");

    private static DatosException Traducir(SqlException ex)
    {
        if (DatosException.EsDuplicado(ex))
        {
            if (ex.Message.Contains("UQ_Cliente_Codigo", StringComparison.OrdinalIgnoreCase))
                return new DatosException("Ya existe un cliente con ese código.", ex);
            if (ex.Message.Contains("UX_Cliente_RucCedula", StringComparison.OrdinalIgnoreCase))
                return new DatosException("Ya existe un cliente con esa cédula o RUC.", ex);
        }
        return DatosException.Desde(ex);
    }
}
```

<a id="kobradatosconexioncs"></a>

### Kobra.Datos/Conexion.cs

```csharp
using System.Configuration;
using Microsoft.Data.SqlClient;

namespace Kobra.Datos;

/// <summary>Punto único para obtener conexiones</summary>
public static class Conexion
{
    private const string NombreCadena = "Kobra";

    /// <summary>Crea una conexión sin abrir</summary>
    public static SqlConnection Crear()
    {
        var cadena = ConfigurationManager.ConnectionStrings[NombreCadena]?.ConnectionString;
        if (string.IsNullOrWhiteSpace(cadena))
            throw new DatosException(
                $"No se encontró la cadena de conexión \"{NombreCadena}\" en App.config.");

        return new SqlConnection(cadena);
    }

    /// <summary>Servidor y base de datos de la cadena de conexión, para mostrarlos</summary>
    public static (string Servidor, string BaseDatos) Destino()
    {
        try
        {
            var cadena = ConfigurationManager.ConnectionStrings[NombreCadena]?.ConnectionString;
            var b = new SqlConnectionStringBuilder(cadena);
            return (b.DataSource, b.InitialCatalog);
        }
        catch (ArgumentException)
        {
            return (string.Empty, string.Empty);
        }
    }

    /// <summary>Intenta abrir la conexión</summary>
    public static string? Probar()
    {
        try
        {
            using var cn = Crear();
            cn.Open();
            return null;
        }
        catch (SqlException ex)
        {
            return DatosException.Desde(ex).Message;
        }
        catch (DatosException ex)
        {
            return ex.Message;
        }
    }
}
```

<a id="kobradatosdatosexceptioncs"></a>

### Kobra.Datos/DatosException.cs

```csharp
using Microsoft.Data.SqlClient;

namespace Kobra.Datos;

/// <summary>Error de acceso a datos con un mensaje que se puede mostrar al usuario tal cual</summary>
public class DatosException : Exception
{
    public DatosException(string mensaje) : base(mensaje) { }

    public DatosException(string mensaje, Exception interna) : base(mensaje, interna) { }

    public static bool EsDuplicado(SqlException ex) => ex.Number is 2627 or 2601;

    public static DatosException Desde(SqlException ex)
    {
        var mensaje = ex.Number switch
        {
            2627 or 2601 => "Ya existe un registro con esos datos.",
            547 => "La operación no es válida porque incumple una restricción de la base de datos " +
                   "(por ejemplo, el registro está relacionado con otros).",

            -2 or -1 or 2 or 53 or 4060 or 18456 or 10060 or 10061 =>
                "No se pudo conectar con la base de datos. Verifique que SQL Server esté en ejecución " +
                "y que la cadena de conexión de App.config sea correcta.",
            _ => "Ocurrió un error al acceder a la base de datos."
        };
        return new DatosException(mensaje, ex);
    }
}
```

<a id="kobradatosfacturadatoscs"></a>

### Kobra.Datos/FacturaDatos.cs

```csharp
using System.Data;
using Kobra.Entidades;
using Microsoft.Data.SqlClient;

namespace Kobra.Datos;

/// <summary>Acceso a datos de Factura con ADO.NET</summary>
public class FacturaDatos
{
    private const string ConsultaBase = @"
        SELECT {0}
               f.Id, f.Numero, f.ClienteId, c.Codigo AS ClienteCodigo, c.Nombre AS ClienteNombre,
               c.RucCedula AS ClienteRucCedula,
               f.FechaEmision, f.FechaVencimiento, f.Subtotal, f.Itbms, f.Total,
               v.TotalPagado, v.Saldo, v.Estado, v.EstaVencida, v.DiasVencidos,
               f.Anulada, f.MotivoAnulacion, f.FechaAnulacion, f.FechaCreacion
          FROM dbo.Factura f
         INNER JOIN dbo.Cliente c ON c.Id = f.ClienteId
         INNER JOIN dbo.vw_FacturaSaldo v ON v.Id = f.Id";

    public int Insertar(Factura factura)
    {
        const string sql = @"
            INSERT INTO dbo.Factura (Numero, ClienteId, FechaEmision, FechaVencimiento, Subtotal, Itbms, Total)
            OUTPUT INSERTED.Id
            VALUES (@Numero, @ClienteId, @FechaEmision, @FechaVencimiento, @Subtotal, @Itbms, @Total);";

        try
        {
            using var cn = Conexion.Crear();
            using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.Add("@Numero", SqlDbType.NVarChar, 30).Value = factura.Numero;
            cmd.Parameters.Add("@ClienteId", SqlDbType.Int).Value = factura.ClienteId;
            cmd.Parameters.Add("@FechaEmision", SqlDbType.Date).Value = factura.FechaEmision.Date;
            cmd.Parameters.Add("@FechaVencimiento", SqlDbType.Date).Value = factura.FechaVencimiento.Date;
            AgregarDecimal(cmd, "@Subtotal", factura.Subtotal);
            AgregarDecimal(cmd, "@Itbms", factura.Itbms);
            AgregarDecimal(cmd, "@Total", factura.Total);

            cn.Open();
            return (int)cmd.ExecuteScalar()!;
        }
        catch (SqlException ex)
        {
            throw Traducir(ex);
        }
    }

    public bool Anular(int id, string motivo)
    {
        const string sql = @"
            UPDATE dbo.Factura
               SET Anulada = 1,
                   MotivoAnulacion = @Motivo,
                   FechaAnulacion = SYSDATETIME()
             WHERE Id = @Id
               AND Anulada = 0
               AND NOT EXISTS (SELECT 1 FROM dbo.Pago WHERE FacturaId = @Id AND Anulado = 0);";

        try
        {
            using var cn = Conexion.Crear();
            using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;
            cmd.Parameters.Add("@Motivo", SqlDbType.NVarChar, 250).Value = motivo;
            cn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }
        catch (SqlException ex)
        {
            throw Traducir(ex);
        }
    }

    public Factura? ObtenerPorId(int id)
    {
        var sql = string.Format(ConsultaBase, string.Empty) + " WHERE f.Id = @Id;";

        try
        {
            using var cn = Conexion.Crear();
            using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;
            cn.Open();
            using var rd = cmd.ExecuteReader();
            return rd.Read() ? Mapear(rd) : null;
        }
        catch (SqlException ex)
        {
            throw Traducir(ex);
        }
    }

    public Factura? ObtenerPorNumero(string numero)
    {
        var sql = string.Format(ConsultaBase, string.Empty) + " WHERE f.Numero = @Numero;";

        try
        {
            using var cn = Conexion.Crear();
            using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.Add("@Numero", SqlDbType.NVarChar, 30).Value = numero.Trim();
            cn.Open();
            using var rd = cmd.ExecuteReader();
            return rd.Read() ? Mapear(rd) : null;
        }
        catch (SqlException ex)
        {
            throw Traducir(ex);
        }
    }

    public List<Factura> Buscar(string? texto, int? clienteId, string? estado, bool soloVencidas, int maximo = 50)
    {
        var sql = string.Format(ConsultaBase, "TOP (@Maximo)") + @"
             WHERE (@ClienteId IS NULL OR f.ClienteId = @ClienteId)
               AND (@Estado IS NULL OR v.Estado = @Estado)
               AND (@SoloVencidas = 0 OR v.EstaVencida = 1)
               AND (@Texto IS NULL OR f.Numero LIKE @Patron ESCAPE '\'
                                   OR c.Nombre LIKE @Patron ESCAPE '\'
                                   OR c.Codigo LIKE @Patron ESCAPE '\')
             ORDER BY f.FechaEmision DESC, f.Id DESC;";

        var texto2 = string.IsNullOrWhiteSpace(texto) ? null : texto.Trim();
        var estado2 = string.IsNullOrWhiteSpace(estado) || estado == "Todas" ? null : estado.Trim();
        var lista = new List<Factura>();

        try
        {
            using var cn = Conexion.Crear();
            using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.Add("@Maximo", SqlDbType.Int).Value = maximo;
            cmd.Parameters.Add("@ClienteId", SqlDbType.Int).Value = (object?)clienteId ?? DBNull.Value;
            cmd.Parameters.Add("@Estado", SqlDbType.NVarChar, 20).Value = (object?)estado2 ?? DBNull.Value;
            cmd.Parameters.Add("@SoloVencidas", SqlDbType.Bit).Value = soloVencidas;
            cmd.Parameters.Add("@Texto", SqlDbType.NVarChar, 150).Value = (object?)texto2 ?? DBNull.Value;
            cmd.Parameters.Add("@Patron", SqlDbType.NVarChar, 160).Value =
                texto2 is null ? DBNull.Value : "%" + EscaparLike(texto2) + "%";

            cn.Open();
            using var rd = cmd.ExecuteReader();
            while (rd.Read())
                lista.Add(Mapear(rd));
            return lista;
        }
        catch (SqlException ex)
        {
            throw Traducir(ex);
        }
    }

    public List<PagoFactura> ListarPagosDeFactura(int facturaId)
    {
        const string sql = @"
            SELECT Id, FechaPago, Monto, Anulado, MotivoAnulacion, FechaCreacion
              FROM dbo.Pago
             WHERE FacturaId = @FacturaId
             ORDER BY FechaPago DESC, Id DESC;";

        var lista = new List<PagoFactura>();

        try
        {
            using var cn = Conexion.Crear();
            using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.Add("@FacturaId", SqlDbType.Int).Value = facturaId;
            cn.Open();
            using var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                lista.Add(new PagoFactura
                {
                    Id = rd.GetInt32(rd.GetOrdinal("Id")),
                    FechaPago = rd.GetDateTime(rd.GetOrdinal("FechaPago")),
                    Monto = rd.GetDecimal(rd.GetOrdinal("Monto")),
                    Anulado = rd.GetBoolean(rd.GetOrdinal("Anulado")),
                    MotivoAnulacion = LeerTexto(rd, "MotivoAnulacion"),
                    FechaCreacion = rd.GetDateTime(rd.GetOrdinal("FechaCreacion"))
                });
            }
            return lista;
        }
        catch (SqlException ex)
        {
            throw Traducir(ex);
        }
    }

    public bool ExisteNumero(string numero, int excluirId = 0)
    {
        const string sql =
            "SELECT CASE WHEN EXISTS (SELECT 1 FROM dbo.Factura WHERE Numero = @Numero AND Id <> @Id) THEN 1 ELSE 0 END;";
        return EjecutarExiste(sql, cmd =>
        {
            cmd.Parameters.Add("@Numero", SqlDbType.NVarChar, 30).Value = numero;
            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = excluirId;
        });
    }

    public bool TienePagosActivos(int facturaId)
    {
        const string sql =
            "SELECT CASE WHEN EXISTS (SELECT 1 FROM dbo.Pago WHERE FacturaId = @Id AND Anulado = 0) THEN 1 ELSE 0 END;";
        return EjecutarExiste(sql, cmd => cmd.Parameters.Add("@Id", SqlDbType.Int).Value = facturaId);
    }

    public decimal ObtenerSaldoCliente(int clienteId)
    {
        const string sql =
            "SELECT ISNULL(SUM(Saldo), 0) FROM dbo.vw_FacturaSaldo WHERE ClienteId = @ClienteId;";

        try
        {
            using var cn = Conexion.Crear();
            using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.Add("@ClienteId", SqlDbType.Int).Value = clienteId;
            cn.Open();
            return Convert.ToDecimal(cmd.ExecuteScalar());
        }
        catch (SqlException ex)
        {
            throw Traducir(ex);
        }
    }

    private static bool EjecutarExiste(string sql, Action<SqlCommand> parametros)
    {
        try
        {
            using var cn = Conexion.Crear();
            using var cmd = new SqlCommand(sql, cn);
            parametros(cmd);
            cn.Open();
            return (int)cmd.ExecuteScalar()! == 1;
        }
        catch (SqlException ex)
        {
            throw Traducir(ex);
        }
    }

    private static void AgregarDecimal(SqlCommand cmd, string nombre, decimal valor)
    {
        var param = cmd.Parameters.Add(nombre, SqlDbType.Decimal);
        param.Precision = 18;
        param.Scale = 2;
        param.Value = valor;
    }

    private static Factura Mapear(SqlDataReader rd) => new()
    {
        Id = rd.GetInt32(rd.GetOrdinal("Id")),
        Numero = rd.GetString(rd.GetOrdinal("Numero")),
        ClienteId = rd.GetInt32(rd.GetOrdinal("ClienteId")),
        ClienteCodigo = rd.GetString(rd.GetOrdinal("ClienteCodigo")),
        ClienteNombre = rd.GetString(rd.GetOrdinal("ClienteNombre")),
        ClienteRucCedula = LeerTexto(rd, "ClienteRucCedula"),
        FechaEmision = rd.GetDateTime(rd.GetOrdinal("FechaEmision")),
        FechaVencimiento = rd.GetDateTime(rd.GetOrdinal("FechaVencimiento")),
        Subtotal = rd.GetDecimal(rd.GetOrdinal("Subtotal")),
        Itbms = rd.GetDecimal(rd.GetOrdinal("Itbms")),
        Total = rd.GetDecimal(rd.GetOrdinal("Total")),
        TotalPagado = rd.GetDecimal(rd.GetOrdinal("TotalPagado")),
        Saldo = rd.GetDecimal(rd.GetOrdinal("Saldo")),
        Estado = rd.GetString(rd.GetOrdinal("Estado")),
        EstaVencida = Convert.ToBoolean(rd.GetValue(rd.GetOrdinal("EstaVencida"))),
        DiasVencidos = Convert.ToInt32(rd.GetValue(rd.GetOrdinal("DiasVencidos"))),
        Anulada = rd.GetBoolean(rd.GetOrdinal("Anulada")),
        MotivoAnulacion = LeerTexto(rd, "MotivoAnulacion"),
        FechaAnulacion = rd.IsDBNull(rd.GetOrdinal("FechaAnulacion"))
            ? null
            : rd.GetDateTime(rd.GetOrdinal("FechaAnulacion")),
        FechaCreacion = rd.GetDateTime(rd.GetOrdinal("FechaCreacion"))
    };

    private static string? LeerTexto(SqlDataReader rd, string columna)
    {
        var i = rd.GetOrdinal(columna);
        return rd.IsDBNull(i) ? null : rd.GetString(i);
    }

    private static string EscaparLike(string texto) =>
        texto.Replace(@"\", @"\\").Replace("%", @"\%").Replace("_", @"\_").Replace("[", @"\[");

    private static DatosException Traducir(SqlException ex)
    {
        if (DatosException.EsDuplicado(ex))
            return new DatosException("Ya existe una factura con ese número.", ex);

        return DatosException.Desde(ex);
    }
}
```

<a id="kobradatospagodatoscs"></a>

### Kobra.Datos/PagoDatos.cs

```csharp
using System.Data;
using Kobra.Entidades;
using Microsoft.Data.SqlClient;

namespace Kobra.Datos;

/// <summary>Acceso a datos de Pago con ADO.NET</summary>
public class PagoDatos
{
    private const string ConsultaBase = @"
        SELECT {0}
               p.Id, p.FacturaId, p.FechaPago, p.Monto, p.Anulado,
               p.MotivoAnulacion, p.FechaAnulacion, p.FechaCreacion,
               f.Numero AS NumeroFactura, f.ClienteId,
               c.Codigo AS ClienteCodigo, c.Nombre AS ClienteNombre,
               v.Saldo AS SaldoFactura, f.Anulada AS FacturaAnulada
          FROM dbo.Pago p
         INNER JOIN dbo.Factura f ON f.Id = p.FacturaId
         INNER JOIN dbo.Cliente c ON c.Id = f.ClienteId
         INNER JOIN dbo.vw_FacturaSaldo v ON v.Id = f.Id";

    /// <summary>Inserta un pago de forma atómica</summary>
    public int? Insertar(Pago pago)
    {
        const string sqlBloquear = @"
            SELECT f.Anulada, v.Saldo
              FROM dbo.Factura AS f WITH (UPDLOCK, HOLDLOCK)
             INNER JOIN dbo.vw_FacturaSaldo AS v ON v.Id = f.Id
             WHERE f.Id = @FacturaId;";

        const string sqlInsertar = @"
            INSERT INTO dbo.Pago (FacturaId, FechaPago, Monto)
            OUTPUT INSERTED.Id
            VALUES (@FacturaId, @FechaPago, @Monto);";

        try
        {
            using var cn = Conexion.Crear();
            cn.Open();
            using var tran = cn.BeginTransaction();

            try
            {
                bool anulada;
                decimal saldo;

                using (var cmdBloquear = new SqlCommand(sqlBloquear, cn, tran))
                {
                    cmdBloquear.Parameters.Add("@FacturaId", SqlDbType.Int).Value = pago.FacturaId;
                    using var rd = cmdBloquear.ExecuteReader();
                    if (!rd.Read())
                    {
                        tran.Rollback();
                        return null;
                    }

                    anulada = rd.GetBoolean(rd.GetOrdinal("Anulada"));
                    saldo = rd.GetDecimal(rd.GetOrdinal("Saldo"));
                }

                if (anulada || pago.Monto > saldo)
                {
                    tran.Rollback();
                    return null;
                }

                using var cmdInsertar = new SqlCommand(sqlInsertar, cn, tran);
                cmdInsertar.Parameters.Add("@FacturaId", SqlDbType.Int).Value = pago.FacturaId;
                cmdInsertar.Parameters.Add("@FechaPago", SqlDbType.Date).Value = pago.FechaPago.Date;
                AgregarDecimal(cmdInsertar, "@Monto", pago.Monto);

                var id = (int)cmdInsertar.ExecuteScalar()!;
                tran.Commit();
                return id;
            }
            catch
            {
                tran.Rollback();
                throw;
            }
        }
        catch (SqlException ex)
        {
            throw Traducir(ex);
        }
    }

    /// <summary>Anulación lógica de un pago</summary>
    public bool Anular(int id, string motivo)
    {
        const string sql = @"
            UPDATE dbo.Pago
               SET Anulado = 1,
                   MotivoAnulacion = @Motivo,
                   FechaAnulacion = SYSDATETIME()
             WHERE Id = @Id
               AND Anulado = 0;";

        try
        {
            using var cn = Conexion.Crear();
            using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;
            cmd.Parameters.Add("@Motivo", SqlDbType.NVarChar, 250).Value = motivo;
            cn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }
        catch (SqlException ex)
        {
            throw Traducir(ex);
        }
    }

    public Pago? ObtenerPorId(int id)
    {
        var sql = string.Format(ConsultaBase, string.Empty) + " WHERE p.Id = @Id;";

        try
        {
            using var cn = Conexion.Crear();
            using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;
            cn.Open();
            using var rd = cmd.ExecuteReader();
            return rd.Read() ? Mapear(rd) : null;
        }
        catch (SqlException ex)
        {
            throw Traducir(ex);
        }
    }

    /// <summary>Busca pagos por número de pago, número de factura o cliente</summary>
    public List<Pago> Buscar(string? texto, int? clienteId, bool? anulado, int maximo = 50)
    {
        var sql = string.Format(ConsultaBase, "TOP (@Maximo)") + @"
             WHERE (@ClienteId IS NULL OR p.FacturaId IN (SELECT Id FROM dbo.Factura WHERE ClienteId = @ClienteId))
               AND (@Anulado IS NULL OR p.Anulado = @Anulado)
               AND (@Texto IS NULL OR CONVERT(NVARCHAR(20), p.Id) LIKE @Patron ESCAPE '\'
                                    OR f.Numero LIKE @Patron ESCAPE '\'
                                    OR c.Nombre LIKE @Patron ESCAPE '\'
                                    OR c.Codigo LIKE @Patron ESCAPE '\')
             ORDER BY p.FechaPago DESC, p.Id DESC;";

        var texto2 = string.IsNullOrWhiteSpace(texto) ? null : texto.Trim();
        var lista = new List<Pago>();

        try
        {
            using var cn = Conexion.Crear();
            using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.Add("@Maximo", SqlDbType.Int).Value = maximo;
            cmd.Parameters.Add("@ClienteId", SqlDbType.Int).Value = (object?)clienteId ?? DBNull.Value;
            cmd.Parameters.Add("@Anulado", SqlDbType.Bit).Value = (object?)anulado ?? DBNull.Value;
            cmd.Parameters.Add("@Texto", SqlDbType.NVarChar, 150).Value = (object?)texto2 ?? DBNull.Value;
            cmd.Parameters.Add("@Patron", SqlDbType.NVarChar, 160).Value =
                texto2 is null ? DBNull.Value : "%" + EscaparLike(texto2) + "%";

            cn.Open();
            using var rd = cmd.ExecuteReader();
            while (rd.Read())
                lista.Add(Mapear(rd));
            return lista;
        }
        catch (SqlException ex)
        {
            throw Traducir(ex);
        }
    }

    /// <summary>Saldo actual de una factura, tomado directamente de la vista</summary>
    public decimal ObtenerSaldoFactura(int facturaId)
    {
        const string sql = "SELECT Saldo FROM dbo.vw_FacturaSaldo WHERE Id = @FacturaId;";

        try
        {
            using var cn = Conexion.Crear();
            using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.Add("@FacturaId", SqlDbType.Int).Value = facturaId;
            cn.Open();
            var resultado = cmd.ExecuteScalar();
            return resultado is null || resultado is DBNull ? 0m : Convert.ToDecimal(resultado);
        }
        catch (SqlException ex)
        {
            throw Traducir(ex);
        }
    }

    public List<Pago> ListarPorFactura(int facturaId)
    {
        var sql = string.Format(ConsultaBase, string.Empty) + @"
             WHERE p.FacturaId = @FacturaId
             ORDER BY p.FechaPago DESC, p.Id DESC;";

        var lista = new List<Pago>();

        try
        {
            using var cn = Conexion.Crear();
            using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.Add("@FacturaId", SqlDbType.Int).Value = facturaId;
            cn.Open();
            using var rd = cmd.ExecuteReader();
            while (rd.Read())
                lista.Add(Mapear(rd));
            return lista;
        }
        catch (SqlException ex)
        {
            throw Traducir(ex);
        }
    }

    private static void AgregarDecimal(SqlCommand cmd, string nombre, decimal valor)
    {
        var param = cmd.Parameters.Add(nombre, SqlDbType.Decimal);
        param.Precision = 18;
        param.Scale = 2;
        param.Value = valor;
    }

    private static Pago Mapear(SqlDataReader rd) => new()
    {
        Id = rd.GetInt32(rd.GetOrdinal("Id")),
        FacturaId = rd.GetInt32(rd.GetOrdinal("FacturaId")),
        FechaPago = rd.GetDateTime(rd.GetOrdinal("FechaPago")),
        Monto = rd.GetDecimal(rd.GetOrdinal("Monto")),
        Anulado = rd.GetBoolean(rd.GetOrdinal("Anulado")),
        MotivoAnulacion = LeerTexto(rd, "MotivoAnulacion"),
        FechaAnulacion = rd.IsDBNull(rd.GetOrdinal("FechaAnulacion"))
            ? null
            : rd.GetDateTime(rd.GetOrdinal("FechaAnulacion")),
        FechaCreacion = rd.GetDateTime(rd.GetOrdinal("FechaCreacion")),
        NumeroFactura = rd.GetString(rd.GetOrdinal("NumeroFactura")),
        ClienteId = rd.GetInt32(rd.GetOrdinal("ClienteId")),
        ClienteCodigo = rd.GetString(rd.GetOrdinal("ClienteCodigo")),
        ClienteNombre = rd.GetString(rd.GetOrdinal("ClienteNombre")),
        SaldoFactura = rd.GetDecimal(rd.GetOrdinal("SaldoFactura")),
        FacturaAnulada = rd.GetBoolean(rd.GetOrdinal("FacturaAnulada"))
    };

    private static string? LeerTexto(SqlDataReader rd, string columna)
    {
        var i = rd.GetOrdinal(columna);
        return rd.IsDBNull(i) ? null : rd.GetString(i);
    }

    private static string EscaparLike(string texto) =>
        texto.Replace(@"\", @"\\").Replace("%", @"\%").Replace("_", @"\_").Replace("[", @"\[");

    private static DatosException Traducir(SqlException ex)
    {
        if (DatosException.EsDuplicado(ex))
            return new DatosException("Ya existe un pago con esos datos.", ex);

        return DatosException.Desde(ex);
    }
}
```

<a id="kobradatosreportedatoscs"></a>

### Kobra.Datos/ReporteDatos.cs

```csharp
using System.Data;
using Kobra.Entidades;
using Microsoft.Data.SqlClient;

namespace Kobra.Datos;

/// <summary>Consultas de solo lectura para el estado de cuenta y la antigüedad de saldos</summary>
public class ReporteDatos
{
    private const string SqlCliente = @"
        SELECT Codigo, Nombre, RucCedula, LimiteCredito, Activo
          FROM dbo.Cliente
         WHERE Id = @ClienteId;";

    private const string SqlFacturasCliente = @"
        SELECT f.Id, f.Numero, f.FechaEmision, f.FechaVencimiento,
               f.Total, v.TotalPagado, v.Saldo, v.Estado, v.EstaVencida, v.DiasVencidos
          FROM dbo.Factura f
         INNER JOIN dbo.vw_FacturaSaldo v ON v.Id = f.Id
         WHERE f.ClienteId = @ClienteId
         ORDER BY f.FechaEmision DESC, f.Id DESC;";

    private const string SqlAntiguedad = @"
        SELECT v.Id, v.Numero, v.ClienteId, c.Codigo, c.Nombre,
               v.FechaVencimiento, v.Saldo, v.DiasVencidos, v.EstaVencida
          FROM dbo.vw_FacturaSaldo v
         INNER JOIN dbo.Cliente c ON c.Id = v.ClienteId
         WHERE v.Saldo > 0
           AND (@ClienteId IS NULL OR v.ClienteId = @ClienteId)
         ORDER BY v.DiasVencidos DESC, v.Numero;";

    /// <summary>Cabecera y facturas del estado de cuenta</summary>
    public EstadoCuenta? ObtenerEstadoCuenta(int clienteId)
    {
        try
        {
            using var cn = Conexion.Crear();
            using var cmd = new SqlCommand(SqlCliente + SqlFacturasCliente, cn);
            cmd.Parameters.Add("@ClienteId", SqlDbType.Int).Value = clienteId;
            cn.Open();
            using var rd = cmd.ExecuteReader();

            if (!rd.Read())
                return null;

            var cuenta = new EstadoCuenta
            {
                ClienteId = clienteId,
                ClienteCodigo = rd.GetString(rd.GetOrdinal("Codigo")),
                ClienteNombre = rd.GetString(rd.GetOrdinal("Nombre")),
                ClienteRucCedula = LeerTexto(rd, "RucCedula"),
                LimiteCredito = rd.GetDecimal(rd.GetOrdinal("LimiteCredito")),
                Activo = rd.GetBoolean(rd.GetOrdinal("Activo"))
            };

            rd.NextResult();
            while (rd.Read())
            {
                cuenta.Facturas.Add(new LineaEstadoCuenta
                {
                    FacturaId = rd.GetInt32(rd.GetOrdinal("Id")),
                    Numero = rd.GetString(rd.GetOrdinal("Numero")),
                    FechaEmision = rd.GetDateTime(rd.GetOrdinal("FechaEmision")),
                    FechaVencimiento = rd.GetDateTime(rd.GetOrdinal("FechaVencimiento")),
                    Total = rd.GetDecimal(rd.GetOrdinal("Total")),
                    TotalPagado = rd.GetDecimal(rd.GetOrdinal("TotalPagado")),
                    Saldo = rd.GetDecimal(rd.GetOrdinal("Saldo")),
                    Estado = rd.GetString(rd.GetOrdinal("Estado")),
                    EstaVencida = Convert.ToBoolean(rd.GetValue(rd.GetOrdinal("EstaVencida"))),
                    DiasVencidos = Convert.ToInt32(rd.GetValue(rd.GetOrdinal("DiasVencidos")))
                });
            }

            cuenta.TotalFacturado = cuenta.Facturas.Sum(f => f.Total);
            cuenta.TotalCobrado = cuenta.Facturas.Sum(f => f.TotalPagado);
            cuenta.Saldo = cuenta.Facturas.Sum(f => f.Saldo);
            return cuenta;
        }
        catch (SqlException ex)
        {
            throw DatosException.Desde(ex);
        }
    }

    /// <summary>Facturas con saldo para alimentar los cinco tramos de antigüedad</summary>
    public List<LineaAntiguedad> ObtenerFacturasConSaldo(int? clienteId)
    {
        var lista = new List<LineaAntiguedad>();

        try
        {
            using var cn = Conexion.Crear();
            using var cmd = new SqlCommand(SqlAntiguedad, cn);
            cmd.Parameters.Add("@ClienteId", SqlDbType.Int).Value = (object?)clienteId ?? DBNull.Value;
            cn.Open();
            using var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                var codigo = rd.GetString(rd.GetOrdinal("Codigo"));
                var nombre = rd.GetString(rd.GetOrdinal("Nombre"));
                lista.Add(new LineaAntiguedad
                {
                    FacturaId = rd.GetInt32(rd.GetOrdinal("Id")),
                    Numero = rd.GetString(rd.GetOrdinal("Numero")),
                    ClienteDescripcion = $"{codigo} - {nombre}",
                    FechaVencimiento = rd.GetDateTime(rd.GetOrdinal("FechaVencimiento")),
                    Saldo = rd.GetDecimal(rd.GetOrdinal("Saldo")),
                    DiasVencidos = Convert.ToInt32(rd.GetValue(rd.GetOrdinal("DiasVencidos")))
                });
            }
            return lista;
        }
        catch (SqlException ex)
        {
            throw DatosException.Desde(ex);
        }
    }

    private static string? LeerTexto(SqlDataReader rd, string columna)
    {
        var i = rd.GetOrdinal(columna);
        return rd.IsDBNull(i) ? null : rd.GetString(i);
    }
}
```

<a id="kobradatosresumendatoscs"></a>

### Kobra.Datos/ResumenDatos.cs

```csharp
using System.Data;
using Kobra.Entidades;
using Microsoft.Data.SqlClient;

namespace Kobra.Datos;

/// <summary>Consultas agregadas para el panel de inicio</summary>
public class ResumenDatos
{
    private const string Sql = @"
        SELECT COUNT(*)                                                         AS FacturasConSaldo,
               ISNULL(SUM(Saldo), 0)                                            AS SaldoTotal,
               ISNULL(SUM(CASE WHEN EstaVencida = 1 THEN 1 ELSE 0 END), 0)      AS FacturasVencidas,
               ISNULL(SUM(CASE WHEN DiasVencidos = 0 THEN Saldo END), 0)        AS Corriente,
               ISNULL(SUM(CASE WHEN DiasVencidos BETWEEN 1 AND 30 THEN Saldo END), 0)  AS Tramo1a30,
               ISNULL(SUM(CASE WHEN DiasVencidos BETWEEN 31 AND 60 THEN Saldo END), 0) AS Tramo31a60,
               ISNULL(SUM(CASE WHEN DiasVencidos BETWEEN 61 AND 90 THEN Saldo END), 0) AS Tramo61a90,
               ISNULL(SUM(CASE WHEN DiasVencidos > 90 THEN Saldo END), 0)       AS TramoMasDe90
          FROM dbo.vw_FacturaSaldo
         WHERE Saldo > 0;

        SELECT ISNULL(SUM(CASE WHEN Activo = 1 THEN 1 ELSE 0 END), 0) AS Activos,
               ISNULL(SUM(CASE WHEN Activo = 0 THEN 1 ELSE 0 END), 0) AS Inactivos
          FROM dbo.Cliente;

        SELECT TOP (@Maximo) c.Codigo, c.Nombre, c.RucCedula, c.Telefono, c.LimiteCredito,
               ISNULL(s.Saldo, 0) AS Saldo
          FROM dbo.Cliente AS c
         OUTER APPLY (SELECT SUM(v.Saldo) AS Saldo FROM dbo.vw_FacturaSaldo AS v WHERE v.ClienteId = c.Id) AS s
         WHERE c.Activo = 1
         ORDER BY ISNULL(s.Saldo, 0) DESC, c.Nombre;";

    public ResumenCartera Obtener(int maximoClientes)
    {
        try
        {
            using var cn = Conexion.Crear();
            using var cmd = new SqlCommand(Sql, cn);
            cmd.Parameters.Add("@Maximo", SqlDbType.Int).Value = maximoClientes;
            cn.Open();
            using var rd = cmd.ExecuteReader();

            var r = new ResumenCartera();

            if (rd.Read())
            {
                r.FacturasConSaldo = rd.GetInt32(rd.GetOrdinal("FacturasConSaldo"));
                r.SaldoTotal = rd.GetDecimal(rd.GetOrdinal("SaldoTotal"));
                r.FacturasVencidas = rd.GetInt32(rd.GetOrdinal("FacturasVencidas"));
                r.Corriente = rd.GetDecimal(rd.GetOrdinal("Corriente"));
                r.Tramo1a30 = rd.GetDecimal(rd.GetOrdinal("Tramo1a30"));
                r.Tramo31a60 = rd.GetDecimal(rd.GetOrdinal("Tramo31a60"));
                r.Tramo61a90 = rd.GetDecimal(rd.GetOrdinal("Tramo61a90"));
                r.TramoMasDe90 = rd.GetDecimal(rd.GetOrdinal("TramoMasDe90"));
            }

            rd.NextResult();
            if (rd.Read())
            {
                r.ClientesActivos = rd.GetInt32(rd.GetOrdinal("Activos"));
                r.ClientesInactivos = rd.GetInt32(rd.GetOrdinal("Inactivos"));
            }

            rd.NextResult();
            while (rd.Read())
            {
                r.ClientesPrincipales.Add(new ClienteSaldo
                {
                    Codigo = rd.GetString(rd.GetOrdinal("Codigo")),
                    Nombre = rd.GetString(rd.GetOrdinal("Nombre")),
                    RucCedula = LeerTexto(rd, "RucCedula"),
                    Telefono = LeerTexto(rd, "Telefono"),
                    LimiteCredito = rd.GetDecimal(rd.GetOrdinal("LimiteCredito")),
                    Saldo = rd.GetDecimal(rd.GetOrdinal("Saldo"))
                });
            }

            return r;
        }
        catch (SqlException ex)
        {
            throw DatosException.Desde(ex);
        }
    }

    private static string? LeerTexto(SqlDataReader rd, string columna)
    {
        var i = rd.GetOrdinal(columna);
        return rd.IsDBNull(i) ? null : rd.GetString(i);
    }
}
```

---

<a id="4-capa-de-negocio-kobranegocio"></a>

## 4. Capa de negocio (Kobra.Negocio)

<a id="kobranegociobasedatoscs"></a>

### Kobra.Negocio/BaseDatos.cs

```csharp
using Kobra.Datos;

namespace Kobra.Negocio;

/// <summary>Estado general de la base de datos, para que la UI no dependa de la capa de datos</summary>
public static class BaseDatos
{
    /// <summary>Devuelve el mensaje de error si no hay conexión, o null si todo está bien</summary>
    public static string? ProbarConexion() => Conexion.Probar();

    public static (string Servidor, string BaseDatos) Destino() => Conexion.Destino();
}
```

<a id="kobranegocioclientenegociocs"></a>

### Kobra.Negocio/ClienteNegocio.cs

```csharp
using System.Net.Mail;
using Kobra.Datos;
using Kobra.Entidades;

namespace Kobra.Negocio;

/// <summary>Validaciones y reglas de Cliente</summary>
public class ClienteNegocio
{
    public const int MaximoResultados = 50;

    private readonly ClienteDatos _datos;

    public ClienteNegocio() : this(new ClienteDatos()) { }

    public ClienteNegocio(ClienteDatos datos) => _datos = datos;

    public int Registrar(Cliente cliente)
    {
        Normalizar(cliente);
        Validar(cliente);
        cliente.Activo = true;
        return _datos.Insertar(cliente);
    }

    public void Actualizar(Cliente cliente)
    {
        if (cliente.Id <= 0)
            throw new ReglaNegocioException("Seleccione un cliente para actualizar.");

        Normalizar(cliente);
        Validar(cliente);
        if (!_datos.Actualizar(cliente))
            throw new ReglaNegocioException("El cliente ya no existe. Actualice la lista.");
    }

    public void Desactivar(int id) => CambiarEstado(id, activo: false);

    public void Reactivar(int id) => CambiarEstado(id, activo: true);

    /// <summary>Un cliente con facturas no se elimina: se desactiva</summary>
    public void Eliminar(int id)
    {
        if (_datos.TieneFacturas(id))
            throw new ReglaNegocioException(
                "El cliente tiene facturas y no se puede eliminar. Puede desactivarlo para que no se le facture más.");

        if (!_datos.Eliminar(id))
            throw new ReglaNegocioException(
                "No se pudo eliminar el cliente: ya no existe o se le acaba de registrar una factura.");
    }

    public Cliente? ObtenerPorId(int id) => _datos.ObtenerPorId(id);

    public List<Cliente> Buscar(string? texto, bool soloActivos) =>
        _datos.Buscar(texto, soloActivos, MaximoResultados);

    private void CambiarEstado(int id, bool activo)
    {
        if (id <= 0)
            throw new ReglaNegocioException("Seleccione un cliente.");
        if (!_datos.CambiarEstado(id, activo))
            throw new ReglaNegocioException("El cliente ya no existe. Actualice la lista.");
    }

    private static void Normalizar(Cliente c)
    {
        c.Codigo = (c.Codigo ?? string.Empty).Trim();
        c.Nombre = (c.Nombre ?? string.Empty).Trim();
        c.RucCedula = VacioANulo(c.RucCedula);
        c.Telefono = VacioANulo(c.Telefono);
        c.Email = VacioANulo(c.Email);
        c.Direccion = VacioANulo(c.Direccion);
    }

    private void Validar(Cliente c)
    {
        if (c.Codigo.Length == 0)
            throw Error("El código es obligatorio.", nameof(Cliente.Codigo));
        if (c.Codigo.Length > 20)
            throw Error("El código no puede tener más de 20 caracteres.", nameof(Cliente.Codigo));
        if (c.Nombre.Length == 0)
            throw Error("El nombre es obligatorio.", nameof(Cliente.Nombre));
        if (c.Nombre.Length > 150)
            throw Error("El nombre no puede tener más de 150 caracteres.", nameof(Cliente.Nombre));
        if (c.RucCedula is { Length: > 30 })
            throw Error("La cédula o RUC no puede tener más de 30 caracteres.", nameof(Cliente.RucCedula));
        if (c.Telefono is { Length: > 30 })
            throw Error("El teléfono no puede tener más de 30 caracteres.", nameof(Cliente.Telefono));
        if (c.Direccion is { Length: > 250 })
            throw Error("La dirección no puede tener más de 250 caracteres.", nameof(Cliente.Direccion));
        if (c.Email is not null && !EmailValido(c.Email))
            throw Error("El correo electrónico no tiene un formato válido.", nameof(Cliente.Email));
        if (c.LimiteCredito < 0)
            throw Error("El límite de crédito no puede ser negativo.", nameof(Cliente.LimiteCredito));
        if (c.PlazoDias < 0)
            throw Error("El plazo en días no puede ser negativo.", nameof(Cliente.PlazoDias));

        if (_datos.ExisteCodigo(c.Codigo, c.Id))
            throw Error("Ya existe otro cliente con ese código.", nameof(Cliente.Codigo));
    }

    private static ReglaNegocioException Error(string mensaje, string campo) =>
        new(mensaje) { Campo = campo };

    private static string? VacioANulo(string? texto)
    {
        var t = texto?.Trim();
        return string.IsNullOrEmpty(t) ? null : t;
    }

    /// <summary>Indica si el correo tiene un formato válido</summary>
    public static bool EmailValido(string email)
    {
        if (email.Length > 150 || !MailAddress.TryCreate(email, out var direccion))
            return false;
        return direccion.Address == email && direccion.Host.Contains('.');
    }
}
```

<a id="kobranegociofacturanegociocs"></a>

### Kobra.Negocio/FacturaNegocio.cs

```csharp
using Kobra.Datos;
using Kobra.Entidades;

namespace Kobra.Negocio;

/// <summary>Validaciones y reglas de negocio de Factura</summary>
public class FacturaNegocio
{
    public const int MaximoResultados = 50;

    private readonly FacturaDatos _datos;
    private readonly ClienteDatos _clienteDatos;

    public FacturaNegocio() : this(new FacturaDatos(), new ClienteDatos()) { }

    public FacturaNegocio(FacturaDatos datos, ClienteDatos clienteDatos)
    {
        _datos = datos;
        _clienteDatos = clienteDatos;
    }

    public int Registrar(Factura factura)
    {
        Normalizar(factura);
        Validar(factura);
        return _datos.Insertar(factura);
    }

    public void Anular(int id, string? motivo)
    {
        if (id <= 0)
            throw new ReglaNegocioException("Seleccione una factura para anular.");

        var motivoLimpio = (motivo ?? string.Empty).Trim();
        if (motivoLimpio.Length == 0)
            throw Error("El motivo de anulación es obligatorio.", nameof(Factura.MotivoAnulacion));
        if (motivoLimpio.Length > 250)
            throw Error("El motivo de anulación no puede tener más de 250 caracteres.", nameof(Factura.MotivoAnulacion));

        var actual = _datos.ObtenerPorId(id)
            ?? throw new ReglaNegocioException("La factura seleccionada ya no existe.");

        if (actual.Anulada)
            throw new ReglaNegocioException("La factura ya se encuentra anulada.");

        if (_datos.TienePagosActivos(id))
            throw new ReglaNegocioException(
                "No se puede anular una factura que tiene pagos activos. Anule primero sus pagos.");

        if (!_datos.Anular(id, motivoLimpio))
            throw new ReglaNegocioException(
                "No se pudo anular la factura: ya está anulada o se le acaba de registrar un pago.");
    }

    public Factura? ObtenerPorId(int id) => _datos.ObtenerPorId(id);

    public Factura? ObtenerPorNumero(string? numero)
    {
        var limpio = (numero ?? string.Empty).Trim();
        return limpio.Length == 0 ? null : _datos.ObtenerPorNumero(limpio);
    }

    public decimal ObtenerSaldoCliente(int clienteId) =>
        clienteId > 0 ? _datos.ObtenerSaldoCliente(clienteId) : 0m;

    public List<Factura> Buscar(string? texto, int? clienteId = null, string? estado = null, bool soloVencidas = false) =>
        _datos.Buscar(texto, clienteId, estado, soloVencidas, MaximoResultados);

    public List<PagoFactura> ListarPagosDeFactura(int facturaId) =>
        facturaId > 0 ? _datos.ListarPagosDeFactura(facturaId) : [];

    private static void Normalizar(Factura f)
    {
        f.Numero = (f.Numero ?? string.Empty).Trim().ToUpperInvariant();
        f.FechaEmision = f.FechaEmision.Date;
        f.FechaVencimiento = f.FechaVencimiento.Date;
        f.Subtotal = Math.Round(f.Subtotal, 2, MidpointRounding.AwayFromZero);
        f.Itbms = Math.Round(f.Itbms, 2, MidpointRounding.AwayFromZero);
        f.Total = Math.Round(f.Subtotal + f.Itbms, 2, MidpointRounding.AwayFromZero);
    }

    private void Validar(Factura f)
    {
        if (f.ClienteId <= 0)
            throw Error("Seleccione un cliente.", nameof(Factura.ClienteId));

        var cliente = _clienteDatos.ObtenerPorId(f.ClienteId)
            ?? throw Error("El cliente seleccionado no existe.", nameof(Factura.ClienteId));

        if (!cliente.Activo)
            throw Error("No se puede facturar a un cliente inactivo.", nameof(Factura.ClienteId));

        if (f.Numero.Length == 0)
            throw Error("El número de factura es obligatorio.", nameof(Factura.Numero));
        if (f.Numero.Length > 30)
            throw Error("El número de factura no puede tener más de 30 caracteres.", nameof(Factura.Numero));

        if (f.FechaVencimiento < f.FechaEmision)
            throw Error("La fecha de vencimiento no puede ser anterior a la emisión.", nameof(Factura.FechaVencimiento));

        if (f.Subtotal < 0)
            throw Error("El subtotal no puede ser negativo.", nameof(Factura.Subtotal));
        if (f.Itbms < 0)
            throw Error("El ITBMS no puede ser negativo.", nameof(Factura.Itbms));
        if (f.Total <= 0)
            throw Error("El total de la factura debe ser mayor que 0.", nameof(Factura.Total));

        if (_datos.ExisteNumero(f.Numero, f.Id))
            throw Error("Ya existe una factura con ese número.", nameof(Factura.Numero));

        var saldoActual = _datos.ObtenerSaldoCliente(cliente.Id);
        if (saldoActual + f.Total > cliente.LimiteCredito)
        {
            var disponible = Math.Max(0m, cliente.LimiteCredito - saldoActual);
            throw Error(
                $"La factura excede el límite de crédito del cliente (Límite: B/. {cliente.LimiteCredito:N2}, " +
                $"Saldo actual: B/. {saldoActual:N2}, Disponible: B/. {disponible:N2}).",
                nameof(Factura.Subtotal));
        }
    }

    private static ReglaNegocioException Error(string mensaje, string campo) =>
        new(mensaje) { Campo = campo };
}
```

<a id="kobranegociopagonegociocs"></a>

### Kobra.Negocio/PagoNegocio.cs

```csharp
using Kobra.Datos;
using Kobra.Entidades;

namespace Kobra.Negocio;

/// <summary>Validaciones y reglas de Pago</summary>
public class PagoNegocio
{
    public const int MaximoResultados = 50;

    private readonly PagoDatos _datos;
    private readonly FacturaDatos _facturaDatos;

    public PagoNegocio() : this(new PagoDatos(), new FacturaDatos()) { }

    public PagoNegocio(PagoDatos datos, FacturaDatos facturaDatos)
    {
        _datos = datos;
        _facturaDatos = facturaDatos;
    }

    /// <summary>Registra un pago y devuelve su Id</summary>
    public int Registrar(Pago pago)
    {
        Normalizar(pago);
        Validar(pago);

        var nuevoId = _datos.Insertar(pago);
        if (nuevoId is int id)
            return id;

        var factura = _facturaDatos.ObtenerPorId(pago.FacturaId);
        if (factura is null)
            throw new ReglaNegocioException("La factura seleccionada ya no existe.");

        if (factura.Anulada)
            throw Error("No se puede registrar un pago a una factura anulada.", nameof(Pago.FacturaId));

        throw Error(
            $"El monto supera el saldo disponible de la factura (B/. {factura.Saldo:N2}).",
            nameof(Pago.Monto));
    }

    /// <summary>Anulación lógica: exige motivo y comprueba que el pago siga activo</summary>
    public void Anular(int id, string? motivo)
    {
        if (id <= 0)
            throw new ReglaNegocioException("Seleccione un pago para anular.");

        var motivoLimpio = (motivo ?? string.Empty).Trim();
        if (motivoLimpio.Length == 0)
            throw Error("El motivo de anulación es obligatorio.", nameof(Pago.MotivoAnulacion));
        if (motivoLimpio.Length > 250)
            throw Error("El motivo de anulación no puede tener más de 250 caracteres.", nameof(Pago.MotivoAnulacion));

        var actual = _datos.ObtenerPorId(id)
            ?? throw new ReglaNegocioException("El pago seleccionado ya no existe.");

        if (actual.Anulado)
            throw new ReglaNegocioException("El pago ya se encuentra anulado.");

        if (!_datos.Anular(id, motivoLimpio))
            throw new ReglaNegocioException("No se pudo anular el pago: ya estaba anulado.");
    }

    public Pago? ObtenerPorId(int id) => _datos.ObtenerPorId(id);

    public decimal ObtenerSaldoFactura(int facturaId) =>
        facturaId > 0 ? _datos.ObtenerSaldoFactura(facturaId) : 0m;

    public List<Pago> Buscar(string? texto, int? clienteId = null, bool? anulado = null) =>
        _datos.Buscar(texto, clienteId, anulado, MaximoResultados);

    public List<Pago> ListarPorFactura(int facturaId) =>
        facturaId > 0 ? _datos.ListarPorFactura(facturaId) : [];

    /// <summary>Facturas del cliente que todavía tienen saldo, ordenadas por vencimiento</summary>
    public List<Factura> ObtenerFacturasConSaldo(int clienteId) =>
        clienteId > 0
            ? _facturaDatos.Buscar(null, clienteId, null, false, MaximoResultados)
                .Where(f => !f.Anulada && f.Saldo > 0)
                .OrderBy(f => f.FechaVencimiento)
                .ToList()
            : [];

    private static void Normalizar(Pago p)
    {
        p.FechaPago = p.FechaPago.Date;
        p.Monto = Math.Round(p.Monto, 2, MidpointRounding.AwayFromZero);
    }

    private void Validar(Pago p)
    {
        if (p.FacturaId <= 0)
            throw Error("Seleccione una factura.", nameof(Pago.FacturaId));

        var factura = _facturaDatos.ObtenerPorId(p.FacturaId)
            ?? throw Error("La factura seleccionada ya no existe.", nameof(Pago.FacturaId));

        if (factura.Anulada)
            throw Error("No se puede pagar una factura anulada.", nameof(Pago.FacturaId));

        if (p.FechaPago.Date > DateTime.Today)
            throw Error("La fecha de pago no puede ser futura.", nameof(Pago.FechaPago));

        if (p.Monto <= 0)
            throw Error("El monto del pago debe ser mayor que 0.", nameof(Pago.Monto));

        if (p.Monto > factura.Saldo)
            throw Error(
                $"El monto supera el saldo disponible de la factura (B/. {factura.Saldo:N2}).",
                nameof(Pago.Monto));
    }

    private static ReglaNegocioException Error(string mensaje, string campo) =>
        new(mensaje) { Campo = campo };
}
```

<a id="kobranegocioreglanegocioexceptioncs"></a>

### Kobra.Negocio/ReglaNegocioException.cs

```csharp
namespace Kobra.Negocio;

/// <summary>Se incumple una regla de negocio o una validación</summary>
public class ReglaNegocioException : Exception
{
    public ReglaNegocioException(string mensaje) : base(mensaje) { }

    /// <summary>Nombre del campo con el problema, para que la UI marque el control con ErrorProvider</summary>
    public string? Campo { get; init; }
}
```

<a id="kobranegocioreportenegociocs"></a>

### Kobra.Negocio/ReporteNegocio.cs

```csharp
using Kobra.Datos;
using Kobra.Entidades;

namespace Kobra.Negocio;

/// <summary>Reglas de presentación de los reportes de Pagos: estado de cuenta del cliente y antigüedad de saldos</summary>
public class ReporteNegocio
{
    private static readonly string[] NombresTramos = ["Corriente", "1-30", "31-60", "61-90", "Más de 90"];

    private readonly ReporteDatos _datos;
    private readonly PagoDatos _pagoDatos;

    public ReporteNegocio() : this(new ReporteDatos(), new PagoDatos()) { }

    public ReporteNegocio(ReporteDatos datos, PagoDatos pagoDatos)
    {
        _datos = datos;
        _pagoDatos = pagoDatos;
    }

    /// <summary>Estado de cuenta de un cliente, con facturas y totales</summary>
    public EstadoCuenta? ObtenerEstadoCuenta(int clienteId)
    {
        if (clienteId <= 0)
            throw new ReglaNegocioException("Seleccione un cliente para ver su estado de cuenta.");

        return _datos.ObtenerEstadoCuenta(clienteId);
    }

    /// <summary>Antigüedad de saldos con los mismos cinco tramos del panel de inicio</summary>
    public AntiguedadSaldos ObtenerAntiguedad(int? clienteId)
    {
        var filtro = clienteId is > 0 ? clienteId : null;
        var facturas = _datos.ObtenerFacturasConSaldo(filtro);

        var resultado = new AntiguedadSaldos
        {
            Facturas = facturas.Count,
            SaldoTotal = facturas.Sum(f => f.Saldo)
        };

        for (var i = 0; i < NombresTramos.Length; i++)
            resultado.Tramos.Add(new TramoAntiguedad { Nombre = NombresTramos[i], Orden = i });

        foreach (var factura in facturas)
        {
            var tramo = resultado.Tramos[ObtenerOrdenTramo(factura)];
            tramo.Detalle.Add(factura);
            tramo.Facturas++;
            tramo.Saldo += factura.Saldo;
        }

        foreach (var tramo in resultado.Tramos)
            tramo.Porcentaje = resultado.SaldoTotal == 0 ? 0 : tramo.Saldo / resultado.SaldoTotal * 100m;

        return resultado;
    }

    /// <summary>Pagos de una factura, para el detalle del estado de cuenta</summary>
    public List<Pago> ListarPagosDeFactura(int facturaId) =>
        facturaId > 0 ? _pagoDatos.ListarPorFactura(facturaId) : [];

    private static int ObtenerOrdenTramo(LineaAntiguedad factura) => factura.DiasVencidos switch
    {
        <= 0 => 0,
        <= 30 => 1,
        <= 60 => 2,
        <= 90 => 3,
        _ => 4
    };
}
```

<a id="kobranegocioresumennegociocs"></a>

### Kobra.Negocio/ResumenNegocio.cs

```csharp
using Kobra.Datos;
using Kobra.Entidades;

namespace Kobra.Negocio;

/// <summary>Datos del panel de inicio</summary>
public class ResumenNegocio
{
    public const int MaximoClientes = 8;

    private readonly ResumenDatos _datos;

    public ResumenNegocio() : this(new ResumenDatos()) { }

    public ResumenNegocio(ResumenDatos datos) => _datos = datos;

    public ResumenCartera Obtener() => _datos.Obtener(MaximoClientes);
}
```

---

<a id="5-capa-de-presentacion-kobraui"></a>

## 5. Capa de presentación (Kobra.UI)

<a id="punto-de-entrada"></a>

### Punto de entrada

<a id="kobrauiprogramcs"></a>

#### Kobra.UI/Program.cs

```csharp
namespace Kobra.UI
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new FrmPrincipal());
        }
    }
}
```

<a id="ventana-principal-y-panel-de-inicio"></a>

### Ventana principal y panel de inicio

<a id="kobrauiprincipalctliniciocs"></a>

#### Kobra.UI/Principal/CtlInicio.cs

```csharp
using Kobra.Datos;
using Kobra.Entidades;
using Kobra.Negocio;

namespace Kobra.UI
{
    public enum Modulo { Clientes, Facturas, Pagos }

    /// <summary>Panel de inicio: estado de la cartera, accesos a los módulos, antigüedad de saldos y clientes con más saldo</summary>
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

        /// <summary>Consulta el resumen en un hilo aparte para no congelar la ventana</summary>
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
```

<a id="kobrauiprincipalfrmprincipalcs"></a>

#### Kobra.UI/Principal/FrmPrincipal.cs

```csharp
using System.Reflection;
using Kobra.Negocio;

namespace Kobra.UI
{
    public partial class FrmPrincipal : Form
    {
        private readonly CtlInicio _inicio = new();

        public FrmPrincipal()
        {
            InitializeComponent();
            AplicarTema();

            _inicio.ModuloSolicitado += AbrirModulo;
            Controls.Add(_inicio);
            _inicio.BringToFront();
        }

        private void AplicarTema()
        {
            BackColor = Tema.Superficie;

            menuPrincipal.Renderer = new ToolStripProfessionalRenderer(new ColoresMenu());
            menuPrincipal.BackColor = Tema.ContenedorBajo;
            menuPrincipal.ForeColor = Tema.TextoSecundario;
            menuPrincipal.Font = Tema.Normal(9.5F);
            menuPrincipal.Padding = new Padding(6, 4, 6, 4);

            barraEstado.Renderer = new ToolStripProfessionalRenderer(new ColoresMenu());
            barraEstado.BackColor = Tema.ContenedorAlto;
            barraEstado.ForeColor = Tema.TextoSecundario;
            barraEstado.Font = Tema.Numeros(8.5F);
            foreach (var item in new[] { lblEstadoBd, lblEstadoServidor })
                item.Padding = new Padding(0, 0, 14, 0);

            var (servidor, baseDatos) = BaseDatos.Destino();
            lblEstadoBd.Text = $"BD: {(baseDatos.Length > 0 ? baseDatos : "—")}";
            lblEstadoServidor.Text = $"SERVIDOR: {(servidor.Length > 0 ? servidor : "—")}";
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            lblEstadoVersion.Text = version is null ? string.Empty : $"v{version.Major}.{version.Minor}.{version.Build}";
        }

        private async void FrmPrincipal_Shown(object? sender, EventArgs e)
        {
            var error = await Task.Run(BaseDatos.ProbarConexion);
            if (IsDisposed)
                return;

            if (error is not null)
                MessageBox.Show(this, error, "Sin conexión a la base de datos",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);

            await _inicio.ActualizarAsync();
            _inicio.FocoInicial();
        }

        private void FrmPrincipal_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5 && _inicio.Visible)
            {
                e.Handled = true;
                _ = _inicio.ActualizarAsync();
            }
        }

        private void mnuInicio_Click(object? sender, EventArgs e)
        {
            foreach (var hijo in MdiChildren.ToList())
                hijo.Close();
            MostrarInicioSiCorresponde();
        }

        private void mnuClientes_Click(object? sender, EventArgs e) => AbrirModulo(Modulo.Clientes);

        private void mnuFacturas_Click(object? sender, EventArgs e) => AbrirModulo(Modulo.Facturas);

        private void mnuPagos_Click(object? sender, EventArgs e) => AbrirModulo(Modulo.Pagos);

        private void mnuSalir_Click(object? sender, EventArgs e) => Close();

        private void AbrirModulo(Modulo modulo)
        {
            switch (modulo)
            {
                case Modulo.Clientes: AbrirModulo<FrmClientes>(); break;
                case Modulo.Facturas: AbrirModulo<FrmFacturas>(); break;
                case Modulo.Pagos: AbrirModulo<FrmPagos>(); break;
            }
        }

        private void AbrirModulo<T>() where T : Form, new()
        {
            var abierto = MdiChildren.OfType<T>().FirstOrDefault();
            if (abierto is not null)
            {
                abierto.Activate();
                return;
            }

            _inicio.Visible = false;
            var formulario = new T { MdiParent = this };

            formulario.FormClosed += (_, _) => BeginInvoke(MostrarInicioSiCorresponde);
            formulario.Show();
        }

        private void MostrarInicioSiCorresponde()
        {
            if (IsDisposed || MdiChildren.Any(f => !f.IsDisposed))
                return;

            _inicio.Visible = true;
            _inicio.BringToFront();
            _ = _inicio.ActualizarAsync();
        }

        private sealed class ColoresMenu : ProfessionalColorTable
        {
            public ColoresMenu() => UseSystemColors = false;

            public override Color MenuItemSelected => Tema.ContenedorAlto;
            public override Color MenuItemSelectedGradientBegin => Tema.ContenedorAlto;
            public override Color MenuItemSelectedGradientEnd => Tema.ContenedorAlto;
            public override Color MenuItemBorder => Tema.ContenedorAlto;
            public override Color MenuItemPressedGradientBegin => Tema.ContenedorAlto;
            public override Color MenuItemPressedGradientEnd => Tema.ContenedorAlto;
            public override Color MenuBorder => Tema.Borde;
            public override Color ToolStripDropDownBackground => Tema.Superficie;
            public override Color ImageMarginGradientBegin => Tema.Superficie;
            public override Color ImageMarginGradientMiddle => Tema.Superficie;
            public override Color ImageMarginGradientEnd => Tema.Superficie;
            public override Color MenuStripGradientBegin => Tema.ContenedorBajo;
            public override Color MenuStripGradientEnd => Tema.ContenedorBajo;
            public override Color StatusStripGradientBegin => Tema.ContenedorAlto;
            public override Color StatusStripGradientEnd => Tema.ContenedorAlto;
        }
    }
}
```

<a id="kobrauiprincipalfrmprincipaldesignercs"></a>

#### Kobra.UI/Principal/FrmPrincipal.Designer.cs

```csharp
namespace Kobra.UI
{
    partial class FrmPrincipal
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
            menuPrincipal = new MenuStrip();
            mnuInicio = new ToolStripMenuItem();
            mnuClientes = new ToolStripMenuItem();
            mnuFacturas = new ToolStripMenuItem();
            mnuPagos = new ToolStripMenuItem();
            mnuSalir = new ToolStripMenuItem();
            barraEstado = new StatusStrip();
            lblEstadoBd = new ToolStripStatusLabel();
            lblEstadoServidor = new ToolStripStatusLabel();
            lblEstadoVersion = new ToolStripStatusLabel();
            menuPrincipal.SuspendLayout();
            barraEstado.SuspendLayout();
            SuspendLayout();

            menuPrincipal.Items.AddRange(new ToolStripItem[] { mnuInicio, mnuClientes, mnuFacturas, mnuPagos, mnuSalir });
            menuPrincipal.Location = new Point(0, 0);
            menuPrincipal.Name = "menuPrincipal";
            menuPrincipal.Size = new Size(1100, 24);
            menuPrincipal.TabIndex = 0;

            mnuInicio.Name = "mnuInicio";
            mnuInicio.ShortcutKeys = Keys.Control | Keys.D0;
            mnuInicio.Size = new Size(48, 20);
            mnuInicio.Text = "&Inicio";
            mnuInicio.Click += mnuInicio_Click;

            mnuClientes.Name = "mnuClientes";
            mnuClientes.ShortcutKeys = Keys.Control | Keys.D1;
            mnuClientes.Size = new Size(61, 20);
            mnuClientes.Text = "&Clientes";
            mnuClientes.Click += mnuClientes_Click;

            mnuFacturas.Name = "mnuFacturas";
            mnuFacturas.ShortcutKeys = Keys.Control | Keys.D2;
            mnuFacturas.Size = new Size(61, 20);
            mnuFacturas.Text = "&Facturas";
            mnuFacturas.Click += mnuFacturas_Click;

            mnuPagos.Name = "mnuPagos";
            mnuPagos.ShortcutKeys = Keys.Control | Keys.D3;
            mnuPagos.Size = new Size(48, 20);
            mnuPagos.Text = "&Pagos";
            mnuPagos.Click += mnuPagos_Click;

            mnuSalir.Name = "mnuSalir";
            mnuSalir.Size = new Size(41, 20);
            mnuSalir.Text = "&Salir";
            mnuSalir.Click += mnuSalir_Click;

            barraEstado.Items.AddRange(new ToolStripItem[] { lblEstadoBd, lblEstadoServidor, lblEstadoVersion });
            barraEstado.Name = "barraEstado";
            barraEstado.SizingGrip = false;
            barraEstado.TabIndex = 1;

            lblEstadoBd.Name = "lblEstadoBd";

            lblEstadoServidor.Name = "lblEstadoServidor";
            lblEstadoServidor.Spring = true;
            lblEstadoServidor.TextAlign = ContentAlignment.MiddleLeft;

            lblEstadoVersion.Name = "lblEstadoVersion";

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
        private ToolStripStatusLabel lblEstadoBd;
        private ToolStripStatusLabel lblEstadoServidor;
        private ToolStripStatusLabel lblEstadoVersion;
    }
}
```

<a id="kobrauiprincipaltarjetamodulocs"></a>

#### Kobra.UI/Principal/TarjetaModulo.cs

```csharp
namespace Kobra.UI
{
    /// <summary>Fila del panel de inicio que abre un módulo</summary>
    internal sealed class TarjetaModulo : Panel
    {
        private readonly Label _titulo = new();
        private readonly Label _descripcion = new();
        private readonly Label _acceso = new();

        public TarjetaModulo(string titulo, string descripcion, string acceso)
        {
            SetStyle(ControlStyles.Selectable | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
            TabStop = true;
            Cursor = Cursors.Hand;
            BackColor = Tema.PrimarioContenedor;
            Dock = DockStyle.Fill;
            Margin = new Padding(0, 0, 0, 8);

            Size = new Size(LogicalToDeviceUnits(200), LogicalToDeviceUnits(60));
            MinimumSize = new Size(0, LogicalToDeviceUnits(60));

            var diseno = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 2,
                BackColor = Color.Transparent,
                Padding = new Padding(12, 8, 12, 8)
            };
            diseno.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            diseno.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            diseno.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            diseno.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            Configurar(_titulo, titulo, Tema.Negrita(9.5F), Tema.SobrePrimario);
            Configurar(_descripcion, descripcion, Tema.Normal(8.5F), Tema.ContenedorBajo);
            _descripcion.AutoSize = false;
            Configurar(_acceso, acceso, Tema.Numeros(8.5F), Tema.SobrePrimarioContenedor);
            _acceso.TextAlign = ContentAlignment.MiddleRight;

            diseno.Controls.Add(_titulo, 0, 0);
            diseno.Controls.Add(_acceso, 1, 0);
            diseno.Controls.Add(_descripcion, 0, 1);
            diseno.SetColumnSpan(_descripcion, 2);
            Controls.Add(diseno);

            foreach (Control c in new Control[] { diseno, _titulo, _descripcion, _acceso })
            {
                c.Click += (_, _) => Abrir();
                c.MouseEnter += (_, _) => Resaltar(true);
                c.MouseLeave += (_, _) => Resaltar(false);
                c.Cursor = Cursors.Hand;
            }
        }

        /// <summary>Se dispara al hacer clic, o con Enter o Espacio con la tarjeta enfocada</summary>
        public event EventHandler? Abierta;

        private void Abrir()
        {
            Focus();
            Abierta?.Invoke(this, EventArgs.Empty);
        }

        private static void Configurar(Label l, string texto, Font fuente, Color color)
        {
            l.Text = texto;
            l.Font = fuente;
            l.ForeColor = color;
            l.BackColor = Color.Transparent;
            l.AutoSize = true;
            l.Dock = DockStyle.Fill;
            l.Margin = new Padding(0, 0, 0, 2);
        }

        private void Resaltar(bool encima) =>
            BackColor = encima || Focused ? Tema.Primario : Tema.PrimarioContenedor;

        protected override bool IsInputKey(Keys keyData) =>
            keyData is Keys.Enter or Keys.Space || base.IsInputKey(keyData);

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.KeyCode is Keys.Enter or Keys.Space)
            {
                e.Handled = true;
                Abierta?.Invoke(this, EventArgs.Empty);
            }
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            Resaltar(false);
            Invalidate();
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            Resaltar(false);
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            using (var pincel = new SolidBrush(BackColor))
                e.Graphics.FillRectangle(pincel, ClientRectangle);

            if (Focused)
            {
                using var lapiz = new Pen(Tema.SobrePrimarioContenedor, 2);
                e.Graphics.DrawRectangle(lapiz, 1, 1, Width - 3, Height - 3);
            }
        }
    }
}
```

<a id="modulo-de-clientes"></a>

### Módulo de Clientes

<a id="kobrauiclientesctlclientefichacs"></a>

#### Kobra.UI/Clientes/CtlClienteFicha.cs

```csharp
using Kobra.Entidades;
using Kobra.Negocio;

namespace Kobra.UI
{
    /// <summary>Ficha de datos de un cliente: lectura y escritura de los campos, y validación al salir de cada campo</summary>
    internal sealed class CtlClienteFicha : UserControl
    {
        private readonly TarjetaPanel _tarjeta = new("INFORMACIÓN DEL CLIENTE");
        private readonly ErrorProvider _errores = new() { BlinkStyle = ErrorBlinkStyle.NeverBlink };

        private readonly TextBox _txtCodigo = new() { MaxLength = 20 };
        private readonly TextBox _txtNombre = new() { MaxLength = 150 };
        private readonly TextBox _txtRucCedula = new() { MaxLength = 30 };
        private readonly TextBox _txtTelefono = new() { MaxLength = 30 };
        private readonly TextBox _txtEmail = new() { MaxLength = 150 };
        private readonly TextBox _txtDireccion = new()
        {
            MaxLength = 250,
            Multiline = true,
            ScrollBars = ScrollBars.Vertical
        };
        private readonly NumericUpDown _nudLimite = new()
        {
            DecimalPlaces = 2,
            ThousandsSeparator = true,
            TextAlign = HorizontalAlignment.Right,
            Font = Tema.Numeros(9.5F),

            Maximum = Formato.MaximoMonto
        };
        private readonly NumericUpDown _nudPlazo = new()
        {
            Maximum = 3650,
            Value = 30,
            TextAlign = HorizontalAlignment.Right,
            Font = Tema.Numeros(9.5F)
        };
        private readonly CheckBox _chkActivo = new() { Text = "Cliente activo", AutoSize = true, Enabled = false, TabStop = false };
        private readonly TextBox _txtCreacion = CampoFicha.CrearSoloLectura();
        private readonly TextBox _txtModificacion = CampoFicha.CrearSoloLectura();

        public CtlClienteFicha()
        {
            SuspendLayout();
            AutoScaleMode = AutoScaleMode.None;
            Dock = DockStyle.Fill;
            Margin = new Padding(0, 8, 0, 0);
            AutoSize = true;

            _errores.ContainerControl = this;
            _txtCodigo.Validating += (_, _) => _errores.SetError(_txtCodigo,
                string.IsNullOrWhiteSpace(_txtCodigo.Text) ? "El código es obligatorio." : string.Empty);
            _txtNombre.Validating += (_, _) => _errores.SetError(_txtNombre,
                string.IsNullOrWhiteSpace(_txtNombre.Text) ? "El nombre es obligatorio." : string.Empty);
            _txtEmail.Validating += (_, _) =>
            {
                var correo = _txtEmail.Text.Trim();
                var invalido = correo.Length > 0 && !ClienteNegocio.EmailValido(correo);
                _errores.SetError(_txtEmail, invalido ? "El correo electrónico no tiene un formato válido." : string.Empty);
            };

            var orden = new Control[] { _txtCodigo, _txtNombre, _txtRucCedula, _txtTelefono, _txtEmail,
                                        _nudLimite, _nudPlazo, _txtDireccion };
            for (var i = 0; i < orden.Length; i++)
                orden[i].TabIndex = i;

            var rejilla = CampoFicha.CrearRejilla(6);
            CampoFicha.Agregar(rejilla, "Código", _txtCodigo, 0, 0, obligatorio: true);
            CampoFicha.Agregar(rejilla, "Nombre", _txtNombre, 1, 0, obligatorio: true);
            CampoFicha.Agregar(rejilla, "Cédula / RUC", _txtRucCedula, 2, 0);
            CampoFicha.Agregar(rejilla, "Teléfono", _txtTelefono, 3, 0);
            CampoFicha.Agregar(rejilla, "Correo", _txtEmail, 4, 0);
            CampoFicha.Agregar(rejilla, "Creado", _txtCreacion, 5, 0);

            CampoFicha.Agregar(rejilla, "Límite de crédito (B/.)", _nudLimite, 0, 1);
            CampoFicha.Agregar(rejilla, "Plazo (días)", _nudPlazo, 1, 1);
            CampoFicha.Agregar(rejilla, "Dirección", _txtDireccion, 2, 1, filasOcupadas: 2);
            CampoFicha.Agregar(rejilla, "Estado", _chkActivo, 4, 1);
            CampoFicha.Agregar(rejilla, "Modificado", _txtModificacion, 5, 1);

            _tarjeta.MostrarNota("* obligatorio");
            _tarjeta.Cuerpo.Controls.Add(rejilla, 0, 0);
            _tarjeta.Margin = new Padding(0);
            Controls.Add(_tarjeta);

            _tarjeta.Enter += (_, _) => FichaEnfocada?.Invoke(this, EventArgs.Empty);
            ResumeLayout(true);
        }

        public event EventHandler? FichaEnfocada;

        public string Codigo => _txtCodigo.Text.Trim();

        public string Nombre => _txtNombre.Text.Trim();

        public void EnfocarPrimero() => _txtCodigo.Focus();

        public Cliente Leer(int id, bool activo) => new()
        {
            Id = id,
            Codigo = _txtCodigo.Text,
            Nombre = _txtNombre.Text,
            RucCedula = _txtRucCedula.Text,
            Telefono = _txtTelefono.Text,
            Email = _txtEmail.Text,
            Direccion = _txtDireccion.Text,
            LimiteCredito = _nudLimite.Value,
            PlazoDias = (int)_nudPlazo.Value,
            Activo = activo
        };

        public void Cargar(Cliente c)
        {
            LimpiarErrores();
            _txtCodigo.Text = c.Codigo;
            _txtNombre.Text = c.Nombre;
            _txtRucCedula.Text = c.RucCedula ?? string.Empty;
            _txtTelefono.Text = c.Telefono ?? string.Empty;
            _txtEmail.Text = c.Email ?? string.Empty;
            _txtDireccion.Text = c.Direccion ?? string.Empty;
            _nudLimite.Value = Math.Min(c.LimiteCredito, _nudLimite.Maximum);
            _nudPlazo.Value = Math.Min(c.PlazoDias, _nudPlazo.Maximum);
            _chkActivo.Checked = c.Activo;
            _txtCreacion.Text = c.FechaCreacion.ToString(Formato.FechaHora);
            _txtModificacion.Text = c.FechaModificacion?.ToString(Formato.FechaHora) ?? "—";
        }

        /// <summary>Deja la ficha lista para un cliente nuevo (activo, plazo de 30 días)</summary>
        public void Limpiar()
        {
            LimpiarErrores();
            _txtCodigo.Clear();
            _txtNombre.Clear();
            _txtRucCedula.Clear();
            _txtTelefono.Clear();
            _txtEmail.Clear();
            _txtDireccion.Clear();
            _nudLimite.Value = 0;
            _nudPlazo.Value = 30;
            _chkActivo.Checked = true;
            _txtCreacion.Text = "Al guardar";
            _txtModificacion.Text = "—";
        }

        public void MostrarActivo(bool activo) => _chkActivo.Checked = activo;

        public void LimpiarErrores() => _errores.Clear();

        /// <summary>Marca el campo que causó el error de negocio y le da el foco</summary>
        public void MarcarCampo(string? campo, string mensaje)
        {
            Control? control = campo switch
            {
                nameof(Cliente.Codigo) => _txtCodigo,
                nameof(Cliente.Nombre) => _txtNombre,
                nameof(Cliente.RucCedula) => _txtRucCedula,
                nameof(Cliente.Telefono) => _txtTelefono,
                nameof(Cliente.Email) => _txtEmail,
                nameof(Cliente.Direccion) => _txtDireccion,
                nameof(Cliente.LimiteCredito) => _nudLimite,
                nameof(Cliente.PlazoDias) => _nudPlazo,
                _ => null
            };

            if (control is null)
                return;

            _errores.SetError(control, mensaje);
            control.Focus();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _errores.Dispose();
            base.Dispose(disposing);
        }
    }
}
```

<a id="kobrauiclientesctlclientelistacs"></a>

#### Kobra.UI/Clientes/CtlClienteLista.cs

```csharp
using Kobra.Datos;
using Kobra.Entidades;
using Kobra.Negocio;

namespace Kobra.UI
{
    /// <summary>Búsqueda de clientes (código, nombre o cédula/RUC) y rejilla de resultados</summary>
    internal sealed class CtlClienteLista : UserControl
    {
        private readonly ClienteNegocio _negocio = new();
        private readonly TarjetaPanel _tarjetaFiltros = new("FILTROS DE BÚSQUEDA");
        private readonly TarjetaPanel _tarjeta = new("CLIENTES");
        private readonly TextBox _txtBuscar = new();
        private readonly BotonTema _btnBuscar = new("&Buscar", EstiloBoton.Primario);
        private readonly CheckBox _chkSoloActivos = new();
        private readonly DataGridView _dgv = new();
        private readonly System.Windows.Forms.Timer _timer = new() { Interval = 300 };

        private List<Cliente> _clientes = [];
        private bool _cargando;

        public CtlClienteLista()
        {
            SuspendLayout();
            AutoScaleMode = AutoScaleMode.None;
            Dock = DockStyle.Fill;
            Margin = new Padding(0);

            _tarjetaFiltros.Margin = new Padding(0, 0, 0, 10);
            _tarjetaFiltros.Cuerpo.Controls.Add(CrearBusqueda(), 0, 0);

            _tarjeta.AutoSize = false;
            _tarjeta.Cuerpo.AutoSize = false;
            _tarjeta.Cuerpo.RowCount = 1;
            _tarjeta.Cuerpo.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            _tarjeta.Cuerpo.Controls.Add(CrearRejilla(), 0, 0);

            var raiz = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                BackColor = Color.Transparent,
                Margin = new Padding(0)
            };
            raiz.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            raiz.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            raiz.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            raiz.Controls.Add(_tarjetaFiltros, 0, 0);
            raiz.Controls.Add(_tarjeta, 0, 1);
            Controls.Add(raiz);

            _timer.Tick += (_, _) =>
            {
                _timer.Stop();
                BusquedaSolicitada?.Invoke(false);
            };
            ResumeLayout(true);
        }

        /// <summary>Se solicita recargar la lista</summary>
        public event Action<bool>? BusquedaSolicitada;

        /// <summary>El usuario eligió una fila (clic o teclado), no un cambio hecho por código</summary>
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

        /// <summary>Recarga según el filtro y deja marcado seleccionarId si aparece en los resultados</summary>
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
                _clientes = [];
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

        private Control CrearBusqueda()
        {
            var fila = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                ColumnCount = 4,
                RowCount = 1,
                BackColor = Color.Transparent,
                Margin = new Padding(0, 6, 0, 2)
            };
            fila.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            fila.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            fila.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            fila.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            _txtBuscar.MaxLength = 150;
            _txtBuscar.PlaceholderText = "Código, nombre o cédula/RUC";
            _txtBuscar.BorderStyle = BorderStyle.FixedSingle;
            _txtBuscar.Dock = DockStyle.Fill;
            _txtBuscar.Margin = new Padding(0, 3, 8, 3);
            _txtBuscar.TabIndex = 0;
            _txtBuscar.TextChanged += (_, _) =>
            {
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

            fila.Controls.Add(CampoFicha.CrearEtiquetaFiltro("Buscar (F2):"), 0, 0);
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
            EstiloGrid.AgregarColumna(_dgv, "colLimite", "Límite de crédito (B/.)", 15, derecha: true, formato: Formato.Monto,
                propiedad: "LimiteCredito");
            EstiloGrid.AgregarColumna(_dgv, "colPlazo", "Plazo", 8, derecha: true, propiedad: "PlazoDias");
            var activo = EstiloGrid.AgregarColumna(_dgv, "colActivo", "Activo", 8, propiedad: "Activo");
            activo.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            activo.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            _dgv.CellFormatting += Rejilla_CellFormatting;
            _dgv.Enter += (_, _) => RejillaEnfocada?.Invoke(this, EventArgs.Empty);

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
```

<a id="kobrauiclientesfrmclientescs"></a>

#### Kobra.UI/Clientes/FrmClientes.cs

```csharp
using Kobra.Datos;
using Kobra.Entidades;
using Kobra.Negocio;

namespace Kobra.UI
{
    /// <summary>Módulo de Clientes: registrar, consultar, actualizar, desactivar y eliminar</summary>
    public partial class FrmClientes : FormularioModulo
    {
        private readonly ClienteNegocio _negocio = new();

        private int _clienteId;
        private bool _activo = true;

        public FrmClientes()
        {
            InitializeComponent();
        }

        private void FrmClientes_Load(object? sender, EventArgs e)
        {
            PrepararNuevo();
            CargarLista(mostrarMensaje: true);
            lista.EnfocarBusqueda();
        }

        protected override bool ProcesarAtajo(Keys tecla)
        {
            switch (tecla)
            {
                case Keys.F2:
                    lista.EnfocarBusqueda();
                    return true;
                case Keys.F5:
                    CargarLista(mostrarMensaje: true);
                    return true;
                case Keys.Control | Keys.S:
                    Guardar();
                    return true;
                case Keys.Control | Keys.N:
                    PrepararNuevo();
                    return true;
                default:
                    return false;
            }
        }

        private void lista_BusquedaSolicitada(bool explicita) => CargarLista(mostrarMensaje: explicita);

        private void lista_ClienteSeleccionado(Cliente cliente)
        {
            if (cliente.Id != _clienteId)
                CargarEnFormulario(cliente);
        }

        private void btnNuevo_Click(object? sender, EventArgs e) => PrepararNuevo();

        private void btnGuardar_Click(object? sender, EventArgs e) => Guardar();

        private void btnCerrar_Click(object? sender, EventArgs e) => Close();

        private void btnDesactivar_Click(object? sender, EventArgs e)
        {
            if (_clienteId == 0)
                return;

            var desactivar = _activo;
            var confirmado = Confirmar(
                $"¿Desea {(desactivar ? "desactivar" : "reactivar")} a este cliente?",
                $"Código: {ficha.Codigo}  ·  Nombre: {ficha.Nombre}",
                desactivar ? "No se le podrá facturar mientras esté inactivo." : null);
            if (!confirmado)
                return;

            try
            {
                if (desactivar)
                    _negocio.Desactivar(_clienteId);
                else
                    _negocio.Reactivar(_clienteId);

                _activo = !_activo;
                ficha.MostrarActivo(_activo);
                ActualizarEstado();
                CargarLista(mostrarMensaje: false);
                barraAcciones.MostrarMensaje(_activo ? "Cliente reactivado." : "Cliente desactivado.", esError: false);
            }
            catch (Exception ex) when (ex is ReglaNegocioException or DatosException)
            {
                MostrarError(ex);
            }
        }

        private void btnEliminar_Click(object? sender, EventArgs e)
        {
            if (_clienteId == 0)
                return;

            var confirmado = Confirmar(
                "¿Eliminar definitivamente a este cliente?",
                $"Código: {ficha.Codigo}  ·  Nombre: {ficha.Nombre}",
                "Esta acción no se puede deshacer. Si el cliente tiene facturas, no se eliminará.",
                peligro: true, textoSi: "&Eliminar");
            if (!confirmado)
                return;

            try
            {
                _negocio.Eliminar(_clienteId);
                PrepararNuevo();
                CargarLista(mostrarMensaje: false);
                barraAcciones.MostrarMensaje("Cliente eliminado.", esError: false);
            }
            catch (Exception ex) when (ex is ReglaNegocioException or DatosException)
            {
                MostrarError(ex);
            }
        }

        private void CargarLista(bool mostrarMensaje, int? seleccionarId = null)
        {
            var mantener = seleccionarId ?? (_clienteId > 0 ? _clienteId : (int?)null);
            var error = lista.Cargar(mantener);

            if (error is not null && mostrarMensaje)
                MostrarAdvertencia(error);
        }

        private void Guardar()
        {
            ficha.LimpiarErrores();
            var cliente = ficha.Leer(_clienteId, _activo);

            try
            {
                if (cliente.Id == 0)
                {
                    cliente.Id = _negocio.Registrar(cliente);
                    _clienteId = cliente.Id;
                    ActualizarEstado();
                }
                else
                {
                    _negocio.Actualizar(cliente);
                }

                CargarLista(mostrarMensaje: false, seleccionarId: cliente.Id);

                if (_negocio.ObtenerPorId(cliente.Id) is { } guardado)
                    ficha.Cargar(guardado);
                barraAcciones.MostrarMensaje("Cliente guardado.", esError: false);
            }
            catch (ReglaNegocioException ex)
            {
                ficha.MarcarCampo(ex.Campo, ex.Message);
                MostrarError(ex);
            }
            catch (DatosException ex)
            {
                MostrarError(ex);
            }
        }

        private void PrepararNuevo()
        {
            _clienteId = 0;
            _activo = true;
            ficha.Limpiar();
            lista.QuitarSeleccion();
            ActualizarEstado();
            barraAcciones.RestablecerPista();
            ficha.EnfocarPrimero();
        }

        private void CargarEnFormulario(Cliente c)
        {
            _clienteId = c.Id;
            _activo = c.Activo;
            ficha.Cargar(c);
            ActualizarEstado();
        }

        private void ActualizarEstado()
        {
            var existe = _clienteId > 0;
            btnDesactivar.Enabled = existe;
            btnEliminar.Enabled = existe;
            btnDesactivar.Text = _activo ? "&Desactivar" : "&Reactivar";
            barraTitulo.Info = existe ? $"MODO: EDICIÓN · ID {_clienteId}" : "MODO: NUEVO CLIENTE";
        }
    }
}
```

<a id="kobrauiclientesfrmclientesdesignercs"></a>

#### Kobra.UI/Clientes/FrmClientes.Designer.cs

```csharp
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

            lista.Name = "lista";
            lista.TabIndex = 0;
            lista.BusquedaSolicitada += lista_BusquedaSolicitada;
            lista.ClienteSeleccionado += lista_ClienteSeleccionado;
            lista.BusquedaEnfocada += (_, _) => AcceptButton = lista.BotonBuscar;
            lista.RejillaEnfocada += (_, _) => AcceptButton = null;

            ficha.Name = "ficha";
            ficha.TabIndex = 1;
            ficha.FichaEnfocada += (_, _) => AcceptButton = btnGuardar;

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

            barraAcciones.Agregar(btnNuevo, btnGuardar, btnDesactivar, btnEliminar, btnCerrar);
            barraAcciones.Name = "barraAcciones";
            barraAcciones.TabIndex = 1;

            barraTitulo.Name = "barraTitulo";
            barraTitulo.TabIndex = 2;

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
```

<a id="modulo-de-facturas"></a>

### Módulo de Facturas

<a id="kobrauifacturasctlfacturaanulacioncs"></a>

#### Kobra.UI/Facturas/CtlFacturaAnulacion.cs

```csharp
using Kobra.Datos;
using Kobra.Entidades;
using Kobra.Negocio;

namespace Kobra.UI
{
    /// <summary>Ficha de anulación lógica de factura: busca por número, muestra datos en solo lectura y exige un motivo de hasta 250 caracteres</summary>
    internal sealed class CtlFacturaAnulacion : UserControl
    {
        private readonly FacturaNegocio _negocio = new();
        private readonly ErrorProvider _errores = new() { BlinkStyle = ErrorBlinkStyle.NeverBlink };

        private readonly TarjetaPanel _tarjetaDatos = new("DATOS DE LA FACTURA A ANULAR");
        private readonly TextBox _txtBuscarNumero = new() { MaxLength = 30, CharacterCasing = CharacterCasing.Upper };
        private readonly BotonTema _btnBuscar = new("&Buscar", EstiloBoton.Primario);

        private readonly TextBox _txtNumero = CampoFicha.CrearSoloLectura();
        private readonly TextBox _txtCliente = CampoFicha.CrearSoloLectura();
        private readonly TextBox _txtRuc = CampoFicha.CrearSoloLectura();
        private readonly TextBox _txtEmision = CampoFicha.CrearSoloLectura();
        private readonly TextBox _txtVencimiento = CampoFicha.CrearSoloLectura();
        private readonly TextBox _txtSubtotal = CampoFicha.CrearSoloLectura(derecha: true);
        private readonly TextBox _txtItbms = CampoFicha.CrearSoloLectura(derecha: true);
        private readonly TextBox _txtTotal = CampoFicha.CrearSoloLectura(derecha: true);
        private readonly TextBox _txtSaldo = CampoFicha.CrearSoloLectura(derecha: true);
        private readonly Label _lblEstado = new() { AutoSize = true, Font = Tema.Negrita(9F) };

        private readonly TarjetaMotivo _motivo = new(tabIndex: 2);

        public CtlFacturaAnulacion()
        {
            SuspendLayout();
            AutoScaleMode = AutoScaleMode.None;
            Dock = DockStyle.Fill;
            Margin = new Padding(0);

            _errores.ContainerControl = this;
            _motivo.Enfocado += (_, _) => MotivoEnfocado?.Invoke(this, EventArgs.Empty);

            _motivo.Caja.Validating += (_, _) =>
            {
                if (FacturaActual is { Anulada: false })
                    _errores.SetError(_motivo.Caja,
                        string.IsNullOrWhiteSpace(_motivo.Caja.Text) ? Mensajes.MotivoObligatorio : string.Empty);
            };

            var raiz = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                BackColor = Color.Transparent,
                Margin = new Padding(0)
            };
            raiz.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            raiz.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            raiz.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            raiz.Controls.Add(CrearTarjetaDatos(), 0, 0);
            raiz.Controls.Add(_motivo.Tarjeta, 0, 1);

            Controls.Add(raiz);
            ResumeLayout(true);
        }

        public event Action<Factura?>? FacturaCargada;
        public event EventHandler? BusquedaEnfocada;
        public event EventHandler? MotivoEnfocado;

        public Button BotonBuscar => _btnBuscar;
        public Factura? FacturaActual { get; private set; }
        public string Motivo => _motivo.Caja.Text.Trim();

        public void EnfocarBusqueda()
        {
            _txtBuscarNumero.Focus();
            _txtBuscarNumero.SelectAll();
        }

        public void Cargar(Factura f)
        {
            FacturaActual = f;
            _errores.Clear();

            _txtBuscarNumero.Text = f.Numero;
            _txtNumero.Text = f.Numero;
            _txtCliente.Text = f.ClienteDescripcion;
            _txtRuc.Text = string.IsNullOrWhiteSpace(f.ClienteRucCedula) ? "—" : f.ClienteRucCedula;
            _txtEmision.Text = f.FechaEmision.ToString(Formato.Fecha);
            _txtVencimiento.Text = f.FechaVencimiento.ToString(Formato.Fecha);
            _txtSubtotal.Text = f.Subtotal.Importe();
            _txtItbms.Text = f.Itbms.Importe();
            _txtTotal.Text = f.Total.Importe();
            _txtSaldo.Text = f.Saldo.Importe();

            _lblEstado.ForeColor = f.Anulada || f.EstaVencida ? Tema.Secundario : Tema.Primario;
            _lblEstado.Text = f.EstaVencida ? $"{f.Estado} · Vencida ({f.DiasVencidos} d)" : f.Estado;

            _motivo.Caja.Text = f.Anulada ? (f.MotivoAnulacion ?? string.Empty) : string.Empty;
            _motivo.Caja.ReadOnly = f.Anulada;

            if (f.Anulada)
                _tarjetaDatos.MostrarNota("Ya se encuentra anulada", esError: true);
            else if (f.TotalPagado > 0)
                _tarjetaDatos.MostrarNota("Tiene pagos activos; anúlelos primero", esError: true);
            else
                _tarjetaDatos.MostrarNota("Lista para anular", esError: false);

            FacturaCargada?.Invoke(f);

            if (!f.Anulada && f.TotalPagado == 0)
                _motivo.Caja.Focus();
        }

        public void Limpiar()
        {
            FacturaActual = null;
            _errores.Clear();
            _txtBuscarNumero.Clear();
            _txtNumero.Clear();
            _txtCliente.Clear();
            _txtRuc.Clear();
            _txtEmision.Clear();
            _txtVencimiento.Clear();
            _txtSubtotal.Clear();
            _txtItbms.Clear();
            _txtTotal.Clear();
            _txtSaldo.Clear();
            _lblEstado.Text = "—";
            _motivo.Caja.Clear();
            _motivo.Caja.ReadOnly = false;
            _tarjetaDatos.MostrarNota(string.Empty);
            FacturaCargada?.Invoke(null);
        }

        public void MarcarErrorMotivo(string mensaje)
        {
            _errores.SetError(_motivo.Caja, mensaje);
            _motivo.Caja.Focus();
        }

        private Control CrearTarjetaDatos()
        {
            _tarjetaDatos.Margin = new Padding(0, 0, 0, 10);

            var barra = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                Margin = new Padding(0, 6, 0, 6)
            };

            var lbl = new Label
            {
                Text = "Número de factura:",
                ForeColor = Tema.TextoSecundario,
                AutoSize = true,
                Margin = new Padding(0, 6, 8, 0)
            };

            _txtBuscarNumero.Width = LogicalToDeviceUnits(200);
            _txtBuscarNumero.BorderStyle = BorderStyle.FixedSingle;
            _txtBuscarNumero.PlaceholderText = "Ej. F-001";
            _txtBuscarNumero.Margin = new Padding(0, 3, 8, 3);
            _txtBuscarNumero.TabIndex = 0;
            _txtBuscarNumero.Enter += (_, _) => BusquedaEnfocada?.Invoke(this, EventArgs.Empty);

            _btnBuscar.TabIndex = 1;
            _btnBuscar.Click += (_, _) => BuscarPorNumero();

            barra.Controls.AddRange([lbl, _txtBuscarNumero, _btnBuscar]);

            _txtSaldo.ForeColor = Tema.Secundario;
            _txtSaldo.Font = Tema.Negrita(9F);
            _lblEstado.Margin = new Padding(0, 4, 0, 0);

            var rejilla = CampoFicha.CrearRejilla(5);
            CampoFicha.Agregar(rejilla, "Número", _txtNumero, 0, 0);
            CampoFicha.Agregar(rejilla, "Subtotal (B/.)", _txtSubtotal, 0, 1);
            CampoFicha.Agregar(rejilla, "Cliente", _txtCliente, 1, 0);
            CampoFicha.Agregar(rejilla, "ITBMS (B/.)", _txtItbms, 1, 1);
            CampoFicha.Agregar(rejilla, "Cédula / RUC", _txtRuc, 2, 0);
            CampoFicha.Agregar(rejilla, "Monto total (B/.)", _txtTotal, 2, 1);
            CampoFicha.Agregar(rejilla, "Fecha emisión", _txtEmision, 3, 0);
            CampoFicha.Agregar(rejilla, "Saldo pendiente (B/.)", _txtSaldo, 3, 1);
            CampoFicha.Agregar(rejilla, "Fecha vencimiento", _txtVencimiento, 4, 0);
            CampoFicha.Agregar(rejilla, "Estado actual", _lblEstado, 4, 1);

            _tarjetaDatos.Cuerpo.RowCount = 2;
            _tarjetaDatos.Cuerpo.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            _tarjetaDatos.Cuerpo.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            _tarjetaDatos.Cuerpo.Controls.Add(barra, 0, 0);
            _tarjetaDatos.Cuerpo.Controls.Add(rejilla, 0, 1);
            return _tarjetaDatos;
        }

        private void BuscarPorNumero()
        {
            _errores.Clear();
            var numero = _txtBuscarNumero.Text.Trim();
            if (numero.Length == 0)
            {
                _errores.SetError(_txtBuscarNumero, "Ingrese el número de factura.");
                _txtBuscarNumero.Focus();
                return;
            }

            try
            {
                var factura = _negocio.ObtenerPorNumero(numero);
                if (factura is null)
                {
                    Limpiar();
                    _txtBuscarNumero.Text = numero;
                    _tarjetaDatos.MostrarNota($"No existe la factura '{numero}'", esError: true);
                    _txtBuscarNumero.Focus();
                    return;
                }
                Cargar(factura);
            }
            catch (DatosException ex)
            {
                _tarjetaDatos.MostrarNota(ex.Message, esError: true);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _errores.Dispose();
            base.Dispose(disposing);
        }
    }
}
```

<a id="kobrauifacturasctlfacturaconsultacs"></a>

#### Kobra.UI/Facturas/CtlFacturaConsulta.cs

```csharp
using Kobra.Datos;
using Kobra.Entidades;
using Kobra.Negocio;

namespace Kobra.UI
{
    /// <summary>Consulta de facturas y saldos: filtros, grilla principal, pagos asociados y balance</summary>
    internal sealed class CtlFacturaConsulta : UserControl
    {
        private readonly FacturaNegocio _negocio = new();
        private readonly ClienteNegocio _clienteNegocio = new();
        private readonly System.Windows.Forms.Timer _timer = new() { Interval = 300 };

        private readonly TarjetaPanel _tarjetaFiltros = new("FILTROS DE BÚSQUEDA");
        private readonly TextBox _txtBuscar = new();
        private readonly ComboBox _cboCliente = new();
        private readonly ComboBox _cboEstado = new();
        private readonly CheckBox _chkSoloVencidas = new();
        private readonly BotonTema _btnBuscar = new("&Buscar", EstiloBoton.Primario);

        private readonly TarjetaPanel _tarjetaLista = new("FACTURAS Y SALDOS");
        private readonly DataGridView _dgvFacturas = new();
        private readonly BarraTotales _lblResumenCartera = new();

        private readonly TarjetaPanel _tarjetaPagos = new("PAGOS ASOCIADOS");
        private readonly DataGridView _dgvPagos = new();

        private readonly TarjetaPanel _tarjetaBalance = new("BALANCE DE FACTURA");
        private readonly Label _lblBalTotal = new();
        private readonly Label _lblBalPagado = new();
        private readonly Label _lblBalSaldo = new();
        private readonly Label _lblBalEstado = new();

        private List<Factura> _facturas = [];
        private bool _cargando;

        public CtlFacturaConsulta()
        {
            SuspendLayout();
            AutoScaleMode = AutoScaleMode.None;
            Dock = DockStyle.Fill;
            Margin = new Padding(0);

            var raiz = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                BackColor = Color.Transparent,
                Margin = new Padding(0)
            };
            raiz.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            raiz.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            raiz.RowStyles.Add(new RowStyle(SizeType.Percent, 64));
            raiz.RowStyles.Add(new RowStyle(SizeType.Percent, 36));

            raiz.Controls.Add(CrearFiltros(), 0, 0);
            raiz.Controls.Add(CrearListaPrincipal(), 0, 1);
            raiz.Controls.Add(CrearDetalleInferior(), 0, 2);

            Controls.Add(raiz);

            _timer.Tick += (_, _) =>
            {
                _timer.Stop();
                BusquedaSolicitada?.Invoke(false);
            };
            ResumeLayout(true);
        }

        public event Action<bool>? BusquedaSolicitada;
        public event Action<Factura?>? FacturaSeleccionada;
        public event EventHandler? BusquedaEnfocada;
        public event EventHandler? RejillaEnfocada;

        public Button BotonBuscar => _btnBuscar;

        public void EnfocarBusqueda()
        {
            _txtBuscar.Focus();
            _txtBuscar.SelectAll();
        }

        public void CargarClientesFiltro()
        {
            _cargando = true;
            try
            {
                ItemCliente.Llenar(_cboCliente, _clienteNegocio.Buscar(null, soloActivos: false), "Todos los clientes");
            }
            catch (DatosException ex)
            {
                _tarjetaFiltros.MostrarNota(ex.Message, esError: true);
            }
            finally
            {
                _cargando = false;
            }
        }

        public void LimpiarFiltros()
        {
            _cargando = true;
            try
            {
                _txtBuscar.Clear();
                if (_cboCliente.Items.Count > 0)
                    _cboCliente.SelectedIndex = 0;
                _cboEstado.SelectedIndex = 0;
                _chkSoloVencidas.Checked = false;
            }
            finally
            {
                _cargando = false;
            }
        }

        public string? Cargar(int? seleccionarId)
        {
            _timer.Stop();
            string? error = null;

            var clienteId = (_cboCliente.SelectedItem as ItemCliente)?.Id;
            if (clienteId == 0)
                clienteId = null;

            try
            {
                _facturas = _negocio.Buscar(
                    _txtBuscar.Text,
                    clienteId,
                    _cboEstado.SelectedItem?.ToString(),
                    _chkSoloVencidas.Checked);
            }
            catch (DatosException ex)
            {
                _facturas = [];
                error = ex.Message;
            }

            _cargando = true;
            Factura? seleccionada = null;
            try
            {
                _dgvFacturas.DataSource = _facturas;
                _dgvFacturas.ClearSelection();
                _dgvFacturas.CurrentCell = null;

                var fila = seleccionarId is int id ? _facturas.FindIndex(f => f.Id == id) : -1;
                if (fila < 0 && _facturas.Count > 0)
                    fila = 0;

                if (fila >= 0)
                {
                    _dgvFacturas.Rows[fila].Selected = true;
                    _dgvFacturas.CurrentCell = _dgvFacturas.Rows[fila].Cells["colNumero"];
                    seleccionada = _facturas[fila];
                }
            }
            finally
            {
                _cargando = false;
            }

            ActualizarResumenCartera();

            if (error is not null)
                _tarjetaLista.MostrarNota(error, esError: true);
            else
                _tarjetaLista.MostrarNota(_facturas.Count >= FacturaNegocio.MaximoResultados
                    ? $"Primeras {FacturaNegocio.MaximoResultados}: afine la búsqueda"
                    : $"{_facturas.Count} factura(s)");

            MostrarDetalleFactura(seleccionada);
            FacturaSeleccionada?.Invoke(seleccionada);
            return error;
        }

        private Control CrearFiltros()
        {
            _tarjetaFiltros.Margin = new Padding(0, 0, 0, 10);

            var fila = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                ColumnCount = 8,
                RowCount = 1,
                BackColor = Color.Transparent,
                Margin = new Padding(0, 6, 0, 2)
            };
            fila.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            fila.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 32));
            fila.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            fila.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 38));
            fila.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            fila.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));
            fila.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            fila.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            _txtBuscar.MaxLength = 150;
            _txtBuscar.PlaceholderText = "Número o cliente";
            _txtBuscar.BorderStyle = BorderStyle.FixedSingle;
            _txtBuscar.Dock = DockStyle.Fill;
            _txtBuscar.Margin = new Padding(0, 3, 10, 3);
            _txtBuscar.TabIndex = 0;
            _txtBuscar.TextChanged += (_, _) =>
            {
                _timer.Stop();
                _timer.Start();
            };
            _txtBuscar.Enter += (_, _) => BusquedaEnfocada?.Invoke(this, EventArgs.Empty);

            _cboCliente.DropDownStyle = ComboBoxStyle.DropDownList;
            _cboCliente.Dock = DockStyle.Fill;
            _cboCliente.Margin = new Padding(0, 3, 10, 3);
            _cboCliente.TabIndex = 1;
            _cboCliente.SelectedIndexChanged += (_, _) =>
            {
                if (!_cargando)
                    BusquedaSolicitada?.Invoke(false);
            };
            _cboCliente.Enter += (_, _) => BusquedaEnfocada?.Invoke(this, EventArgs.Empty);

            _cboEstado.DropDownStyle = ComboBoxStyle.DropDownList;
            _cboEstado.Dock = DockStyle.Fill;
            _cboEstado.Margin = new Padding(0, 3, 10, 3);
            _cboEstado.TabIndex = 2;
            _cboEstado.Items.AddRange(["Todas", "Pendiente", "Parcial", "Pagada", "Anulada"]);
            _cboEstado.SelectedIndex = 0;
            _cboEstado.SelectedIndexChanged += (_, _) =>
            {
                if (!_cargando)
                    BusquedaSolicitada?.Invoke(false);
            };
            _cboEstado.Enter += (_, _) => BusquedaEnfocada?.Invoke(this, EventArgs.Empty);

            _chkSoloVencidas.Text = "Solo vencidas";
            _chkSoloVencidas.ForeColor = Tema.Secundario;
            _chkSoloVencidas.Font = Tema.Negrita(8.5F);
            _chkSoloVencidas.AutoSize = true;
            _chkSoloVencidas.Anchor = AnchorStyles.Left;
            _chkSoloVencidas.Margin = new Padding(0, 0, 10, 0);
            _chkSoloVencidas.TabIndex = 3;
            _chkSoloVencidas.CheckedChanged += (_, _) =>
            {
                if (!_cargando)
                    BusquedaSolicitada?.Invoke(false);
            };
            _chkSoloVencidas.Enter += (_, _) => BusquedaEnfocada?.Invoke(this, EventArgs.Empty);

            _btnBuscar.TabIndex = 4;
            _btnBuscar.Margin = new Padding(0);
            _btnBuscar.Click += (_, _) =>
            {
                _timer.Stop();
                BusquedaSolicitada?.Invoke(true);
            };

            fila.Controls.Add(CampoFicha.CrearEtiquetaFiltro("Buscar (F2):"), 0, 0);
            fila.Controls.Add(_txtBuscar, 1, 0);
            fila.Controls.Add(CampoFicha.CrearEtiquetaFiltro("Cliente:"), 2, 0);
            fila.Controls.Add(_cboCliente, 3, 0);
            fila.Controls.Add(CampoFicha.CrearEtiquetaFiltro("Estado:"), 4, 0);
            fila.Controls.Add(_cboEstado, 5, 0);
            fila.Controls.Add(_chkSoloVencidas, 6, 0);
            fila.Controls.Add(_btnBuscar, 7, 0);

            _tarjetaFiltros.Cuerpo.Controls.Add(fila, 0, 0);
            return _tarjetaFiltros;
        }

        private Control CrearListaPrincipal()
        {
            _tarjetaLista.AutoSize = false;
            _tarjetaLista.Margin = new Padding(0, 0, 0, 10);
            _tarjetaLista.Cuerpo.AutoSize = false;
            _tarjetaLista.Cuerpo.RowCount = 2;
            _tarjetaLista.Cuerpo.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            _tarjetaLista.Cuerpo.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            EstiloGrid.Aplicar(_dgvFacturas);
            _dgvFacturas.Margin = new Padding(0, 6, 0, 0);
            _dgvFacturas.TabIndex = 5;

            EstiloGrid.AgregarColumna(_dgvFacturas, "colNumero", "Número", 12, negrita: true, propiedad: nameof(Factura.Numero));
            EstiloGrid.AgregarColumna(_dgvFacturas, "colCliente", "Cliente", 24, propiedad: nameof(Factura.ClienteDescripcion));
            EstiloGrid.AgregarColumna(_dgvFacturas, "colEmision", "Emisión", 10, formato: Formato.Fecha, propiedad: nameof(Factura.FechaEmision));
            EstiloGrid.AgregarColumna(_dgvFacturas, "colVenc", "Vencimiento", 10, formato: Formato.Fecha, propiedad: nameof(Factura.FechaVencimiento));
            EstiloGrid.AgregarColumna(_dgvFacturas, "colTotal", "Total (B/.)", 10, derecha: true, formato: Formato.Monto, propiedad: nameof(Factura.Total));
            EstiloGrid.AgregarColumna(_dgvFacturas, "colPagado", "Pagado (B/.)", 10, derecha: true, formato: Formato.Monto, propiedad: nameof(Factura.TotalPagado));
            EstiloGrid.AgregarColumna(_dgvFacturas, "colSaldo", "Saldo (B/.)", 10, derecha: true, formato: Formato.Monto, negrita: true, propiedad: nameof(Factura.Saldo));
            EstiloGrid.AgregarColumna(_dgvFacturas, "colEstado", "Estado", 9, propiedad: nameof(Factura.Estado));
            EstiloGrid.AgregarColumna(_dgvFacturas, "colDias", "Días venc.", 7, derecha: true, propiedad: nameof(Factura.DiasVencidos));
            EstiloGrid.AgregarColumna(_dgvFacturas, "colVencida", "Vencida", 9, propiedad: nameof(Factura.TextoVencida));

            _dgvFacturas.CellFormatting += DgvFacturas_CellFormatting;
            _dgvFacturas.Enter += (_, _) => RejillaEnfocada?.Invoke(this, EventArgs.Empty);
            _dgvFacturas.SelectionChanged += DgvFacturas_CambioSeleccion;
            _dgvFacturas.CurrentCellChanged += DgvFacturas_CambioSeleccion;

            _tarjetaLista.Cuerpo.Controls.Add(_dgvFacturas, 0, 0);
            _tarjetaLista.Cuerpo.Controls.Add(_lblResumenCartera, 0, 1);
            return _tarjetaLista;
        }

        private Control CrearDetalleInferior()
        {
            var panel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                BackColor = Color.Transparent,
                Margin = new Padding(0)
            };
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 66));
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 34));
            panel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            _tarjetaPagos.AutoSize = false;
            _tarjetaPagos.Margin = new Padding(0, 0, 10, 0);
            _tarjetaPagos.Cuerpo.AutoSize = false;
            _tarjetaPagos.Cuerpo.RowCount = 1;
            _tarjetaPagos.Cuerpo.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            EstiloGrid.Aplicar(_dgvPagos);
            _dgvPagos.Margin = new Padding(0, 6, 0, 0);
            _dgvPagos.TabStop = false;
            EstiloGrid.AgregarColumna(_dgvPagos, "colPagoId", "ID Pago", 14, negrita: true, propiedad: nameof(PagoFactura.Id));
            EstiloGrid.AgregarColumna(_dgvPagos, "colPagoFecha", "Fecha pago", 20, formato: Formato.Fecha, propiedad: nameof(PagoFactura.FechaPago));
            EstiloGrid.AgregarColumna(_dgvPagos, "colPagoMonto", "Monto (B/.)", 22, derecha: true, formato: Formato.Monto, negrita: true, propiedad: nameof(PagoFactura.Monto));
            EstiloGrid.AgregarColumna(_dgvPagos, "colPagoEstado", "Estado", 18, propiedad: nameof(PagoFactura.EstadoTexto));
            EstiloGrid.AgregarColumna(_dgvPagos, "colPagoReg", "Registrado", 26, formato: Formato.FechaHora, propiedad: nameof(PagoFactura.FechaCreacion));
            _tarjetaPagos.Cuerpo.Controls.Add(_dgvPagos, 0, 0);

            _tarjetaBalance.AutoSize = false;
            _tarjetaBalance.Cuerpo.AutoSize = false;
            _tarjetaBalance.Cuerpo.RowCount = 1;
            _tarjetaBalance.Cuerpo.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            var tabla = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 4,
                Margin = new Padding(0, 6, 0, 0)
            };
            tabla.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 52));
            tabla.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 48));
            for (var i = 0; i < 4; i++)
                tabla.RowStyles.Add(new RowStyle(SizeType.Percent, 25));

            ConfigurarValor(_lblBalTotal, Tema.Texto, false);
            ConfigurarValor(_lblBalPagado, Tema.PrimarioContenedor, false);
            ConfigurarValor(_lblBalSaldo, Tema.Secundario, true);
            ConfigurarValor(_lblBalEstado, Tema.Primario, true);

            tabla.Controls.Add(CrearEtiquetaBalance("Total factura:"), 0, 0);
            tabla.Controls.Add(_lblBalTotal, 1, 0);
            tabla.Controls.Add(CrearEtiquetaBalance("Total pagado:"), 0, 1);
            tabla.Controls.Add(_lblBalPagado, 1, 1);
            tabla.Controls.Add(CrearEtiquetaBalance("Saldo pendiente:"), 0, 2);
            tabla.Controls.Add(_lblBalSaldo, 1, 2);
            tabla.Controls.Add(CrearEtiquetaBalance("Condición:"), 0, 3);
            tabla.Controls.Add(_lblBalEstado, 1, 3);

            _tarjetaBalance.Cuerpo.Controls.Add(tabla, 0, 0);

            panel.Controls.Add(_tarjetaPagos, 0, 0);
            panel.Controls.Add(_tarjetaBalance, 1, 0);
            return panel;
        }

        private void DgvFacturas_CambioSeleccion(object? sender, EventArgs e)
        {
            if (_cargando || _dgvFacturas.CurrentRow?.DataBoundItem is not Factura factura)
                return;

            MostrarDetalleFactura(factura);
            FacturaSeleccionada?.Invoke(factura);
        }

        private void DgvFacturas_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || _dgvFacturas.Rows[e.RowIndex].DataBoundItem is not Factura f)
                return;

            if (f.Anulada)
                e.CellStyle!.ForeColor = Tema.TextoApagado;
            else if (f.EstaVencida)
                e.CellStyle!.ForeColor = Tema.Secundario;
        }

        private void ActualizarResumenCartera()
        {
            var activas = _facturas.Where(f => !f.Anulada).ToList();
            var facturado = activas.Sum(f => f.Total);
            var cobrado = activas.Sum(f => f.TotalPagado);
            var saldo = activas.Sum(f => f.Saldo);

            _lblResumenCartera.Text =
                $"Facturado activo: {facturado.Dinero()}   ·   " +
                $"Cobrado: {cobrado.Dinero()}   ·   " +
                $"Saldo pendiente: {saldo.Dinero()}";
        }

        private void MostrarDetalleFactura(Factura? f)
        {
            if (f is null)
            {
                _dgvPagos.DataSource = null;
                _tarjetaPagos.MostrarNota(string.Empty);
                _tarjetaBalance.MostrarNota(string.Empty);
                _lblBalTotal.Text = 0m.Dinero();
                _lblBalPagado.Text = 0m.Dinero();
                _lblBalSaldo.Text = 0m.Dinero();
                _lblBalEstado.Text = "—";
                return;
            }

            _tarjetaBalance.MostrarNota(f.Numero);
            _lblBalTotal.Text = $"{f.Total.Dinero()}";
            _lblBalPagado.Text = $"{f.TotalPagado.Dinero()}";
            _lblBalSaldo.Text = $"{f.Saldo.Dinero()}";
            _lblBalEstado.ForeColor = f.Anulada || f.EstaVencida ? Tema.Secundario : Tema.Primario;
            _lblBalEstado.Text = f.EstaVencida ? $"{f.Estado} ({f.DiasVencidos} d)" : f.Estado;

            try
            {
                var pagos = _negocio.ListarPagosDeFactura(f.Id);
                _dgvPagos.DataSource = pagos;
                _dgvPagos.ClearSelection();
                _tarjetaPagos.MostrarNota($"{pagos.Count} recibo(s)");
            }
            catch (DatosException ex)
            {
                _tarjetaPagos.MostrarNota(ex.Message, esError: true);
            }
        }

        private static Label CrearEtiquetaBalance(string texto) => new()
        {
            Text = texto,
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleLeft,
            ForeColor = Tema.TextoSecundario,
            Font = Tema.Normal(9F)
        };

        private static void ConfigurarValor(Label lbl, Color color, bool negrita)
        {
            lbl.Text = 0m.Dinero();
            lbl.Dock = DockStyle.Fill;
            lbl.TextAlign = ContentAlignment.MiddleRight;
            lbl.ForeColor = color;
            lbl.Font = negrita ? Tema.Negrita(9.5F) : Tema.Numeros(9F);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _timer.Dispose();
            base.Dispose(disposing);
        }
    }
}
```

<a id="kobrauifacturasctlfacturafichacs"></a>

#### Kobra.UI/Facturas/CtlFacturaFicha.cs

```csharp
using Kobra.Datos;
using Kobra.Entidades;
using Kobra.Negocio;

namespace Kobra.UI
{
    /// <summary>Ficha de emisión de factura a crédito: lectura, escritura y validación al salir de cada campo</summary>
    internal sealed class CtlFacturaFicha : UserControl
    {
        private readonly FacturaNegocio _facturaNegocio = new();
        private readonly ClienteNegocio _clienteNegocio = new();
        private readonly TarjetaPanel _tarjeta = new("EMISIÓN DE FACTURA A CRÉDITO");
        private readonly ErrorProvider _errores = new() { BlinkStyle = ErrorBlinkStyle.NeverBlink };

        private readonly TextBox _txtNumero = new() { MaxLength = 30, CharacterCasing = CharacterCasing.Upper };
        private readonly ComboBox _cboCliente = new() { DropDownStyle = ComboBoxStyle.DropDownList };
        private readonly DateTimePicker _dtpEmision = new() { Format = DateTimePickerFormat.Custom, CustomFormat = Formato.Fecha };
        private readonly DateTimePicker _dtpVencimiento = new() { Format = DateTimePickerFormat.Custom, CustomFormat = Formato.Fecha };
        private readonly Label _lblPlazo = new() { AutoSize = true, ForeColor = Tema.TextoSecundario };
        private readonly Label _lblCredito = new() { AutoSize = true, ForeColor = Tema.Primario };

        private readonly NumericUpDown _nudSubtotal = CrearNumerico();
        private readonly NumericUpDown _nudItbms = CrearNumerico();
        private readonly CheckBox _chkAutoItbms = new()
        {
            Text = "Calcular 7% automático",
            Checked = true,
            AutoSize = true,
            ForeColor = Tema.TextoSecundario,
            Font = Tema.Normal(8.5F),
            Margin = new Padding(8, 5, 0, 0)
        };
        private readonly TextBox _txtTotal = new()
        {
            ReadOnly = true,
            TabStop = false,
            BorderStyle = BorderStyle.FixedSingle,
            TextAlign = HorizontalAlignment.Right,
            BackColor = Tema.Contenedor,
            ForeColor = Tema.PrimarioContenedor,
            Font = Tema.Negrita(10.5F),
            Text = "0.00"
        };

        private bool _actualizando;

        public CtlFacturaFicha()
        {
            SuspendLayout();
            AutoScaleMode = AutoScaleMode.None;
            Dock = DockStyle.Fill;
            Margin = new Padding(0);
            AutoSize = true;

            _errores.ContainerControl = this;

            _txtNumero.Validating += (_, _) => _errores.SetError(_txtNumero,
                string.IsNullOrWhiteSpace(_txtNumero.Text) ? "El número de factura es obligatorio." : string.Empty);

            _cboCliente.Validating += (_, _) => _errores.SetError(_cboCliente,
                _cboCliente.SelectedItem is not ItemCliente ? "Seleccione un cliente activo." : string.Empty);

            _dtpVencimiento.Validating += (_, _) => _errores.SetError(_dtpVencimiento,
                _dtpVencimiento.Value.Date < _dtpEmision.Value.Date
                    ? "La fecha de vencimiento no puede ser anterior a la emisión."
                    : string.Empty);

            _nudSubtotal.Validating += (_, _) => _errores.SetError(_nudSubtotal,
                (_nudSubtotal.Value + _nudItbms.Value) <= 0
                    ? "El total de la factura debe ser mayor que 0."
                    : string.Empty);

            var orden = new Control[] { _txtNumero, _cboCliente, _dtpEmision, _dtpVencimiento, _nudSubtotal, _chkAutoItbms, _nudItbms };
            for (var i = 0; i < orden.Length; i++)
                orden[i].TabIndex = i;

            _lblPlazo.Font = Tema.Normal(8.5F);
            _lblPlazo.Margin = new Padding(0, 4, 0, 0);
            _lblCredito.Font = Tema.Numeros(8.5F);
            _lblCredito.Margin = new Padding(0, 4, 0, 0);

            var panelItbms = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                ColumnCount = 2,
                RowCount = 1,
                Margin = new Padding(0)
            };
            panelItbms.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 52));
            panelItbms.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 48));
            _nudItbms.Dock = DockStyle.Fill;
            _nudItbms.Margin = new Padding(0);
            panelItbms.Controls.Add(_nudItbms, 0, 0);
            panelItbms.Controls.Add(_chkAutoItbms, 1, 0);

            var rejilla = CampoFicha.CrearRejilla(5);
            CampoFicha.Agregar(rejilla, "Número de factura", _txtNumero, 0, 0, obligatorio: true);
            CampoFicha.Agregar(rejilla, "Cliente destino", _cboCliente, 0, 1, obligatorio: true);
            CampoFicha.Agregar(rejilla, "Fecha de emisión", _dtpEmision, 1, 0, obligatorio: true);
            CampoFicha.Agregar(rejilla, "Fecha vencimiento", _dtpVencimiento, 1, 1, obligatorio: true);
            CampoFicha.Agregar(rejilla, "Condición / Plazo", _lblPlazo, 2, 0);
            CampoFicha.Agregar(rejilla, "Crédito disponible", _lblCredito, 2, 1);
            CampoFicha.Agregar(rejilla, "Subtotal neto (B/.)", _nudSubtotal, 3, 0, obligatorio: true);
            CampoFicha.Agregar(rejilla, "ITBMS 7% (B/.)", panelItbms, 3, 1);
            CampoFicha.Agregar(rejilla, "TOTAL FACTURA (B/.)", _txtTotal, 4, 1);

            _tarjeta.MostrarNota("* obligatorio");
            _tarjeta.Cuerpo.Controls.Add(rejilla, 0, 0);
            _tarjeta.Margin = new Padding(0);
            Controls.Add(_tarjeta);

            ConectarEventos();
            ResumeLayout(true);
        }

        public event EventHandler? FichaEnfocada;

        public void EnfocarPrimero() => _txtNumero.Focus();

        public void CargarClientesActivos()
        {
            try
            {
                ItemCliente.Llenar(_cboCliente, _clienteNegocio.Buscar(null, soloActivos: true));
            }
            catch (DatosException ex)
            {
                _tarjeta.MostrarNota(ex.Message, esError: true);
            }
        }

        public Factura Leer() => new()
        {
            Numero = _txtNumero.Text,
            ClienteId = (_cboCliente.SelectedItem as ItemCliente)?.Id ?? 0,
            FechaEmision = _dtpEmision.Value.Date,
            FechaVencimiento = _dtpVencimiento.Value.Date,
            Subtotal = _nudSubtotal.Value,
            Itbms = _nudItbms.Value,
            Total = _nudSubtotal.Value + _nudItbms.Value
        };

        public void Limpiar()
        {
            LimpiarErrores();
            _actualizando = true;
            try
            {
                _txtNumero.Clear();
                _dtpEmision.Value = DateTime.Today;
                _chkAutoItbms.Checked = true;
                _nudItbms.Enabled = false;
                _nudSubtotal.Value = 0;
                _nudItbms.Value = 0;
                _txtTotal.Text = 0m.Importe();

                if (_cboCliente.Items.Count > 0 && _cboCliente.SelectedIndex < 0)
                    _cboCliente.SelectedIndex = 0;
            }
            finally
            {
                _actualizando = false;
            }

            ActualizarClienteSeleccionado();
        }

        public void LimpiarErrores() => _errores.Clear();

        public void MarcarCampo(string? campo, string mensaje)
        {
            Control? control = campo switch
            {
                nameof(Factura.Numero) => _txtNumero,
                nameof(Factura.ClienteId) => _cboCliente,
                nameof(Factura.FechaEmision) => _dtpEmision,
                nameof(Factura.FechaVencimiento) => _dtpVencimiento,
                nameof(Factura.Subtotal) => _nudSubtotal,
                nameof(Factura.Itbms) => _nudItbms,
                _ => null
            };

            if (control is null)
                return;

            _errores.SetError(control, mensaje);
            control.Focus();
        }

        private void ConectarEventos()
        {
            _tarjeta.Enter += (_, _) => FichaEnfocada?.Invoke(this, EventArgs.Empty);

            _cboCliente.SelectedIndexChanged += (_, _) =>
            {
                if (!_actualizando)
                    ActualizarClienteSeleccionado();
            };

            _dtpEmision.ValueChanged += (_, _) =>
            {
                if (!_actualizando)
                    RecalcularVencimiento();
            };

            _nudSubtotal.ValueChanged += (_, _) => RecalcularTotales();
            _nudItbms.ValueChanged += (_, _) => RecalcularTotales();
            _chkAutoItbms.CheckedChanged += (_, _) =>
            {
                _nudItbms.Enabled = !_chkAutoItbms.Checked;
                RecalcularTotales();
            };
        }

        private void ActualizarClienteSeleccionado()
        {
            if ((_cboCliente.SelectedItem as ItemCliente)?.Cliente is not { } cliente)
            {
                _lblPlazo.Text = "—";
                _lblCredito.Text = "Sin cliente activo";
                return;
            }

            RecalcularVencimiento();
            _lblPlazo.Text = $"Crédito a {cliente.PlazoDias} día(s)";

            try
            {
                var saldo = _facturaNegocio.ObtenerSaldoCliente(cliente.Id);
                var disponible = cliente.LimiteCredito - saldo;
                _lblCredito.ForeColor = disponible <= 0 ? Tema.Secundario : Tema.Primario;
                _lblCredito.Text =
                    $"Límite: {cliente.LimiteCredito.Dinero()} · " +
                    $"Saldo: {saldo.Dinero()} · " +
                    $"Disp.: {disponible.Dinero()}";
            }
            catch (DatosException)
            {
                _lblCredito.Text = $"Límite: {cliente.LimiteCredito.Dinero()}";
            }
        }

        private void RecalcularVencimiento()
        {
            if ((_cboCliente.SelectedItem as ItemCliente)?.Cliente is not { } cliente)
                return;

            _actualizando = true;
            try
            {
                _dtpVencimiento.Value = _dtpEmision.Value.Date.AddDays(cliente.PlazoDias);
            }
            finally
            {
                _actualizando = false;
            }
        }

        private void RecalcularTotales()
        {
            if (_actualizando)
                return;

            _actualizando = true;
            try
            {
                if (_chkAutoItbms.Checked)
                    _nudItbms.Value = Math.Round(_nudSubtotal.Value * 0.07m, 2, MidpointRounding.AwayFromZero);

                var total = _nudSubtotal.Value + _nudItbms.Value;
                _txtTotal.Text = total.Importe();
            }
            finally
            {
                _actualizando = false;
            }
        }

        private static NumericUpDown CrearNumerico() => new()
        {
            DecimalPlaces = 2,
            ThousandsSeparator = true,
            TextAlign = HorizontalAlignment.Right,
            Font = Tema.Numeros(9.5F),
            Maximum = Formato.MaximoMonto
        };

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _errores.Dispose();
            base.Dispose(disposing);
        }
    }
}
```

<a id="kobrauifacturasfrmfacturascs"></a>

#### Kobra.UI/Facturas/FrmFacturas.cs

```csharp
using Kobra.Datos;
using Kobra.Entidades;
using Kobra.Negocio;

namespace Kobra.UI
{
    /// <summary>Módulo de Facturas: orquesta Consulta y Saldos, Nueva Factura y Anulación</summary>
    public partial class FrmFacturas : FormularioConVistas
    {
        private enum Vista { Consulta, Registro, Anulacion }

        private readonly FacturaNegocio _negocio = new();

        private readonly BarraTituloModulo _barraTitulo = new("FACTURAS");
        private readonly BarraAcciones _barraAcciones = new(Mensajes.PistaAtajos("nueva"));

        private readonly BotonTema _btnTabConsulta = new("Consulta y Saldos");
        private readonly BotonTema _btnTabRegistro = new("Nueva Factura");
        private readonly BotonTema _btnTabAnulacion = new("Anular Factura");

        private readonly BotonTema _btnNueva = new("&Nueva");
        private readonly BotonTema _btnGuardar = new("&Guardar", EstiloBoton.Primario);
        private readonly BotonTema _btnIrAnular = new("&Anular", EstiloBoton.Peligro) { Enabled = false };
        private readonly BotonTema _btnConfirmarAnular = new("&Anular factura", EstiloBoton.Peligro) { Enabled = false };
        private readonly BotonTema _btnLimpiar = new("&Limpiar");
        private readonly BotonTema _btnCerrar = new("&Cerrar");

        private readonly CtlFacturaConsulta _consulta = new();
        private readonly CtlFacturaFicha _ficha = new();
        private readonly CtlFacturaAnulacion _anulacion = new();

        private Vista _vistaActual = Vista.Consulta;
        private Factura? _seleccionada;

        public FrmFacturas()
        {
            InitializeComponent();
            ConstruirInterfaz();
            ConectarEventos();
        }

        private void ConstruirInterfaz()
        {
            ConfigurarVentana("Facturas");

            _barraAcciones.Agregar(_btnNueva, _btnGuardar, _btnIrAnular, _btnConfirmarAnular, _btnLimpiar, _btnCerrar);

            ArmarMarco(_barraTitulo, _barraAcciones, _btnCerrar,
                [_btnTabConsulta, _btnTabRegistro, _btnTabAnulacion], _consulta, _ficha, _anulacion);
        }

        private void ConectarEventos()
        {
            Load += FrmFacturas_Load;

            _btnTabConsulta.Click += (_, _) => CambiarVista(Vista.Consulta);
            _btnTabRegistro.Click += (_, _) => CambiarVista(Vista.Registro);
            _btnTabAnulacion.Click += (_, _) =>
            {
                CambiarVista(Vista.Anulacion);
                if (_seleccionada is not null)
                    _anulacion.Cargar(_seleccionada);
            };

            _btnNueva.Click += (_, _) =>
            {
                CambiarVista(Vista.Registro);
                _ficha.Limpiar();
                _ficha.EnfocarPrimero();
            };
            _btnGuardar.Click += (_, _) => Guardar();
            _btnIrAnular.Click += (_, _) =>
            {
                CambiarVista(Vista.Anulacion);
                if (_seleccionada is not null)
                    _anulacion.Cargar(_seleccionada);
            };
            _btnConfirmarAnular.Click += (_, _) => Anular();
            _btnLimpiar.Click += (_, _) => LimpiarVistaActual();
            _btnCerrar.Click += (_, _) => Close();

            _consulta.BusquedaSolicitada += explicita => CargarConsulta(mostrarMensaje: explicita);
            _consulta.FacturaSeleccionada += f =>
            {
                _seleccionada = f;
                _btnIrAnular.Enabled = f is { Anulada: false, TotalPagado: 0 };
            };
            _consulta.BusquedaEnfocada += (_, _) => AcceptButton = _consulta.BotonBuscar;
            _consulta.RejillaEnfocada += (_, _) => AcceptButton = null;

            _ficha.FichaEnfocada += (_, _) => AcceptButton = _btnGuardar;

            _anulacion.FacturaCargada += f =>
                _btnConfirmarAnular.Enabled = f is { Anulada: false, TotalPagado: 0 };
            _anulacion.BusquedaEnfocada += (_, _) => AcceptButton = _anulacion.BotonBuscar;
            _anulacion.MotivoEnfocado += (_, _) => AcceptButton = null;
        }

        private void FrmFacturas_Load(object? sender, EventArgs e)
        {
            _consulta.CargarClientesFiltro();
            _ficha.CargarClientesActivos();
            _ficha.Limpiar();
            CambiarVista(Vista.Consulta);
            CargarConsulta(mostrarMensaje: true);
            _consulta.EnfocarBusqueda();
        }

        protected override bool ProcesarAtajo(Keys tecla)
        {
            switch (tecla)
            {
                case Keys.F2:
                    if (_vistaActual == Vista.Consulta)
                        _consulta.EnfocarBusqueda();
                    else if (_vistaActual == Vista.Anulacion)
                        _anulacion.EnfocarBusqueda();
                    else
                        return false;
                    return true;
                case Keys.F5:
                    _consulta.CargarClientesFiltro();
                    _ficha.CargarClientesActivos();
                    CargarConsulta(mostrarMensaje: true);
                    return true;
                case Keys.Control | Keys.N:
                    CambiarVista(Vista.Registro);
                    _ficha.Limpiar();
                    _ficha.EnfocarPrimero();
                    return true;
                case Keys.Control | Keys.S:
                    if (_vistaActual != Vista.Registro)
                        return false;
                    Guardar();
                    return true;
                default:
                    return false;
            }
        }

        private void CambiarVista(Vista vista)
        {
            _vistaActual = vista;

            _consulta.Visible = vista == Vista.Consulta;
            _ficha.Visible = vista == Vista.Registro;
            _anulacion.Visible = vista == Vista.Anulacion;

            MarcarPestana((int)vista);

            _btnNueva.Visible = vista == Vista.Consulta;
            _btnIrAnular.Visible = vista == Vista.Consulta;
            _btnGuardar.Visible = vista == Vista.Registro;
            _btnConfirmarAnular.Visible = vista == Vista.Anulacion;

            AcceptButton = vista switch
            {
                Vista.Consulta => _consulta.BotonBuscar,
                Vista.Registro => _btnGuardar,
                Vista.Anulacion => _anulacion.BotonBuscar,
                _ => null
            };

            _barraTitulo.Info = vista switch
            {
                Vista.Consulta => "CONSULTA Y SALDOS",
                Vista.Registro => "MODO: NUEVA FACTURA",
                Vista.Anulacion => "ANULACIÓN DE FACTURA",
                _ => string.Empty
            };

            _barraAcciones.RestablecerPista();

            if (vista == Vista.Registro)
                _ficha.EnfocarPrimero();
            else if (vista == Vista.Anulacion)
                _anulacion.EnfocarBusqueda();
        }

        private void CargarConsulta(bool mostrarMensaje, int? seleccionarId = null)
        {
            var mantener = seleccionarId ?? _seleccionada?.Id;
            var error = _consulta.Cargar(mantener);
            if (error is not null && mostrarMensaje)
                MostrarAdvertencia(error);
        }

        private void Guardar()
        {
            _ficha.LimpiarErrores();
            var factura = _ficha.Leer();

            try
            {
                var nuevoId = _negocio.Registrar(factura);
                _ficha.Limpiar();
                CambiarVista(Vista.Consulta);
                CargarConsulta(mostrarMensaje: false, seleccionarId: nuevoId);
                _barraAcciones.MostrarMensaje($"Factura {factura.Numero.Trim()} guardada.", esError: false);
            }
            catch (ReglaNegocioException ex)
            {
                _ficha.MarcarCampo(ex.Campo, ex.Message);
                MostrarError(ex);
            }
            catch (DatosException ex)
            {
                MostrarError(ex);
            }
        }

        private void Anular()
        {
            var factura = _anulacion.FacturaActual;
            if (factura is null)
                return;

            var motivo = _anulacion.Motivo;
            if (motivo.Length == 0)
            {
                _anulacion.MarcarErrorMotivo(Mensajes.MotivoObligatorio);
                MostrarAdvertencia(Mensajes.MotivoFaltante);
                return;
            }

            var confirmado = Confirmar(
                $"¿Anular definitivamente la factura {factura.Numero}?",
                $"Cliente: {factura.ClienteDescripcion}  ·  Total: {factura.Total.Dinero()}",
                "Esta acción es irreversible y dejará el saldo de la factura en {0m.Dinero()}.",
                peligro: true,
                textoSi: "&Anular");

            if (!confirmado)
                return;

            try
            {
                _negocio.Anular(factura.Id, motivo);
                _anulacion.Limpiar();
                CambiarVista(Vista.Consulta);
                CargarConsulta(mostrarMensaje: false, seleccionarId: factura.Id);
                _barraAcciones.MostrarMensaje($"Factura {factura.Numero} anulada.", esError: false);
            }
            catch (ReglaNegocioException ex)
            {
                if (ex.Campo == nameof(Factura.MotivoAnulacion))
                    _anulacion.MarcarErrorMotivo(ex.Message);
                MostrarError(ex);
            }
            catch (DatosException ex)
            {
                MostrarError(ex);
            }
        }

        private void LimpiarVistaActual()
        {
            switch (_vistaActual)
            {
                case Vista.Consulta:
                    _consulta.LimpiarFiltros();
                    CargarConsulta(mostrarMensaje: false);
                    break;
                case Vista.Registro:
                    _ficha.Limpiar();
                    _ficha.EnfocarPrimero();
                    break;
                case Vista.Anulacion:
                    _anulacion.Limpiar();
                    _anulacion.EnfocarBusqueda();
                    break;
            }
            _barraAcciones.RestablecerPista();
        }
    }
}
```

<a id="kobrauifacturasfrmfacturasdesignercs"></a>

#### Kobra.UI/Facturas/FrmFacturas.Designer.cs

```csharp
namespace Kobra.UI
{
    partial class FrmFacturas
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
            Name = "FrmFacturas";
            Text = "Kobra · Facturas";
        }

        #endregion
    }
}
```

<a id="modulo-de-pagos"></a>

### Módulo de Pagos

<a id="kobrauipagosctlpagoanulacioncs"></a>

#### Kobra.UI/Pagos/CtlPagoAnulacion.cs

```csharp
using Kobra.Datos;
using Kobra.Entidades;
using Kobra.Negocio;

namespace Kobra.UI
{
    /// <summary>Ficha de anulación lógica de pago: busca el recibo, muestra su impacto en el saldo de la factura y exige un motivo de hasta 250 caracteres</summary>
    internal sealed class CtlPagoAnulacion : UserControl
    {
        private readonly PagoNegocio _negocio = new();
        private readonly ErrorProvider _errores = new() { BlinkStyle = ErrorBlinkStyle.NeverBlink };

        private readonly TarjetaPanel _tarjetaDatos = new("DATOS DEL PAGO A ANULAR");
        private readonly TextBox _txtBuscar = new() { MaxLength = 150 };
        private readonly BotonTema _btnBuscar = new("&Buscar", EstiloBoton.Primario);

        private readonly TextBox _txtNumero = CampoFicha.CrearSoloLectura();
        private readonly TextBox _txtFactura = CampoFicha.CrearSoloLectura();
        private readonly TextBox _txtCliente = CampoFicha.CrearSoloLectura();
        private readonly TextBox _txtFecha = CampoFicha.CrearSoloLectura();
        private readonly TextBox _txtMonto = CampoFicha.CrearSoloLectura(derecha: true);
        private readonly TextBox _txtSaldoActual = CampoFicha.CrearSoloLectura(derecha: true);
        private readonly TextBox _txtSaldoFinal = CampoFicha.CrearSoloLectura(derecha: true);
        private readonly Label _lblEstado = new() { AutoSize = true, Font = Tema.Negrita(9F) };

        private readonly TarjetaMotivo _motivo = new(tabIndex: 2);

        public CtlPagoAnulacion()
        {
            SuspendLayout();
            AutoScaleMode = AutoScaleMode.None;
            Dock = DockStyle.Fill;
            Margin = new Padding(0);

            _errores.ContainerControl = this;
            _motivo.Enfocado += (_, _) => MotivoEnfocado?.Invoke(this, EventArgs.Empty);

            _motivo.Caja.Validating += (_, _) =>
            {
                if (PagoActual is { Anulado: false })
                    _errores.SetError(_motivo.Caja,
                        string.IsNullOrWhiteSpace(_motivo.Caja.Text) ? Mensajes.MotivoObligatorio : string.Empty);
            };

            var raiz = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                BackColor = Color.Transparent,
                Margin = new Padding(0)
            };
            raiz.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            raiz.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            raiz.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            raiz.Controls.Add(CrearTarjetaDatos(), 0, 0);
            raiz.Controls.Add(_motivo.Tarjeta, 0, 1);
            Controls.Add(raiz);

            ResumeLayout(true);
        }

        public event Action<Pago?>? PagoCargado;
        public event EventHandler? BusquedaEnfocada;
        public event EventHandler? MotivoEnfocado;

        public Button BotonBuscar => _btnBuscar;
        public Pago? PagoActual { get; private set; }
        public string Motivo => _motivo.Caja.Text.Trim();

        public void EnfocarBusqueda()
        {
            _txtBuscar.Focus();
            _txtBuscar.SelectAll();
        }

        public void Cargar(Pago pago)
        {
            PagoActual = pago;
            _errores.Clear();

            _txtBuscar.Text = pago.Id.ToString(Formato.Cultura);
            _txtNumero.Text = pago.Id.ToString(Formato.Cultura);
            _txtFactura.Text = pago.NumeroFactura;
            _txtCliente.Text = pago.ClienteDescripcion;
            _txtFecha.Text = pago.FechaPago.ToString(Formato.Fecha);
            _txtMonto.Text = pago.Monto.Importe();
            _txtSaldoActual.Text = pago.SaldoFactura.Importe();
            _txtSaldoFinal.Text = pago.Anulado
                ? pago.SaldoFactura.Importe()
                : (pago.SaldoFactura + pago.Monto).Importe();

            _lblEstado.ForeColor = pago.Anulado || pago.FacturaAnulada ? Tema.Secundario : Tema.Primario;
            _lblEstado.Text = pago.Anulado ? "Anulado" : pago.FacturaAnulada ? "Factura anulada" : "Activo";

            _motivo.Caja.Text = pago.Anulado ? (pago.MotivoAnulacion ?? string.Empty) : string.Empty;
            _motivo.Caja.ReadOnly = pago.Anulado;

            if (pago.Anulado)
                _tarjetaDatos.MostrarNota("Este pago ya está anulado", esError: true);
            else if (pago.FacturaAnulada)
                _tarjetaDatos.MostrarNota("La factura está anulada", esError: true);
            else
                _tarjetaDatos.MostrarNota("El saldo de la factura aumentará al anular", esError: false);

            PagoCargado?.Invoke(pago);

            if (!pago.Anulado)
                _motivo.Caja.Focus();
        }

        public void Limpiar()
        {
            PagoActual = null;
            _errores.Clear();
            _txtBuscar.Clear();
            _txtNumero.Clear();
            _txtFactura.Clear();
            _txtCliente.Clear();
            _txtFecha.Clear();
            _txtMonto.Clear();
            _txtSaldoActual.Clear();
            _txtSaldoFinal.Clear();
            _lblEstado.Text = "—";
            _motivo.Caja.Clear();
            _motivo.Caja.ReadOnly = false;
            _tarjetaDatos.MostrarNota(string.Empty);
            PagoCargado?.Invoke(null);
        }

        public void MarcarErrorMotivo(string mensaje)
        {
            _errores.SetError(_motivo.Caja, mensaje);
            _motivo.Caja.Focus();
        }

        private Control CrearTarjetaDatos()
        {
            _tarjetaDatos.Margin = new Padding(0, 0, 0, 10);

            var barra = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                Margin = new Padding(0, 6, 0, 6)
            };
            barra.Controls.Add(new Label
            {
                Text = "N.º de pago o factura:",
                ForeColor = Tema.TextoSecundario,
                AutoSize = true,
                Margin = new Padding(0, 6, 8, 0)
            });

            _txtBuscar.Width = LogicalToDeviceUnits(220);
            _txtBuscar.BorderStyle = BorderStyle.FixedSingle;
            _txtBuscar.PlaceholderText = "Ej. 12 o F-001";
            _txtBuscar.Margin = new Padding(0, 3, 8, 3);
            _txtBuscar.TabIndex = 0;
            _txtBuscar.Enter += (_, _) => BusquedaEnfocada?.Invoke(this, EventArgs.Empty);

            _btnBuscar.TabIndex = 1;
            _btnBuscar.Click += (_, _) => BuscarPago();
            barra.Controls.AddRange([_txtBuscar, _btnBuscar]);

            _txtSaldoFinal.ForeColor = Tema.Secundario;
            _txtSaldoFinal.Font = Tema.Negrita(9F);
            _lblEstado.Margin = new Padding(0, 4, 0, 0);

            var rejilla = CampoFicha.CrearRejilla(4);
            CampoFicha.Agregar(rejilla, "Número de pago", _txtNumero, 0, 0);
            CampoFicha.Agregar(rejilla, "Saldo actual (B/.)", _txtSaldoActual, 0, 1);
            CampoFicha.Agregar(rejilla, "Factura", _txtFactura, 1, 0);
            CampoFicha.Agregar(rejilla, "Saldo al anular (B/.)", _txtSaldoFinal, 1, 1);
            CampoFicha.Agregar(rejilla, "Cliente", _txtCliente, 2, 0);
            CampoFicha.Agregar(rejilla, "Estado del pago", _lblEstado, 2, 1);
            CampoFicha.Agregar(rejilla, "Fecha de pago", _txtFecha, 3, 0);
            CampoFicha.Agregar(rejilla, "Monto (B/.)", _txtMonto, 3, 1);

            _tarjetaDatos.Cuerpo.RowCount = 2;
            _tarjetaDatos.Cuerpo.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            _tarjetaDatos.Cuerpo.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            _tarjetaDatos.Cuerpo.Controls.Add(barra, 0, 0);
            _tarjetaDatos.Cuerpo.Controls.Add(rejilla, 0, 1);
            return _tarjetaDatos;
        }

        private void BuscarPago()
        {
            _errores.Clear();
            var texto = _txtBuscar.Text.Trim();
            if (texto.Length == 0)
            {
                _errores.SetError(_txtBuscar, "Ingrese el número de pago o de factura.");
                _txtBuscar.Focus();
                return;
            }

            try
            {
                Pago? pago = int.TryParse(texto, out var id)
                    ? _negocio.ObtenerPorId(id)
                    : _negocio.Buscar(texto).FirstOrDefault();

                if (pago is null)
                {
                    Limpiar();
                    _txtBuscar.Text = texto;
                    _tarjetaDatos.MostrarNota($"No existe el pago '{texto}'", esError: true);
                    _txtBuscar.Focus();
                    return;
                }

                Cargar(pago);
            }
            catch (DatosException ex)
            {
                _tarjetaDatos.MostrarNota(ex.Message, esError: true);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _errores.Dispose();
            base.Dispose(disposing);
        }
    }
}
```

<a id="kobrauipagosctlpagoconsultacs"></a>

#### Kobra.UI/Pagos/CtlPagoConsulta.cs

```csharp
using Kobra.Datos;
using Kobra.Entidades;
using Kobra.Negocio;

namespace Kobra.UI
{
    /// <summary>Consulta de pagos: filtros por texto, cliente y estado, rejilla de solo lectura y resumen de totales</summary>
    internal sealed class CtlPagoConsulta : UserControl
    {
        private readonly PagoNegocio _negocio = new();
        private readonly ClienteNegocio _clienteNegocio = new();
        private readonly System.Windows.Forms.Timer _timer = new() { Interval = 300 };

        private readonly TarjetaPanel _tarjetaFiltros = new("FILTROS DE BÚSQUEDA");
        private readonly TextBox _txtBuscar = new();
        private readonly ComboBox _cboCliente = new();
        private readonly ComboBox _cboEstado = new();
        private readonly BotonTema _btnBuscar = new("&Buscar", EstiloBoton.Primario);

        private readonly TarjetaPanel _tarjetaLista = new("PAGOS REGISTRADOS");
        private readonly DataGridView _dgvPagos = new();
        private readonly BarraTotales _lblResumen = new();

        private List<Pago> _pagos = [];
        private bool _cargando;

        public CtlPagoConsulta()
        {
            SuspendLayout();
            AutoScaleMode = AutoScaleMode.None;
            Dock = DockStyle.Fill;
            Margin = new Padding(0);

            var raiz = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                BackColor = Color.Transparent,
                Margin = new Padding(0)
            };
            raiz.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            raiz.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            raiz.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            raiz.Controls.Add(CrearFiltros(), 0, 0);
            raiz.Controls.Add(CrearLista(), 0, 1);
            Controls.Add(raiz);

            _timer.Tick += (_, _) =>
            {
                _timer.Stop();
                BusquedaSolicitada?.Invoke(false);
            };
            ResumeLayout(true);
        }

        public event Action<bool>? BusquedaSolicitada;
        public event Action<Pago?>? PagoSeleccionado;
        public event EventHandler? BusquedaEnfocada;
        public event EventHandler? RejillaEnfocada;

        public Button BotonBuscar => _btnBuscar;

        /// <summary>Número de factura del pago indicado, ya cargado en la rejilla</summary>
        public string ObtenerNumeroFactura(int pagoId) =>
            _pagos.FirstOrDefault(p => p.Id == pagoId)?.NumeroFactura ?? string.Empty;

        public void EnfocarBusqueda()
        {
            _txtBuscar.Focus();
            _txtBuscar.SelectAll();
        }

        public void CargarClientesFiltro()
        {
            _cargando = true;
            try
            {
                ItemCliente.Llenar(_cboCliente, _clienteNegocio.Buscar(null, soloActivos: false), "Todos los clientes");
            }
            catch (DatosException ex)
            {
                _tarjetaFiltros.MostrarNota(ex.Message, esError: true);
            }
            finally
            {
                _cargando = false;
            }
        }

        public void LimpiarFiltros()
        {
            _cargando = true;
            try
            {
                _txtBuscar.Clear();
                if (_cboCliente.Items.Count > 0)
                    _cboCliente.SelectedIndex = 0;
                _cboEstado.SelectedIndex = 0;
            }
            finally
            {
                _cargando = false;
            }
        }

        /// <summary>Recarga la rejilla</summary>
        public string? Cargar(int? seleccionarId)
        {
            _timer.Stop();
            string? error = null;

            var clienteId = (_cboCliente.SelectedItem as ItemCliente)?.Id;
            if (clienteId == 0)
                clienteId = null;

            bool? anulado = _cboEstado.SelectedItem?.ToString() switch
            {
                "Activos" => false,
                "Anulados" => true,
                _ => null
            };

            try
            {
                _pagos = _negocio.Buscar(_txtBuscar.Text, clienteId, anulado);
            }
            catch (DatosException ex)
            {
                _pagos = [];
                error = ex.Message;
            }

            _cargando = true;
            Pago? seleccionado = null;
            try
            {
                _dgvPagos.DataSource = _pagos;
                _dgvPagos.ClearSelection();
                _dgvPagos.CurrentCell = null;

                var fila = seleccionarId is int id ? _pagos.FindIndex(p => p.Id == id) : -1;
                if (fila < 0 && _pagos.Count > 0)
                    fila = 0;

                if (fila >= 0)
                {
                    _dgvPagos.Rows[fila].Selected = true;
                    _dgvPagos.CurrentCell = _dgvPagos.Rows[fila].Cells["colNumero"];
                    seleccionado = _pagos[fila];
                }
            }
            finally
            {
                _cargando = false;
            }

            ActualizarResumen();

            if (error is not null)
                _tarjetaLista.MostrarNota(error, esError: true);
            else
                _tarjetaLista.MostrarNota(_pagos.Count >= PagoNegocio.MaximoResultados
                    ? $"Primeros {PagoNegocio.MaximoResultados}: afine la búsqueda"
                    : $"{_pagos.Count} pago(s)");

            PagoSeleccionado?.Invoke(seleccionado);
            return error;
        }

        private Control CrearFiltros()
        {
            _tarjetaFiltros.Margin = new Padding(0, 0, 0, 10);

            var fila = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                ColumnCount = 7,
                RowCount = 1,
                BackColor = Color.Transparent,
                Margin = new Padding(0, 6, 0, 2)
            };
            fila.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            fila.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));
            fila.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            fila.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 36));
            fila.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            fila.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 24));
            fila.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            _txtBuscar.MaxLength = 150;
            _txtBuscar.PlaceholderText = "N.º pago, factura o cliente";
            _txtBuscar.BorderStyle = BorderStyle.FixedSingle;
            _txtBuscar.Dock = DockStyle.Fill;
            _txtBuscar.Margin = new Padding(0, 3, 10, 3);
            _txtBuscar.TabIndex = 0;
            _txtBuscar.TextChanged += (_, _) =>
            {
                _timer.Stop();
                _timer.Start();
            };
            _txtBuscar.Enter += (_, _) => BusquedaEnfocada?.Invoke(this, EventArgs.Empty);

            _cboCliente.DropDownStyle = ComboBoxStyle.DropDownList;
            _cboCliente.Dock = DockStyle.Fill;
            _cboCliente.Margin = new Padding(0, 3, 10, 3);
            _cboCliente.TabIndex = 1;
            _cboCliente.SelectedIndexChanged += (_, _) =>
            {
                if (!_cargando)
                    BusquedaSolicitada?.Invoke(false);
            };
            _cboCliente.Enter += (_, _) => BusquedaEnfocada?.Invoke(this, EventArgs.Empty);

            _cboEstado.DropDownStyle = ComboBoxStyle.DropDownList;
            _cboEstado.Dock = DockStyle.Fill;
            _cboEstado.Margin = new Padding(0, 3, 10, 3);
            _cboEstado.TabIndex = 2;
            _cboEstado.Items.AddRange(["Todos", "Activos", "Anulados"]);
            _cboEstado.SelectedIndex = 0;
            _cboEstado.SelectedIndexChanged += (_, _) =>
            {
                if (!_cargando)
                    BusquedaSolicitada?.Invoke(false);
            };
            _cboEstado.Enter += (_, _) => BusquedaEnfocada?.Invoke(this, EventArgs.Empty);

            _btnBuscar.TabIndex = 3;
            _btnBuscar.Margin = new Padding(0);
            _btnBuscar.Click += (_, _) =>
            {
                _timer.Stop();
                BusquedaSolicitada?.Invoke(true);
            };

            fila.Controls.Add(CampoFicha.CrearEtiquetaFiltro("Buscar (F2):"), 0, 0);
            fila.Controls.Add(_txtBuscar, 1, 0);
            fila.Controls.Add(CampoFicha.CrearEtiquetaFiltro("Cliente:"), 2, 0);
            fila.Controls.Add(_cboCliente, 3, 0);
            fila.Controls.Add(CampoFicha.CrearEtiquetaFiltro("Estado:"), 4, 0);
            fila.Controls.Add(_cboEstado, 5, 0);
            fila.Controls.Add(_btnBuscar, 6, 0);

            _tarjetaFiltros.Cuerpo.Controls.Add(fila, 0, 0);
            return _tarjetaFiltros;
        }

        private Control CrearLista()
        {
            _tarjetaLista.AutoSize = false;
            _tarjetaLista.Cuerpo.AutoSize = false;
            _tarjetaLista.Cuerpo.RowCount = 2;
            _tarjetaLista.Cuerpo.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            _tarjetaLista.Cuerpo.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            EstiloGrid.Aplicar(_dgvPagos);
            _dgvPagos.Margin = new Padding(0, 6, 0, 0);
            _dgvPagos.TabIndex = 4;

            EstiloGrid.AgregarColumna(_dgvPagos, "colNumero", "N.º pago", 10, negrita: true, propiedad: nameof(Pago.Id));
            EstiloGrid.AgregarColumna(_dgvPagos, "colFactura", "Factura", 14, propiedad: nameof(Pago.NumeroFactura));
            EstiloGrid.AgregarColumna(_dgvPagos, "colCliente", "Cliente", 26, propiedad: nameof(Pago.ClienteDescripcion));
            EstiloGrid.AgregarColumna(_dgvPagos, "colFecha", "Fecha pago", 13, formato: Formato.Fecha, propiedad: nameof(Pago.FechaPago));
            EstiloGrid.AgregarColumna(_dgvPagos, "colMonto", "Monto (B/.)", 13, derecha: true, formato: Formato.Monto, negrita: true, propiedad: nameof(Pago.Monto));
            EstiloGrid.AgregarColumna(_dgvPagos, "colEstado", "Estado", 11, propiedad: nameof(Pago.EstadoTexto));
            EstiloGrid.AgregarColumna(_dgvPagos, "colSaldo", "Saldo factura (B/.)", 13, derecha: true, formato: Formato.Monto, propiedad: nameof(Pago.SaldoFactura));
            EstiloGrid.AgregarColumna(_dgvPagos, "colMotivo", "Motivo anulación", 20, propiedad: nameof(Pago.MotivoAnulacion));

            _dgvPagos.CellFormatting += (_, e) =>
            {
                if (e.RowIndex < 0 || _dgvPagos.Rows[e.RowIndex].DataBoundItem is not Pago pago)
                    return;
                if (pago.Anulado || pago.FacturaAnulada)
                    e.CellStyle!.ForeColor = Tema.TextoApagado;
            };
            _dgvPagos.Enter += (_, _) => RejillaEnfocada?.Invoke(this, EventArgs.Empty);
            _dgvPagos.SelectionChanged += DgvPagos_CambioSeleccion;
            _dgvPagos.CurrentCellChanged += DgvPagos_CambioSeleccion;

            _tarjetaLista.Cuerpo.Controls.Add(_dgvPagos, 0, 0);
            _tarjetaLista.Cuerpo.Controls.Add(_lblResumen, 0, 1);
            return _tarjetaLista;
        }

        private void DgvPagos_CambioSeleccion(object? sender, EventArgs e)
        {
            if (_cargando || _dgvPagos.CurrentRow?.DataBoundItem is not Pago pago)
                return;
            PagoSeleccionado?.Invoke(pago);
        }

        private void ActualizarResumen()
        {
            var activos = _pagos.Where(p => !p.Anulado).ToList();
            var anulados = _pagos.Where(p => p.Anulado).ToList();

            _lblResumen.Text =
                $"Cobrado activo: {activos.Sum(p => p.Monto).Dinero()}   ·   " +
                $"Anulado: {anulados.Sum(p => p.Monto).Dinero()}   ·   " +
                $"{activos.Count} activo(s) / {anulados.Count} anulado(s)";
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _timer.Dispose();
            base.Dispose(disposing);
        }
    }
}
```

<a id="kobrauipagosctlpagoregistrocs"></a>

#### Kobra.UI/Pagos/CtlPagoRegistro.cs

```csharp
using Kobra.Datos;
using Kobra.Entidades;
using Kobra.Negocio;

namespace Kobra.UI
{
    /// <summary>Registro de pago: elige un cliente, muestra sus facturas con saldo y captura fecha y monto</summary>
    internal sealed class CtlPagoRegistro : UserControl
    {
        private readonly PagoNegocio _negocio = new();
        private readonly ClienteNegocio _clienteNegocio = new();
        private readonly ErrorProvider _errores = new() { BlinkStyle = ErrorBlinkStyle.NeverBlink };

        private readonly TarjetaPanel _tarjetaFacturas = new("FACTURAS CON SALDO DEL CLIENTE");
        private readonly ComboBox _cboCliente = new() { DropDownStyle = ComboBoxStyle.DropDownList };
        private readonly DataGridView _dgvFacturas = new();

        private readonly TarjetaPanel _tarjetaPago = new("DATOS DEL PAGO");
        private readonly TextBox _txtFactura = CampoFicha.CrearSoloLectura();
        private readonly Label _lblSaldo = new()
        {
            Text = 0m.Dinero(),
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleRight,
            Font = Tema.Numeros(12F, FontStyle.Bold),
            ForeColor = Tema.Secundario
        };
        private readonly DateTimePicker _dtpFecha = new() { Format = DateTimePickerFormat.Custom, CustomFormat = Formato.Fecha };
        private readonly NumericUpDown _nudMonto = new()
        {
            DecimalPlaces = 2,
            ThousandsSeparator = true,
            TextAlign = HorizontalAlignment.Right,
            Font = Tema.Numeros(9.5F),
            Maximum = Formato.MaximoMonto,
            Minimum = 0
        };

        private List<Factura> _facturas = [];
        private bool _cargando;

        public CtlPagoRegistro()
        {
            SuspendLayout();
            AutoScaleMode = AutoScaleMode.None;
            Dock = DockStyle.Fill;
            Margin = new Padding(0);

            _errores.ContainerControl = this;

            _dgvFacturas.Validating += (_, _) => _errores.SetError(_dgvFacturas,
                _dgvFacturas.CurrentRow?.DataBoundItem is not Factura ? "Seleccione una factura." : string.Empty);
            _nudMonto.Validating += (_, _) => _errores.SetError(_nudMonto,
                _nudMonto.Value <= 0 ? "El monto del pago debe ser mayor que 0." : string.Empty);

            var raiz = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                BackColor = Color.Transparent,
                Margin = new Padding(0)
            };
            raiz.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            raiz.RowStyles.Add(new RowStyle(SizeType.Percent, 62));
            raiz.RowStyles.Add(new RowStyle(SizeType.Percent, 38));

            raiz.Controls.Add(CrearTarjetaFacturas(), 0, 0);
            raiz.Controls.Add(CrearTarjetaPago(), 0, 1);
            Controls.Add(raiz);

            ResumeLayout(true);
        }

        public event EventHandler? FichaEnfocada;

        public Factura? FacturaActual { get; private set; }

        public void EnfocarPrimero() => _cboCliente.Focus();

        public void CargarClientesActivos()
        {
            _cargando = true;
            try
            {
                ItemCliente.Llenar(_cboCliente, _clienteNegocio.Buscar(null, soloActivos: true), "Seleccione un cliente");
            }
            catch (DatosException ex)
            {
                _tarjetaFacturas.MostrarNota(ex.Message, esError: true);
            }
            finally
            {
                _cargando = false;
            }
        }

        public Pago Leer() => new()
        {
            FacturaId = FacturaActual?.Id ?? 0,
            FechaPago = _dtpFecha.Value.Date,
            Monto = _nudMonto.Value
        };

        public void Limpiar()
        {
            LimpiarErrores();
            _cargando = true;
            try
            {
                if (_cboCliente.Items.Count > 0)
                    _cboCliente.SelectedIndex = 0;
                _dtpFecha.Value = DateTime.Today;
                _nudMonto.Value = 0;
            }
            finally
            {
                _cargando = false;
            }

            CargarFacturas();
            MostrarFactura(null);
        }

        public void LimpiarErrores() => _errores.Clear();

        public void MarcarCampo(string? campo, string mensaje)
        {
            Control? control = campo switch
            {
                nameof(Pago.FacturaId) => _dgvFacturas,
                nameof(Pago.FechaPago) => _dtpFecha,
                nameof(Pago.Monto) => _nudMonto,
                _ => null
            };

            if (control is null)
                return;

            _errores.SetError(control, mensaje);
            control.Focus();
        }

        private Control CrearTarjetaFacturas()
        {
            _tarjetaFacturas.AutoSize = false;
            _tarjetaFacturas.Margin = new Padding(0, 0, 0, 10);
            _tarjetaFacturas.Cuerpo.AutoSize = false;
            _tarjetaFacturas.Cuerpo.RowCount = 2;
            _tarjetaFacturas.Cuerpo.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            _tarjetaFacturas.Cuerpo.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            var barra = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                ColumnCount = 2,
                RowCount = 1,
                Margin = new Padding(0),
                BackColor = Color.Transparent
            };
            barra.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            barra.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            _cboCliente.Dock = DockStyle.Fill;
            _cboCliente.Width = LogicalToDeviceUnits(320);
            _cboCliente.Margin = new Padding(8, 6, 0, 6);
            _cboCliente.TabIndex = 0;
            _cboCliente.SelectedIndexChanged += (_, _) =>
            {
                if (!_cargando)
                {
                    CargarFacturas();
                    MostrarFactura(null);
                }
            };
            _cboCliente.Enter += (_, _) => FichaEnfocada?.Invoke(this, EventArgs.Empty);

            barra.Controls.Add(new Label
            {
                Text = "Cliente:",
                ForeColor = Tema.TextoSecundario,
                AutoSize = true,
                Margin = new Padding(0, 10, 8, 0)
            }, 0, 0);
            barra.Controls.Add(_cboCliente, 1, 0);

            EstiloGrid.Aplicar(_dgvFacturas);
            _dgvFacturas.Margin = new Padding(0, 6, 0, 0);
            _dgvFacturas.TabIndex = 1;
            EstiloGrid.AgregarColumna(_dgvFacturas, "colNumero", "Factura", 15, negrita: true, propiedad: nameof(Factura.Numero));
            EstiloGrid.AgregarColumna(_dgvFacturas, "colEmision", "Emisión", 13, formato: Formato.Fecha, propiedad: nameof(Factura.FechaEmision));
            EstiloGrid.AgregarColumna(_dgvFacturas, "colVenc", "Vencimiento", 13, formato: Formato.Fecha, propiedad: nameof(Factura.FechaVencimiento));
            EstiloGrid.AgregarColumna(_dgvFacturas, "colTotal", "Total (B/.)", 13, derecha: true, formato: Formato.Monto, propiedad: nameof(Factura.Total));
            EstiloGrid.AgregarColumna(_dgvFacturas, "colPagado", "Pagado (B/.)", 13, derecha: true, formato: Formato.Monto, propiedad: nameof(Factura.TotalPagado));
            EstiloGrid.AgregarColumna(_dgvFacturas, "colSaldo", "Saldo (B/.)", 16, derecha: true, formato: Formato.Monto, negrita: true, propiedad: nameof(Factura.Saldo));
            EstiloGrid.AgregarColumna(_dgvFacturas, "colEstado", "Estado", 12, propiedad: nameof(Factura.Estado));
            EstiloGrid.AgregarColumna(_dgvFacturas, "colVencida", "Vencida", 11, propiedad: nameof(Factura.TextoVencida));

            _dgvFacturas.CellFormatting += (_, e) =>
            {
                if (e.RowIndex < 0 || _dgvFacturas.Rows[e.RowIndex].DataBoundItem is not Factura factura)
                    return;
                if (factura.EstaVencida)
                    e.CellStyle!.ForeColor = Tema.Secundario;
            };
            _dgvFacturas.SelectionChanged += DgvFacturas_CambioSeleccion;
            _dgvFacturas.CurrentCellChanged += DgvFacturas_CambioSeleccion;

            _tarjetaFacturas.Cuerpo.Controls.Add(barra, 0, 0);
            _tarjetaFacturas.Cuerpo.Controls.Add(_dgvFacturas, 0, 1);
            return _tarjetaFacturas;
        }

        private Control CrearTarjetaPago()
        {
            _tarjetaPago.AutoSize = false;
            _tarjetaPago.Cuerpo.AutoSize = false;

            var rejilla = CampoFicha.CrearRejilla(2);
            CampoFicha.Agregar(rejilla, "Factura seleccionada", _txtFactura, 0, 0);
            CampoFicha.Agregar(rejilla, "Saldo disponible (B/.)", _lblSaldo, 0, 1);
            CampoFicha.Agregar(rejilla, "Fecha de pago", _dtpFecha, 1, 0, obligatorio: true);
            CampoFicha.Agregar(rejilla, "Monto a abonar (B/.)", _nudMonto, 1, 1, obligatorio: true);

            _dtpFecha.TabIndex = 2;
            _nudMonto.TabIndex = 3;
            _tarjetaPago.MostrarNota("* obligatorio · el monto no puede superar el saldo");
            _tarjetaPago.Cuerpo.Controls.Add(rejilla, 0, 0);
            return _tarjetaPago;
        }

        private void CargarFacturas()
        {
            var clienteId = (_cboCliente.SelectedItem as ItemCliente)?.Id ?? 0;
            try
            {
                _facturas = _negocio.ObtenerFacturasConSaldo(clienteId);
                _tarjetaFacturas.MostrarNota(clienteId == 0
                    ? "Seleccione un cliente"
                    : $"{_facturas.Count} factura(s) con saldo");
            }
            catch (DatosException ex)
            {
                _facturas = [];
                _tarjetaFacturas.MostrarNota(ex.Message, esError: true);
            }

            _cargando = true;
            try
            {
                _dgvFacturas.DataSource = _facturas;
                _dgvFacturas.ClearSelection();
                _dgvFacturas.CurrentCell = null;
            }
            finally
            {
                _cargando = false;
            }
        }

        private void DgvFacturas_CambioSeleccion(object? sender, EventArgs e)
        {
            if (_cargando || _dgvFacturas.CurrentRow?.DataBoundItem is not Factura factura)
                return;
            MostrarFactura(factura);
        }

        private void MostrarFactura(Factura? factura)
        {
            FacturaActual = factura;
            _errores.Clear();

            if (factura is null)
            {
                _txtFactura.Clear();
                _lblSaldo.Text = 0m.Dinero();
                _nudMonto.Value = 0;
                return;
            }

            _txtFactura.Text = factura.Numero;
            _lblSaldo.Text = $"{factura.Saldo.Dinero()}";
            _nudMonto.Maximum = Math.Min(Formato.MaximoMonto, Math.Max(0.01m, factura.Saldo));
            _nudMonto.Value = Math.Min(factura.Saldo, _nudMonto.Maximum);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _errores.Dispose();
            base.Dispose(disposing);
        }
    }
}
```

<a id="kobrauipagosfrmpagoscs"></a>

#### Kobra.UI/Pagos/FrmPagos.cs

```csharp
using Kobra.Datos;
using Kobra.Entidades;
using Kobra.Negocio;

namespace Kobra.UI
{
    /// <summary>Módulo de Pagos: orquesta consulta, registro, anulación y reportes</summary>
    public partial class FrmPagos : FormularioConVistas
    {
        private enum Vista { Consulta, Registro, Anulacion, Reportes }

        private readonly PagoNegocio _negocio = new();

        private readonly BarraTituloModulo _barraTitulo = new("PAGOS");
        private readonly BarraAcciones _barraAcciones =
            new(Mensajes.PistaAtajos("nuevo"));

        private readonly BotonTema _btnTabConsulta = new("Consulta de Pagos");
        private readonly BotonTema _btnTabRegistro = new("Registrar Pago");
        private readonly BotonTema _btnTabAnulacion = new("Anular Pago");
        private readonly BotonTema _btnTabReportes = new("Consultas y reportes");

        private readonly BotonTema _btnNuevo = new("&Nuevo");
        private readonly BotonTema _btnGuardar = new("&Guardar", EstiloBoton.Primario);
        private readonly BotonTema _btnIrAnular = new("&Anular", EstiloBoton.Peligro) { Enabled = false };
        private readonly BotonTema _btnConfirmarAnular = new("&Anular pago", EstiloBoton.Peligro) { Enabled = false };
        private readonly BotonTema _btnLimpiar = new("&Limpiar");
        private readonly BotonTema _btnCerrar = new("&Cerrar");

        private readonly CtlPagoConsulta _consulta = new();
        private readonly CtlPagoRegistro _registro = new();
        private readonly CtlPagoAnulacion _anulacion = new();
        private readonly CtlReporteCuenta _reporteCuenta = new();
        private readonly CtlReporteAntiguedad _reporteAntiguedad = new();
        private readonly TableLayoutPanel _panelReportes = new();

        private Vista _vistaActual = Vista.Consulta;
        private Pago? _seleccionado;

        public FrmPagos()
        {
            InitializeComponent();
            ConstruirInterfaz();
            ConectarEventos();
        }

        private void ConstruirInterfaz()
        {
            ConfigurarVentana("Pagos");

            _barraAcciones.Agregar(_btnNuevo, _btnGuardar, _btnIrAnular, _btnConfirmarAnular, _btnLimpiar, _btnCerrar);

            _panelReportes.Dock = DockStyle.Fill;
            _panelReportes.ColumnCount = 2;
            _panelReportes.RowCount = 1;
            _panelReportes.BackColor = Color.Transparent;
            _panelReportes.Margin = new Padding(0);
            _panelReportes.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            _panelReportes.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            _panelReportes.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            _reporteCuenta.Margin = new Padding(0, 0, 5, 0);
            _reporteAntiguedad.Margin = new Padding(5, 0, 0, 0);
            _panelReportes.Controls.Add(_reporteCuenta, 0, 0);
            _panelReportes.Controls.Add(_reporteAntiguedad, 1, 0);

            ArmarMarco(_barraTitulo, _barraAcciones, _btnCerrar,
                [_btnTabConsulta, _btnTabRegistro, _btnTabAnulacion, _btnTabReportes],
                _consulta, _registro, _anulacion, _panelReportes);
        }

        private void ConectarEventos()
        {
            Load += FrmPagos_Load;

            _btnTabConsulta.Click += (_, _) => CambiarVista(Vista.Consulta);
            _btnTabRegistro.Click += (_, _) =>
            {
                CambiarVista(Vista.Registro);
                _registro.EnfocarPrimero();
            };
            _btnTabAnulacion.Click += (_, _) =>
            {
                CambiarVista(Vista.Anulacion);
                if (_seleccionado is not null)
                    _anulacion.Cargar(_seleccionado);
            };
            _btnTabReportes.Click += (_, _) => CambiarVista(Vista.Reportes);

            _btnNuevo.Click += (_, _) =>
            {
                CambiarVista(Vista.Registro);
                _registro.Limpiar();
                _registro.EnfocarPrimero();
            };
            _btnGuardar.Click += (_, _) => Guardar();
            _btnIrAnular.Click += (_, _) =>
            {
                CambiarVista(Vista.Anulacion);
                if (_seleccionado is not null)
                    _anulacion.Cargar(_seleccionado);
            };
            _btnConfirmarAnular.Click += (_, _) => Anular();
            _btnLimpiar.Click += (_, _) => LimpiarVistaActual();
            _btnCerrar.Click += (_, _) => Close();

            _consulta.BusquedaSolicitada += explicita => CargarConsulta(mostrarMensaje: explicita);
            _consulta.PagoSeleccionado += pago =>
            {
                _seleccionado = pago;
                _btnIrAnular.Enabled = pago is { Anulado: false, FacturaAnulada: false };
            };
            _consulta.BusquedaEnfocada += (_, _) => AcceptButton = _consulta.BotonBuscar;
            _consulta.RejillaEnfocada += (_, _) => AcceptButton = null;

            _registro.FichaEnfocada += (_, _) => AcceptButton = _btnGuardar;

            _anulacion.PagoCargado += pago =>
                _btnConfirmarAnular.Enabled = pago is { Anulado: false, FacturaAnulada: false };
            _anulacion.BusquedaEnfocada += (_, _) => AcceptButton = _anulacion.BotonBuscar;
            _anulacion.MotivoEnfocado += (_, _) => AcceptButton = null;

            _reporteCuenta.GeneracionEnfocada += (_, _) => AcceptButton = _reporteCuenta.BotonGenerar;
            _reporteAntiguedad.GeneracionEnfocada += (_, _) => AcceptButton = _reporteAntiguedad.BotonGenerar;
        }

        private void FrmPagos_Load(object? sender, EventArgs e)
        {
            _consulta.CargarClientesFiltro();
            _registro.CargarClientesActivos();
            _registro.Limpiar();
            _reporteCuenta.CargarClientes();
            _reporteAntiguedad.CargarClientes();
            CambiarVista(Vista.Consulta);
            CargarConsulta(mostrarMensaje: true);
            _consulta.EnfocarBusqueda();
        }

        protected override bool ProcesarAtajo(Keys tecla)
        {
            switch (tecla)
            {
                case Keys.F2:
                    if (_vistaActual == Vista.Consulta)
                        _consulta.EnfocarBusqueda();
                    else if (_vistaActual == Vista.Anulacion)
                        _anulacion.EnfocarBusqueda();
                    else if (_vistaActual == Vista.Reportes)
                        _reporteCuenta.EnfocarGenerar();
                    else
                        return false;
                    return true;

                case Keys.F5:
                    RecargarFiltros();
                    CargarConsulta(mostrarMensaje: true);
                    if (_vistaActual == Vista.Reportes)
                    {
                        _reporteCuenta.Generar();
                        _reporteAntiguedad.Generar();
                    }
                    return true;

                case Keys.Control | Keys.N:
                    CambiarVista(Vista.Registro);
                    _registro.Limpiar();
                    _registro.EnfocarPrimero();
                    return true;

                case Keys.Control | Keys.S:
                    if (_vistaActual != Vista.Registro)
                        return false;
                    Guardar();
                    return true;

                default:
                    return false;
            }
        }

        private void RecargarFiltros()
        {
            _consulta.CargarClientesFiltro();
            _registro.CargarClientesActivos();
            _reporteCuenta.CargarClientes();
            _reporteAntiguedad.CargarClientes();
        }

        private void CambiarVista(Vista vista)
        {
            _vistaActual = vista;

            _consulta.Visible = vista == Vista.Consulta;
            _registro.Visible = vista == Vista.Registro;
            _anulacion.Visible = vista == Vista.Anulacion;
            _panelReportes.Visible = vista == Vista.Reportes;

            MarcarPestana((int)vista);

            _btnNuevo.Visible = vista == Vista.Consulta;
            _btnIrAnular.Visible = vista == Vista.Consulta;
            _btnGuardar.Visible = vista == Vista.Registro;
            _btnConfirmarAnular.Visible = vista == Vista.Anulacion;

            AcceptButton = vista switch
            {
                Vista.Consulta => _consulta.BotonBuscar,
                Vista.Registro => _btnGuardar,
                Vista.Anulacion => _anulacion.BotonBuscar,
                Vista.Reportes => _reporteCuenta.BotonGenerar,
                _ => null
            };

            _barraTitulo.Info = vista switch
            {
                Vista.Consulta => "CONSULTA DE PAGOS",
                Vista.Registro => "MODO: NUEVO PAGO",
                Vista.Anulacion => "ANULACIÓN DE PAGO",
                Vista.Reportes => "ESTADO DE CUENTA Y ANTIGÜEDAD",
                _ => string.Empty
            };

            _barraAcciones.RestablecerPista();

            if (vista == Vista.Registro)
                _registro.EnfocarPrimero();
            else if (vista == Vista.Anulacion)
                _anulacion.EnfocarBusqueda();
        }

        private void CargarConsulta(bool mostrarMensaje, int? seleccionarId = null)
        {
            var mantener = seleccionarId ?? _seleccionado?.Id;
            var error = _consulta.Cargar(mantener);
            if (error is not null && mostrarMensaje)
                MostrarAdvertencia(error);
        }

        private void Guardar()
        {
            _registro.LimpiarErrores();
            var pago = _registro.Leer();

            try
            {
                var nuevoId = _negocio.Registrar(pago);
                _registro.Limpiar();
                CambiarVista(Vista.Consulta);
                CargarConsulta(mostrarMensaje: false, seleccionarId: nuevoId);
                var numeroFactura = _consulta.ObtenerNumeroFactura(nuevoId);
                var mensaje = string.IsNullOrEmpty(numeroFactura)
                    ? $"Pago n.º {nuevoId} registrado."
                    : $"Pago registrado en la factura {numeroFactura}.";
                _barraAcciones.MostrarMensaje(mensaje, esError: false);
            }
            catch (ReglaNegocioException ex)
            {
                _registro.MarcarCampo(ex.Campo, ex.Message);
                MostrarError(ex);
            }
            catch (DatosException ex)
            {
                MostrarError(ex);
            }
        }

        private void Anular()
        {
            var pago = _anulacion.PagoActual;
            if (pago is null)
                return;

            var motivo = _anulacion.Motivo;
            if (motivo.Length == 0)
            {
                _anulacion.MarcarErrorMotivo(Mensajes.MotivoObligatorio);
                MostrarAdvertencia(Mensajes.MotivoFaltante);
                return;
            }

            var confirmado = Confirmar(
                $"¿Anular definitivamente el pago n.º {pago.Id}?",
                $"Factura: {pago.NumeroFactura}  ·  Cliente: {pago.ClienteDescripcion}  ·  Monto: {pago.Monto.Dinero()}",
                "El saldo de la factura volverá a aumentar. Esta acción es irreversible.",
                peligro: true,
                textoSi: "&Anular");

            if (!confirmado)
                return;

            try
            {
                _negocio.Anular(pago.Id, motivo);
                _anulacion.Limpiar();
                CambiarVista(Vista.Consulta);
                CargarConsulta(mostrarMensaje: false, seleccionarId: pago.Id);
                _barraAcciones.MostrarMensaje($"Pago n.º {pago.Id} anulado.", esError: false);
            }
            catch (ReglaNegocioException ex)
            {
                if (ex.Campo == nameof(Pago.MotivoAnulacion))
                    _anulacion.MarcarErrorMotivo(ex.Message);
                MostrarError(ex);
            }
            catch (DatosException ex)
            {
                MostrarError(ex);
            }
        }

        private void LimpiarVistaActual()
        {
            switch (_vistaActual)
            {
                case Vista.Consulta:
                    _consulta.LimpiarFiltros();
                    CargarConsulta(mostrarMensaje: false);
                    break;
                case Vista.Registro:
                    _registro.Limpiar();
                    _registro.EnfocarPrimero();
                    break;
                case Vista.Anulacion:
                    _anulacion.Limpiar();
                    _anulacion.EnfocarBusqueda();
                    break;
                case Vista.Reportes:
                    _reporteCuenta.Limpiar();
                    _reporteAntiguedad.Limpiar();
                    break;
            }
            _barraAcciones.RestablecerPista();
        }
    }
}
```

<a id="kobrauipagosfrmpagosdesignercs"></a>

#### Kobra.UI/Pagos/FrmPagos.Designer.cs

```csharp
namespace Kobra.UI
{
    partial class FrmPagos
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
            Name = "FrmPagos";
            Text = "Kobra · Pagos";
        }

        #endregion
    }
}
```

<a id="reportes"></a>

### Reportes

<a id="kobrauireportesctlreporteantiguedadcs"></a>

#### Kobra.UI/Reportes/CtlReporteAntiguedad.cs

```csharp
using Kobra.Datos;
using Kobra.Entidades;
using Kobra.Negocio;

namespace Kobra.UI
{
    /// <summary>Reporte de antigüedad de saldos: cinco tramos con su detalle de facturas, con filtro opcional por cliente</summary>
    internal sealed class CtlReporteAntiguedad : UserControl
    {
        private readonly ReporteNegocio _negocio = new();
        private readonly ClienteNegocio _clienteNegocio = new();

        private readonly TarjetaPanel _tarjetaFiltros = new("ANTIGÜEDAD DE SALDOS");
        private readonly ComboBox _cboCliente = new() { DropDownStyle = ComboBoxStyle.DropDownList };
        private readonly BotonTema _btnGenerar = new("&Generar", EstiloBoton.Primario);
        private readonly BarraTotales _lblResumen = new();

        private readonly TarjetaPanel _tarjetaTramos = new("TRAMOS DE ANTIGÜEDAD");
        private readonly DataGridView _dgvTramos = new();

        private readonly TarjetaPanel _tarjetaDetalle = new("DETALLE DEL TRAMO");
        private readonly DataGridView _dgvDetalle = new();

        public CtlReporteAntiguedad()
        {
            SuspendLayout();
            AutoScaleMode = AutoScaleMode.None;
            Dock = DockStyle.Fill;
            Margin = new Padding(0);

            var raiz = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                BackColor = Color.Transparent,
                Margin = new Padding(0)
            };
            raiz.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            raiz.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            raiz.RowStyles.Add(new RowStyle(SizeType.Percent, 38));
            raiz.RowStyles.Add(new RowStyle(SizeType.Percent, 62));

            raiz.Controls.Add(CrearFiltros(), 0, 0);
            raiz.Controls.Add(CrearTramos(), 0, 1);
            raiz.Controls.Add(CrearDetalle(), 0, 2);
            Controls.Add(raiz);

            ResumeLayout(true);
        }

        public event EventHandler? GeneracionEnfocada;

        public Button BotonGenerar => _btnGenerar;

        /// <summary>Deja los filtros y las rejillas sin datos, sin consultar la base</summary>
        public void Limpiar()
        {
            if (_cboCliente.Items.Count > 0)
                _cboCliente.SelectedIndex = 0;
            _dgvTramos.DataSource = null;
            _dgvDetalle.DataSource = null;
            _lblResumen.Text = string.Empty;
            _tarjetaFiltros.MostrarNota(string.Empty);
            _tarjetaTramos.MostrarNota(string.Empty);
            _tarjetaDetalle.MostrarNota(string.Empty);
        }

        public void EnfocarGenerar()
        {
            _cboCliente.Focus();
            GeneracionEnfocada?.Invoke(this, EventArgs.Empty);
        }

        public void CargarClientes()
        {
            try
            {
                ItemCliente.Llenar(_cboCliente, _clienteNegocio.Buscar(null, soloActivos: false), "Todos los clientes");
            }
            catch (DatosException ex)
            {
                _tarjetaFiltros.MostrarNota(ex.Message, esError: true);
            }
        }

        public void Generar()
        {
            var clienteId = (_cboCliente.SelectedItem as ItemCliente)?.Id;
            if (clienteId == 0)
                clienteId = null;

            try
            {
                var reporte = _negocio.ObtenerAntiguedad(clienteId);
                _dgvTramos.DataSource = reporte.Tramos;
                _dgvTramos.ClearSelection();
                _dgvTramos.CurrentCell = null;
                _dgvDetalle.DataSource = null;

                _lblResumen.Text =
                    $"Saldo total: {reporte.SaldoTotal.Dinero()}   ·   " +
                    $"{reporte.Facturas} factura(s) con saldo";
                _tarjetaTramos.MostrarNota("Seleccione un tramo para ver el detalle");
                _tarjetaFiltros.MostrarNota(string.Empty);
            }
            catch (DatosException ex)
            {
                _dgvTramos.DataSource = null;
                _dgvDetalle.DataSource = null;
                _lblResumen.Text = string.Empty;
                _tarjetaFiltros.MostrarNota(ex.Message, esError: true);
            }
        }

        private Control CrearFiltros()
        {
            _tarjetaFiltros.Margin = new Padding(0, 0, 0, 10);
            _tarjetaFiltros.Cuerpo.RowCount = 2;
            _tarjetaFiltros.Cuerpo.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            _tarjetaFiltros.Cuerpo.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            var fila = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                ColumnCount = 3,
                RowCount = 1,
                BackColor = Color.Transparent,
                Margin = new Padding(0, 6, 0, 0)
            };
            fila.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            fila.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            fila.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            _cboCliente.Dock = DockStyle.Fill;
            _cboCliente.Margin = new Padding(0, 3, 10, 3);
            _cboCliente.TabIndex = 0;
            _cboCliente.Enter += (_, _) => GeneracionEnfocada?.Invoke(this, EventArgs.Empty);

            _btnGenerar.TabIndex = 1;
            _btnGenerar.Click += (_, _) => Generar();

            fila.Controls.Add(new Label
            {
                Text = "Cliente:",
                ForeColor = Tema.TextoSecundario,
                AutoSize = true,
                Margin = new Padding(0, 8, 8, 0)
            }, 0, 0);
            fila.Controls.Add(_cboCliente, 1, 0);
            fila.Controls.Add(_btnGenerar, 2, 0);

            _tarjetaFiltros.Cuerpo.Controls.Add(fila, 0, 0);
            _tarjetaFiltros.Cuerpo.Controls.Add(_lblResumen, 0, 1);
            return _tarjetaFiltros;
        }

        private Control CrearTramos()
        {
            _tarjetaTramos.AutoSize = false;
            _tarjetaTramos.Margin = new Padding(0, 0, 10, 0);
            _tarjetaTramos.Cuerpo.AutoSize = false;
            _tarjetaTramos.Cuerpo.RowCount = 1;
            _tarjetaTramos.Cuerpo.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            EstiloGrid.Aplicar(_dgvTramos);
            _dgvTramos.Margin = new Padding(0, 6, 0, 0);
            EstiloGrid.AgregarColumna(_dgvTramos, "colTramo", "Tramo", 24, negrita: true, propiedad: nameof(TramoAntiguedad.Nombre));
            EstiloGrid.AgregarColumna(_dgvTramos, "colFacturas", "Facturas", 16, derecha: true, propiedad: nameof(TramoAntiguedad.Facturas));
            EstiloGrid.AgregarColumna(_dgvTramos, "colSaldo", "Saldo (B/.)", 30, derecha: true, formato: Formato.Monto, negrita: true, propiedad: nameof(TramoAntiguedad.Saldo));
            EstiloGrid.AgregarColumna(_dgvTramos, "colPorcentaje", "% del saldo", 30, derecha: true, formato: Formato.Porcentaje, propiedad: nameof(TramoAntiguedad.Porcentaje));

            _dgvTramos.SelectionChanged += DgvTramos_CambioSeleccion;
            _dgvTramos.CurrentCellChanged += DgvTramos_CambioSeleccion;

            _tarjetaTramos.Cuerpo.Controls.Add(_dgvTramos, 0, 0);
            return _tarjetaTramos;
        }

        private Control CrearDetalle()
        {
            _tarjetaDetalle.AutoSize = false;
            _tarjetaDetalle.Cuerpo.AutoSize = false;
            _tarjetaDetalle.Cuerpo.RowCount = 1;
            _tarjetaDetalle.Cuerpo.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            EstiloGrid.Aplicar(_dgvDetalle);
            _dgvDetalle.Margin = new Padding(0, 6, 0, 0);
            EstiloGrid.AgregarColumna(_dgvDetalle, "colFactura", "Factura", 18, negrita: true, propiedad: nameof(LineaAntiguedad.Numero));
            EstiloGrid.AgregarColumna(_dgvDetalle, "colCliente", "Cliente", 34, propiedad: nameof(LineaAntiguedad.ClienteDescripcion));
            EstiloGrid.AgregarColumna(_dgvDetalle, "colVenc", "Vencimiento", 18, formato: Formato.Fecha, propiedad: nameof(LineaAntiguedad.FechaVencimiento));
            EstiloGrid.AgregarColumna(_dgvDetalle, "colSaldo", "Saldo (B/.)", 18, derecha: true, formato: Formato.Monto, propiedad: nameof(LineaAntiguedad.Saldo));
            EstiloGrid.AgregarColumna(_dgvDetalle, "colDias", "Días venc.", 12, derecha: true, propiedad: nameof(LineaAntiguedad.DiasVencidos));

            _tarjetaDetalle.Cuerpo.Controls.Add(_dgvDetalle, 0, 0);
            return _tarjetaDetalle;
        }

        private void DgvTramos_CambioSeleccion(object? sender, EventArgs e)
        {
            if (_dgvTramos.CurrentRow?.DataBoundItem is not TramoAntiguedad tramo)
                return;

            _dgvDetalle.DataSource = tramo.Detalle;
            _dgvDetalle.ClearSelection();
            _tarjetaDetalle.MostrarNota($"{tramo.Nombre} · {tramo.Facturas} factura(s)");
        }
    }
}
```

<a id="kobrauireportesctlreportecuentacs"></a>

#### Kobra.UI/Reportes/CtlReporteCuenta.cs

```csharp
using Kobra.Datos;
using Kobra.Entidades;
using Kobra.Negocio;

namespace Kobra.UI
{
    /// <summary>Estado de cuenta del cliente: facturas con saldo, pagos de la factura seleccionada y totales</summary>
    internal sealed class CtlReporteCuenta : UserControl
    {
        private readonly ReporteNegocio _negocio = new();
        private readonly ClienteNegocio _clienteNegocio = new();

        private readonly TarjetaPanel _tarjetaFiltros = new("ESTADO DE CUENTA DEL CLIENTE");
        private readonly ComboBox _cboCliente = new() { DropDownStyle = ComboBoxStyle.DropDownList };
        private readonly BotonTema _btnGenerar = new("&Generar", EstiloBoton.Primario);
        private readonly Label _lblCliente = new() { AutoSize = true, Font = Tema.Negrita(9.5F), ForeColor = Tema.Primario };
        private readonly Label _lblLimite = new() { AutoSize = true, Font = Tema.Normal(8.5F), ForeColor = Tema.TextoSecundario };

        private readonly TarjetaPanel _tarjetaFacturas = new("FACTURAS DEL CLIENTE");
        private readonly DataGridView _dgvFacturas = new();
        private readonly BarraTotales _lblTotales = new();

        private readonly TarjetaPanel _tarjetaPagos = new("PAGOS DE LA FACTURA SELECCIONADA");
        private readonly DataGridView _dgvPagos = new();

        public CtlReporteCuenta()
        {
            SuspendLayout();
            AutoScaleMode = AutoScaleMode.None;
            Dock = DockStyle.Fill;
            Margin = new Padding(0);

            var raiz = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                BackColor = Color.Transparent,
                Margin = new Padding(0)
            };
            raiz.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            raiz.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            raiz.RowStyles.Add(new RowStyle(SizeType.Percent, 62));
            raiz.RowStyles.Add(new RowStyle(SizeType.Percent, 38));

            raiz.Controls.Add(CrearFiltros(), 0, 0);
            raiz.Controls.Add(CrearFacturas(), 0, 1);
            raiz.Controls.Add(CrearPagos(), 0, 2);
            Controls.Add(raiz);

            ResumeLayout(true);
        }

        public event EventHandler? GeneracionEnfocada;

        public Button BotonGenerar => _btnGenerar;

        /// <summary>Deja los filtros y las rejillas sin datos, sin consultar la base</summary>
        public void Limpiar()
        {
            if (_cboCliente.Items.Count > 0)
                _cboCliente.SelectedIndex = 0;
            LimpiarDetalle();
            _tarjetaFiltros.MostrarNota(string.Empty);
        }

        public void EnfocarGenerar()
        {
            _cboCliente.Focus();
            GeneracionEnfocada?.Invoke(this, EventArgs.Empty);
        }

        public void CargarClientes()
        {
            try
            {
                ItemCliente.Llenar(_cboCliente, _clienteNegocio.Buscar(null, soloActivos: false), "Seleccione un cliente");
            }
            catch (DatosException ex)
            {
                _tarjetaFiltros.MostrarNota(ex.Message, esError: true);
            }
        }

        public void Generar()
        {
            var clienteId = (_cboCliente.SelectedItem as ItemCliente)?.Id ?? 0;

            try
            {
                var cuenta = _negocio.ObtenerEstadoCuenta(clienteId);
                if (cuenta is null)
                {
                    LimpiarDetalle();
                    _tarjetaFiltros.MostrarNota("El cliente ya no existe.", esError: true);
                    return;
                }

                _lblCliente.Text = cuenta.ClienteDescripcion +
                    (string.IsNullOrWhiteSpace(cuenta.ClienteRucCedula) ? string.Empty : $"  ·  RUC: {cuenta.ClienteRucCedula}");
                _lblLimite.Text = $"Límite de crédito: {cuenta.LimiteCredito.Dinero()}" +
                    (cuenta.Activo ? string.Empty : "  ·  Cliente inactivo");

                _dgvFacturas.DataSource = cuenta.Facturas;
                _lblTotales.Text =
                    $"Facturado: {cuenta.TotalFacturado.Dinero()}   ·   " +
                    $"Cobrado: {cuenta.TotalCobrado.Dinero()}   ·   " +
                    $"Saldo: {cuenta.Saldo.Dinero()}";

                _dgvFacturas.ClearSelection();
                _dgvFacturas.CurrentCell = null;
                _dgvPagos.DataSource = null;
                _tarjetaPagos.MostrarNota("Seleccione una factura");
                _tarjetaFiltros.MostrarNota($"{cuenta.Facturas.Count} factura(s)");
            }
            catch (ReglaNegocioException ex)
            {
                LimpiarDetalle();
                _tarjetaFiltros.MostrarNota(ex.Message, esError: true);
            }
            catch (DatosException ex)
            {
                LimpiarDetalle();
                _tarjetaFiltros.MostrarNota(ex.Message, esError: true);
            }
        }

        private Control CrearFiltros()
        {
            _tarjetaFiltros.Margin = new Padding(0, 0, 0, 10);
            _tarjetaFiltros.Cuerpo.RowCount = 2;
            _tarjetaFiltros.Cuerpo.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            _tarjetaFiltros.Cuerpo.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            var fila = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                ColumnCount = 3,
                RowCount = 1,
                BackColor = Color.Transparent,
                Margin = new Padding(0, 6, 0, 0)
            };
            fila.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            fila.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            fila.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            _cboCliente.Dock = DockStyle.Fill;
            _cboCliente.Margin = new Padding(0, 3, 10, 3);
            _cboCliente.TabIndex = 0;
            _cboCliente.Enter += (_, _) => GeneracionEnfocada?.Invoke(this, EventArgs.Empty);

            _btnGenerar.TabIndex = 1;
            _btnGenerar.Click += (_, _) => Generar();

            fila.Controls.Add(new Label
            {
                Text = "Cliente:",
                ForeColor = Tema.TextoSecundario,
                AutoSize = true,
                Margin = new Padding(0, 8, 8, 0)
            }, 0, 0);
            fila.Controls.Add(_cboCliente, 1, 0);
            fila.Controls.Add(_btnGenerar, 2, 0);

            var cabecera = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                ColumnCount = 2,
                RowCount = 1,
                BackColor = Color.Transparent,
                Margin = new Padding(0, 6, 0, 0)
            };
            cabecera.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60));
            cabecera.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));
            _lblCliente.Margin = new Padding(0, 6, 0, 0);
            _lblLimite.Margin = new Padding(0, 6, 0, 0);
            _lblLimite.TextAlign = ContentAlignment.MiddleRight;
            cabecera.Controls.Add(_lblCliente, 0, 0);
            cabecera.Controls.Add(_lblLimite, 1, 0);

            _tarjetaFiltros.Cuerpo.Controls.Add(fila, 0, 0);
            _tarjetaFiltros.Cuerpo.Controls.Add(cabecera, 0, 1);
            return _tarjetaFiltros;
        }

        private Control CrearFacturas()
        {
            _tarjetaFacturas.AutoSize = false;
            _tarjetaFacturas.Margin = new Padding(0, 0, 0, 10);
            _tarjetaFacturas.Cuerpo.AutoSize = false;
            _tarjetaFacturas.Cuerpo.RowCount = 2;
            _tarjetaFacturas.Cuerpo.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            _tarjetaFacturas.Cuerpo.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            EstiloGrid.Aplicar(_dgvFacturas);
            _dgvFacturas.Margin = new Padding(0, 6, 0, 0);
            EstiloGrid.AgregarColumna(_dgvFacturas, "colNumero", "Factura", 14, negrita: true, propiedad: nameof(LineaEstadoCuenta.Numero));
            EstiloGrid.AgregarColumna(_dgvFacturas, "colEmision", "Emisión", 12, formato: Formato.Fecha, propiedad: nameof(LineaEstadoCuenta.FechaEmision));
            EstiloGrid.AgregarColumna(_dgvFacturas, "colVenc", "Vencimiento", 12, formato: Formato.Fecha, propiedad: nameof(LineaEstadoCuenta.FechaVencimiento));
            EstiloGrid.AgregarColumna(_dgvFacturas, "colTotal", "Total (B/.)", 13, derecha: true, formato: Formato.Monto, propiedad: nameof(LineaEstadoCuenta.Total));
            EstiloGrid.AgregarColumna(_dgvFacturas, "colPagado", "Pagado (B/.)", 13, derecha: true, formato: Formato.Monto, propiedad: nameof(LineaEstadoCuenta.TotalPagado));
            EstiloGrid.AgregarColumna(_dgvFacturas, "colSaldo", "Saldo (B/.)", 14, derecha: true, formato: Formato.Monto, negrita: true, propiedad: nameof(LineaEstadoCuenta.Saldo));
            EstiloGrid.AgregarColumna(_dgvFacturas, "colEstado", "Estado", 11, propiedad: nameof(LineaEstadoCuenta.Estado));
            EstiloGrid.AgregarColumna(_dgvFacturas, "colVencida", "Vencida", 11, propiedad: nameof(LineaEstadoCuenta.TextoVencida));

            _dgvFacturas.SelectionChanged += DgvFacturas_CambioSeleccion;
            _dgvFacturas.CurrentCellChanged += DgvFacturas_CambioSeleccion;
            _dgvFacturas.CellFormatting += (_, e) =>
            {
                if (e.RowIndex < 0 || _dgvFacturas.Rows[e.RowIndex].DataBoundItem is not LineaEstadoCuenta linea)
                    return;
                if (linea.EstaVencida)
                    e.CellStyle!.ForeColor = Tema.Secundario;
            };

            _tarjetaFacturas.Cuerpo.Controls.Add(_dgvFacturas, 0, 0);
            _tarjetaFacturas.Cuerpo.Controls.Add(_lblTotales, 0, 1);
            return _tarjetaFacturas;
        }

        private Control CrearPagos()
        {
            _tarjetaPagos.AutoSize = false;
            _tarjetaPagos.Cuerpo.AutoSize = false;
            _tarjetaPagos.Cuerpo.RowCount = 1;
            _tarjetaPagos.Cuerpo.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            EstiloGrid.Aplicar(_dgvPagos);
            _dgvPagos.Margin = new Padding(0, 6, 0, 0);
            EstiloGrid.AgregarColumna(_dgvPagos, "colNumero", "N.º pago", 12, negrita: true, propiedad: nameof(Pago.Id));
            EstiloGrid.AgregarColumna(_dgvPagos, "colFecha", "Fecha pago", 18, formato: Formato.Fecha, propiedad: nameof(Pago.FechaPago));
            EstiloGrid.AgregarColumna(_dgvPagos, "colMonto", "Monto (B/.)", 16, derecha: true, formato: Formato.Monto, propiedad: nameof(Pago.Monto));
            EstiloGrid.AgregarColumna(_dgvPagos, "colEstado", "Estado", 14, propiedad: nameof(Pago.EstadoTexto));
            EstiloGrid.AgregarColumna(_dgvPagos, "colMotivo", "Motivo anulación", 40, propiedad: nameof(Pago.MotivoAnulacion));

            _tarjetaPagos.Cuerpo.Controls.Add(_dgvPagos, 0, 0);
            return _tarjetaPagos;
        }

        private void DgvFacturas_CambioSeleccion(object? sender, EventArgs e)
        {
            if (_dgvFacturas.CurrentRow?.DataBoundItem is not LineaEstadoCuenta linea)
                return;

            try
            {
                var pagos = _negocio.ListarPagosDeFactura(linea.FacturaId);
                _dgvPagos.DataSource = pagos;
                _dgvPagos.ClearSelection();
                _tarjetaPagos.MostrarNota($"Factura {linea.Numero} · {pagos.Count} pago(s)");
            }
            catch (DatosException ex)
            {
                _tarjetaPagos.MostrarNota(ex.Message, esError: true);
            }
        }

        private void LimpiarDetalle()
        {
            _lblCliente.Text = "—";
            _lblLimite.Text = string.Empty;
            _lblTotales.Text = string.Empty;
            _dgvFacturas.DataSource = null;
            _dgvPagos.DataSource = null;
            _tarjetaPagos.MostrarNota(string.Empty);
        }
    }
}
```

<a id="componentes-compartidos"></a>

### Componentes compartidos

<a id="kobrauicomponentesbarraaccionescs"></a>

#### Kobra.UI/Componentes/BarraAcciones.cs

```csharp
namespace Kobra.UI
{
    /// <summary>Barra inferior del módulo: a la izquierda una pista de teclas (o el último mensaje de resultado) y a la derecha los botones de acción</summary>
    internal sealed class BarraAcciones : TableLayoutPanel
    {
        private readonly Label _mensaje = new();
        private readonly FlowLayoutPanel _botones = new();
        private readonly string _pista;

        public BarraAcciones(string pista)
        {
            _pista = pista;
            Dock = DockStyle.Bottom;
            AutoSize = true;
            ColumnCount = 2;
            RowCount = 1;
            BackColor = Tema.ContenedorBajo;
            Padding = new Padding(12, 8, 12, 8);
            Margin = new Padding(0);
            ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            _mensaje.AutoSize = false;
            _mensaje.AutoEllipsis = true;
            _mensaje.Dock = DockStyle.Fill;
            _mensaje.TextAlign = ContentAlignment.MiddleLeft;
            _mensaje.Font = Tema.Normal(8.5F);
            _mensaje.Margin = new Padding(0, 0, 12, 0);

            _botones.AutoSize = true;
            _botones.WrapContents = false;
            _botones.FlowDirection = FlowDirection.LeftToRight;
            _botones.Anchor = AnchorStyles.Right;
            _botones.Margin = new Padding(0);

            Controls.Add(_mensaje, 0, 0);
            Controls.Add(_botones, 1, 0);
            RestablecerPista();
        }

        /// <summary>Agrega botones de izquierda a derecha, con separación entre ellos</summary>
        public void Agregar(params Button[] botones)
        {
            foreach (var b in botones)
            {
                b.Margin = new Padding(_botones.Controls.Count == 0 ? 0 : 6, 0, 0, 0);
                _botones.Controls.Add(b);
            }
        }

        public void MostrarMensaje(string texto, bool esError)
        {
            _mensaje.ForeColor = esError ? Tema.Secundario : Tema.Primario;
            _mensaje.Text = texto;
        }

        public void RestablecerPista()
        {
            _mensaje.ForeColor = Tema.TextoSecundario;
            _mensaje.Text = _pista;
        }
    }
}
```

<a id="kobrauicomponentesbarratitulomodulocs"></a>

#### Kobra.UI/Componentes/BarraTituloModulo.cs

```csharp
namespace Kobra.UI
{
    /// <summary>Franja azul con el título del módulo a la izquierda y datos de contexto (modo, ID) a la derecha</summary>
    internal sealed class BarraTituloModulo : TableLayoutPanel
    {
        private readonly Label _titulo = new();
        private readonly Label _info = new();

        public BarraTituloModulo(string titulo)
        {
            Dock = DockStyle.Top;
            AutoSize = true;
            ColumnCount = 2;
            RowCount = 1;
            BackColor = Tema.PrimarioContenedor;
            Padding = new Padding(12, 7, 12, 7);
            Margin = new Padding(0);
            ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            _titulo.Text = titulo;
            _titulo.Font = Tema.Negrita(10F);
            _titulo.ForeColor = Tema.SobrePrimario;
            _titulo.AutoSize = true;
            _titulo.Margin = new Padding(0);

            _info.Font = Tema.Numeros(8.5F);
            _info.ForeColor = Tema.SobrePrimarioContenedor;
            _info.AutoSize = true;
            _info.Anchor = AnchorStyles.Right;
            _info.Margin = new Padding(0);

            Controls.Add(_titulo, 0, 0);
            Controls.Add(_info, 1, 0);
        }

        [System.ComponentModel.Browsable(false)]
        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public string Info
        {
            get => _info.Text;
            set => _info.Text = value;
        }
    }
}
```

<a id="kobrauicomponentesbarratotalescs"></a>

#### Kobra.UI/Componentes/BarraTotales.cs

```csharp
namespace Kobra.UI
{
    /// <summary>Barra de resumen bajo una rejilla: totales en Consolas, alineados a la derecha</summary>
    internal sealed class BarraTotales : Label
    {
        public BarraTotales()
        {
            Dock = DockStyle.Fill;
            AutoSize = false;
            Height = LogicalToDeviceUnits(24);
            TextAlign = ContentAlignment.MiddleRight;
            Font = Tema.Numeros(8.5F);
            ForeColor = Tema.Texto;
            BackColor = Tema.ContenedorBajo;
            Padding = new Padding(8, 0, 8, 0);
            Margin = new Padding(0, 4, 0, 0);
        }
    }
}
```

<a id="kobrauicomponentesbotontemacs"></a>

#### Kobra.UI/Componentes/BotonTema.cs

```csharp
namespace Kobra.UI
{
    internal enum EstiloBoton { Primario, Secundario, Peligro }

    /// <summary>Botón plano con la paleta del tema</summary>
    internal sealed class BotonTema : Button
    {
        private readonly EstiloBoton _estilo;
        private bool _seleccionado;

        public BotonTema(string texto, EstiloBoton estilo = EstiloBoton.Secundario)
        {
            _estilo = estilo;
            Text = texto;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            FlatStyle = FlatStyle.Flat;
            UseVisualStyleBackColor = false;
            Font = Tema.Normal(9F);
            Padding = new Padding(8, 2, 8, 2);
            Margin = new Padding(0);
            MinimumSize = new Size(LogicalToDeviceUnits(96), LogicalToDeviceUnits(30));
            Cursor = Cursors.Hand;
            AplicarColores();
        }

        [System.ComponentModel.Browsable(false)]
        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public bool Seleccionado
        {
            get => _seleccionado;
            set
            {
                if (_seleccionado == value)
                    return;
                _seleccionado = value;
                AplicarColores();
            }
        }

        private void AplicarColores()
        {
            FlatAppearance.BorderSize = 1;

            if (!Enabled)
            {
                BackColor = Tema.ContenedorAlto;
                ForeColor = Tema.TextoApagado;
                FlatAppearance.BorderColor = Tema.ContenedorAlto;
                return;
            }

            if (_seleccionado)
            {
                BackColor = Tema.PrimarioContenedor;
                ForeColor = Tema.SobrePrimario;
                FlatAppearance.BorderColor = Tema.PrimarioContenedor;
                FlatAppearance.MouseOverBackColor = ControlPaint.Dark(Tema.PrimarioContenedor, 0.15f);
                FlatAppearance.MouseDownBackColor = ControlPaint.Dark(Tema.PrimarioContenedor, 0.25f);
                return;
            }

            switch (_estilo)
            {
                case EstiloBoton.Primario:
                    BackColor = Tema.PrimarioContenedor;
                    ForeColor = Tema.SobrePrimario;
                    FlatAppearance.BorderColor = Tema.PrimarioContenedor;
                    FlatAppearance.MouseOverBackColor = Tema.Primario;
                    FlatAppearance.MouseDownBackColor = Tema.Primario;
                    break;
                case EstiloBoton.Peligro:
                    BackColor = Tema.Secundario;
                    ForeColor = Tema.SobrePrimario;
                    FlatAppearance.BorderColor = Tema.Secundario;
                    FlatAppearance.MouseOverBackColor = Tema.PeligroOscuro;
                    FlatAppearance.MouseDownBackColor = Tema.PeligroOscuro;
                    break;
                default:
                    BackColor = Tema.Blanco;
                    ForeColor = Tema.Primario;
                    FlatAppearance.BorderColor = Tema.Borde;
                    FlatAppearance.MouseOverBackColor = Tema.Contenedor;
                    FlatAppearance.MouseDownBackColor = Tema.ContenedorAlto;
                    break;
            }
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            AplicarColores();
        }
    }
}
```

<a id="kobrauicomponentescampofichacs"></a>

#### Kobra.UI/Componentes/CampoFicha.cs

```csharp
namespace Kobra.UI
{
    /// <summary>Rejilla de fichas: dos pares "etiqueta + control" por fila (4 columnas)</summary>
    internal static class CampoFicha
    {
        /// <summary>Caja de texto de solo lectura con el fondo de los datos calculados; los importes van a la derecha</summary>
        public static TextBox CrearSoloLectura(bool derecha = false) => new()
        {
            ReadOnly = true,
            TabStop = false,
            BorderStyle = BorderStyle.FixedSingle,
            BackColor = Tema.Contenedor,
            ForeColor = Tema.TextoSecundario,
            TextAlign = derecha ? HorizontalAlignment.Right : HorizontalAlignment.Left,
            Font = derecha ? Tema.Numeros(9F) : Tema.Normal(9F)
        };

        /// <summary>Etiqueta de una fila de filtros ("Buscar (F2):", "Cliente:")</summary>
        public static Label CrearEtiquetaFiltro(string texto) => new()
        {
            Text = texto,
            ForeColor = Tema.TextoSecundario,
            AutoSize = true,
            Anchor = AnchorStyles.Left,
            Margin = new Padding(0, 0, 6, 0)
        };

        public static TableLayoutPanel CrearRejilla(int filas)
        {
            var t = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                ColumnCount = 4,
                RowCount = filas,
                BackColor = Color.Transparent,
                Margin = new Padding(0, 8, 0, 0)
            };
            t.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            t.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            t.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            t.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            for (var i = 0; i < filas; i++)
                t.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            return t;
        }

        /// <summary>Agrega un campo</summary>
        public static void Agregar(TableLayoutPanel t, string texto, Control control, int fila, int par,
            bool obligatorio = false, int filasOcupadas = 1)
        {
            var etiqueta = new Label
            {
                Text = obligatorio ? texto + " *" : texto,
                ForeColor = Tema.TextoSecundario,
                AutoSize = true,
                Dock = DockStyle.Fill,
                TextAlign = filasOcupadas > 1 ? ContentAlignment.TopRight : ContentAlignment.MiddleRight,

                Margin = new Padding(par == 0 ? 0 : 30, 4, 8, 4)
            };

            if (control is TextBox caja)
                caja.BorderStyle = BorderStyle.FixedSingle;

            control.Dock = DockStyle.Fill;
            control.Margin = new Padding(0, 4, 0, 4);

            t.Controls.Add(etiqueta, par * 2, fila);
            t.Controls.Add(control, par * 2 + 1, fila);
            if (filasOcupadas > 1)
            {
                t.SetRowSpan(etiqueta, filasOcupadas);
                t.SetRowSpan(control, filasOcupadas);
            }
        }
    }
}
```

<a id="kobrauicomponentesdialogoconfirmacioncs"></a>

#### Kobra.UI/Componentes/DialogoConfirmacion.cs

```csharp
namespace Kobra.UI
{
    /// <summary>Confirmación Sí/No con el tema de la aplicación</summary>
    internal sealed class DialogoConfirmacion : Form
    {
        private readonly BotonTema _btnNo = new("&No");

        private DialogoConfirmacion(string titulo, string pregunta, string? detalle, string? advertencia,
            bool peligro, string textoSi)
        {
            AutoScaleMode = AutoScaleMode.None;
            Font = Tema.Normal();
            BackColor = Tema.Blanco;
            Text = titulo;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;

            var ancho = LogicalToDeviceUnits(380);

            var raiz = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                ColumnCount = 1,
                RowCount = 2,
                Margin = new Padding(0)
            };

            var cuerpo = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                ColumnCount = 2,
                RowCount = 1,
                Padding = new Padding(16, 16, 16, 12),
                Margin = new Padding(0)
            };
            var icono = new Label
            {
                Text = peligro ? "!" : "?",
                Font = Tema.Negrita(18F),
                ForeColor = peligro ? Tema.Secundario : Tema.Primario,
                BackColor = peligro ? Tema.ErrorContenedor : Tema.Contenedor,
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(LogicalToDeviceUnits(44), LogicalToDeviceUnits(44)),
                Margin = new Padding(0, 0, 14, 0)
            };

            var texto = new FlowLayoutPanel
            {
                AutoSize = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Margin = new Padding(0)
            };
            texto.Controls.Add(CrearTexto(pregunta, Tema.Negrita(10F), Tema.Texto, ancho, 0));
            if (!string.IsNullOrWhiteSpace(detalle))
                texto.Controls.Add(CrearTexto(detalle, Tema.Normal(9F), Tema.TextoSecundario, ancho, 6));
            if (!string.IsNullOrWhiteSpace(advertencia))
                texto.Controls.Add(CrearTexto(advertencia, Tema.Normal(8.5F), Tema.Secundario, ancho, 8));

            cuerpo.Controls.Add(icono, 0, 0);
            cuerpo.Controls.Add(texto, 1, 0);

            var btnSi = new BotonTema(textoSi, peligro ? EstiloBoton.Peligro : EstiloBoton.Primario)
            {
                DialogResult = DialogResult.Yes
            };
            _btnNo.DialogResult = DialogResult.No;

            var pie = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                FlowDirection = FlowDirection.RightToLeft,
                BackColor = Tema.ContenedorBajo,
                Padding = new Padding(12, 8, 12, 8),
                Margin = new Padding(0)
            };

            _btnNo.Margin = new Padding(6, 0, 0, 0);
            pie.Controls.Add(_btnNo);
            pie.Controls.Add(btnSi);

            raiz.Controls.Add(cuerpo, 0, 0);
            raiz.Controls.Add(pie, 0, 1);
            Controls.Add(raiz);

            AcceptButton = _btnNo;
            CancelButton = _btnNo;
            Shown += (_, _) => _btnNo.Focus();
        }

        private static Label CrearTexto(string texto, Font fuente, Color color, int anchoMaximo, int margenSuperior) =>
            new()
            {
                Text = texto,
                Font = fuente,
                ForeColor = color,
                AutoSize = true,
                MaximumSize = new Size(anchoMaximo, 0),
                Margin = new Padding(0, margenSuperior, 0, 0)
            };

        /// <summary>Devuelve true solo si el usuario elige "Sí"</summary>
        public static bool Confirmar(IWin32Window? propietario, string titulo, string pregunta, string? detalle = null,
            string? advertencia = null, bool peligro = false, string textoSi = "&Sí")
        {
            using var dialogo = new DialogoConfirmacion(titulo, pregunta, detalle, advertencia, peligro, textoSi);
            return dialogo.ShowDialog(propietario) == DialogResult.Yes;
        }
    }
}
```

<a id="kobrauicomponentesestilogridcs"></a>

#### Kobra.UI/Componentes/EstiloGrid.cs

```csharp
namespace Kobra.UI
{
    /// <summary>Apariencia común de las rejillas de solo lectura de todos los módulos</summary>
    internal static class EstiloGrid
    {
        public static void Aplicar(DataGridView g)
        {
            g.Dock = DockStyle.Fill;
            g.ReadOnly = true;
            g.AutoGenerateColumns = false;
            g.AllowUserToAddRows = false;
            g.AllowUserToDeleteRows = false;
            g.AllowUserToResizeRows = false;
            g.RowHeadersVisible = false;
            g.MultiSelect = false;
            g.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            g.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            g.BorderStyle = BorderStyle.None;
            g.BackgroundColor = Tema.Blanco;
            g.GridColor = Tema.ContenedorAlto;
            g.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            g.EnableHeadersVisualStyles = false;
            g.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            g.ColumnHeadersHeight = g.LogicalToDeviceUnits(28);
            g.RowTemplate.Height = g.LogicalToDeviceUnits(24);
            g.ColumnHeadersDefaultCellStyle.BackColor = Tema.Primario;
            g.ColumnHeadersDefaultCellStyle.ForeColor = Tema.SobrePrimario;
            g.ColumnHeadersDefaultCellStyle.SelectionBackColor = Tema.Primario;
            g.ColumnHeadersDefaultCellStyle.Font = Tema.Negrita(9F);
            g.DefaultCellStyle.BackColor = Tema.Blanco;
            g.DefaultCellStyle.ForeColor = Tema.Texto;
            g.DefaultCellStyle.SelectionBackColor = Tema.Contenedor;
            g.DefaultCellStyle.SelectionForeColor = Tema.Texto;
            g.AlternatingRowsDefaultCellStyle.BackColor = Tema.ContenedorBajo;
        }

        /// <summary>Agrega una columna de texto</summary>
        public static DataGridViewTextBoxColumn AgregarColumna(DataGridView g, string nombre, string encabezado,
            float peso, bool derecha = false, string? formato = null, bool negrita = false, string? propiedad = null)
        {
            var col = new DataGridViewTextBoxColumn
            {
                Name = nombre,
                HeaderText = encabezado,
                DataPropertyName = propiedad ?? string.Empty,
                FillWeight = peso,
                ReadOnly = true,
                SortMode = DataGridViewColumnSortMode.NotSortable
            };
            if (derecha)
            {
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            if (formato is not null)
                col.DefaultCellStyle.Format = formato;
            if (negrita)
                col.DefaultCellStyle.Font = Tema.Negrita(9F);
            g.Columns.Add(col);
            return col;
        }
    }
}
```

<a id="kobrauicomponentesformatocs"></a>

#### Kobra.UI/Componentes/Formato.cs

```csharp
using System.Globalization;

namespace Kobra.UI
{
    /// <summary>Formatos de montos y fechas compartidos por todas las ventanas</summary>
    internal static class Formato
    {
        /// <summary>Formato de columna para importes (dos decimales, separador de miles)</summary>
        public const string Monto = "N2";

        public const string Porcentaje = "0.00'%'";
        public const string Fecha = "yyyy-MM-dd";
        public const string FechaHora = "yyyy-MM-dd HH:mm";

        public static CultureInfo Cultura => CultureInfo.CurrentCulture;

        /// <summary>Tope de una columna DECIMAL(18,2)</summary>
        public static readonly decimal MaximoMonto = new([276447231, 23027, 0, 131072]);

        /// <summary>Importe sin moneda: "1,234.50"</summary>
        public static string Importe(this decimal monto) => monto.ToString(Monto, Cultura);

        /// <summary>Importe con moneda: "B/. 1,234.50"</summary>
        public static string Dinero(this decimal monto) => "B/. " + monto.Importe();
    }
}
```

<a id="kobrauicomponentesformularioconvistascs"></a>

#### Kobra.UI/Componentes/FormularioConVistas.cs

```csharp
namespace Kobra.UI
{
    /// <summary>Base de los módulos que alternan vistas con pestañas (Facturas, Pagos): arma la barra de pestañas, el cuerpo con los márgenes comunes y la disposición de las barras</summary>
    public class FormularioConVistas : FormularioModulo
    {
        private BotonTema[] _pestanas = [];

        private protected void ArmarMarco(BarraTituloModulo titulo, BarraAcciones acciones, BotonTema cerrar,
            BotonTema[] pestanas, params Control[] vistas)
        {
            _pestanas = pestanas;

            var barraVistas = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                BackColor = Tema.ContenedorBajo,
                Padding = new Padding(MargenCuerpo, 6, MargenCuerpo, 6),
                Margin = new Padding(0)
            };
            foreach (var pestana in pestanas)
            {
                pestana.Margin = new Padding(0, 0, 6, 0);
                barraVistas.Controls.Add(pestana);
            }

            var cuerpo = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(MargenCuerpo),
                BackColor = Color.Transparent
            };
            cuerpo.Controls.AddRange(vistas);

            Controls.Add(cuerpo);
            Controls.Add(barraVistas);
            Controls.Add(titulo);
            Controls.Add(acciones);

            CancelButton = cerrar;
        }

        private protected void MarcarPestana(int activa)
        {
            for (var i = 0; i < _pestanas.Length; i++)
                _pestanas[i].Seleccionado = i == activa;
        }
    }
}
```

<a id="kobrauicomponentesformulariomodulocs"></a>

#### Kobra.UI/Componentes/FormularioModulo.cs

```csharp
using Kobra.Negocio;

namespace Kobra.UI
{
    /// <summary>Base de los formularios de módulo (Clientes, Facturas, Pagos): tema, atajos de teclado comunes, y mensajes de error y confirmación con el mismo aspecto</summary>
    public class FormularioModulo : Form
    {
        /// <summary>Márgenes del cuerpo del módulo, iguales a los de las barras de título y de acciones</summary>
        internal const int MargenCuerpo = 12;

        public FormularioModulo()
        {
            AutoScaleMode = AutoScaleMode.None;
            Font = Tema.Normal();
            BackColor = Tema.Superficie;
            KeyPreview = true;
        }

        /// <summary>Título de ventana ("Kobra · Clientes") y tamaño mínimo comunes a todos los módulos</summary>
        protected void ConfigurarVentana(string modulo)
        {
            Text = $"Kobra · {modulo}";
            MinimumSize = new Size(LogicalToDeviceUnits(1020), LogicalToDeviceUnits(680));
            if (ClientSize.Width < MinimumSize.Width || ClientSize.Height < MinimumSize.Height)
                ClientSize = new Size(LogicalToDeviceUnits(1120), LogicalToDeviceUnits(740));
        }

        /// <summary>Atajos del módulo (F2 buscar, F5 actualizar, Ctrl+S guardar, Ctrl+N nuevo...)</summary>
        protected virtual bool ProcesarAtajo(Keys tecla) => false;

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (!e.Handled && ProcesarAtajo(e.KeyData))
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        /// <summary>Muestra un error de negocio (aviso) o de datos (error) con el título del módulo</summary>
        protected void MostrarError(Exception ex)
        {
            var icono = ex is ReglaNegocioException ? MessageBoxIcon.Warning : MessageBoxIcon.Error;
            MessageBox.Show(this, ex.Message, Text, MessageBoxButtons.OK, icono);
        }

        protected void MostrarAdvertencia(string mensaje) =>
            MessageBox.Show(this, mensaje, Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);

        /// <summary>Confirmación temática con "No" como opción por defecto</summary>
        protected bool Confirmar(string pregunta, string? detalle = null, string? advertencia = null,
            bool peligro = false, string textoSi = "&Sí") =>
            DialogoConfirmacion.Confirmar(this, Text, pregunta, detalle, advertencia, peligro, textoSi);
    }
}
```

<a id="kobrauicomponentesitemclientecs"></a>

#### Kobra.UI/Componentes/ItemCliente.cs

```csharp
using Kobra.Entidades;

namespace Kobra.UI
{
    /// <summary>Elemento de un combo de clientes: "CÓDIGO - Nombre" con el cliente completo a mano</summary>
    internal sealed record ItemCliente(int Id, string Texto, Cliente? Cliente = null)
    {
        /// <summary>Llena el combo con los clientes y conserva la selección anterior</summary>
        public static void Llenar(ComboBox combo, IEnumerable<Cliente> clientes, string? opcionVacia = null)
        {
            var seleccionado = (combo.SelectedItem as ItemCliente)?.Id ?? 0;

            var items = new List<ItemCliente>();
            if (opcionVacia is not null)
                items.Add(new ItemCliente(0, opcionVacia));
            items.AddRange(clientes.Select(c => new ItemCliente(c.Id, $"{c.Codigo} - {c.Nombre}", c)));

            combo.DataSource = items;
            combo.DisplayMember = nameof(Texto);
            combo.ValueMember = nameof(Id);
            if (seleccionado != 0)
                combo.SelectedValue = seleccionado;
        }
    }
}
```

<a id="kobrauicomponentesmensajescs"></a>

#### Kobra.UI/Componentes/Mensajes.cs

```csharp
namespace Kobra.UI
{
    /// <summary>Textos que se repiten en varias ventanas</summary>
    internal static class Mensajes
    {
        public const string MotivoObligatorio = "El motivo de anulación es obligatorio.";
        public const string MotivoFaltante = "Debe indicar el motivo de anulación.";

        /// <summary>Pista de teclas de la barra de acciones</summary>
        public static string PistaAtajos(string nuevo) =>
            $"F2 buscar · F5 actualizar · Ctrl+N {nuevo} · Ctrl+S guardar · Esc cerrar";
    }
}
```

<a id="kobrauicomponentestarjetamotivocs"></a>

#### Kobra.UI/Componentes/TarjetaMotivo.cs

```csharp
namespace Kobra.UI
{
    /// <summary>Tarjeta "motivo de anulación": caja de texto obligatoria con contador de caracteres</summary>
    internal sealed class TarjetaMotivo
    {
        public const int Maximo = 250;

        private readonly Label _contador = new()
        {
            TextAlign = ContentAlignment.MiddleRight,
            Font = Tema.Numeros(8.5F),
            ForeColor = Tema.TextoSecundario,
            Dock = DockStyle.Top
        };

        public TarjetaMotivo(int tabIndex)
        {
            Tarjeta = new TarjetaPanel("MOTIVO DE ANULACIÓN (OBLIGATORIO)");
            Caja = new TextBox
            {
                Multiline = true,
                MaxLength = Maximo,
                ScrollBars = ScrollBars.Vertical,
                BorderStyle = BorderStyle.FixedSingle,
                Dock = DockStyle.Top,
                Margin = new Padding(0, 6, 0, 4),
                TabIndex = tabIndex
            };
            Caja.Height = Caja.LogicalToDeviceUnits(74);
            Caja.TextChanged += (_, _) => ActualizarContador();
            Caja.Enter += (_, _) => Enfocado?.Invoke(this, EventArgs.Empty);
            ActualizarContador();

            Tarjeta.MostrarNota("* obligatorio");
            Tarjeta.Cuerpo.RowCount = 2;
            Tarjeta.Cuerpo.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            Tarjeta.Cuerpo.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            Tarjeta.Cuerpo.Controls.Add(Caja, 0, 0);
            Tarjeta.Cuerpo.Controls.Add(_contador, 0, 1);
        }

        public TarjetaPanel Tarjeta { get; }

        public TextBox Caja { get; }

        /// <summary>El usuario entró a la caja de texto (el formulario quita el botón por defecto)</summary>
        public event EventHandler? Enfocado;

        private void ActualizarContador() => _contador.Text = $"{Caja.Text.Length} / {Maximo} caracteres";
    }
}
```

<a id="kobrauicomponentestarjetapanelcs"></a>

#### Kobra.UI/Componentes/TarjetaPanel.cs

```csharp
namespace Kobra.UI
{
    /// <summary>Tarjeta blanca con título subrayado y una nota opcional a la derecha</summary>
    internal sealed class TarjetaPanel : TableLayoutPanel
    {
        private readonly Label _lblNota = new();

        public TarjetaPanel(string titulo)
        {
            Dock = DockStyle.Fill;
            AutoSize = true;
            ColumnCount = 1;
            RowCount = 2;
            BackColor = Tema.Blanco;
            Padding = new Padding(10);
            Margin = new Padding(0);
            ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            RowStyles.Add(new RowStyle(SizeType.AutoSize));
            RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            var cabecera = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                ColumnCount = 2,
                RowCount = 1,
                BackColor = Color.Transparent,
                Margin = new Padding(0),
                Padding = new Padding(0, 0, 0, 4)
            };
            cabecera.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            cabecera.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            cabecera.Paint += (_, e) =>
            {
                using var lapiz = new Pen(Tema.ContenedorAlto);
                e.Graphics.DrawLine(lapiz, 0, cabecera.Height - 1, cabecera.Width, cabecera.Height - 1);
            };

            cabecera.Controls.Add(new Label
            {
                Text = titulo,
                Font = Tema.Negrita(9.5F),
                ForeColor = Tema.Primario,
                AutoSize = true,
                Margin = new Padding(0)
            }, 0, 0);

            _lblNota.Font = Tema.Numeros(8.5F);
            _lblNota.ForeColor = Tema.TextoSecundario;
            _lblNota.AutoSize = true;
            _lblNota.Anchor = AnchorStyles.Right;
            _lblNota.Margin = new Padding(0);
            cabecera.Controls.Add(_lblNota, 1, 0);

            Cuerpo = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                ColumnCount = 1,
                BackColor = Color.Transparent,
                Margin = new Padding(0)
            };
            Cuerpo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            Controls.Add(cabecera, 0, 0);
            Controls.Add(Cuerpo, 0, 1);
        }

        /// <summary>Panel interior (una columna) donde se agrega el contenido de la tarjeta</summary>
        public TableLayoutPanel Cuerpo { get; }

        /// <summary>Texto corto a la derecha del título (conteos, avisos)</summary>
        public void MostrarNota(string texto, bool esError = false)
        {
            _lblNota.ForeColor = esError ? Tema.Secundario : Tema.TextoSecundario;
            _lblNota.Text = texto;
        }
    }
}
```

<a id="kobrauicomponentestemacs"></a>

#### Kobra.UI/Componentes/Tema.cs

```csharp
using System.Drawing;

namespace Kobra.UI
{
    /// <summary>Paleta y fuentes del diseño "Steel Ledger" compartidas por todas las ventanas</summary>
    internal static class Tema
    {
        public static readonly Color Superficie = ColorTranslator.FromHtml("#F8F9FF");
        public static readonly Color ContenedorBajo = ColorTranslator.FromHtml("#EFF4FF");
        public static readonly Color Contenedor = ColorTranslator.FromHtml("#E5EEFE");
        public static readonly Color ContenedorAlto = ColorTranslator.FromHtml("#E0E9F9");
        public static readonly Color Blanco = Color.White;
        /// <summary>Texto sobre fondos oscuros (barras, cabeceras de rejilla y botones rellenos)</summary>
        public static readonly Color SobrePrimario = Color.White;

        public static readonly Color Primario = ColorTranslator.FromHtml("#00375E");
        public static readonly Color PrimarioContenedor = ColorTranslator.FromHtml("#1F4E79");
        public static readonly Color SobrePrimarioContenedor = ColorTranslator.FromHtml("#95BFF1");

        public static readonly Color Secundario = ColorTranslator.FromHtml("#B12A37");
        public static readonly Color SecundarioContenedor = ColorTranslator.FromHtml("#FF656C");
        public static readonly Color TerciarioContenedor = ColorTranslator.FromHtml("#6A4300");

        public static readonly Color ErrorContenedor = ColorTranslator.FromHtml("#FFDAD6");
        public static readonly Color PeligroOscuro = ColorTranslator.FromHtml("#8F0E22");
        public static readonly Color TextoApagado = ColorTranslator.FromHtml("#72777F");

        public static readonly Color Texto = ColorTranslator.FromHtml("#131C27");
        public static readonly Color TextoSecundario = ColorTranslator.FromHtml("#42474F");
        public static readonly Color Borde = ColorTranslator.FromHtml("#C2C7D0");

        private static readonly Dictionary<(string, float, FontStyle), Font> _fuentes = new();

        private static Font Fuente(string familia, float puntos, FontStyle estilo)
        {
            lock (_fuentes)
            {
                if (!_fuentes.TryGetValue((familia, puntos, estilo), out var fuente))
                    _fuentes[(familia, puntos, estilo)] = fuente = new Font(familia, puntos, estilo);
                return fuente;
            }
        }

        public static Font Normal(float puntos = 9F, FontStyle estilo = FontStyle.Regular) =>
            Fuente("Segoe UI", puntos, estilo);

        public static Font Negrita(float puntos = 9F, FontStyle extra = FontStyle.Regular) =>
            Fuente("Segoe UI", puntos, FontStyle.Bold | extra);

        public static Font Numeros(float puntos = 9F, FontStyle estilo = FontStyle.Regular) =>
            Fuente("Consolas", puntos, estilo);
    }
}
```

---
