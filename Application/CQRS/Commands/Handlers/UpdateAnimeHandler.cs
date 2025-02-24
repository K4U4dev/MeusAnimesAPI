using MediatR;
using Domain.Entities;
using AutoMapper;
using Domain.Interfaces.Services;
using Domain.Exceptions;

namespace Application.CQRS.Commands.Handlers
{
    public class UpdateAnimeHandler : IRequestHandler<UpdateAnimeCommand, bool>
    {
        private readonly IMapper mapper;
        private readonly IAnimeService animeService;

        public UpdateAnimeHandler(IAnimeService animeService, IMapper mapper)
        {
            this.animeService = animeService;
            this.mapper = mapper;
        }

        public async Task<bool> Handle(UpdateAnimeCommand request, CancellationToken cancellationToken)
        {
            var animeForUpdate = await animeService.GetByIdAsync(request.Id);
            if (animeForUpdate == null)
            {
                throw new NotFoundException("Anime not found");
            }
            animeForUpdate.Nome = request.Nome;
            animeForUpdate.Diretor = request.Diretor;
            animeForUpdate.Resumo = request.Resumo;
            await animeService.UpdateAsync(animeForUpdate);
            return true;
        }
    }
}
