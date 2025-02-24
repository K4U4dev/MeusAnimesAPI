using Domain.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class AnimeRepository(AnimeDbContext context) : BaseRepository<Anime, int>(context), IAnimeRepository
{
    private readonly AnimeDbContext context = context;

    public async Task<IEnumerable<Anime>> GetByDirectorAsync(string nameDirector)
    {
        return await context.Animes.Where(anime => anime.Diretor != null && anime.Diretor.ToLower().Contains(nameDirector.ToLower())).ToListAsync();
    }

    public  async Task<IEnumerable<Anime>> GetByNameAsync(string nameAnime)
    {
        return await context.Animes.Where(anime => anime.Nome.ToLower().Contains(nameAnime.ToLower())).ToListAsync();
    }
}