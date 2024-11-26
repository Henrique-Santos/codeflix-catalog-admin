using FC.Codeflix.Catalog.Application.UseCases.Category.Update;
using FC.Codeflix.Catalog.UnitTests.Application.Category.Common;

namespace FC.Codeflix.Catalog.UnitTests.Application.Category.Update;

[CollectionDefinition(nameof(UpdateCategoryTestFixture))]
public class UpdateCategoryTestFixtureCollection : ICollectionFixture<UpdateCategoryTestFixture> { }

public class UpdateCategoryTestFixture : CategoryUseCasesBaseFixture
{
    public UpdateCategoryInput GetValidInput(Guid? id = null)
    {
        return new(id ?? Guid.NewGuid(), GetValidCategoryName(), GetValidCategoryDescription(), GetRandomBoolean());
    }

    public UpdateCategoryInput GetInvalidInputShortName()
    {
        var invalidInputShortName = GetValidCategoryName()[..2];
        return new(Guid.NewGuid(), invalidInputShortName, GetValidCategoryDescription(), GetRandomBoolean());
    }

    public UpdateCategoryInput GetInvalidInputTooLongName()
    {
        var invalidInputTooLongName = LongName();
        return new(Guid.NewGuid(), invalidInputTooLongName, GetValidCategoryDescription(), GetRandomBoolean());

        string LongName() 
        {
            var tooLongNameForCategory = Faker.Commerce.ProductName();
            while (tooLongNameForCategory.Length <= 255)
            {
                tooLongNameForCategory = $"{tooLongNameForCategory} {Faker.Commerce.ProductName()}";
            }
            return tooLongNameForCategory;
        }
    }

    public UpdateCategoryInput GetInvalidInputTooLongDescription()
    {
        var invalidInputTooLongDescription = LongDescription();
        return new(Guid.NewGuid(), GetValidCategoryName(), invalidInputTooLongDescription, GetRandomBoolean());

        string LongDescription() 
        {
            var tooLongDescriptionForCategory = Faker.Commerce.ProductDescription();
            while (tooLongDescriptionForCategory.Length <= 10_000)
            {
                tooLongDescriptionForCategory = $"{tooLongDescriptionForCategory} {Faker.Commerce.ProductDescription()}";
            }
            return tooLongDescriptionForCategory;
        }
    }
}
