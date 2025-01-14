using Bogus;
using FC.Codeflix.Catalog.Infra.Data.EF;
using Microsoft.EntityFrameworkCore;

namespace FC.Codeflix.Catalog.EndToEndTests.Base;

public class BaseFixture
{
    public CustomWebApplicationFactory<Program> WebAppFactory { get; set; }
    public HttpClient HttpClient { get; set; }
    public ApiClient ApiClient { get; set; }
    protected Faker Faker { get; set; }

    public BaseFixture()
    {
        Faker = new Faker("pt_BR");
        WebAppFactory = new CustomWebApplicationFactory<Program>();
        HttpClient = WebAppFactory.CreateClient();
        ApiClient = new ApiClient(HttpClient);
    }

    public CodeflixCatalogDbContext CreateDbContext(string dbId = "")
    {
        var context = new CodeflixCatalogDbContext(
            new DbContextOptionsBuilder<CodeflixCatalogDbContext>()
                .UseInMemoryDatabase($"end2end-tests-db{dbId}")
                .Options
        );
        return context;
    }
}