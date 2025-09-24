using ChaCha.Security.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChaCha.Security.Infra.Schemas;

public class UserSchema : IEntityTypeConfiguration<User>
{
  public void Configure(EntityTypeBuilder<User> builder)
  {
    builder.Property(e => e.CreatedAt)
      .IsRequired()
      .HasComment("The date and time when the entity was created.");

    builder.Property(e => e.UpdatedAt)
      .IsRequired()
      .HasComment("The date and time when the entity was last updated.");
  }
}