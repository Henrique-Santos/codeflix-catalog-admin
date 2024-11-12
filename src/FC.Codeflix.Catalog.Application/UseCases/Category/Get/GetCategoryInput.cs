using FC.Codeflix.Catalog.Application.UseCases.Category.Common;
using MediatR;

namespace FC.Codeflix.Catalog.Application.UseCases.Category.Get;

public record GetCategoryInput(Guid Id) : IRequest<CategoryOutput>;