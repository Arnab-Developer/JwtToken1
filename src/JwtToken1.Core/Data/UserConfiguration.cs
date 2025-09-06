using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JwtToken1.Core.Data;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasIndex(u => u.Name).IsUnique();
        builder.Property(u => u.Name).HasMaxLength(100);
        builder.Property(u => u.Password).HasMaxLength(100);
    }
}
