
using FC.Codeflix.Catalog.Application.UseCases.Category.Common;
using FC.Codeflix.Catalog.Domain.Repository;

namespace FC.Codeflix.Catalog.Application.UseCases.Category.List;

public class ListCategories : IListCategories
{
    private readonly ICategoryRepository _categoryRepository;

    public ListCategories(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<ListCategoriesOutput> Handle(ListCategoriesInput request, CancellationToken cancellationToken)
    {
        var searchOutput = await _categoryRepository
            .Search(new(request.Page, request.PerPage, request.Search, request.Sort, request.Dir), cancellationToken);

        var items = searchOutput.Items
            .Select(CategoryOutput.FromCategory)
            .ToList();

        return new ListCategoriesOutput(searchOutput.CurrentPage, searchOutput.PerPage, searchOutput.Total, items);
    }
}