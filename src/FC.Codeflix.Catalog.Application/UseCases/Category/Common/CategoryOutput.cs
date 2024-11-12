using DomainEntity = FC.Codeflix.Catalog.Domain.Entities;

namespace FC.Codeflix.Catalog.Application.UseCases.Category.Common;

public record CategoryOutput(Guid Id, string Name, string Description, bool IsActive, DateTime CreatedAt)
{
    public static CategoryOutput FromCategory(DomainEntity.Category category)
    {
        return new CategoryOutput(category.Id, category.Name, category.Description, category.IsActive, category.CreatedAt);
    }
}