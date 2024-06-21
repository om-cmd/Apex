using DomainLayer.Interfaces.IRepo.IPostRepos;
using DomainLayer.ViewModels.PostViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using PresentationLayer.Models;

namespace PresentationLayer.Controllers
{
    public class PostController : Controller
    {
        private readonly IPostRepo _postRepo;

        public PostController(IPostRepo repo)
        {
            _postRepo = repo;
        }

        [HttpGet]
        [OutputCache(Duration = 60)]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePostView postView, IFormFile image, IFormFile video)
        {
            if (ModelState.IsValid)
            {
                await _postRepo.Create(postView, image, video);
                return RedirectToAction(nameof(Index));
            }
            return View(postView);
        }


        [HttpPut]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdatePostView postView)
        {
            if (id != postView.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _postRepo.Edit(postView);
                return RedirectToAction(nameof(Index));
            }
            return View(postView);
        }

        [HttpDelete, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _postRepo.FindByIdAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            await _postRepo.Archive(id);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [OutputCache(Duration = 60)]

        public async Task<IActionResult> Archive(int id)
        {
            var post = await _postRepo.FindByIdAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            await _postRepo.Archive(id);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [OutputCache(Duration = 60)]
        public async Task<IActionResult> Index()
        {
            var posts = await _postRepo.GetAllPostsAsync();
            return View(posts);
        }

        [HttpPost]
        [OutputCache(Duration =60)]
        public async Task<IActionResult> AddComment(int postId, string content, int userId)
        {
            var comment = await _postRepo.AddComment(postId, content, userId);
            return View(comment);
        }
        [HttpPost]
        [OutputCache(Duration = 60)]
        public async Task<IActionResult> AddLike(int postId, int userId)
        {
            var like = await _postRepo.AddLike(postId, userId);
            return RedirectToAction(nameof(Index));
        }
    }
}
