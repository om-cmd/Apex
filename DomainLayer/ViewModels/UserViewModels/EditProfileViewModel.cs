using DomainLayer.Models;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.Models
{
    public class EditProfileViewModel
    {
        public string FullName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string? Bio { get; set; }

        public Gender Gender { get; set; }

        public Roles Roles { get; set; }

        public IFormFile ProfilePictureUrl { get; set; } 

    }
}
