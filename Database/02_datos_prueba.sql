/*
  Kobra - Datos de prueba para la demo de Clientes, Facturas, Pagos y reportes

  Ejecutar DESPUÉS de 01_CrearBaseDatos.sql:

      sqlcmd -S .\SQLEXPRESS -E -C -f 65001 -i Database\02_datos_prueba.sql

  Es idempotente: se puede ejecutar varias veces. Primero retira las filas cuyo
  código o número empieza con DEMO (incluidos los pagos hechos a esas facturas
  durante la prueba) y luego las vuelve a crear. No toca los datos que el
  usuario haya registrado con otros códigos.

  Los códigos DEMO001 a DEMO004 se conservan. Las fechas son relativas al día
  de ejecución para que la antigüedad de saldos siga siendo válida en cualquier
  fecha. Los límites de crédito son amplios para permitir registrar facturas y
  pagos nuevos durante la demostración.

  Cobertura de la demo:
    - Cliente activo con factura parcial vencida, corriente y con pago anulado.
    - Cliente con factura pagada y otra con saldo en el tramo de 61 a 90 días.
    - Factura anulada (no suma saldo).
    - Cliente inactivo con saldo pendiente en el tramo de más de 90 días.
    - Los cinco tramos de antigüedad tienen al menos una factura con saldo.
*/

