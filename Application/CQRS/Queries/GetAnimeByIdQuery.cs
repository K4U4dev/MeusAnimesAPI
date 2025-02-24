using Domain.Entities;
using MediatR;

namespace Application.CQRS.Queries;

public class GetAnimeByIdQuery(int id) : IRequest<Anime>
{
    public int Id { get; set; } = id;
}