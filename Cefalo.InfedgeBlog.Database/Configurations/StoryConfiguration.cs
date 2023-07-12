using Cefalo.InfedgeBlog.Database.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cefalo.InfedgeBlog.Database.Configurations
{
    public class StoryConfiguration : IEntityTypeConfiguration<Story>
    {
        public void Configure(EntityTypeBuilder<Story> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Title)
                .IsRequired();

            builder.Property(s => s.Body)
                .IsRequired();

            builder.Property(s => s.AuthorId)
                .IsRequired();

            builder.HasOne(s => s.Author)
                .WithMany(u => u.Stories)
                .HasForeignKey(s => s.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(s => s.CreatedAt);

            builder.Property(s => s.UpdatedAt);
        }
    }
}
