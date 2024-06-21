using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainLayer.Models;
using Microsoft.AspNetCore.Http;

namespace PresentationLayer.Models
{
    public class ApplicationUser
    {
        public string PasswordResetToken;
        public DateTime? PasswordResetTokenExpiry;

        [Key]
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }

        public string CreatedBy { get; set; }
        public string ?ConfirmEmail { get; set; }
        public string ?NewPassword { get; set; }
        public string ?Bio { get; set; }
        public IFormFile ProfilePictureUrl { get; set; }

        public bool IsActive { get; set; }
        public  Gender Gender{ get; set; }
        public Roles Roles { get; set; }

        [ForeignKey(nameof(Tenant))]
        public int? TenantId { get; set; }
        public Branch Tenant { get; set; }
        public ICollection<Post> Posts { get; set; }
        public ICollection<FriendRequest> FriendRequestsSent { get; set; }
        public ICollection<FriendRequest> FriendRequestsReceived { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Like> Likes { get; set; }
        public ICollection<Message> MessagesSent { get; set; }
        public ICollection<Message> MessagesReceived { get; set; }
        public ICollection<Notification> Notifications { get; set; }

    }

}
