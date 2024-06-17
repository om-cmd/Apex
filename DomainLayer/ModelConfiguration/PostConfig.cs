using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PresentationLayer.Models;

namespace DomainLayer.ModelConfiguration
{
    public class PostConfig : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.ToTable("DTbl_Posts");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
            builder.Property(x => x.Title).IsRequired().HasMaxLength(200);
            builder.Property(x => x.Content).IsRequired();
            builder.Property(x => x.CreatedAt).IsRequired();

            builder.HasOne(x => x.User)
                   .WithMany(u => u.Posts)
                   .HasForeignKey(x => x.UserId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Restrict); 

            builder.HasOne(x => x.Tenant)
                   .WithMany(b => b.Posts)
                   .HasForeignKey(x => x.TenantId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.Comments)
                   .WithOne(c => c.Post)
                   .HasForeignKey(c => c.PostId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.Likes)
                   .WithOne(l => l.Post)
                   .HasForeignKey(l => l.PostId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
