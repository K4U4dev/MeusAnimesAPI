using Domain.Entities;
using MediatR;

namespace Application.CQRS.Queries;

public class GetAllAnimesQuery : IRequest<IEnumerable<Anime?>> { }