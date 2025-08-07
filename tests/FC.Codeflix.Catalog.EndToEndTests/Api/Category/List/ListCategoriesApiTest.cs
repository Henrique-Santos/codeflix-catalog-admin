using System.Net;
using FC.Codeflix.Catalog.Application.UseCases.Category.List;
using FluentAssertions;
using Microsoft.AspNetCore.Http;

namespace FC.Codeflix.Catalog.EndToEndTests.Api.Category.List;

[Collection(nameof(ListCategoriesApiTestFixture))]
public class ListCategoriesApiTest : IDisposable
{
    private readonly ListCategoriesApiTestFixture _fixture;

    public ListCategoriesApiTest(ListCategoriesApiTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(ListCategoriesAndTotalByDefault))]
    [Trait("EndToEnd/API", "Category/List - Endpoints")]
    public async Task ListCategoriesAndTotalByDefault()
    {
        // Given
        var categories = _fixture.GetExampleCategoriesList(20);
        await _fixture.Persistence.InsertList(categories);

        // When
        var (response, output) = await _fixture.ApiClient.Get<ListCategoriesOutput>("/categories");

        // Then
        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);

        output.Should().NotBeNull();
        output!.Should().NotBeNull();
        output!.Total.Should().Be(categories.Count);
        output!.Items.Should().HaveCount(15);

        foreach (var item in output.Items)
        {
            var outputItem = categories.FirstOrDefault(p => p.Id == item.Id);
            outputItem.Should().NotBeNull();
            item.Name.Should().Be(outputItem!.Name);
            item.Description.Should().Be(outputItem.Description);
            item.IsActive.Should().Be(outputItem.IsActive);
        }
    }

    [Fact(DisplayName = nameof(ItemsEmptyWhenPersistenceEmpty))]
    [Trait("EndToEnd/API", "Category/List - Endpoints")]
    public async Task ItemsEmptyWhenPersistenceEmpty()
    {
        var (response, output) = await _fixture.ApiClient.Get<ListCategoriesOutput>("/categories");

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
        output.Should().NotBeNull();
        output!.Should().NotBeNull();
        output!.Total.Should().Be(0);
        output.Items.Should().HaveCount(0);
    }

    [Theory(DisplayName = nameof(ListPaginated))]
    [Trait("EndToEnd/API", "Category/List - Endpoints")]
    [InlineData(10, 1, 5, 5)]
    [InlineData(10, 2, 5, 5)]
    [InlineData(7, 2, 5, 2)]
    [InlineData(7, 3, 5, 0)]
    public async Task ListPaginated(int quantityCategoriesToGenerate, int page, int perPage, int expectedQuantityItems)
    {
        var categories = _fixture.GetExampleCategoriesList(quantityCategoriesToGenerate);
        await _fixture.Persistence.InsertList(categories);
        var input = new ListCategoriesInput(page, perPage);

        var (response, output) = await _fixture.ApiClient.Get<ListCategoriesOutput>("/categories", input);

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
        output.Should().NotBeNull();
        output!.Should().NotBeNull();
        output!.Page.Should().Be(input.Page);
        output.PerPage.Should().Be(input.PerPage);
        output.Total.Should().Be(categories.Count);
        output.Items.Should().HaveCount(expectedQuantityItems);
        foreach (var item in output.Items)
        {
            var outputItem = categories.FirstOrDefault(x => x.Id == item.Id);
            outputItem.Should().NotBeNull();
            item.Name.Should().Be(outputItem!.Name);
            item.Description.Should().Be(outputItem.Description);
            item.IsActive.Should().Be(outputItem.IsActive);
        }
    }

    public void Dispose()
    {
        _fixture.DisposeDbContext();
    }
}