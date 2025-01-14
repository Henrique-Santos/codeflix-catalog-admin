using FC.Codeflix.Catalog.Infra.Data.EF;
using Microsoft.EntityFrameworkCore;

namespace FC.Codeflix.Catalog.Api.Configurations;

public static class ConnectionsConfigurations
{
    public static IServiceCollection AddAppConnections(this IServiceCollection services)
    {
        services.AddConnection();

        return services;
    }

    public static IServiceCollection AddConnection(this IServiceCollection services)
    {
        services.AddDbContext<CodeflixCatalogDbContext>(options =>
        {
            options.UseInMemoryDatabase("InMemory-DSV-Database");
        });

        return services;
    }
}