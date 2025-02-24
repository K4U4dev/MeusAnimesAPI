namespace Domain.Exceptions;

public class DuplicatedEntityException : Exception
{
    public DuplicatedEntityException(string entity) : base($"Entity {entity} already exists with the same data") { }

    public DuplicatedEntityException(string entity, string value) : base($"Entity {entity} already exists with the same {value}") { }
}