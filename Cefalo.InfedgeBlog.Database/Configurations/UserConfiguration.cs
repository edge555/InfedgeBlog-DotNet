using Cefalo.InfedgeBlog.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cefalo.InfedgeBlog.Database.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Username)
                .IsRequired();

            builder.Property(u => u.Name)
                .IsRequired();

            builder.Property(u => u.Email)
                .IsRequired();

            builder.Property(u => u.Password)
                .IsRequired();

            builder.Property(u => u.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            builder.Property(u => u.UpdatedAt)
                .HasDefaultValueSql("GETDATE()");

            builder.Property(u => u.PasswordModifiedAt)
                .HasDefaultValueSql("GETDATE()");

            builder.HasMany(u => u.Stories)
                .WithOne(s => s.Author)
                .HasForeignKey(s => s.AuthorId);

            builder.HasIndex(u => u.Username).IsUnique();
            builder.HasIndex(u => u.Email).IsUnique();
        }
    }
}
