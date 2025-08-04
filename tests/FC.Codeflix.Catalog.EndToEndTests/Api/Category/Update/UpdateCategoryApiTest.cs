using System.Net;
using FC.Codeflix.Catalog.Application.UseCases.Category.Common;
using FC.Codeflix.Catalog.Application.UseCases.Category.Update;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FC.Codeflix.Catalog.EndToEndTests.Api.Category.Update;

[Collection(nameof(UpdateCategoryApiTestFixture))]
public class UpdateCategoryApiTest
{
    private readonly UpdateCategoryApiTestFixture _fixture;

    public UpdateCategoryApiTest(UpdateCategoryApiTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(UpdateCategory))]
    [Trait("EndToEnd/API", "Category/Update - Endpoints")]
    public async Task UpdateCategory()
    {
        // Given
        var categories = _fixture.GetExampleCategoriesList(20);
        await _fixture.Persistence.InsertList(categories);
        var category = categories[10];
        var input = _fixture.GetExampleInput(category.Id);

        // When
        var (response, output) = await _fixture.ApiClient.Put<CategoryOutput>($"/categories/{category.Id}", input);

        // Then
        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);

        output.Should().NotBeNull();
        output!.Should().NotBeNull();
        output!.Id.Should().Be(category.Id);
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().Be((bool)input.IsActive!);

        var dbCategory = await _fixture.Persistence.GetById(category.Id);

        dbCategory.Should().NotBeNull();
        dbCategory!.Name.Should().Be(input.Name);
        dbCategory.Description.Should().Be(input.Description);
        dbCategory.IsActive.Should().Be((bool)input.IsActive!);
    }

    [Fact(DisplayName = nameof(UpdateCategoryOnlyName))]
    [Trait("EndToEnd/API", "Category/Update - Endpoints")]
    public async void UpdateCategoryOnlyName()
    {
        // Given
        var categories = _fixture.GetExampleCategoriesList(20);
        await _fixture.Persistence.InsertList(categories);
        var category = categories[10];
        var input = new UpdateCategoryInput(category.Id, _fixture.GetValidCategoryName());

        // When
        var (response, output) = await _fixture.ApiClient.Put<CategoryOutput>($"/categories/{category.Id}", input);

        // Then
        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);

        output.Should().NotBeNull();
        output!.Should().NotBeNull();
        output!.Id.Should().Be(category.Id);
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(category.Description);
        output.IsActive.Should().Be(category.IsActive);

        var dbCategory = await _fixture.Persistence.GetById(category.Id);

        dbCategory.Should().NotBeNull();
        dbCategory!.Name.Should().Be(input.Name);
        dbCategory.Description.Should().Be(category.Description);
        dbCategory.IsActive.Should().Be(category.IsActive);
    }

    [Fact(DisplayName = nameof(ErrorWhenNotFound))]
    [Trait("EndToEnd/API", "Category/Update - Endpoints")]
    public async void ErrorWhenNotFound()
    {
        // Given
        var categories = _fixture.GetExampleCategoriesList(20);
        await _fixture.Persistence.InsertList(categories);
        var id = Guid.NewGuid();
        var input = _fixture.GetExampleInput(id);
        input.Id = id; // Alterando o ID para um que não existe

        // When
        var (response, output) = await _fixture.ApiClient.Put<ProblemDetails>($"/categories/{id}", input);

        // Then
        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status404NotFound);

        output.Should().NotBeNull();
        output!.Title.Should().Be("Not Found");
        output.Type.Should().Be("NotFound");
        output.Status.Should().Be(StatusCodes.Status404NotFound);
        output.Detail.Should().Be($"Category '{id}' not found.");
    }
}