using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces;

public interface IAnimeRepository
{
    Task<IEnumerable<Anime>> GetAllAsync();
    Task<Anime?> GetByIdAsync(Guid id);
    Task AddAsync(Anime anime);
    Task UpdateAsync(Anime anime);
    Task DeleteAsync(Guid id);
}