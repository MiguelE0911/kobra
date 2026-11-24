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

-- Necesario para los índices filtrados. sqlcmd los trae desactivados por defecto.
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
