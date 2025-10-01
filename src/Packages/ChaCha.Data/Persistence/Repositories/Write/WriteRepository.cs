using ChaCha.Core.Domain;
using ChaCha.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ChaCha.Data.Persistence.Repositories.Write;

public abstract class WriteRepository<TEntity, TId, TDbContext> : IWriteRepository<TEntity, TId>
    where TEntity : Entity<TId>
    where TDbContext : DbContext
{
    private readonly TDbContext _context;
    
    protected WriteRepository(TDbContext context)
    {
        _context = context;
    }

    public TEntity Create(TEntity entity)
    {
        return _context.Set<TEntity>().Add(entity).Entity;
    }
    
    public async Task<TEntity> CreateAsync(TEntity entity)
    {
        return (await _context.Set<TEntity>().AddAsync(entity)).Entity;
    }
    
    public TEntity Update(TEntity entity)
    {
        return _context.Set<TEntity>().Update(entity).Entity;
    }

    public TEntity Delete(TEntity entity)
    {
        return _context.Set<TEntity>().Remove(entity).Entity;
    }
    
    public void DeleteRange(IEnumerable<TEntity> entity)
    {
        _context.Set<TEntity>().RemoveRange(entity);
    }
}