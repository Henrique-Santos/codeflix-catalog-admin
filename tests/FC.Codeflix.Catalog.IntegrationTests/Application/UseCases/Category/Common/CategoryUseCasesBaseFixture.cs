using FC.Codeflix.Catalog.IntegrationTests.Base;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entities;

namespace FC.Codeflix.Catalog.IntegrationTests.Application.UseCases.Category.Common;

public class CategoryUseCasesBaseFixture : BaseFixture
{
    public DomainEntity.Category GetExampleCategory() 
    {
        return new(GetValidCategoryName(), GetValidCategoryDescription(), GetRandomBoolean());
    }

    public List<DomainEntity.Category> GetExampleCategoriesList(int length = 10) 
    {
        return Enumerable.Range(1, length).Select(_ => GetExampleCategory()).ToList();
    }

    public List<DomainEntity.Category> GetExampleCategoriesListWithNames(List<string> names)
    {
        return names.Select(name =>
        {
            var category = GetExampleCategory();
            category.Update(name);
            return category;
        }).ToList();
    } 

    public string GetValidCategoryName()
    {
        var categoryName = "";
        while (categoryName.Length < 3)
        {
            categoryName = Faker.Commerce.Categories(1)[0];
        }
        if (categoryName.Length > 255)
        {
            categoryName = categoryName[..255];
        }
        return categoryName;
    }

    public string GetValidCategoryDescription()
    {
        var categoryDescription = Faker.Commerce.ProductDescription();
        if (categoryDescription.Length > 10_000)
        {
            categoryDescription = categoryDescription[..10_000];
        }
        return categoryDescription;
    }

    public bool GetRandomBoolean() => new Random().NextDouble() < 0.5;
}