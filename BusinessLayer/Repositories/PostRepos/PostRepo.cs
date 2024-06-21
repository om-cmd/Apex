using DomainLayer.DataAcess;
using DomainLayer.Interfaces.IRepo.IPostRepos;
using DomainLayer.ViewModels.PostViewModels;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using PresentationLayer.Models;
using BusinessLayer.Services.NotificationServices;
using BusinessLayer.Services.LikeServices;
using BusinessLayer.Services.CommentService;

namespace BusinessLayer.Repositories.PostRepos
{
    public class PostRepo : IPostRepo
    {
        private readonly IUnitOfWork _context;
        private readonly PostService _postService;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IHubContext<NotificationHub> _notificationHubContext;
        private readonly IHubContext<LikeHub> _likeHubContext;
        private readonly IHubContext<SendCommentHub> _commentHubContext;

        public PostRepo(IUnitOfWork context, PostService postService, IHttpContextAccessor accessor, IHubContext<NotificationHub> notificationHubContext, IHubContext<LikeHub> likeHubContext, IHubContext<SendCommentHub> commentHubContext)
        {
            _context = context;
            _postService = postService;
            _contextAccessor = accessor;
            _notificationHubContext = notificationHubContext;
            _likeHubContext = likeHubContext;
            _commentHubContext = commentHubContext;
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

        public async Task<IActionResult> AddLike(int postId, int userId)
        {
            var post = await _context.Context.Posts.FindAsync(postId);
            if (post == null)
            {
                return new NotFoundResult();
            }

            var like = new Like
            {
                PostId = postId,
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Context.Likes.Add(like);
            await _context.Context.SaveChangesAsync();

            var postOwnerUserId = post.UserId;
            var message = $"User {userId} liked your post {postId}";

            var notification = new Notification
            {
                Message = message,
                CreatedAt = DateTime.UtcNow,
                IsRead = false,
                UserId = postOwnerUserId
            };
            _context.Context.Notifications.Add(notification);
            await _context.Context.SaveChangesAsync();

            await _notificationHubContext.Clients.User(postOwnerUserId.ToString()).SendAsync("ReceiveNotification", message);
            await _likeHubContext.Clients.User(postOwnerUserId.ToString()).SendAsync("ReceiveLike", postId, userId);

            return new OkResult();
        }

        public async Task<IActionResult> AddComment(int postId, string content, int userId)
        {
            var post = await _context.Context.Posts.FindAsync(postId);
            if (post == null)
            {
                return new NotFoundResult();
            }

            var comment = new Comment
            {
                Content = content,
                CreatedAt = DateTime.UtcNow,
                PostId = postId,
                UserId = userId
            };

            _context.Context.Comments.Add(comment);
            await _context.Context.SaveChangesAsync();

            var postOwnerUserId = post.UserId;
            var message = $"User {userId} commented on your post {postId}: {content}";

            var notification = new Notification
            {
                Message = message,
                CreatedAt = DateTime.UtcNow,
                IsRead = false,
                UserId = postOwnerUserId
            };
            _context.Context.Notifications.Add(notification);
            await _context.Context.SaveChangesAsync();

            await _notificationHubContext.Clients.User(postOwnerUserId.ToString()).SendAsync("ReceiveNotification", message);
            await _commentHubContext.Clients.User(postOwnerUserId.ToString()).SendAsync("ReceiveCommentNotification", message);

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
    }
}
