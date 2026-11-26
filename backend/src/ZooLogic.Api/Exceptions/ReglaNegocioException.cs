namespace ZooLogic.Api.Exceptions;

/// <summary>
/// Se lanza cuando una operación válida a nivel de formato incumple una regla de negocio.
/// El mensaje debe ser seguro para mostrarlo al cliente. Se traduce en un 409.
/// </summary>
public sealed class ReglaNegocioException(string message) : Exception(message);
