using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PresentationLayer.Models;

namespace DomainLayer.ModelConfiguration
{
    public class MessageConfig : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.ToTable("DTbl_Messages");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
            builder.Property(x => x.Content).IsRequired();
            builder.Property(x => x.SentAt).IsRequired();

            builder.HasOne(x => x.Sender)
                   .WithMany(u => u.MessagesSent)
                   .HasForeignKey(x => x.SenderId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Receiver)
                   .WithMany(u => u.MessagesReceived)
                   .HasForeignKey(x => x.ReceiverId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Restrict); 

          
        }
    }
}
