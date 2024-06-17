using DomainLayer.Data;
using DomainLayer.DataAcess;
using DomainLayer.Interfaces.IRepo.IPostRepos;
using DomainLayer.ViewModels.PostViewModels;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PresentationLayer.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLayer.Repositories.PostRepos
{
    public class PostRepo : IPostRepo
    {
        private readonly IUnitOfWork _context;
        private readonly PostService _postService;
        private readonly IHttpContextAccessor _contextAccessor;

        public PostRepo(IUnitOfWork context, PostService postService, IHttpContextAccessor accessor)
        {
            _context = context;
            _postService = postService;
            _contextAccessor = accessor;
        }

        public async Task<IActionResult> Create(CreatePostView postView, IFormFile image, IFormFile video)
        {
            if (postView == null)
            {
                return new BadRequestResult();
            }

            var post = new Post
            {
                Title = postView.Title,
                TextContent = postView.TextContent,
                CreatedAt = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow
            };

            await ProcessImageAndVideo(post, image, video);

            _context.Context.Posts.Add(post);
            await _context.Context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> Edit(UpdatePostView postView)
        {
            if (postView == null)
            {
                return new BadRequestResult();
            }

            var post = await _context.Context.Posts.FindAsync(postView.Id);
            if (post == null)
            {
                return new NotFoundResult();
            }

            post.Title = postView.Title;
            post.TextContent = postView.TextContent;
            post.ModifiedDate = DateTime.UtcNow;

            await ProcessImageAndVideo(post, postView.Image, postView.Video);

            _context.Context.Update(post);
            await _context.Context.SaveChangesAsync();

            return new OkResult();
        }

        private async Task ProcessImageAndVideo(Post post, IFormFile image, IFormFile video)
        {
            if (image != null)
            {
                post.ImagePath = await SaveFile(image, "images");
            }

            if (video != null)
            {
                post.VideoPath = await SaveFile(video, "videos");
            }
        }

        private async Task<string> SaveFile(IFormFile file, string folderName)
        {
            var path = Path.Combine("wwwroot", folderName, Guid.NewGuid().ToString() + Path.GetExtension(file.FileName));
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return path;
        }

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _context.Context.Posts.FindAsync(id);
            if (post == null)
            {
                return new NotFoundResult();
            }

            await Archive(id);

            return new OkResult();
        }

        public async Task<IActionResult> Archive(int id)
        {
            var post = await _context.Context.Posts.FindAsync(id);
            if (post == null)
            {
                return new NotFoundResult();
            }

            post.IsArchived = true;
            post.DateArchived = DateTime.UtcNow;

            _context.Context.Posts.Update(post);
            await _context.Context.SaveChangesAsync();

            BackgroundJob.Schedule(() => _postService.DeleteArchivedPost(id), TimeSpan.FromDays(7));

            return new OkResult();
        }

        public async Task<Post> FindByIdAsync(int id)
        {
            return await _context.Context.Posts.FindAsync(id);
        }

        public bool PostExists(int id)
        {
            return _context.Context.Posts.Any(e => e.Id == id);
        }

        public async Task<List<Post>> GetAllPostsAsync()
        {
            return await _context.Context.Posts.ToListAsync();
        }
    }
}
