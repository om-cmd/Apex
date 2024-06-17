using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PresentationLayer.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string TextContent { get; set; }
        public string ImagePath { get; set; }
        public string VideoPath { get; set; }
        public DateTime? DateArchived { get; set; } 
        public bool IsArchived { get; set; }
        public DateTime ModifiedDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy {  get; set; }
        
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        public ApplicationUser User { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Like> Likes { get; set; }

        [ForeignKey(nameof(Tenant))]
        public int TenantId { get; set; }
        public Branch Tenant { get; set; }
    }
}
