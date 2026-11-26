namespace ZooLogic.Api.Exceptions;

/// <summary>Se lanza cuando el recurso solicitado no existe. Se traduce en un 404.</summary>
public sealed class RecursoNoEncontradoException(string message) : Exception(message);
