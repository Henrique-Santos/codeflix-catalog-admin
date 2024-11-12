using MediatR;

namespace FC.Codeflix.Catalog.Application.UseCases.Category.Delete;

public interface IDeleteCategory : IRequestHandler<DeleteCategoryInput>
{
}