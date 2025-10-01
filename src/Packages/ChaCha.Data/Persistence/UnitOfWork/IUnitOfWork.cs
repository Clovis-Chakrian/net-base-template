namespace ChaCha.Data.Persistence.UnitOfWork;

public interface IUnitOfWork
{
  Task<bool> CommitAsync(CancellationToken cancellationToken = default);
  bool Commit();
}
