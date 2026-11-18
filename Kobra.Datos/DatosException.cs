using Microsoft.Data.SqlClient;

namespace Kobra.Datos;

/// <summary>
/// Error de acceso a datos con un mensaje que se puede mostrar al usuario tal cual.
/// La capa de datos atrapa SqlException y lanza esta; la UI nunca ve detalles de SQL.
/// </summary>
public class DatosException : Exception
{
    public DatosException(string mensaje) : base(mensaje) { }

    public DatosException(string mensaje, Exception interna) : base(mensaje, interna) { }

    // 2627/2601: índice o restricción UNIQUE. 547: FOREIGN KEY o CHECK.
    public static bool EsDuplicado(SqlException ex) => ex.Number is 2627 or 2601;

    public static DatosException Desde(SqlException ex)
    {
        var mensaje = ex.Number switch
        {
            2627 or 2601 => "Ya existe un registro con esos datos.",
            547 => "La operación no es válida porque incumple una restricción de la base de datos " +
                   "(por ejemplo, el registro está relacionado con otros).",
            // Servidor inaccesible, instancia inexistente, tiempo agotado, base inexistente, login rechazado.
            -2 or -1 or 2 or 53 or 4060 or 18456 or 10060 or 10061 =>
                "No se pudo conectar con la base de datos. Verifique que SQL Server esté en ejecución " +
                "y que la cadena de conexión de App.config sea correcta.",
            _ => "Ocurrió un error al acceder a la base de datos."
        };
        return new DatosException(mensaje, ex);
    }
}
