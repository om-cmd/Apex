using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PresentationLayer.Models;

namespace DomainLayer.ModelConfiguration
{
    public class LikeConfig : IEntityTypeConfiguration<Like>
    {
        public void Configure(EntityTypeBuilder<Like> builder)
        {
            builder.ToTable("DTbl_Likes");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
            builder.Property(x => x.CreatedAt).IsRequired();

            builder.HasOne(x => x.Post)
                   .WithMany(p => p.Likes)
                   .HasForeignKey(x => x.PostId)
                   .OnDelete(DeleteBehavior.Restrict)  
                   .IsRequired();

            builder.HasOne(x => x.User)
                   .WithMany(u => u.Likes)
                   .HasForeignKey(x => x.UserId)
                   .OnDelete(DeleteBehavior.Restrict)  
                   .IsRequired();

            builder.HasOne(x => x.Tenant)
                   .WithMany()
                   .HasForeignKey(x => x.TenantId)
                   .OnDelete(DeleteBehavior.Restrict)  
                   .IsRequired();
        }
    }
}
