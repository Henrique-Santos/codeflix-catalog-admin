using FC.Codeflix.Catalog.Application.UseCases.Category.Create;
using FC.Codeflix.Catalog.UnitTests.Application.Category.Common;

namespace FC.Codeflix.Catalog.UnitTests.Application.Category.Create;

[CollectionDefinition(nameof(CreateCategoryTestFixture))]
public class CreateCategoryTestFixtureCollection : ICollectionFixture<CreateCategoryTestFixture> { }

public class CreateCategoryTestFixture : CategoryUseCasesBaseFixture
{
    public CreateCategoryInput GetInvalidInputShortName()
    {
        var invalidInputShortName = GetValidCategoryName()[..2];
        return new(invalidInputShortName, GetValidCategoryDescription(), GetRandomBoolean());
    }

    public CreateCategoryInput GetInvalidInputTooLongName()
    {
        var invalidInputTooLongName = LongName();
        return new(invalidInputTooLongName, GetValidCategoryDescription(), GetRandomBoolean());

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

    public CreateCategoryInput GetInvalidInputCategoryNull()
    {
        return new(GetValidCategoryName(), null, GetRandomBoolean());
    }

    public CreateCategoryInput GetInvalidInputTooLongDescription()
    {
        var invalidInputTooLongDescription = LongDescription();
        return new(GetValidCategoryName(), invalidInputTooLongDescription, GetRandomBoolean());

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