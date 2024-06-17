using DomainLayer.Interfaces.IRepo.IPostRepos;
using DomainLayer.ViewModels.PostViewModels;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> Index()
        {
            var posts = await _postRepo.GetAllPostsAsync();
            return View(posts);
        }
    }
}
