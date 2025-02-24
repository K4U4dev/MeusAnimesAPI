using Domain.Entities;
using MediatR;
using Domain.Interfaces.Services;

namespace Application.CQRS.Queries.Handlers;

public class GetAllAnimesHandler : IRequestHandler<GetAllAnimesQuery, IEnumerable<Anime?>>
{
    private readonly IAnimeService animeService;

    public GetAllAnimesHandler(IAnimeService animeService)
    {
        this.animeService = animeService;
    }

    public async Task<IEnumerable<Anime?>> Handle(GetAllAnimesQuery request, CancellationToken cancellationToken)
    {
        var animes = await animeService.GetAllAsync();
        return animes;
    }
}