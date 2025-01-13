using FC.Codeflix.Catalog.Application.UseCases.Category.Create;
using FC.Codeflix.Catalog.Infra.Data.EF;
using FC.Codeflix.Catalog.Infra.Data.EF.Repositories;
using FluentAssertions;

namespace FC.Codeflix.Catalog.IntegrationTests.Application.UseCases.Category.Create;

[Collection(nameof(CreateCategoryTestFixture))]
public class CreateCategoryTest 
{
    private readonly CreateCategoryTestFixture _fixture;

    public CreateCategoryTest(CreateCategoryTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(CreateCategory))]
    [Trait("Integration/Application", "CreateCategory - Use Cases")]
    public async void CreateCategory()
    {
        // Given
        var context = _fixture.CreateDbContext();
        var repository = new CategoryRepository(context);
        var uow = new UnitOfWork(context);
        var usecase = new CreateCategory(repository, uow);
        var input = _fixture.GetInput();

        // When
        var output = await usecase.Handle(input, CancellationToken.None);
        var category = await _fixture.CreateDbContext(true).Categories.FindAsync(output.Id);

        // Then
        category.Should().NotBeNull();
        category!.Name.Should().Be(input.Name);
        category.Description.Should().Be(input.Description);
        category.IsActive.Should().Be(input.IsActive);
        category.Id.Should().NotBeEmpty();
        category.CreatedAt.Should().NotBeSameDateAs(default);

        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().Be(input.IsActive);
        output.Id.Should().NotBeEmpty();
        output.CreatedAt.Should().NotBeSameDateAs(default);
    }
}