using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.Models
{
    public class Branch
    {
        [Key]
        public int TenantId { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        public DateTime DateCreated { get; set; }
        [Required]
        public string CreatedBy { get; set; }
        [Required]

        public DateTime? ModifiedDate   { get; set; }

        public bool IsActive {  get; set; }
        public ICollection<ApplicationUser> Users { get; set; }
        public ICollection<Post> Posts { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Like> Likes { get; set; }
        public ICollection<Message> Messages { get; set; }
        public ICollection<FriendRequest> FriendRequests { get; set; }
        public ICollection<Notification> Notifications { get; set; }
    }
}
