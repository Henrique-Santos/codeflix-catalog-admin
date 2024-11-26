using FC.Codeflix.Catalog.Application.UseCases.Category.Common;
using MediatR;

namespace FC.Codeflix.Catalog.Application.UseCases.Category.Update;

public interface IUpdateCategory : IRequestHandler<UpdateCategoryInput, CategoryOutput>;