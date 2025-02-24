using Domain.Entities;
using MediatR;
using Domain.Interfaces.Services;

namespace Application.CQRS.Queries.Handlers;

public class GetAnimesByDiretorHandler : IRequestHandler<GetAnimesByDiretorQuery, IEnumerable<Anime?>>
{
    private readonly IAnimeService animeService;
    public GetAnimesByDiretorHandler(IAnimeService animeService)
    {
        this.animeService = animeService;
    }
    public async Task<IEnumerable<Anime?>> Handle(GetAnimesByDiretorQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.NameDirector))
        {
            throw new ArgumentException("Diretor must not be empty");
        }
        var animes = await animeService.GetByDirectorAsync(request.NameDirector);
        return animes;
    }
}