using System.Data;
using Kobra.Entidades;
using Microsoft.Data.SqlClient;

namespace Kobra.Datos;

/// <summary>
/// Acceso a datos de Pago con ADO.NET. El saldo se lee siempre de dbo.vw_FacturaSaldo.
/// La inserción bloquea la factura con UPDLOCK/HOLDLOCK dentro de una transacción para que dos
/// pagos concurrentes no puedan superar juntos el saldo disponible.
/// </summary>
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

    /// <summary>
    /// Inserta un pago de forma atómica. Devuelve el Id generado, o null si la factura ya no existe,
    /// está anulada o el monto supera el saldo en el momento de la inserción.
    /// </summary>
    public int? Insertar(Pago pago)
    {
        // UPDLOCK + HOLDLOCK: se mantiene el bloqueo sobre la factura hasta que termine la transacción.
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

                // Comprobación defensiva: PagoNegocio ya validó, pero aquí se repite dentro de la
                // transacción para cerrar la ventana entre la validación y la inserción.
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

    /// <summary>Anulación lógica: la condición se repite en el UPDATE por si el pago cambió antes.</summary>
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

    /// <summary>Busca pagos por número de pago, número de factura o cliente.</summary>
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

    /// <summary>Saldo actual de una factura, tomado directamente de la vista.</summary>
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

    // privados

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
