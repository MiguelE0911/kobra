using System.Data;
using Kobra.Entidades;
using Microsoft.Data.SqlClient;

namespace Kobra.Datos;

/// <summary>
/// Consultas de solo lectura para el estado de cuenta y la antigüedad de saldos.
/// Tanto los importes como los tramos salen de dbo.vw_FacturaSaldo: aquí no se recalcula el saldo.
/// </summary>
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

    /// <summary>Cabecera y facturas del estado de cuenta. Devuelve null si el cliente no existe.</summary>
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

    /// <summary>Facturas con saldo para alimentar los cinco tramos de antigüedad.</summary>
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
