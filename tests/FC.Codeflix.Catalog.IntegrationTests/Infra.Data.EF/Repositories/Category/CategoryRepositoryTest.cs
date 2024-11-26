using FC.Codeflix.Catalog.Infra.Data.EF.Repositories;
using FluentAssertions;

namespace FC.Codeflix.Catalog.IntegrationTests.Infra.Data.EF.Repositories.Category;

[Collection(nameof(CategoryRepositoryTestFixture))]
public class CategoryRepositoryTest
{
    private readonly CategoryRepositoryTestFixture _fixture;

    public CategoryRepositoryTest(CategoryRepositoryTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(Insert))]
    [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
    public async Task Insert()
    {
        // Given
        var context = _fixture.CreateDbContext();
        var category = _fixture.GetExampleCategory();
        var repository = new CategoryRepository(context);

        // When
        await repository.Insert(category, CancellationToken.None);
        await context.SaveChangesAsync(CancellationToken.None);
        var data = await context.Categories.FindAsync(category.Id);

        // Then
        data.Should().NotBeNull();
        data!.Name.Should().Be(category.Name);
        data.Description.Should().Be(category.Description);
        data.IsActive.Should().Be(category.IsActive);
        data.CreatedAt.Should().Be(category.CreatedAt);
    }
}