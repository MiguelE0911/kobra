using System.Configuration;
using Microsoft.Data.SqlClient;

namespace Kobra.Datos;

/// <summary>
/// Punto único para obtener conexiones. La cadena vive en el App.config de Kobra.UI
/// (nombre "Kobra"), nunca en el código.
/// </summary>
public static class Conexion
{
    private const string NombreCadena = "Kobra";

    /// <summary>Crea una conexión SIN abrir. Quien la use debe abrirla tarde y envolverla en using.</summary>
    public static SqlConnection Crear()
    {
        var cadena = ConfigurationManager.ConnectionStrings[NombreCadena]?.ConnectionString;
        if (string.IsNullOrWhiteSpace(cadena))
            throw new DatosException(
                $"No se encontró la cadena de conexión \"{NombreCadena}\" en App.config.");

        return new SqlConnection(cadena);
    }

    /// <summary>Intenta abrir la conexión. Devuelve el mensaje de error, o null si funcionó.</summary>
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
