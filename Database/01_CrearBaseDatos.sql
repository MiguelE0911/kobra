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

  Reglas de diseño:
    - El saldo y el estado de una factura NO se guardan: se calculan en vw_FacturaSaldo
      a partir del total y los pagos no anulados.
    - Los documentos no se borran: se anulan con motivo y fecha.
    - Dinero en DECIMAL(18,2).
*/

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

    -- Búsqueda por nombre. El código ya tiene índice por su restricción UNIQUE.
    CREATE NONCLUSTERED INDEX IX_Cliente_Nombre ON dbo.Cliente (Nombre);

    -- La cédula/RUC es única solo cuando se informa.
    CREATE UNIQUE NONCLUSTERED INDEX UX_Cliente_RucCedula
        ON dbo.Cliente (RucCedula) WHERE RucCedula IS NOT NULL;
END
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

    CREATE NONCLUSTERED INDEX IX_Factura_ClienteId ON dbo.Factura (ClienteId);
    CREATE NONCLUSTERED INDEX IX_Factura_FechaVencimiento ON dbo.Factura (FechaVencimiento);
END
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

    CREATE NONCLUSTERED INDEX IX_Pago_FacturaId ON dbo.Pago (FacturaId);
END
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
