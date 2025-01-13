using FC.Codeflix.Catalog.Application.UseCases.Category.Update;
using FC.Codeflix.Catalog.Infra.Data.EF;
using FC.Codeflix.Catalog.Infra.Data.EF.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entities;

namespace FC.Codeflix.Catalog.IntegrationTests.Application.UseCases.Category.Update;


[Collection(nameof(UpdateCategoryTestFixture))]
public class UpdateCategoryTest
{
    private readonly UpdateCategoryTestFixture _fixture;

    public UpdateCategoryTest(UpdateCategoryTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Theory(DisplayName = nameof(UpdateCategory))]
    [Trait("Infrastructure/Application", "UpdateCategory - Use Cases")]
    [MemberData(nameof(UpdateCategoryTestDataGenerator.GetCategoriesToUpdate), parameters: 10, MemberType = typeof(UpdateCategoryTestDataGenerator))]
    public async Task UpdateCategory(DomainEntity.Category category, UpdateCategoryInput input)
    {
        // Given
        var dbId = Guid.NewGuid().ToString();
        var context = _fixture.CreateDbContext(dbId: dbId);
        await context.AddRangeAsync(_fixture.GetExampleCategoriesList());
        var tracking = await context.AddAsync(category);
        context.SaveChanges();
        tracking.State = EntityState.Detached;
        var repository = new CategoryRepository(context);
        var uow = new UnitOfWork(context);
        var useCase = new UpdateCategory(repository, uow);

        // When
        var output = await useCase.Handle(input, CancellationToken.None);
        var data = await _fixture.CreateDbContext(true, dbId).Categories.FindAsync(output.Id);

        // Then
        data.Should().NotBeNull();
        data!.Name.Should().Be(input.Name);
        data.Description.Should().Be(input.Description);
        data.IsActive.Should().Be((bool)input.IsActive!);
        data.Id.Should().NotBeEmpty();
        data.CreatedAt.Should().NotBeSameDateAs(default);

        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().Be((bool)input.IsActive!);
    }
}