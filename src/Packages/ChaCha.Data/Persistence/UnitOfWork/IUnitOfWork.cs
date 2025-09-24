namespace ChaCha.Data.Persistence.UnitOfWork;

public interface IUnitOfWork
{
  Task<bool> Commit(CancellationToken cancellationToken = default);
}