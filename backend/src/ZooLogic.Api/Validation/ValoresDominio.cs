namespace ZooLogic.Api.Validation;

/// <summary>
/// Conjuntos cerrados de valores del dominio, con el texto exacto que se guarda en la base de datos.
/// </summary>
public static class ValoresDominio
{
    public static readonly IReadOnlySet<string> GruposEspecie =
        Crear("Mamifero", "Ave", "Reptil", "Anfibio", "Pez");

    public static readonly IReadOnlySet<string> TiposDieta =
        Crear("Carnivoro", "Herbivoro", "Omnivoro");

    public static readonly IReadOnlySet<string> Sexos =
        Crear("Macho", "Hembra");

    public static readonly IReadOnlySet<string> EstadosAnimal =
        Crear("Activo", "EnObservacion", "EnTratamiento", "Cuarentena");

    public static readonly IReadOnlySet<string> GravedadesAlerta =
        Crear("Baja", "Media", "Alta", "Critica");

    public static readonly IReadOnlySet<string> EstadosAlerta =
        Crear("Abierta", "Revisada", "Cerrada");

    public static readonly IReadOnlySet<string> EstadosDieta =
        Crear("Propuesta", "Validada", "Descartada");

    public static readonly IReadOnlySet<string> OrigenesDieta =
        Crear("Manual", "IA simulada");

    private static IReadOnlySet<string> Crear(params string[] valores) =>
        new HashSet<string>(valores, StringComparer.Ordinal);
}
