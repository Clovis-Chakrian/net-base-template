using ChaCha.Data.Schemas;
using ChaCha.Notification.Domain.TokensSent;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChaCha.Notification.Infra.Schemas;

public class TokenSentSchema : BaseSchema<TokenSent, Guid>
{
  public override void Configure(EntityTypeBuilder<TokenSent> builder)
  {
    base.Configure(builder);

    builder.Property(e => e.Token)
      .IsRequired()
      .HasMaxLength(255);

    builder.Property(e => e.TokenTypeId)
      .IsRequired()
      .HasConversion<int>();

    builder.Property(e => e.Used)
      .IsRequired();

    builder.Property(e => e.Attempts)
      .IsRequired();

    builder.HasOne(e => e.TokenType)
      .WithMany()
      .HasForeignKey(e => e.TokenTypeId)
      .OnDelete(DeleteBehavior.Restrict);

    builder.HasOne(e => e.NotificationSent)
      .WithOne()
      .HasForeignKey<TokenSent>(e => e.NotificationSentId)
      .OnDelete(DeleteBehavior.Restrict);

    builder.HasIndex(e => e.Token)
      .IsUnique(true);
  }
}