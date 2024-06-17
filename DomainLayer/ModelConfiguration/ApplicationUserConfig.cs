using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PresentationLayer.Models;

namespace DomainLayer.ModelConfiguration
{
    public class ApplicationUserConfig : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.ToTable("DTbl_User");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
            builder.Property(x => x.FullName).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Email).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Password).IsRequired().HasMaxLength(100);
            builder.Property(x => x.PhoneNumber).HasMaxLength(15);
            builder.Property(x => x.Bio).HasMaxLength(500);

            builder.HasMany(x => x.Posts)
                   .WithOne(p => p.User)
                   .HasForeignKey(p => p.UserId)
                   .OnDelete(DeleteBehavior.Restrict); 

            builder.HasMany(x => x.FriendRequestsSent)
                   .WithOne(fr => fr.Sender)
                   .HasForeignKey(fr => fr.SenderId)
                   .OnDelete(DeleteBehavior.Restrict); 

            builder.HasMany(x => x.FriendRequestsReceived)
                   .WithOne(fr => fr.Receiver)
                   .HasForeignKey(fr => fr.ReceiverId)
                   .OnDelete(DeleteBehavior.Restrict); 

            builder.HasMany(x => x.Comments)
                   .WithOne(c => c.User)
                   .HasForeignKey(c => c.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.Likes)
                   .WithOne(l => l.User)
                   .HasForeignKey(l => l.UserId)
                   .OnDelete(DeleteBehavior.Restrict); 
            builder.HasMany(x => x.MessagesSent)
                   .WithOne(m => m.Sender)
                   .HasForeignKey(m => m.SenderId)
                   .OnDelete(DeleteBehavior.Restrict); 

            builder.HasMany(x => x.MessagesReceived)
                   .WithOne(m => m.Receiver)
                   .HasForeignKey(m => m.ReceiverId)
                   .OnDelete(DeleteBehavior.Restrict); 

            builder.HasMany(x => x.Notifications)
                   .WithOne(n => n.User)
                   .HasForeignKey(n => n.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Ignore(x => x.ProfilePictureUrl);
        }
    }
}
