namespace Kobra.Negocio;

/// <summary>Se incumple una regla de negocio o una validación. El mensaje se muestra al usuario tal cual.</summary>
public class ReglaNegocioException : Exception
{
    public ReglaNegocioException(string mensaje) : base(mensaje) { }

    /// <summary>Nombre del campo con el problema, para que la UI marque el control con ErrorProvider.</summary>
    public string? Campo { get; init; }
}
