using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PresentationLayer.Models;

namespace DomainLayer.ModelConfiguration
{
    public class FriendRequestConfig : IEntityTypeConfiguration<FriendRequest>
    {
        public void Configure(EntityTypeBuilder<FriendRequest> builder)
        {
            builder.ToTable("DTbl_FriendRequests");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
            builder.Property(x => x.SentAt).IsRequired();
            builder.Property(x => x.IsAccepted).IsRequired();

            builder.HasOne(x => x.Sender)
                   .WithMany(u => u.FriendRequestsSent)
                   .HasForeignKey(x => x.SenderId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .IsRequired();

            builder.HasOne(x => x.Receiver)
                   .WithMany(u => u.FriendRequestsReceived)
                   .HasForeignKey(x => x.ReceiverId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .IsRequired();


        }
    }
}
