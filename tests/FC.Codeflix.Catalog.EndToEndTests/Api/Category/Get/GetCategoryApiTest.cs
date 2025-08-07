using System.Net;
using FC.Codeflix.Catalog.Application.UseCases.Category.Common;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FC.Codeflix.Catalog.EndToEndTests.Api.Category.Get;

[Collection(nameof(GetCategoryApiTestFixture))]
public class GetCategoryApiTest : IDisposable
{
    private readonly GetCategoryApiTestFixture _fixture;

    public GetCategoryApiTest(GetCategoryApiTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(GetCategory))]
    [Trait("EndToEnd/Api", "Category/Get - Endpoints")]
    public async Task GetCategory()
    {
        // Given
        var categories = _fixture.GetExampleCategoriesList(20);
        await _fixture.Persistence.InsertList(categories);
        var category = categories[10];

        // When
        var (response, output) = await _fixture.ApiClient.Get<CategoryOutput>($"/categories/{category.Id}");

        // Then
        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.OK);

        output.Should().NotBeNull();
        output!.Id.Should().Be(category.Id);
        output.Name.Should().Be(category.Name);
        output.Description.Should().Be(category.Description);
        output.IsActive.Should().Be(category.IsActive);
        output.CreatedAt.Should().BeCloseTo(category.CreatedAt, TimeSpan.FromSeconds(1));
    }

    [Fact(DisplayName = nameof(ErrorWhenNotFound))]
    [Trait("EndToEnd/Api", "Category/Get - Endpoints")]
    public async Task ErrorWhenNotFound()
    {
        // Given
        var id = Guid.NewGuid();

        // When
        var (response, output) = await _fixture.ApiClient.Get<ProblemDetails>($"/categories/{id}");

        // Then
        response!.StatusCode.Should().Be(HttpStatusCode.NotFound);

        output.Should().NotBeNull();
        output.Should().NotBeNull();
        output!.Status.Should().Be(StatusCodes.Status404NotFound);
        output.Type.Should().Be("NotFound");
        output.Title.Should().Be("Not Found");
    }
    
    public void Dispose()
    {
        _fixture.DisposeDbContext();
    }
}