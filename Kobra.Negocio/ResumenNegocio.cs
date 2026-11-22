using Kobra.Datos;
using Kobra.Entidades;

namespace Kobra.Negocio;

/// <summary>Datos del panel de inicio. Lanza DatosException si la base no responde.</summary>
public class ResumenNegocio
{
    public const int MaximoClientes = 8;

    private readonly ResumenDatos _datos;

    public ResumenNegocio() : this(new ResumenDatos()) { }

    public ResumenNegocio(ResumenDatos datos) => _datos = datos;

    public ResumenCartera Obtener() => _datos.Obtener(MaximoClientes);
}
