using System.Data;
using Kobra.Entidades;
using Microsoft.Data.SqlClient;

namespace Kobra.Datos;

/// <summary>
/// Acceso a datos de Cliente con ADO.NET. Sin reglas de negocio: solo SQL parametrizado.
/// Este es el patrón a copiar en FacturaDatos y PagoDatos.
/// </summary>
public class ClienteDatos
{
    private const string Columnas =
        "Id, Codigo, Nombre, RucCedula, Telefono, Email, Direccion, LimiteCredito, PlazoDias, Activo, " +
        "FechaCreacion, FechaModificacion";

    /// <summary>Inserta el cliente y devuelve su Id.</summary>
    public int Insertar(Cliente cliente)
    {
        const string sql = @"
            INSERT INTO dbo.Cliente (Codigo, Nombre, RucCedula, Telefono, Email, Direccion, LimiteCredito, PlazoDias, Activo)
            OUTPUT INSERTED.Id
            VALUES (@Codigo, @Nombre, @RucCedula, @Telefono, @Email, @Direccion, @LimiteCredito, @PlazoDias, @Activo);";

        try
        {
            using var cn = Conexion.Crear();
            using var cmd = new SqlCommand(sql, cn);
            AgregarParametros(cmd, cliente);
            cn.Open();
            return (int)cmd.ExecuteScalar()!;
        }
        catch (SqlException ex)
        {
            throw Traducir(ex);
        }
    }

    /// <summary>Actualiza el cliente. Devuelve false si el Id ya no existe.</summary>
    public bool Actualizar(Cliente cliente)
    {
        const string sql = @"
            UPDATE dbo.Cliente
               SET Codigo = @Codigo, Nombre = @Nombre, RucCedula = @RucCedula, Telefono = @Telefono,
                   Email = @Email, Direccion = @Direccion, LimiteCredito = @LimiteCredito,
                   PlazoDias = @PlazoDias, Activo = @Activo, FechaModificacion = SYSDATETIME()
             WHERE Id = @Id;";

        try
        {
            using var cn = Conexion.Crear();
            using var cmd = new SqlCommand(sql, cn);
            AgregarParametros(cmd, cliente);
            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = cliente.Id;
            cn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }
        catch (SqlException ex)
        {
            throw Traducir(ex);
        }
    }

    public bool CambiarEstado(int id, bool activo)
    {
        const string sql =
            "UPDATE dbo.Cliente SET Activo = @Activo, FechaModificacion = SYSDATETIME() WHERE Id = @Id;";

        try
        {
            using var cn = Conexion.Crear();
            using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.Add("@Activo", SqlDbType.Bit).Value = activo;
            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;
            cn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }
        catch (SqlException ex)
        {
            throw Traducir(ex);
        }
    }

