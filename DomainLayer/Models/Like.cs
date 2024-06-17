using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PresentationLayer.Models
{
    public class Like
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }

        [ForeignKey(nameof(Post))]
        public int PostId { get; set; }
        public Post Post { get; set; }

        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        public ApplicationUser User { get; set; }

        [ForeignKey(nameof(Tenant))]
        public int TenantId { get; set; }
        public Branch Tenant { get; set; }
    }
}
