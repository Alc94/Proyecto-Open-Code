using System.ComponentModel.DataAnnotations;

namespace ZooLogic.Api.Validation;

/// <summary>
/// Valida que un texto pertenezca a uno de los conjuntos cerrados de <see cref="ValoresDominio"/>.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class ValorDeDominioAttribute : ValidationAttribute
{
    private readonly IReadOnlySet<string> _valores;

    public ValorDeDominioAttribute(string nombreConjunto)
    {
        _valores = nombreConjunto switch
        {
            nameof(ValoresDominio.GruposEspecie) => ValoresDominio.GruposEspecie,
            nameof(ValoresDominio.TiposDieta) => ValoresDominio.TiposDieta,
            nameof(ValoresDominio.Sexos) => ValoresDominio.Sexos,
            nameof(ValoresDominio.EstadosAnimal) => ValoresDominio.EstadosAnimal,
            nameof(ValoresDominio.GravedadesAlerta) => ValoresDominio.GravedadesAlerta,
            nameof(ValoresDominio.EstadosAlerta) => ValoresDominio.EstadosAlerta,
            nameof(ValoresDominio.EstadosDieta) => ValoresDominio.EstadosDieta,
            nameof(ValoresDominio.OrigenesDieta) => ValoresDominio.OrigenesDieta,
            _ => throw new ArgumentException(
                $"El conjunto de dominio '{nombreConjunto}' no existe.", nameof(nombreConjunto))
        };
    }

    public override bool IsValid(object? value)
    {
        if (value is null)
        {
            return true;
        }

        return value is string texto && _valores.Contains(texto);
    }

    public override string FormatErrorMessage(string name) =>
        $"El campo {name} debe ser uno de estos valores: {string.Join(", ", _valores)}.";
}
