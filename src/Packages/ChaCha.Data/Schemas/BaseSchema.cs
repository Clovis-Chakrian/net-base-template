using ChaCha.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChaCha.Data.Schemas;

public class BaseSchema<TEntity, TKey> : IEntityTypeConfiguration<TEntity> where TEntity : Entity<TKey>
{
  public virtual void Configure(EntityTypeBuilder<TEntity> builder)
  {
    builder.HasKey(e => e.Id);

    builder.Property(e => e.CreatedAt)
      .IsRequired();

    builder.Property(e => e.UpdatedAt)
      .IsRequired();
  }
}