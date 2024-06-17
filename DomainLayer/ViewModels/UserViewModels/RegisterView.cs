using DomainLayer.Models;
using Microsoft.AspNetCore.Http;

namespace DomainLayer.ViewModels.UserVIewmodels
{
    public class RegisterView
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string? ConfirmEmail { get; set; }

        public IFormFile ProfilePictureUrl { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string CreatedBy { get; set; }

        public Gender Gender { get; set; }
    }
}
