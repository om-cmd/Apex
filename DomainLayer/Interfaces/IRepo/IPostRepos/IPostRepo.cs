using DomainLayer.ViewModels.PostViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Interfaces.IRepo.IPostRepos
{
    public interface IPostRepo
    {
        public  Task<IActionResult> Create(CreatePostView postView, IFormFile image, IFormFile video);
        public Task<IActionResult> Edit(UpdatePostView postView);
        public Task<IActionResult> DeleteConfirmed(int id);
        public Task<IActionResult> Archive(int id);
        public Task<List<Post>> GetAllPostsAsync();
        public Task<Post> FindByIdAsync(int id);
        public bool PostExists(int id);

    }
}
