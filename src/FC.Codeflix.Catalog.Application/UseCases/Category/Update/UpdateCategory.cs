using FC.Codeflix.Catalog.Application.Interfaces;
using FC.Codeflix.Catalog.Application.UseCases.Category.Common;
using FC.Codeflix.Catalog.Domain.Repository;

namespace FC.Codeflix.Catalog.Application.UseCases.Category.Update;

public class UpdateCategory : IUpdateCategory
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCategory(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CategoryOutput> Handle(UpdateCategoryInput input, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.Get(input.Id, cancellationToken);

        category.Update(input.Name, input.Description);

        if (input.IsActive != null && input.IsActive != category.IsActive)
        {
            if ((bool)input.IsActive!) 
                category.Activate();
            else 
                category.Deactivate();
        }

        await _categoryRepository.Update(category, cancellationToken);

        await _unitOfWork.Commit(cancellationToken);
        
        return CategoryOutput.FromCategory(category);
    }
}