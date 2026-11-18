using System.Net.Mail;
using Kobra.Datos;
using Kobra.Entidades;

namespace Kobra.Negocio;

/// <summary>
/// Validaciones y reglas de Cliente. No contiene SQL (eso es ClienteDatos) ni controles de UI.
/// Este es el patrón a copiar en FacturaNegocio y PagoNegocio.
/// </summary>
public class ClienteNegocio
{
    public const int MaximoResultados = 50;

    private readonly ClienteDatos _datos;

    public ClienteNegocio() : this(new ClienteDatos()) { }

    public ClienteNegocio(ClienteDatos datos) => _datos = datos;

    public int Registrar(Cliente cliente)
    {
        Normalizar(cliente);
        Validar(cliente);
        cliente.Activo = true;
        return _datos.Insertar(cliente);
    }

    public void Actualizar(Cliente cliente)
    {
        if (cliente.Id <= 0)
            throw new ReglaNegocioException("Seleccione un cliente para actualizar.");

        Normalizar(cliente);
        Validar(cliente);
        if (!_datos.Actualizar(cliente))
            throw new ReglaNegocioException("El cliente ya no existe. Actualice la lista.");
    }

    public void Desactivar(int id) => CambiarEstado(id, activo: false);

    public void Reactivar(int id) => CambiarEstado(id, activo: true);

    /// <summary>Un cliente con facturas no se elimina: se desactiva.</summary>
    public void Eliminar(int id)
    {
        if (_datos.TieneFacturas(id))
            throw new ReglaNegocioException(
                "El cliente tiene facturas y no se puede eliminar. Puede desactivarlo para que no se le facture más.");

        // El DELETE vuelve a comprobarlo: si alguien le creó una factura justo ahora, no borra nada.
        if (!_datos.Eliminar(id))
            throw new ReglaNegocioException(
                "No se pudo eliminar el cliente: ya no existe o se le acaba de registrar una factura.");
    }

    public Cliente? ObtenerPorId(int id) => _datos.ObtenerPorId(id);

    public List<Cliente> Buscar(string? texto, bool soloActivos) =>
        _datos.Buscar(texto, soloActivos, MaximoResultados);

    // ---------------------------------------------------------------- privados

    private void CambiarEstado(int id, bool activo)
    {
        if (id <= 0)
            throw new ReglaNegocioException("Seleccione un cliente.");
        if (!_datos.CambiarEstado(id, activo))
            throw new ReglaNegocioException("El cliente ya no existe. Actualice la lista.");
    }

    private static void Normalizar(Cliente c)
    {
        c.Codigo = (c.Codigo ?? string.Empty).Trim();
        c.Nombre = (c.Nombre ?? string.Empty).Trim();
        c.RucCedula = VacioANulo(c.RucCedula);
        c.Telefono = VacioANulo(c.Telefono);
        c.Email = VacioANulo(c.Email);
        c.Direccion = VacioANulo(c.Direccion);
    }

    private void Validar(Cliente c)
    {
        if (c.Codigo.Length == 0)
            throw Error("El código es obligatorio.", nameof(Cliente.Codigo));
        if (c.Codigo.Length > 20)
            throw Error("El código no puede tener más de 20 caracteres.", nameof(Cliente.Codigo));
        if (c.Nombre.Length == 0)
            throw Error("El nombre es obligatorio.", nameof(Cliente.Nombre));
        if (c.Nombre.Length > 150)
            throw Error("El nombre no puede tener más de 150 caracteres.", nameof(Cliente.Nombre));
        if (c.RucCedula is { Length: > 30 })
            throw Error("La cédula o RUC no puede tener más de 30 caracteres.", nameof(Cliente.RucCedula));
        if (c.Telefono is { Length: > 30 })
            throw Error("El teléfono no puede tener más de 30 caracteres.", nameof(Cliente.Telefono));
        if (c.Direccion is { Length: > 250 })
            throw Error("La dirección no puede tener más de 250 caracteres.", nameof(Cliente.Direccion));
        if (c.Email is not null && !EmailValido(c.Email))
            throw Error("El correo electrónico no tiene un formato válido.", nameof(Cliente.Email));
        if (c.LimiteCredito < 0)
            throw Error("El límite de crédito no puede ser negativo.", nameof(Cliente.LimiteCredito));
        if (c.PlazoDias < 0)
            throw Error("El plazo en días no puede ser negativo.", nameof(Cliente.PlazoDias));

        if (_datos.ExisteCodigo(c.Codigo, c.Id))
            throw Error("Ya existe otro cliente con ese código.", nameof(Cliente.Codigo));
    }

    private static ReglaNegocioException Error(string mensaje, string campo) =>
        new(mensaje) { Campo = campo };

    private static string? VacioANulo(string? texto)
    {
        var t = texto?.Trim();
        return string.IsNullOrEmpty(t) ? null : t;
    }

    /// <summary>Compartido con la UI para validar al salir del campo con la misma regla.</summary>
    public static bool EmailValido(string email)
    {
        if (email.Length > 150 || !MailAddress.TryCreate(email, out var direccion))
            return false;
        return direccion.Address == email && direccion.Host.Contains('.');
    }
}
