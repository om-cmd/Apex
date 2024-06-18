using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PresentationLayer.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime SentAt { get; set; }

        [ForeignKey(nameof(Sender))]
        public int SenderId { get; set; }
        public ApplicationUser Sender { get; set; }

        [ForeignKey(nameof(Receiver))]
        public int ReceiverId { get; set; }
        public ApplicationUser Receiver { get; set; }

       
    }
}
