using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace DomainLayer.ViewModels.PostViewModels
{
    public class CreatePostView
    {
        [Required]
        public string Title { get; set; }

        public string TextContent { get; set; }

        public IFormFile Image { get; set; }

        public IFormFile Video { get; set; }
    }
}
