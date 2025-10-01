using ChaCha.Core.Domain;

namespace ChaCha.Core.Repositories;

public interface IWriteRepository<TEntity, TId>
    where TEntity : Entity<TId>
{
    TEntity Create(TEntity entity);
    Task<TEntity> CreateAsync(TEntity entity);
    public TEntity Update(TEntity entity);
    TEntity Delete(TEntity entity);
    void DeleteRange(IEnumerable<TEntity> entity);
}