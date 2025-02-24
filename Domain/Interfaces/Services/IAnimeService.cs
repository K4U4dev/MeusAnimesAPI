using Domain.Entities;

namespace Domain.Interfaces.Services;

public interface IAnimeService
{
    Task<Anime> CreateAsync(Anime anime);
    Task<Anime> UpdateAsync(Anime anime);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<Anime?>> GetAllAsync();
    Task<Anime?> GetByIdAsync(int id);
    Task<IEnumerable<Anime?>> GetByNameAsync(string nameAnime);
    Task<IEnumerable<Anime?>> GetByDirectorAsync(string nameDirector);
}