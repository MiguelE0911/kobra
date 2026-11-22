using System.Data;
using Kobra.Entidades;
using Microsoft.Data.SqlClient;

namespace Kobra.Datos;

/// <summary>
/// Acceso a datos de Factura con ADO.NET. Lee saldos y estados desde dbo.vw_FacturaSaldo.
/// </summary>
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

    // privados

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