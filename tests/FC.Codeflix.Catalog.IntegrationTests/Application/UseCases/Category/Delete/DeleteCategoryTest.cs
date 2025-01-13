using FC.Codeflix.Catalog.Application.UseCases.Category.Delete;
using FC.Codeflix.Catalog.Infra.Data.EF;
using FC.Codeflix.Catalog.Infra.Data.EF.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace FC.Codeflix.Catalog.IntegrationTests.Application.UseCases.Category.Delete;

[Collection(nameof(DeleteCategoryTestFixture))]
public class DeleteCategoryTest
{
    private readonly DeleteCategoryTestFixture _fixture;

    public DeleteCategoryTest(DeleteCategoryTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(DeleteCategory))]
    [Trait("Infrastructure/Application", "DeleteCategory - Use Cases")]
    public async Task DeleteCategory()
    {
        // Given
        var dbId = Guid.NewGuid().ToString();
        var context = _fixture.CreateDbContext(dbId: dbId);
        var repository = new CategoryRepository(context);
        var uow = new UnitOfWork(context);
        var category = _fixture.GetExampleCategory();
        var categories = _fixture.GetExampleCategoriesList();
        await context.AddRangeAsync(categories);
        var tracking = await context.AddAsync(category);
        await context.SaveChangesAsync();
        tracking.State = EntityState.Detached;
        var input = new DeleteCategoryInput(category.Id);
        var useCase = new DeleteCategory(repository, uow);

        // When
        await useCase.Handle(input, CancellationToken.None);
        var categoryDeleted = await _fixture.CreateDbContext(true).Categories.FindAsync(category.Id);
        var data = await _fixture.CreateDbContext(true, dbId).Categories.ToListAsync();

        // Then
        categoryDeleted.Should().BeNull();
        data.Should().HaveCount(categories.Count);
    }

}