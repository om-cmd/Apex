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

           
        }
    }
}
