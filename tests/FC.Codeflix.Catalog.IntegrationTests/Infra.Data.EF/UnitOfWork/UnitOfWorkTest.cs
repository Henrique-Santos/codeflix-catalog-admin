using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using UowInfra = FC.Codeflix.Catalog.Infra.Data.EF;

namespace FC.Codeflix.Catalog.IntegrationTests.Infra.Data.EF.UnitOfWork;

[Collection(nameof(UnitOfWorkTestFixture))]
public class UnitOfWorkTest
{
    private readonly UnitOfWorkTestFixture _fixture;

    public UnitOfWorkTest(UnitOfWorkTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(Commit))]
    [Trait("Integration/Infra.Data", "UnitOfWork - Persistence")]
    public async void Commit()
    {
        // Given
        var dbId = Guid.NewGuid().ToString();
        var context = _fixture.CreateDbContext(dbId: dbId);
        var categories = _fixture.GetExampleCategoriesList();
        await context.AddRangeAsync(categories);
        var uow = new UowInfra.UnitOfWork(context);

        // When
        await uow.Commit(CancellationToken.None);
        var assertContext = _fixture.CreateDbContext(true);
        var data = assertContext.Categories.AsNoTracking().ToList();

        // Then
        data.Should().HaveCount(categories.Count);
    }

    [Fact(DisplayName = nameof(Rollback))]
    [Trait("Integration/Infra.Data", "UnitOfWork - Persistence")]
    public async void Rollback()
    {
        // Given
        var context = _fixture.CreateDbContext();
        var uow = new UowInfra.UnitOfWork(context);

        // When
        var task = async () => await uow.Rollback(CancellationToken.None);

        // Then
        await task.Should().NotThrowAsync();
    }
}