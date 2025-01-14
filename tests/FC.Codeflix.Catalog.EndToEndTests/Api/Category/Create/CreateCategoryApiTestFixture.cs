using FC.Codeflix.Catalog.Application.UseCases.Category.Create;
using FC.Codeflix.Catalog.EndToEndTests.Api.Category.Common;

namespace FC.Codeflix.Catalog.EndToEndTests.Api.Category.Create;

[CollectionDefinition(nameof(CreateCategoryApiTestFixture))]
public class CreateCategoryApiTestFixtureCollection : ICollectionFixture<CreateCategoryApiTestFixture>
{
}

public class CreateCategoryApiTestFixture : CategoryBaseFixture
{
    public CreateCategoryInput GetExampleInput()
    {
        return new(GetValidCategoryName(), GetValidCategoryDescription(), GetRandomBoolean());
    }

    public CreateCategoryInput GetExampleInputWithName(string name)
    {
        return new(name, GetValidCategoryDescription(), GetRandomBoolean());
    }

    public CreateCategoryInput GetExampleInputWithDescription(string description)
    {
        return new(GetValidCategoryName(), description, GetRandomBoolean());
    }
}