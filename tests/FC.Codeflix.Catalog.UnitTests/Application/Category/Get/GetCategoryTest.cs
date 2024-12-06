using FC.Codeflix.Catalog.Application.Exceptions;
using FC.Codeflix.Catalog.Application.UseCases.Category.Get;
using FluentAssertions;
using Moq;

namespace FC.Codeflix.Catalog.UnitTests.Application.Category.Get;

[Collection(nameof(GetCategoryTestFixture))]
public class GetCategoryTest
{
    private readonly GetCategoryTestFixture _fixture;

    public GetCategoryTest(GetCategoryTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(GetCategory))]
    [Trait("Application", "GetCategory - Use Cases")]
    public async Task GetCategory()
    {
        // Given
        var repository = _fixture.GetRepositoryMock();
        var category = _fixture.GetExampleCategory();
        repository.Setup(x => x.Get(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(category);
        var input = new GetCategoryInput(category.Id);
        var useCase = new GetCategory(repository.Object);

        // When
        var output = await useCase.Handle(input, CancellationToken.None);

        // Then
        repository.Verify(x => x.Get(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);

        output.Should().NotBeNull();
        output.Name.Should().Be(category.Name);
        output.Description.Should().Be(category.Description);
        output.IsActive.Should().Be(category.IsActive);
        output.Id.Should().Be(category.Id);
        output.CreatedAt.Should().Be(category.CreatedAt);
    }

    [Fact(DisplayName = nameof(NotFoundExceptionWhenCategoryDoesntExist))]
    [Trait("Application", "GetCategory - Use Cases")]
    public async Task NotFoundExceptionWhenCategoryDoesntExist()
    {
        // Given
        var repository = _fixture.GetRepositoryMock();
        var categoryId = Guid.NewGuid();
        var category = _fixture.GetExampleCategory();
        repository.Setup(x => x.Get(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ThrowsAsync(new NotFoundException($"Category '{categoryId}' not found"));
        var input = new GetCategoryInput(category.Id);
        var useCase = new GetCategory(repository.Object);

        // When
        var task = async () => await useCase.Handle(input, CancellationToken.None);

        // Then
        await task.Should().ThrowAsync<NotFoundException>();

        repository.Verify(x => x.Get(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}