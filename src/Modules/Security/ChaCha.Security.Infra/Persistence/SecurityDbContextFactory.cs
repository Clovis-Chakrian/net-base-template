using ChaCha.MediatR.Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ChaCha.Security.Infra.Persistence;

#nullable disable
public class SecurityDbContextFactory : IDesignTimeDbContextFactory<SecurityDbContext>
{
  private readonly IMediator _mediator;

  public SecurityDbContextFactory()
  { }

  public SecurityDbContextFactory(IMediator mediator)
  {
    _mediator = mediator;
  }
  public SecurityDbContext CreateDbContext(string[] args)
  {
    var optionsBuilder = new DbContextOptionsBuilder<SecurityDbContext>();
    optionsBuilder.UseNpgsql("Server=localhost; Database=ModularMonolithDb; User Id=user; Password=12345678");
    optionsBuilder.UseOpenIddict();

    return new SecurityDbContext(optionsBuilder.Options, _mediator);
  }
}