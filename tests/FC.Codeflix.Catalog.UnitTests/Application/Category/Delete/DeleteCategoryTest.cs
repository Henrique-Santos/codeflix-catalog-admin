using FC.Codeflix.Catalog.Application.UseCases.Category.Delete;
using FC.Codeflix.Catalog.Domain.Exceptions;
using FluentAssertions;
using Moq;

namespace FC.Codeflix.Catalog.UnitTests.Application.Category.Delete;

[Collection(nameof(DeleteCategoryTestFixture))]
public class DeleteCategoryTest
{
    private readonly DeleteCategoryTestFixture _fixture;

    public DeleteCategoryTest(DeleteCategoryTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(DeleteCategory))]
    [Trait("Application", "DeleteCategory - Use Cases")]
    public async Task DeleteCategory()
    {
        // Given
        var repository = _fixture.GetRepositoryMock();
        var uow = _fixture.GetUnitOfWorkMock();
        var category = _fixture.GetExampleCategory();
        var input = new DeleteCategoryInput(category.Id);
        var useCase = new DeleteCategory(repository.Object, uow.Object);
        repository.Setup(x => x.Get(category.Id, It.IsAny<CancellationToken>())).ReturnsAsync(category);

        // When
        await useCase.Handle(input, CancellationToken.None);

        // Then
        repository.Verify(r => r.Get(category.Id, It.IsAny<CancellationToken>()), Times.Once);
        repository.Verify(r => r.Delete(category, It.IsAny<CancellationToken>()), Times.Once);
        uow.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact(DisplayName = nameof(ThrowWhenCategoryNotFound))]
    [Trait("Application", "DeleteCategory - Use Cases")]
    public async Task ThrowWhenCategoryNotFound()
    {
        // Given
        var repository = _fixture.GetRepositoryMock();
        var uow = _fixture.GetUnitOfWorkMock();
        var categoryId = Guid.NewGuid();
        var input = new DeleteCategoryInput(categoryId);
        var useCase = new DeleteCategory(repository.Object, uow.Object);
        repository
            .Setup(x => x.Get(categoryId, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new NotFoundException($"Category '{categoryId}' not found"));

        // When
        var task = async () => await useCase.Handle(input, CancellationToken.None);

        // Then
        await task.Should().ThrowAsync<NotFoundException>();
        repository.Verify(x => x.Get(categoryId, It.IsAny<CancellationToken>()), Times.Once);
    }
}