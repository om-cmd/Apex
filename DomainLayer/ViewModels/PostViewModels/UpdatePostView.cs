using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace DomainLayer.ViewModels.PostViewModels
{
    public class UpdatePostView
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string TextContent { get; set; }

        public IFormFile Image { get; set; }

        public IFormFile Video { get; set; }

        public string ExistingImagePath { get; set; }

        public string ExistingVideoPath { get; set; }
    }
}
