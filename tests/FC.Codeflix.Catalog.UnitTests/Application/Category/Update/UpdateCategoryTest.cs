using Moq;
using FluentAssertions;
using FC.Codeflix.Catalog.Application.UseCases.Category.Update;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entities;
using FC.Codeflix.Catalog.Domain.Exceptions;
using FC.Codeflix.Catalog.Application.Exceptions;

namespace FC.Codeflix.Catalog.UnitTests.Application.Category.Update;

[Collection(nameof(UpdateCategoryTestFixture))]
public class UpdateCategoryTest
{
    private readonly UpdateCategoryTestFixture _fixture;

    public UpdateCategoryTest(UpdateCategoryTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Theory(DisplayName = nameof(UpdateCategory))]
    [Trait("Application", "UpdateCategory - Use Cases")]
    [MemberData(nameof(UpdateCategoryTestDataGenerator.GetCategoriesToUpdate), parameters: 10, MemberType = typeof(UpdateCategoryTestDataGenerator))]
    public async Task UpdateCategory(DomainEntity.Category category, UpdateCategoryInput input)
    {
        // Given
        var repository = _fixture.GetRepositoryMock();
        var uow = _fixture.GetUnitOfWorkMock();
        var useCase = new UpdateCategory(repository.Object,uow.Object);
        repository.Setup(x => x.Get(category.Id, It.IsAny<CancellationToken>())).ReturnsAsync(category);

        // When
        var output = await useCase.Handle(input, CancellationToken.None);

        // Then
        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().Be((bool)input.IsActive!);
        repository.Verify(x => x.Get(category.Id, It.IsAny<CancellationToken>()), Times.Once);
        repository.Verify(x => x.Update(category, It.IsAny<CancellationToken>()), Times.Once);
        uow.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Theory(DisplayName = nameof(UpdateCategoryWithoutProvidingIsActive))]
    [Trait("Application", "UpdateCategory - Use Cases")]
    [MemberData(nameof(UpdateCategoryTestDataGenerator.GetCategoriesToUpdate), parameters: 10, MemberType = typeof(UpdateCategoryTestDataGenerator))]
    public async Task UpdateCategoryWithoutProvidingIsActive(DomainEntity.Category category, UpdateCategoryInput exampleInput)
    {
        // Given
        var input = new UpdateCategoryInput(exampleInput.Id, exampleInput.Name, exampleInput.Description);
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        repositoryMock.Setup(x => x.Get(category.Id, It.IsAny<CancellationToken>())).ReturnsAsync(category);
        var useCase = new UpdateCategory(repositoryMock.Object, unitOfWorkMock.Object);

        // When
        var output = await useCase.Handle(input, CancellationToken.None);

        // Then
        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().Be(category.IsActive);
        repositoryMock.Verify(x => x.Get(category.Id, It.IsAny<CancellationToken>()), Times.Once);
        repositoryMock.Verify(x => x.Update(category, It.IsAny<CancellationToken>()), Times.Once);
        unitOfWorkMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Theory(DisplayName = nameof(UpdateCategoryOnlyName))]
    [Trait("Application", "UpdateCategory - Use Cases")]
    [MemberData(nameof(UpdateCategoryTestDataGenerator.GetCategoriesToUpdate), parameters: 10, MemberType = typeof(UpdateCategoryTestDataGenerator))]
    public async Task UpdateCategoryOnlyName(DomainEntity.Category category, UpdateCategoryInput exampleInput)
    {
        // Given
        var input = new UpdateCategoryInput(exampleInput.Id, exampleInput.Name);
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        repositoryMock.Setup(x => x.Get(category.Id, It.IsAny<CancellationToken>())).ReturnsAsync(category);
        var useCase = new UpdateCategory(repositoryMock.Object, unitOfWorkMock.Object);

        // When
        var output = await useCase.Handle(input, CancellationToken.None);

        // Then
        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(category.Description);
        output.IsActive.Should().Be(category.IsActive);
        repositoryMock.Verify(x => x.Get(category.Id, It.IsAny<CancellationToken>()), Times.Once);
        repositoryMock.Verify(x => x.Update(category, It.IsAny<CancellationToken>()), Times.Once);
        unitOfWorkMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact(DisplayName = nameof(ThrowWhenCategoryNotFound))]
    [Trait("Application", "UpdateCategory - Use Cases")]
    public async Task ThrowWhenCategoryNotFound()
    {
        // Given
        var repository = _fixture.GetRepositoryMock();
        var uow = _fixture.GetUnitOfWorkMock();
        var input = _fixture.GetValidInput();
        var useCase = new UpdateCategory(repository.Object, uow.Object);
        repository
            .Setup(x => x.Get(input.Id, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new NotFoundException($"Category '{input.Id}' not found"));

        // When
        var task = async () => await useCase.Handle(input, CancellationToken.None);

        // Then
        await task.Should().ThrowAsync<NotFoundException>();
        repository.Verify(x => x.Get(input.Id, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Theory(DisplayName = nameof(ThrowWhenCantUpdateCategory))]
    [Trait("Application", "UpdateCategory - Use Cases")]
    [MemberData(nameof(UpdateCategoryTestDataGenerator.GetInvalidInputs), parameters: 12, MemberType = typeof(UpdateCategoryTestDataGenerator))]
    public async Task ThrowWhenCantUpdateCategory(UpdateCategoryInput input, string exceptionMessage)
    {
        // Given
        var exampleCategory = _fixture.GetExampleCategory();
        input.Id = exampleCategory.Id;
        var repository = _fixture.GetRepositoryMock();
        var uow = _fixture.GetUnitOfWorkMock();
        repository.Setup(x => x.Get(exampleCategory.Id, It.IsAny<CancellationToken>())).ReturnsAsync(exampleCategory);
        var useCase = new UpdateCategory(repository.Object, uow.Object);

        // When
        var task = async () => await useCase.Handle(input, CancellationToken.None);

        // Then
        await task.Should().ThrowAsync<EntityValidationException>().WithMessage(exceptionMessage);
        repository.Verify(x => x.Get(exampleCategory.Id, It.IsAny<CancellationToken>()), Times.Once);
    }
}