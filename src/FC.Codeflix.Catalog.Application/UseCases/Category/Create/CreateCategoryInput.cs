using FC.Codeflix.Catalog.Application.UseCases.Category.Common;
using MediatR;

namespace FC.Codeflix.Catalog.Application.UseCases.Category.Create;

public record CreateCategoryInput(string Name, string Description = "", bool IsActive = true) : IRequest<CategoryOutput>;