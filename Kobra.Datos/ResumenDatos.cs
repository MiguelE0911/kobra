using System.Data;
using Kobra.Entidades;
using Microsoft.Data.SqlClient;

namespace Kobra.Datos;

/// <summary>Consultas agregadas para el panel de inicio. Solo lectura, sin reglas de negocio.</summary>
public class ResumenDatos
{
    // Una sola ida a la base: cartera por tramos, conteo de clientes y los clientes con más saldo.
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