    public Cliente? ObtenerPorId(int id)
    {
        var sql = $"SELECT {Columnas} FROM dbo.Cliente WHERE Id = @Id;";

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

    /// <summary>
    /// Busca por código o nombre (contiene). Nunca trae toda la tabla: devuelve como máximo
    /// <paramref name="maximo"/> filas ordenadas por nombre.
    /// </summary>
    public List<Cliente> Buscar(string? texto, bool soloActivos, int maximo = 50)
    {
        var sql = $@"
            SELECT TOP (@Maximo) {Columnas}
              FROM dbo.Cliente
             WHERE (@SoloActivos = 0 OR Activo = 1)
               AND (@Texto IS NULL OR Codigo LIKE @Patron ESCAPE '\' OR Nombre LIKE @Patron ESCAPE '\'
                                   OR RucCedula LIKE @Patron ESCAPE '\')
             ORDER BY Nombre;";

        var texto2 = string.IsNullOrWhiteSpace(texto) ? null : texto.Trim();
        var lista = new List<Cliente>();

        try
        {
            using var cn = Conexion.Crear();
            using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.Add("@Maximo", SqlDbType.Int).Value = maximo;
            cmd.Parameters.Add("@SoloActivos", SqlDbType.Bit).Value = soloActivos;
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

    /// <summary>¿Existe otro cliente con ese código? <paramref name="excluirId"/> es el cliente que se está editando.</summary>
    public bool ExisteCodigo(string codigo, int excluirId = 0)
    {
        const string sql = "SELECT CASE WHEN EXISTS (SELECT 1 FROM dbo.Cliente WHERE Codigo = @Codigo AND Id <> @Id) THEN 1 ELSE 0 END;";
        return EjecutarExiste(sql, cmd =>
        {
            cmd.Parameters.Add("@Codigo", SqlDbType.NVarChar, 20).Value = codigo;
            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = excluirId;
        });
    }

    public bool TieneFacturas(int id)
    {
        const string sql = "SELECT CASE WHEN EXISTS (SELECT 1 FROM dbo.Factura WHERE ClienteId = @Id) THEN 1 ELSE 0 END;";
        return EjecutarExiste(sql, cmd => cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id);
    }

    /// <summary>
    /// Borra el cliente solo si no tiene facturas (la condición va en el mismo DELETE, así no hay
    /// ventana entre comprobar y borrar). Devuelve false si no se borró nada.
    /// </summary>
    public bool Eliminar(int id)
    {
        const string sql = @"
            DELETE FROM dbo.Cliente
             WHERE Id = @Id
               AND NOT EXISTS (SELECT 1 FROM dbo.Factura WHERE ClienteId = @Id);";

        try
        {
            using var cn = Conexion.Crear();
            using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;
            cn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }
        catch (SqlException ex)
        {
            throw Traducir(ex);
        }
    }

    // ---------------------------------------------------------------- privados

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

    private static void AgregarParametros(SqlCommand cmd, Cliente c)
    {
        cmd.Parameters.Add("@Codigo", SqlDbType.NVarChar, 20).Value = c.Codigo;
        cmd.Parameters.Add("@Nombre", SqlDbType.NVarChar, 150).Value = c.Nombre;
        cmd.Parameters.Add("@RucCedula", SqlDbType.NVarChar, 30).Value = (object?)c.RucCedula ?? DBNull.Value;
        cmd.Parameters.Add("@Telefono", SqlDbType.NVarChar, 30).Value = (object?)c.Telefono ?? DBNull.Value;
        cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 150).Value = (object?)c.Email ?? DBNull.Value;
        cmd.Parameters.Add("@Direccion", SqlDbType.NVarChar, 250).Value = (object?)c.Direccion ?? DBNull.Value;

        var limite = cmd.Parameters.Add("@LimiteCredito", SqlDbType.Decimal);
        limite.Precision = 18;
        limite.Scale = 2;
        limite.Value = c.LimiteCredito;

        cmd.Parameters.Add("@PlazoDias", SqlDbType.Int).Value = c.PlazoDias;
        cmd.Parameters.Add("@Activo", SqlDbType.Bit).Value = c.Activo;
    }

    private static Cliente Mapear(SqlDataReader rd) => new()
    {
        Id = rd.GetInt32(rd.GetOrdinal("Id")),
        Codigo = rd.GetString(rd.GetOrdinal("Codigo")),
        Nombre = rd.GetString(rd.GetOrdinal("Nombre")),
        RucCedula = LeerTexto(rd, "RucCedula"),
        Telefono = LeerTexto(rd, "Telefono"),
        Email = LeerTexto(rd, "Email"),
        Direccion = LeerTexto(rd, "Direccion"),
        LimiteCredito = rd.GetDecimal(rd.GetOrdinal("LimiteCredito")),
        PlazoDias = rd.GetInt32(rd.GetOrdinal("PlazoDias")),
        Activo = rd.GetBoolean(rd.GetOrdinal("Activo")),
        FechaCreacion = rd.GetDateTime(rd.GetOrdinal("FechaCreacion")),
        FechaModificacion = rd.IsDBNull(rd.GetOrdinal("FechaModificacion"))
            ? null
            : rd.GetDateTime(rd.GetOrdinal("FechaModificacion"))
    };

    private static string? LeerTexto(SqlDataReader rd, string columna)
    {
        var i = rd.GetOrdinal(columna);
        return rd.IsDBNull(i) ? null : rd.GetString(i);
    }

    /// <summary>Escapa %, _ y [ para que el texto del usuario se busque literalmente.</summary>
    private static string EscaparLike(string texto) =>
        texto.Replace(@"\", @"\\").Replace("%", @"\%").Replace("_", @"\_").Replace("[", @"\[");

    /// <summary>Traduce los duplicados de Cliente a mensajes concretos; el resto, al mensaje genérico.</summary>
    private static DatosException Traducir(SqlException ex)
    {
        if (DatosException.EsDuplicado(ex))
        {
            if (ex.Message.Contains("UQ_Cliente_Codigo", StringComparison.OrdinalIgnoreCase))
                return new DatosException("Ya existe un cliente con ese código.", ex);
            if (ex.Message.Contains("UX_Cliente_RucCedula", StringComparison.OrdinalIgnoreCase))
                return new DatosException("Ya existe un cliente con esa cédula o RUC.", ex);
        }
        return DatosException.Desde(ex);
    }
}
