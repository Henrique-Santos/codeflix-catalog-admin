using FC.Codeflix.Catalog.Application.UseCases.Category.Common;
using MediatR;

namespace FC.Codeflix.Catalog.Application.UseCases.Category.Get;

public interface IGetCategory : IRequestHandler<GetCategoryInput, CategoryOutput>
{
}