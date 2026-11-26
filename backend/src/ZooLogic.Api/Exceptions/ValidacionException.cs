namespace ZooLogic.Api.Exceptions;

/// <summary>Error de validación de negocio que debe devolverse como 400 con errores por campo.</summary>
public sealed class ValidacionException : Exception
{
    public ValidacionException(IDictionary<string, string[]> errores)
        : base("La petición contiene datos no válidos.")
    {
        Errores = errores;
    }

    public ValidacionException(string campo, string mensaje)
        : this(new Dictionary<string, string[]> { [campo] = [mensaje] })
    {
    }

    /// <summary>Errores agrupados por nombre de campo.</summary>
    public IDictionary<string, string[]> Errores { get; }
}
