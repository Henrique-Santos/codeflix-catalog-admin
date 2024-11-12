using FC.Codeflix.Catalog.Application.UseCases.Category.Common;
using MediatR;

namespace FC.Codeflix.Catalog.Application.UseCases.Category.Create;

public interface ICreateCategory : IRequestHandler<CreateCategoryInput, CategoryOutput>
{
}