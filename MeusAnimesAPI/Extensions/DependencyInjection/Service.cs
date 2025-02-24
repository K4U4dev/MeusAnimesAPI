using Application.Services; 
using Domain.Interfaces.Services;

namespace MeusAnimesAPI.Extensions.DependencyInjection;

public static class Service
{
    public static IServiceCollection AddServicesDependencies(this IServiceCollection services)
    {
        services.AddScoped<IAnimeService, AnimeService>();
        return services;
    }
}