using Domain.Entities;
using MediatR;

namespace Application.CQRS.Queries;

public class GetAnimesByDiretorQuery : IRequest<IEnumerable<Anime>>
{
    public string NameDirector { get; }

    public GetAnimesByDiretorQuery(string nameDirector)
    {
        NameDirector = nameDirector;
    }
}