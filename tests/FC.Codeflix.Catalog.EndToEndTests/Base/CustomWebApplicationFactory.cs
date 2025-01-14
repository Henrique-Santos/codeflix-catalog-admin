using FC.Codeflix.Catalog.Infra.Data.EF;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FC.Codeflix.Catalog.EndToEndTests.Base;

public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class 
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services
                .FirstOrDefault(p => p.ServiceType == typeof(DbContextOptions<CodeflixCatalogDbContext>));

            if (descriptor is not null) services.Remove(descriptor);

            services.AddDbContext<CodeflixCatalogDbContext>(options =>
            {
                options.UseInMemoryDatabase("end2end-tests-db");
            });
        });

        base.ConfigureWebHost(builder);
    }
}