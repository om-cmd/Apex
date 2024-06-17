using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PresentationLayer.Models;

namespace DomainLayer.ModelConfiguration
{
    public class BranchConfig : IEntityTypeConfiguration<Branch>
    {
        public void Configure(EntityTypeBuilder<Branch> builder)
        {
            builder.ToTable("DTbl_Branches");
            builder.HasKey(x => x.TenantId);
            builder.Property(x => x.TenantId).ValueGeneratedNever().IsRequired();
            builder.Property(x => x.Name).IsRequired();

            builder.HasMany(x => x.Users)
                   .WithOne(u => u.Tenant)
                   .OnDelete(DeleteBehavior.Restrict)
                   .HasForeignKey(u => u.TenantId);

            builder.HasMany(x => x.Posts)
                   .WithOne(p => p.Tenant)
                   .OnDelete(DeleteBehavior.Restrict)
                   .HasForeignKey(p => p.TenantId);

            builder.HasMany(x => x.Comments)
                   .WithOne(c => c.Tenant)
                   .OnDelete(DeleteBehavior.Restrict)
                   .HasForeignKey(c => c.TenantId);

            builder.HasMany(x => x.Likes)
                   .WithOne(l => l.Tenant)
                   .OnDelete(DeleteBehavior.Restrict)
                   .HasForeignKey(l => l.TenantId);

            builder.HasMany(x => x.Messages)
                   .WithOne(m => m.Tenant)
                   .OnDelete(DeleteBehavior.Restrict)
                   .HasForeignKey(m => m.TenantId);

            builder.HasMany(x => x.FriendRequests)
                   .WithOne(fr => fr.Tenant)
                   .OnDelete(DeleteBehavior.Restrict)
                   .HasForeignKey(fr => fr.TenantId);

            builder.HasMany(x => x.Notifications)
                   .WithOne(n => n.Tenant)
                   .OnDelete(DeleteBehavior.Restrict)
                   .HasForeignKey(n => n.TenantId);
        }
    }
}
