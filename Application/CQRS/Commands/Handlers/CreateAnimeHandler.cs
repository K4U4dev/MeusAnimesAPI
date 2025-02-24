using MediatR;
using Domain.Entities;
using AutoMapper;
using Domain.Interfaces.Services;
using Domain.Exceptions;

namespace Application.CQRS.Commands.Handlers
{
    public class CreateAnimeHandler : IRequestHandler<CreateAnimeCommand, Anime>
    {
        private readonly IMapper mapper;
        private readonly IAnimeService animeService;

        public CreateAnimeHandler(IAnimeService animeService, IMapper mapper)
        {
            this.animeService = animeService;
            this.mapper = mapper;
        }
        public async Task<Anime> Handle(CreateAnimeCommand request, CancellationToken cancellationToken)
        {
            var animeEqual = await animeService.GetByNameAsync(request.Nome);
            if (animeEqual.Any())
            {
                throw new DuplicatedEntityException("Anime already exists");
            }

            var anime = await animeService.CreateAsync(new Anime { 
                                                                    Nome = request.Nome, 
                                                                    Diretor = request.Diretor, 
                                                                    Resumo = request.Resumo
                                                                  });
            return anime;
        }
    }
}
