using MediatR;

namespace FC.Codeflix.Catalog.Application.UseCases.Category.Delete;

public record DeleteCategoryInput(Guid Id) : IRequest;