using Domain.Entities;
using MediatR;
using AutoMapper;
using Domain.Interfaces.Services;

namespace Application.CQRS.Queries.Handlers;

public class GetAnimeByIdHandler : IRequestHandler<GetAnimeByIdQuery, Anime?>
{
    private readonly IAnimeService animeService;

    public GetAnimeByIdHandler(IAnimeService animeService, IMapper mapper)
    {
        this.animeService = animeService;
    }

    public async Task<Anime?> Handle(GetAnimeByIdQuery request, CancellationToken cancellationToken)
    {
        if (request.Id <= 0)
        {
            throw new ArgumentException("Id must be greater than zero");
        }
        var anime = await animeService.GetByIdAsync(request.Id);
        return anime;
    }
}