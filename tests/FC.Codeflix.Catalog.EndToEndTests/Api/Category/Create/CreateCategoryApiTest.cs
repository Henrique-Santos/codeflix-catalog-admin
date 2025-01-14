using System.Net;
using FC.Codeflix.Catalog.Application.UseCases.Category.Common;
using FC.Codeflix.Catalog.Application.UseCases.Category.Create;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace FC.Codeflix.Catalog.EndToEndTests.Api.Category.Create;

[Collection(nameof(CreateCategoryApiTestFixture))]
public class CreateCategoryApiTest
{
    private readonly CreateCategoryApiTestFixture _fixture;

    public CreateCategoryApiTest(CreateCategoryApiTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(CreateCategory))]
    [Trait("EndToEnd/Api", "Category - Endpoints")]
    public async Task CreateCategory()
    {
        // Given
        var input = _fixture.GetExampleInput();

        // When
        var (response, output) = await _fixture.ApiClient.Post<CategoryOutput>("/categories", input);

        // Then
        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.Created);

        var category = await _fixture.Persistence.GetById(output!.Id);
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

    [Theory(DisplayName = nameof(ThrowWhenCantInstantiateAggregate))]
    [Trait("EndToEnd/Api", "Category - Endpoints")]
    [MemberData(nameof(CreateCategoryTestDataGenerator.GetInvalidInputs), MemberType = typeof(CreateCategoryTestDataGenerator))]
    public async Task ThrowWhenCantInstantiateAggregate(CreateCategoryInput input, string errorMessage)
    {
        // Given & When
        var (response, problemDetails) = await _fixture.ApiClient.Post<ProblemDetails>("/categories", input);

        // Then
        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);

        problemDetails.Should().NotBeNull();
        problemDetails!.Title.Should().Be("One or more validation errors occurred.");
        problemDetails.Type.Should().Be("UnprocessableEntity");
        problemDetails.Status.Should().Be((int)HttpStatusCode.UnprocessableEntity);
        problemDetails.Detail.Should().Be(errorMessage);
    }
}