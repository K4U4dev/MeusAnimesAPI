using Domain.Interfaces.Repositories;
using Infrastructure.Repositories;

namespace MeusAnimesAPI.Extensions.DependencyInjection;

public static class Repository
{
    public static IServiceCollection AddRepositoryDependencies(this IServiceCollection services)
    {
        services.AddScoped<IAnimeRepository, AnimeRepository>();
        return services;
    }
}