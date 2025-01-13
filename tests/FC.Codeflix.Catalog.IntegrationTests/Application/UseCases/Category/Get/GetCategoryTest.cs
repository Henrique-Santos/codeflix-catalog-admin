using FC.Codeflix.Catalog.Application.Exceptions;
using FC.Codeflix.Catalog.Application.UseCases.Category.Get;
using FC.Codeflix.Catalog.Infra.Data.EF.Repositories;
using FluentAssertions;

namespace FC.Codeflix.Catalog.IntegrationTests.Application.UseCases.Category.Get;

[Collection(nameof(GetCategoryTestFixture))]
public class GetCategoryTest 
{
    private readonly GetCategoryTestFixture _fixture;

    public GetCategoryTest(GetCategoryTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(GetCategory))]
    [Trait("Integration/Application", "GetCategory - Use Cases")]
    public async Task GetCategory()
    {
        // Given
        var context = _fixture.CreateDbContext();
        var repository = new CategoryRepository(context);
        var category = _fixture.GetExampleCategory();
        context.Categories.Add(category);
        context.SaveChanges();
        var input = new GetCategoryInput(category.Id);
        var useCase = new GetCategory(repository);

        // When
        var output = await useCase.Handle(input, CancellationToken.None);

        // Then
        output.Should().NotBeNull();
        output.Name.Should().Be(category.Name);
        output.Description.Should().Be(category.Description);
        output.IsActive.Should().Be(category.IsActive);
        output.Id.Should().Be(category.Id);
        output.CreatedAt.Should().Be(category.CreatedAt);
    }

    [Fact(DisplayName = nameof(NotFoundExceptionWhenCategoryDoesntExist))]
    [Trait("Integration/Application", "GetCategory - Use Cases")]
    public async Task NotFoundExceptionWhenCategoryDoesntExist()
    {
        // Given
        var context = _fixture.CreateDbContext();
        var repository = new CategoryRepository(context);
        var category = _fixture.GetExampleCategory();
        var categoryId = Guid.NewGuid();
        var input = new GetCategoryInput(categoryId);
        var useCase = new GetCategory(repository);

        // When
        var task = async () => await useCase.Handle(input, CancellationToken.None);

        // Then
        await task.Should().ThrowAsync<NotFoundException>().WithMessage($"Category '{categoryId}' not found.");
    }
}