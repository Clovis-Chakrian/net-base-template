using ChaCha.MediatR.Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ChaCha.Notification.Infra.Persistence;

#nullable disable
public class NotificationDbContextFactory : IDesignTimeDbContextFactory<NotificationDbContext>
{
  private readonly IMediator _mediator;

  public NotificationDbContextFactory()
  { }

  public NotificationDbContextFactory(IMediator mediator)
  {
    _mediator = mediator;
  }
  public NotificationDbContext CreateDbContext(string[] args)
  {
    var optionsBuilder = new DbContextOptionsBuilder<NotificationDbContext>();
    optionsBuilder.UseNpgsql("Server=localhost; Database=ModularMonolithDb; User Id=user; Password=12345678");

    return new NotificationDbContext(optionsBuilder.Options, _mediator);
  }
}