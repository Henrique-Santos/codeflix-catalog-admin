using FC.Codeflix.Catalog.Application.Exceptions;
using FC.Codeflix.Catalog.Domain.SeedWork.SearchableRepository;
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

    [Fact(DisplayName = nameof(Get))]
    [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
    public async Task Get()
    {
        // Given
        var context = _fixture.CreateDbContext();
        var category = _fixture.GetExampleCategory();
        var categories = _fixture.GetExampleCategoriesList(15);
        categories.Add(category);
        await context.AddRangeAsync(categories);
        await context.SaveChangesAsync(CancellationToken.None);
        var repository = new CategoryRepository(_fixture.CreateDbContext(true));

        // When
        var data = await repository.Get(category.Id, CancellationToken.None);

        // Then
        data.Should().NotBeNull();
        data!.Name.Should().Be(category.Name);
        data.Id.Should().Be(category.Id);
        data.Description.Should().Be(category.Description);
        data.IsActive.Should().Be(category.IsActive);
        data.CreatedAt.Should().Be(category.CreatedAt);
    }

    [Fact(DisplayName = nameof(GetThrowIfNotFound))]
    [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
    public async Task GetThrowIfNotFound()
    {
        // Given
        var id = Guid.NewGuid();
        var categories = _fixture.GetExampleCategoriesList(15);
        var context = _fixture.CreateDbContext();
        await context.AddRangeAsync(categories);
        await context.SaveChangesAsync(CancellationToken.None);
        var repository = new CategoryRepository(context);

        // When
        var task = async () => await repository.Get(id, CancellationToken.None);

        // Then
       await task.Should().ThrowAsync<NotFoundException>().WithMessage($"Category '{id}' not found.");
    }

    [Fact(DisplayName = nameof(Update))]
    [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
    public async Task Update()
    {
        // Given
        var context = _fixture.CreateDbContext();
        var category = _fixture.GetExampleCategory();
        var newCategory = _fixture.GetExampleCategory();
        var categories = _fixture.GetExampleCategoriesList(15);
        categories.Add(category);
        await context.AddRangeAsync(categories);
        await context.SaveChangesAsync(CancellationToken.None);
        var repository = new CategoryRepository(_fixture.CreateDbContext(true));

        // When
        category.Update(newCategory.Name, newCategory.Description);
        await repository.Update(category, CancellationToken.None);
        await context.SaveChangesAsync();
        var data = await _fixture.CreateDbContext(true).Categories.FindAsync(category.Id);

        // Then
        data.Should().NotBeNull();
        data!.Name.Should().Be(category.Name);
        data.Id.Should().Be(category.Id);
        data.Description.Should().Be(category.Description);
        data.IsActive.Should().Be(category.IsActive);
        data.CreatedAt.Should().Be(category.CreatedAt);
    }

    [Fact(DisplayName = nameof(Delete))]
    [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
    public async Task Delete()
    {
        // Given
        var context = _fixture.CreateDbContext();
        var category = _fixture.GetExampleCategory();
        var categories = _fixture.GetExampleCategoriesList(15);
        categories.Add(category);
        await context.AddRangeAsync(categories);
        await context.SaveChangesAsync(CancellationToken.None);
        var repository = new CategoryRepository(context);

        // When
        await repository.Delete(category, CancellationToken.None);
        await context.SaveChangesAsync();
        var data = await _fixture.CreateDbContext(true).Categories.FindAsync(category.Id);

        // Then
        data.Should().BeNull();        
    }

    [Fact(DisplayName = nameof(SearchRetursListAndTotal))]
    [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
    public async Task SearchRetursListAndTotal()
    {
        // Given
        var context = _fixture.CreateDbContext();
        var categories = _fixture.GetExampleCategoriesList(15);
        await context.AddRangeAsync(categories);
        await context.SaveChangesAsync(CancellationToken.None);
        var repository = new CategoryRepository(context);
        var searchInput = new SearchInput(1, 20, "", "", SearchOrder.Asc);

        // When
        var output = await repository.Search(searchInput, CancellationToken.None);

        // Then
        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.CurrentPage.Should().Be(searchInput.Page);
        output.PerPage.Should().Be(searchInput.PerPage);
        output.Total.Should().Be(categories.Count);
        output.Items.Should().HaveCount(categories.Count);
        foreach(var outputItem in output.Items)
        {
            var item = categories.Find(category => category.Id == outputItem.Id);
            item.Should().NotBeNull();
            outputItem.Name.Should().Be(item!.Name);
            outputItem.Description.Should().Be(item.Description);
            outputItem.IsActive.Should().Be(item.IsActive);
            outputItem.CreatedAt.Should().Be(item.CreatedAt);
        }
    }

    [Fact(DisplayName = nameof(SearchRetursEmptyWhenPersistenceIsEmpty))]
    [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
    public async Task SearchRetursEmptyWhenPersistenceIsEmpty()
    {
        // Given
        var context = _fixture.CreateDbContext();
        var repository = new CategoryRepository(context);
        var searchInput = new SearchInput(1, 20, "", "", SearchOrder.Asc);

        // When
        var output = await repository.Search(searchInput, CancellationToken.None);

        // Then
        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.CurrentPage.Should().Be(searchInput.Page);
        output.PerPage.Should().Be(searchInput.PerPage);
        output.Total.Should().Be(0);
        output.Items.Should().HaveCount(0);
    }

    [Theory(DisplayName = nameof(SearchRetursPaginated))]
    [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
    [InlineData(10, 1, 5, 5)]
    [InlineData(10, 2, 5, 5)]
    [InlineData(7, 2, 5, 2)]
    [InlineData(7, 3, 5, 0)]
    public async Task SearchRetursPaginated(int quantityCategoriesToGenerate, int page, int perPage, int expectedQuantityItems)
    {
        // Given
        var context = _fixture.CreateDbContext();
        var categories = _fixture.GetExampleCategoriesList(quantityCategoriesToGenerate);
        await context.AddRangeAsync(categories);
        await context.SaveChangesAsync(CancellationToken.None);
        var repository = new CategoryRepository(context);
        var searchInput = new SearchInput(page, perPage, "", "", SearchOrder.Asc);

        // When
        var output = await repository.Search(searchInput, CancellationToken.None);

        // Then
        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.CurrentPage.Should().Be(searchInput.Page);
        output.PerPage.Should().Be(searchInput.PerPage);
        output.Total.Should().Be(quantityCategoriesToGenerate);
        output.Items.Should().HaveCount(expectedQuantityItems);
        foreach (var outputItem in output.Items)
        {
            var item = categories.Find(category => category.Id == outputItem.Id);
            item.Should().NotBeNull();
            outputItem.Name.Should().Be(item!.Name);
            outputItem.Description.Should().Be(item.Description);
            outputItem.IsActive.Should().Be(item.IsActive);
            outputItem.CreatedAt.Should().Be(item.CreatedAt);
        }
    }

    [Theory(DisplayName = nameof(SearchByText))]
    [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
    [InlineData("Action", 1, 5, 1, 1)]
    [InlineData("Horror", 1, 5, 3, 3)]
    [InlineData("Horror", 2, 5, 0, 3)]
    [InlineData("Sci-fi", 1, 5, 4, 4)]
    [InlineData("Sci-fi", 1, 2, 2, 4)]
    [InlineData("Sci-fi", 2, 3, 1, 4)]
    [InlineData("Sci-fi Other", 1, 3, 0, 0)]
    [InlineData("Robots", 1, 5, 2, 2)]
    public async Task SearchByText(string search, int page, int perPage, int expectedQuantityItemsReturned, int expectedQuantityTotalItems)
    {
        // Given
        var context = _fixture.CreateDbContext();
        var categories = _fixture.GetExampleCategoriesListWithNames(["Action", "Horror", "Horror - Robots", "Horror - Based on Real Facts", "Drama", "Sci-fi IA", "Sci-fi Space", "Sci-fi Robots", "Sci-fi Future"]);
        await context.AddRangeAsync(categories);
        await context.SaveChangesAsync(CancellationToken.None);
        var repository = new CategoryRepository(context);
        var searchInput = new SearchInput(page, perPage, search, "", SearchOrder.Asc);

        // When
        var output = await repository.Search(searchInput, CancellationToken.None);

        // Then
        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.CurrentPage.Should().Be(searchInput.Page);
        output.PerPage.Should().Be(searchInput.PerPage);
        output.Total.Should().Be(expectedQuantityTotalItems);
        output.Items.Should().HaveCount(expectedQuantityItemsReturned);
        foreach (var outputItem in output.Items)
        {
            var item = categories.Find(category => category.Id == outputItem.Id);
            item.Should().NotBeNull();
            outputItem.Name.Should().Be(item!.Name);
            outputItem.Description.Should().Be(item.Description);
            outputItem.IsActive.Should().Be(item.IsActive);
            outputItem.CreatedAt.Should().Be(item.CreatedAt);
        }
    }

    [Theory(DisplayName = nameof(SearchOrdered))]
    [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
    [InlineData("name", "asc")]
    [InlineData("name", "desc")]
    [InlineData("id", "asc")]
    [InlineData("id", "desc")]
    [InlineData("createdAt", "asc")]
    [InlineData("createdAt", "desc")]
    [InlineData("", "asc")]
    public async Task SearchOrdered(string orderBy, string order)
    {
        // Given
        var dbId = Guid.NewGuid().ToString();
        var context = _fixture.CreateDbContext(dbId: dbId);
        var categories = _fixture.GetExampleCategoriesList(10);
        await context.AddRangeAsync(categories);
        await context.SaveChangesAsync(CancellationToken.None);
        var repository = new CategoryRepository(context);
        var searchOrder = order.ToLower() == "asc" ? SearchOrder.Asc : SearchOrder.Desc;
        var searchInput = new SearchInput(1, 20, "", orderBy, searchOrder);

        // When
        var output = await repository.Search(searchInput, CancellationToken.None);
        var expectedOrderedList = _fixture.CloneCategoriesListOrdered(categories, orderBy, searchOrder);

        // Then
        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.CurrentPage.Should().Be(searchInput.Page);
        output.PerPage.Should().Be(searchInput.PerPage);
        output.Total.Should().Be(categories.Count);
        output.Items.Should().HaveCount(categories.Count);
        for(int indice = 0; indice < expectedOrderedList.Count; indice++)
        {
            var expectedItem = expectedOrderedList[indice];
            var outputItem = output.Items[indice];
            expectedItem.Should().NotBeNull();
            outputItem.Should().NotBeNull();
            outputItem.Name.Should().Be(expectedItem!.Name);
            outputItem.Id.Should().Be(expectedItem.Id);
            outputItem.Description.Should().Be(expectedItem.Description);
            outputItem.IsActive.Should().Be(expectedItem.IsActive);
            outputItem.CreatedAt.Should().Be(expectedItem.CreatedAt);
        }
    }
}