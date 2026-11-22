using Kobra.Datos;

namespace Kobra.Negocio;

/// <summary>Estado general de la base de datos, para que la UI no dependa de la capa de datos.</summary>
public static class BaseDatos
{
    /// <summary>Devuelve el mensaje de error si no hay conexión, o null si todo está bien.</summary>
    public static string? ProbarConexion() => Conexion.Probar();

    public static (string Servidor, string BaseDatos) Destino() => Conexion.Destino();
}
