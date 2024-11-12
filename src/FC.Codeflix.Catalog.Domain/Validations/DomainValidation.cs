using FC.Codeflix.Catalog.Domain.Exceptions;

namespace FC.Codeflix.Catalog.Domain.Validations;

public static class DomainValidation
{
    public static void NotNull(object? target, string fieldName)
    {
        if (target is null) throw new EntityValidationException($"{fieldName} cannot be null");
    }
    
    public static void NotNullOrEmpty(string? target, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(target)) throw new EntityValidationException($"{fieldName} is required");
    }

    public static void MinLength(string target, int minLength, string fieldName)
    {
        if (target.Length < minLength) throw new EntityValidationException($"{fieldName} must be longer than {minLength} characters");
    }

    public static void MaxLength(string target, int maxLength, string fieldName)
    {
        if (target.Length > maxLength) throw new EntityValidationException($"{fieldName} must be less than {maxLength} characters");
    }
}