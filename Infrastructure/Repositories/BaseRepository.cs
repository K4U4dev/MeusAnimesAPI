using Microsoft.EntityFrameworkCore;
using Domain.Interfaces.Repositories;
using Infrastructure.Context;

namespace Infrastructure.Repositories;

public abstract class BaseRepository<T, K> : IBaseRepository<T, K> where T : class where K : struct
{
    private readonly AnimeDbContext context;

    public BaseRepository(AnimeDbContext context) => this.context = context;

    public virtual async Task<T> CreateAsync(T entity)
    {
        await context.Set<T>().AddAsync(entity);
        await context.SaveChangesAsync();
        return entity;
    }
    public virtual async Task<T> UpdateAsync(T entity)
    {
        context.Set<T>().Update(entity);
        await context.SaveChangesAsync();
        return entity;
    }
    public virtual async Task<bool> DeleteAsync(K id)
    {
        var entity = await context.Set<T>().FindAsync(id);
        if (entity == null) return false;
        context.Set<T>().Remove(entity);
        await context.SaveChangesAsync();
        return true;
    }

    public virtual async Task<IEnumerable<T?>> GetAllAsync()
    {
        return await context.Set<T>().ToListAsync();
    }

    public virtual async Task<IEnumerable<T?>> GetAllAsyncWithouTracking()
    {
        return await context.Set<T>().AsNoTracking().ToListAsync();
    }

    public virtual async Task<T?> GetByIdAsync(K id)
    {
        return await context.Set<T>().FindAsync(id);
    }
}