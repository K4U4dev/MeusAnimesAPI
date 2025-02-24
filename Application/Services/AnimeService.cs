using Domain.Entities;
using Domain.Interfaces.Services;
using Domain.Interfaces.Repositories;

namespace Application.Services;

public class AnimeService : IAnimeService
{
    private readonly IAnimeRepository animeRepository;

    public AnimeService(IAnimeRepository animeRepository)
    {
        this.animeRepository = animeRepository;
    }

    public async Task<Anime> CreateAsync(Anime anime)
    {
        return await animeRepository.CreateAsync(anime);
    }

    public async Task<Anime> UpdateAsync(Anime anime)
    {
        return await animeRepository.UpdateAsync(anime);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await animeRepository.DeleteAsync(id);
    }

    public async Task<IEnumerable<Anime?>> GetAllAsync()
    {
        return await animeRepository.GetAllAsync();
    }

    public async Task<IEnumerable<Anime?>> GetByDirectorAsync(string nameDirector)
    {
        return await animeRepository.GetByDirectorAsync(nameDirector);
    }

    public async Task<Anime?> GetByIdAsync(int id)
    {
        return await animeRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Anime?>> GetByNameAsync(string nameAnime)
    {
        return await animeRepository.GetByNameAsync(nameAnime);
    }
}