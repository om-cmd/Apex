using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PresentationLayer.Models
{
    public class FriendRequest
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey(nameof(Sender))]
        public int SenderId { get; set; }
        public ApplicationUser Sender { get; set; }

        [ForeignKey(nameof(Receiver))]
        public int ReceiverId { get; set; }
        public ApplicationUser Receiver { get; set; }
        public DateTime SentAt { get; set; }
        public bool IsAccepted { get; set; }

        [ForeignKey(nameof(Tenant))]
        public int TenantId { get; set; }
        public Branch Tenant { get; set; }
    }
}
