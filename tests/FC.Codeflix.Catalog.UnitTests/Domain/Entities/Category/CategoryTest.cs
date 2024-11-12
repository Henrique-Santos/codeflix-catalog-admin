using FC.Codeflix.Catalog.Domain.Exceptions;
using FluentAssertions;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entities;

namespace FC.Codeflix.Catalog.UnitTests.Domain.Entities.Category;

[Collection(nameof(CategoryTestFixture))]
public class CategoryTest
{
    private readonly CategoryTestFixture _fixture;

    public CategoryTest(CategoryTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(Instantiate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Instantiate()
    {
        // Given
        var data = _fixture.GetValidCategory();

        // When
        var beforeDate = DateTime.Now;
        var category = new DomainEntity.Category(data.Name, data.Description);
        var afterDate = DateTime.Now.AddSeconds(1);

        // Then
        category.Should().NotBeNull();
        category.Name.Should().Be(data.Name);
        category.Description.Should().Be(data.Description);
        category.Id.Should().NotBeEmpty();
        category.IsActive.Should().BeTrue();
        category.CreatedAt.Should().NotBeSameDateAs(default);
        (category.CreatedAt >= beforeDate).Should().BeTrue();
        (category.CreatedAt <= afterDate).Should().BeTrue();
    }

    [Theory(DisplayName = nameof(InstantiateWithIsActive))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData(true)]
    [InlineData(false)]
    public void InstantiateWithIsActive(bool isActive)
    {
        // Given
        var data = _fixture.GetValidCategory();

        // When
        var beforeDate = DateTime.Now;
        var category = new DomainEntity.Category(data.Name, data.Description, isActive);
        var afterDate = DateTime.Now.AddSeconds(1);

        // Then
        category.Should().NotBeNull();
        category.Name.Should().Be(data.Name);
        category.Description.Should().Be(data.Description);
        category.Id.Should().NotBeEmpty();
        category.IsActive.Should().Be(isActive);
        category.CreatedAt.Should().NotBeSameDateAs(default);
        (category.CreatedAt >= beforeDate).Should().BeTrue();
        (category.CreatedAt <= afterDate).Should().BeTrue();
    }

    [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsNullOrEmpty))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void InstantiateErrorWhenNameIsNullOrEmpty(string name)
    {
        // Given
        var data = _fixture.GetValidCategory();
        var action = () => new DomainEntity.Category(name, data.Description);

        // When && Then
        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Name is required");
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsNull))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateErrorWhenDescriptionIsNull()
    {
        // Given
        var data = _fixture.GetValidCategory();
        var action = () => new DomainEntity.Category(data.Name, null!);

        // When && Then
        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Description cannot be null");
    }

    [Theory(DisplayName = nameof(InstantiateErrorWhenNameLessThen3Characters))]
    [Trait("Domain", "Category - Aggregates")]
    [MemberData(nameof(GetNamesWithLessThan3Characters), parameters: 10)]
    public void InstantiateErrorWhenNameLessThen3Characters(string name)
    {
        // Given
        var data = _fixture.GetValidCategory();
        var action = () => new DomainEntity.Category(name, data.Description);

        // When && Then
        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Name must be longer than 3 characters");
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenNameIsGreaterThan255Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateErrorWhenNameIsGreaterThan255Characters()
    {
        // Given
        var invalidName = string.Join(null, Enumerable.Range(1, 256).Select(_ => "a").ToArray());
        var action = () => new DomainEntity.Category(invalidName, "category-description");

        // When && Then
        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Name must be less than 255 characters");
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsGreaterThan10_000Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateErrorWhenDescriptionIsGreaterThan10_000Characters()
    {
        // Given
        var data = _fixture.GetValidCategory();
        var invalidDescription = string.Join(null, Enumerable.Range(1, 10_001).Select(_ => "a").ToArray());
        var action = () => new DomainEntity.Category(data.Name, invalidDescription);

        // When && Then
        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Description must be less than 10000 characters");
    }

    [Fact(DisplayName = nameof(Activate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Activate()
    {
        // Given
        var data = _fixture.GetValidCategory();
        var category = new DomainEntity.Category(data.Name, data.Description, !data.IsActive);

        // When
        category.Activate();

        // Then
        category.IsActive.Should().BeTrue();
    }

    [Fact(DisplayName = nameof(Deactivate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Deactivate()
    {
        // Given
        var data = _fixture.GetValidCategory();
        var category = new DomainEntity.Category(data.Name, data.Description, data.IsActive);

        // When
        category.Deactivate();

        // Then
        category.IsActive.Should().BeFalse();
    }

    [Fact(DisplayName = nameof(Update))]
    [Trait("Domain", "Category - Aggregates")]
    public void Update()
    {
        // Given
        var data = _fixture.GetValidCategory();
        var newData = _fixture.GetValidCategory();
        var category = new DomainEntity.Category(data.Name, data.Description);

        // When
        category.Update(newData.Name, newData.Description);

        // Then
        category.Name.Should().Be(newData.Name);
        category.Description.Should().Be(newData.Description);
    }

    [Fact(DisplayName = nameof(UpdateOnlyName))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateOnlyName()
    {
        // Given
        var data = _fixture.GetValidCategory();
        var name = _fixture.GetValidCategoryName();
        var category = new DomainEntity.Category(data.Name, data.Description);

        // When
        category.Update(name);

        // Then
        category.Name.Should().Be(name);
        category.Description.Should().Be(data.Description);
    }

    public static IEnumerable<object[]> GetNamesWithLessThan3Characters(int numberOfTests = 6)
    {
        var fixture = new CategoryTestFixture();
        for (int i = 0; i < numberOfTests; i++)
        { 
            var isOdd = i % 2 == 1;
            yield return new object[] {
                fixture.GetValidCategoryName()[..(isOdd ? 1 : 2)]
            };
        }
    }
}