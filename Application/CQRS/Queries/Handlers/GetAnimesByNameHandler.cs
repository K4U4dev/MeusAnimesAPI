using Domain.Entities;
using MediatR;
using Domain.Interfaces.Services;

namespace Application.CQRS.Queries.Handlers
{
    public class GetAnimesByNameHandler : IRequestHandler<GetAnimesByNameQuery, IEnumerable<Anime?>>
    {
        private readonly IAnimeService animeService;
        public GetAnimesByNameHandler(IAnimeService animeService)
        {
            this.animeService = animeService;
        }
        public async Task<IEnumerable<Anime?>> Handle(GetAnimesByNameQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Name))
            {
                throw new ArgumentException("Name must not be empty");
            }
            var animes = await animeService.GetByNameAsync(request.Name);
            return animes;
        }
    }
}

