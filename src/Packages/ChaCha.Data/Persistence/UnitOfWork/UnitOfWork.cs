using Microsoft.EntityFrameworkCore;

namespace ChaCha.Data.Persistence.UnitOfWork;

public class UnitOfWork<TDbContext> : IUnitOfWork where TDbContext : DbContext
{
  private readonly TDbContext _context;

  public UnitOfWork(TDbContext context)
  {
    _context = context;
  }

  public async Task<bool> CommitAsync(CancellationToken cancellationToken = default)
  {
    var result = await _context.SaveChangesAsync(cancellationToken);
    return result > 0;
  }

  public bool Commit()
  {
    var result =  _context.SaveChanges();
    return result > 0;
  }
}