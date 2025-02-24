namespace Domain.Interfaces.Repositories;

public interface IBaseRepository<TEntity, TPrimaryKey> where TEntity : class where TPrimaryKey : struct
{
    Task<TEntity> CreateAsync(TEntity entity);
    Task<TEntity> UpdateAsync(TEntity entity);
    Task<bool> DeleteAsync(TPrimaryKey id);
    Task<TEntity?> GetByIdAsync(TPrimaryKey id);
    Task<IEnumerable<TEntity?>> GetAllAsync();
    Task<IEnumerable<TEntity?>> GetAllAsyncWithouTracking();
}