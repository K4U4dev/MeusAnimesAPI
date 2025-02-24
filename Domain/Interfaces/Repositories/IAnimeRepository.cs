using Domain.Entities;

namespace Domain.Interfaces.Repositories;
public interface IAnimeRepository : IBaseRepository<Anime, int>
{
    Task<IEnumerable<Anime>> GetByNameAsync(string nameAnime);
    Task<IEnumerable<Anime>> GetByDirectorAsync(string nameDirector);
}
