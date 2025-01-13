using FC.Codeflix.Catalog.Application.UseCases.Category.List;
using FC.Codeflix.Catalog.Infra.Data.EF.Repositories;
using FluentAssertions;

namespace FC.Codeflix.Catalog.IntegrationTests.Application.UseCases.Category.List;

[Collection(nameof(ListCategoriesTestFixture))]
public class ListCategoriesTest
{
    private readonly ListCategoriesTestFixture _fixture;

    public ListCategoriesTest(ListCategoriesTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(SearchRetursListAndTotal))]
    [Trait("Integration/Application", "ListCategories - Use Cases")]
    public async Task SearchRetursListAndTotal()
    {
        // Given
        var dbId = Guid.NewGuid().ToString();
        var context = _fixture.CreateDbContext(dbId: dbId);
        var categories = _fixture.GetExampleCategoriesList();
        await context.AddRangeAsync(categories);
        await context.SaveChangesAsync(CancellationToken.None);
        var repository = new CategoryRepository(context);
        var input = new ListCategoriesInput(1, 20);
        var useCase = new ListCategories(repository);

        // When
        var output = await useCase.Handle(input, CancellationToken.None);

        // Then
        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.Page.Should().Be(input.Page);
        output.PerPage.Should().Be(input.PerPage);
        output.Total.Should().Be(categories.Count);
        output.Items.Should().HaveCount(categories.Count);
        foreach (var outputItem in output.Items)
        {
            var exampleItem = categories.Find(category => category.Id == outputItem.Id);
            exampleItem.Should().NotBeNull();
            outputItem.Name.Should().Be(exampleItem!.Name);
            outputItem.Description.Should().Be(exampleItem.Description);
            outputItem.IsActive.Should().Be(exampleItem.IsActive);
            outputItem.CreatedAt.Should().Be(exampleItem.CreatedAt);
        }
    }

    [Fact(DisplayName = nameof(SearchReturnsEmptyWhenEmpty))]
    [Trait("Integration/Application", "ListCategories - Use Cases")]
    public async Task SearchReturnsEmptyWhenEmpty()
    {
        // Given
        var dbId = Guid.NewGuid().ToString();
        var context = _fixture.CreateDbContext(dbId: dbId);
        var repository = new CategoryRepository(context);
        var input = new ListCategoriesInput(1, 20);
        var useCase = new ListCategories(repository);

        // When
        var output = await useCase.Handle(input, CancellationToken.None);

        // Then
        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.Page.Should().Be(input.Page);
        output.PerPage.Should().Be(input.PerPage);
        output.Total.Should().Be(0);
        output.Items.Should().HaveCount(0);
    }
}