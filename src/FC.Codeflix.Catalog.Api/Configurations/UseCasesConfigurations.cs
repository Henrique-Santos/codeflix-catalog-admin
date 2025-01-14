using FC.Codeflix.Catalog.Application.Interfaces;
using FC.Codeflix.Catalog.Application.UseCases.Category.Create;
using FC.Codeflix.Catalog.Domain.Repository;
using FC.Codeflix.Catalog.Infra.Data.EF;
using FC.Codeflix.Catalog.Infra.Data.EF.Repositories;

namespace FC.Codeflix.Catalog.Api.Configurations;

public static class UseCasesConfigurations
{
    public static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        services.AddMediatR(options => 
        {
            options.RegisterServicesFromAssemblies(typeof(CreateCategory).Assembly);
        });
        services.AddRepositories();
        
        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddTransient<ICategoryRepository, CategoryRepository>();
        services.AddTransient<IUnitOfWork, UnitOfWork>();

        return services;
    }
}