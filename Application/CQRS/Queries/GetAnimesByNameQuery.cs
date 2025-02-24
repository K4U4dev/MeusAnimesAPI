using Domain.Entities;
using MediatR;

namespace Application.CQRS.Queries
{
    public class GetAnimesByNameQuery : IRequest<IEnumerable<Anime>>
    {
        public string Name { get; }

        public GetAnimesByNameQuery(string name)
        {
            Name = name;
        }
    }
}