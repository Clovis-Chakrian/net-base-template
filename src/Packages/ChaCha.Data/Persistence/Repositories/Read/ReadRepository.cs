using System.Linq.Expressions;
using ChaCha.Core.Domain;
using ChaCha.Core.Repositories;
using ChaCha.Core.Repositories.Pagination;
using ChaCha.Data.Persistence.Repositories.Cache;
using Microsoft.EntityFrameworkCore;

namespace ChaCha.Data.Persistence.Repositories.Read;

public abstract class ReadRepository<TEntity, TId, TDbContext> : IReadRepository<TEntity, TId>
    where TEntity : Entity<TId> 
    where TDbContext : DbContext
{
    private readonly TDbContext _context;
    private readonly ICacheRepository _cacheRepository;

    protected ReadRepository(TDbContext context, ICacheRepository cacheRepository)
    {
        _context = context;
        _cacheRepository = cacheRepository;
    }

    public TEntity? Find(TId id)
    {
        var cacheResponse = _cacheRepository.Get<TEntity>(id!.ToString()!);
        
        if (cacheResponse != null)
        {
            return cacheResponse;
        }

        var queryResponse = _context.Set<TEntity>().Find(id);

        if (queryResponse == null)
        {
            return queryResponse;
        }
        
        _cacheRepository.Set(id.ToString()!, queryResponse);
        
        return _context.Set<TEntity>().Find(id);
    }

    public async Task<TEntity?> FindAsync(TId id)
    {
        var cacheResponse = await _cacheRepository.GetAsync<TEntity>(id!.ToString()!);
        
        if (cacheResponse != null)
        {
            return cacheResponse;
        }

        var queryResponse = await _context.Set<TEntity>().FindAsync(id);

        if (queryResponse == null)
        {
            return queryResponse;
        }
        
        await _cacheRepository.SetAsync(id.ToString()!, queryResponse);
        
        return await _context.Set<TEntity>().FindAsync(id);
    }

    public Page<TEntity> FindAll<T>(Expression<Func<TEntity, bool>> predicate, int page, int pageSize, Expression<Func<TEntity, T>> orderBy)
    {
        var key = $"{predicate}_{page.ToString()}_{pageSize.ToString()}_{orderBy}";
        
        var cacheResponse = _cacheRepository.Get<Page<TEntity>>(key);
        
        if (cacheResponse != null)
        {
            return cacheResponse;
        }
        
        var count = _context.Set<TEntity>().Count(predicate);
        var skip = (page - 1) * pageSize;

        var queryResponse = _context.Set<TEntity>()
            .AsQueryable()
            .Where(predicate)
            .Skip(skip)
            .Take(pageSize)
            .OrderByDescending(orderBy)
            .ToList();

        var response = new Page<TEntity>(
            items: queryResponse,
            page: page,
            pageSize: pageSize,
            totalItems: count
        );

        _cacheRepository.Set(key, response);

        return response;
    }
    
    public Page<TEntity> FindAll()
    {
        return FindAll(
            predicate: _ => true,
            page: 1,
            pageSize: 10,
            orderBy: e => e.UpdatedAt
            );
    }
    
    public Page<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate, int page, int pageSize)
    {
        return FindAll(
            predicate: predicate,
            page: page,
            pageSize: pageSize,
            orderBy: e => e.UpdatedAt
        );
    }

    public async Task<Page<TEntity>> FindAllAsync<T>(Expression<Func<TEntity, bool>> predicate, int page, int pageSize, Expression<Func<TEntity, T>> orderBy)
    {
        var key = $"{predicate}_{page.ToString()}_{pageSize.ToString()}_{orderBy}";
        
        var cacheResponse = await _cacheRepository.GetAsync<Page<TEntity>>(key);
        
        if (cacheResponse != null)
        {
            return cacheResponse;
        }
        
        var count = _context.Set<TEntity>().Count(predicate);
        var skip = (page - 1) * pageSize;

        var queryResponse = await _context.Set<TEntity>()
            .AsQueryable()
            .Where(predicate)
            .Skip(skip)
            .Take(pageSize)
            .OrderByDescending(orderBy)
            .ToListAsync();
        
        var response = new Page<TEntity>(
            items: queryResponse,
            page: page,
            pageSize: pageSize,
            totalItems: count
        );

        await _cacheRepository.SetAsync(key, response);

        return response;
    }
    
    public async Task<Page<TEntity>> FindAllAsync()
    {
        return await FindAllAsync(
            predicate: _ => true,
            page: 1,
            pageSize: 10,
            orderBy: e => e.UpdatedAt
        );
    }
    
    public async Task<Page<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> predicate, int page, int pageSize)
    {
        return await FindAllAsync(
            predicate: predicate,
            page: page,
            pageSize: pageSize,
            orderBy: e => e.UpdatedAt
        );
    }
}