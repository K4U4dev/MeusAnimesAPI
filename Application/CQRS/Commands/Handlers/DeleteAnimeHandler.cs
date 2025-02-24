using MediatR;
using Domain.Entities;
using AutoMapper;
using Domain.Interfaces.Services;
using Domain.Exceptions;

namespace Application.CQRS.Commands.Handlers;

public class DeleteAnimeHandler : IRequestHandler<DeleteAnimeCommand, bool>
{
    private readonly IMapper mapper;
    private readonly IAnimeService animeService;
    public DeleteAnimeHandler(IAnimeService animeService, IMapper mapper)
    {
        this.animeService = animeService;
        this.mapper = mapper;
    }

    public async Task<bool> Handle(DeleteAnimeCommand request, CancellationToken cancellationToken)
    {
        var animeForDelete = await animeService.GetByIdAsync(request.Id);
        if (animeForDelete == null)
        {
            throw new NotFoundException("Anime not found");
        }
        await animeService.DeleteAsync(request.Id);
        return true;
    }
}