USE Kobra;
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

    /* ------------------------------------------- 2. Clientes DEMO001-004 */
    ;WITH Demo (Codigo, Nombre, RucCedula, Telefono, Email, Direccion, LimiteCredito, PlazoDias, Activo)
    AS
    (
        SELECT N'DEMO001', N'Compañía Demo, S.A.', N'155000001001', N'507-2200-1101',
               N'cobros@demosa.example', N'Ciudad de Panamá', CAST(50000.00 AS DECIMAL(18,2)), 30, CAST(1 AS BIT)
        UNION ALL SELECT N'DEMO002', N'Distribuidora Demo', N'155000002002', N'507-2200-1102',
               N'cartera@distdemo.example', N'Colón', CAST(40000.00 AS DECIMAL(18,2)), 30, CAST(1 AS BIT)
        UNION ALL SELECT N'DEMO003', N'Servicios Demo', N'155000003003', N'507-2200-1103',
               N'facturacion@servdemo.example', N'David', CAST(30000.00 AS DECIMAL(18,2)), 15, CAST(1 AS BIT)
        UNION ALL SELECT N'DEMO004', N'Cliente Inactivo Demo', N'155000004004', N'507-2200-1104',
               N'contacto@demo4.example', N'Chitré', CAST(20000.00 AS DECIMAL(18,2)), 30, CAST(0 AS BIT)
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

    DECLARE @c1 INT = (SELECT Id FROM dbo.Cliente WHERE Codigo = N'DEMO001');
    DECLARE @c2 INT = (SELECT Id FROM dbo.Cliente WHERE Codigo = N'DEMO002');
    DECLARE @c3 INT = (SELECT Id FROM dbo.Cliente WHERE Codigo = N'DEMO003');
    DECLARE @c4 INT = (SELECT Id FROM dbo.Cliente WHERE Codigo = N'DEMO004');

    /* ------------------------------------------------ 3. Ocho facturas demo */
    INSERT INTO dbo.Factura
        (Numero, ClienteId, FechaEmision, FechaVencimiento, Subtotal, Itbms, Total,
         Anulada, MotivoAnulacion, FechaAnulacion)
    SELECT v.Numero, v.ClienteId,
           DATEADD(DAY, v.Emision, Hoy.Fecha),
           DATEADD(DAY, v.Vencimiento, Hoy.Fecha),
           v.Subtotal, v.Itbms, v.Total, v.Anulada, v.Motivo, v.FechaAnulacion
      FROM (VALUES
          (N'F-DEMO-001', @c1, -40, -10, CAST(1121.50 AS DECIMAL(18,2)), CAST(78.50 AS DECIMAL(18,2)), CAST(1200.00 AS DECIMAL(18,2)), CAST(0 AS BIT), CAST(NULL AS NVARCHAR(250)), CAST(NULL AS DATETIME2(0))),
          (N'F-DEMO-002', @c1,  -5,  25, CAST(2336.45 AS DECIMAL(18,2)), CAST(163.55 AS DECIMAL(18,2)), CAST(2500.00 AS DECIMAL(18,2)), CAST(0 AS BIT), CAST(NULL AS NVARCHAR(250)), CAST(NULL AS DATETIME2(0))),
          (N'F-DEMO-003', @c1, -70, -40, CAST(2803.74 AS DECIMAL(18,2)), CAST(196.26 AS DECIMAL(18,2)), CAST(3000.00 AS DECIMAL(18,2)), CAST(0 AS BIT), CAST(NULL AS NVARCHAR(250)), CAST(NULL AS DATETIME2(0))),
          (N'F-DEMO-004', @c2,-100, -70, CAST(1682.24 AS DECIMAL(18,2)), CAST(117.76 AS DECIMAL(18,2)), CAST(1800.00 AS DECIMAL(18,2)), CAST(0 AS BIT), CAST(NULL AS NVARCHAR(250)), CAST(NULL AS DATETIME2(0))),
          (N'F-DEMO-005', @c2,-105, -75, CAST(4205.61 AS DECIMAL(18,2)), CAST(294.39 AS DECIMAL(18,2)), CAST(4500.00 AS DECIMAL(18,2)), CAST(0 AS BIT), CAST(NULL AS NVARCHAR(250)), CAST(NULL AS DATETIME2(0))),
          (N'F-DEMO-006', @c3, -20,  10, CAST(887.85 AS DECIMAL(18,2)),  CAST(62.15 AS DECIMAL(18,2)),  CAST(950.00 AS DECIMAL(18,2)),  CAST(0 AS BIT), CAST(NULL AS NVARCHAR(250)), CAST(NULL AS DATETIME2(0))),
          (N'F-DEMO-007', @c4,-150,-120, CAST(2056.07 AS DECIMAL(18,2)), CAST(143.93 AS DECIMAL(18,2)), CAST(2200.00 AS DECIMAL(18,2)), CAST(0 AS BIT), CAST(NULL AS NVARCHAR(250)), CAST(NULL AS DATETIME2(0))),
          (N'F-DEMO-008', @c3, -60, -30, CAST(1028.04 AS DECIMAL(18,2)), CAST(71.96 AS DECIMAL(18,2)),  CAST(1100.00 AS DECIMAL(18,2)), CAST(1 AS BIT), N'Factura de prueba anulada', DATEADD(DAY, -29, CAST(GETDATE() AS DATETIME2(0))))
      ) AS v (Numero, ClienteId, Emision, Vencimiento, Subtotal, Itbms, Total, Anulada, Motivo, FechaAnulacion)
    CROSS JOIN (SELECT CAST(GETDATE() AS DATE) AS Fecha) AS Hoy;

    DECLARE @f1 INT = (SELECT Id FROM dbo.Factura WHERE Numero = N'F-DEMO-001');
    DECLARE @f3 INT = (SELECT Id FROM dbo.Factura WHERE Numero = N'F-DEMO-003');
    DECLARE @f4 INT = (SELECT Id FROM dbo.Factura WHERE Numero = N'F-DEMO-004');
    DECLARE @f5 INT = (SELECT Id FROM dbo.Factura WHERE Numero = N'F-DEMO-005');

    /* --------------------------------------------- 4. Pagos activos/anulados */
    INSERT INTO dbo.Pago
        (FacturaId, FechaPago, Monto, Anulado, MotivoAnulacion, FechaAnulacion)
    SELECT v.FacturaId, DATEADD(DAY, v.Dias, Hoy.Fecha), v.Monto, v.Anulado, v.Motivo, v.FechaAnulacion
      FROM (VALUES
          (@f1, -8,   CAST(400.00 AS DECIMAL(18,2)), CAST(0 AS BIT), CAST(NULL AS NVARCHAR(250)), CAST(NULL AS DATETIME2(0))),
          (@f1, -6,   CAST(200.00 AS DECIMAL(18,2)), CAST(1 AS BIT), N'Depósito rechazado por el banco', DATEADD(DAY, -5, CAST(GETDATE() AS DATETIME2(0)))),
          (@f3, -35,  CAST(1000.00 AS DECIMAL(18,2)), CAST(0 AS BIT), CAST(NULL AS NVARCHAR(250)), CAST(NULL AS DATETIME2(0))),
          (@f3, -30,  CAST(500.00 AS DECIMAL(18,2)), CAST(1 AS BIT), N'Pago duplicado registrado por el cliente', DATEADD(DAY, -28, CAST(GETDATE() AS DATETIME2(0)))),
          (@f4, -65,  CAST(1800.00 AS DECIMAL(18,2)), CAST(0 AS BIT), CAST(NULL AS NVARCHAR(250)), CAST(NULL AS DATETIME2(0))),
          (@f5, -100, CAST(1500.00 AS DECIMAL(18,2)), CAST(0 AS BIT), CAST(NULL AS NVARCHAR(250)), CAST(NULL AS DATETIME2(0)))
      ) AS v (FacturaId, Dias, Monto, Anulado, Motivo, FechaAnulacion)
    CROSS JOIN (SELECT CAST(GETDATE() AS DATE) AS Fecha) AS Hoy;

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
SELECT c.Codigo, c.Nombre, c.Activo,
       COUNT(v.Id) AS Facturas,
       ISNULL(SUM(v.Total), 0) AS Facturado,
       ISNULL(SUM(v.TotalPagado), 0) AS Cobrado,
       ISNULL(SUM(v.Saldo), 0) AS Saldo
  FROM dbo.Cliente AS c
  LEFT JOIN dbo.vw_FacturaSaldo AS v ON v.ClienteId = c.Id
 WHERE c.Codigo LIKE N'DEMO%'
 GROUP BY c.Codigo, c.Nombre, c.Activo
 ORDER BY c.Codigo;

SELECT
    ISNULL(SUM(CASE WHEN DiasVencidos = 0 THEN Saldo END), 0) AS Corriente,
    ISNULL(SUM(CASE WHEN DiasVencidos BETWEEN 1 AND 30 THEN Saldo END), 0) AS Tramo1a30,
    ISNULL(SUM(CASE WHEN DiasVencidos BETWEEN 31 AND 60 THEN Saldo END), 0) AS Tramo31a60,
    ISNULL(SUM(CASE WHEN DiasVencidos BETWEEN 61 AND 90 THEN Saldo END), 0) AS Tramo61a90,
    ISNULL(SUM(CASE WHEN DiasVencidos > 90 THEN Saldo END), 0) AS TramoMasDe90,
    ISNULL(SUM(Saldo), 0) AS SaldoTotal
  FROM dbo.vw_FacturaSaldo
 WHERE Saldo > 0;
GO
