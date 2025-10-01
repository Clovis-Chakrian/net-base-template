using System.Linq.Expressions;
using ChaCha.Core.Domain;
using ChaCha.Core.Repositories.Pagination;

namespace ChaCha.Core.Repositories;

public interface IReadRepository<TEntity, TId> 
    where TEntity : Entity<TId> 
    
{
    TEntity? Find(TId id);
    Task<TEntity?> FindAsync(TId id);

    Page<TEntity> FindAll<T>(Expression<Func<TEntity, bool>> predicate, int page, int pageSize,
        Expression<Func<TEntity, T>> orderBy);

    Page<TEntity> FindAll();
    Page<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate, int page, int pageSize);

    Task<Page<TEntity>> FindAllAsync<T>(Expression<Func<TEntity, bool>> predicate, int page, int pageSize,
        Expression<Func<TEntity, T>> orderBy);

    Task<Page<TEntity>> FindAllAsync();
    Task<Page<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> predicate, int page, int pageSize);
}