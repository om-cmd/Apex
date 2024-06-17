using Microsoft.AspNetCore.Mvc;
using DomainLayer.Interfaces.IService.IFriendRequestServices;
using DomainLayer.ViewModels.FriendRequestViewModels;

namespace PresentationLayer.Controllers.FriendRequestControllers
{
    public class FriendRequestController : Controller
    {
        private readonly IFriendRequestService _friendRequestService;

        public FriendRequestController(IFriendRequestService friendRequestService)
        {
            _friendRequestService = friendRequestService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Details(int userId)
        {
            var requests = await _friendRequestService.GetFriendRequestsAsync(userId);
            return View(requests);
        }

        [HttpGet]
        public IActionResult SendFriendRequest()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendFriendRequest(SendFriendRequestView request)
        {
            if (ModelState.IsValid)
            {
                await _friendRequestService.SendFriendRequestAsync(request.SenderId, request.ReceiverId);
                return RedirectToAction("Index");
            }
            return View(request);
        }

        [HttpPost]
        public async Task<IActionResult> AcceptFriendRequest(int requestId)
        {
            await _friendRequestService.AcceptFriendRequestAsync(requestId);
            return RedirectToAction("Details");
        }

        [HttpPost]
        public async Task<IActionResult> RejectFriendRequest(int requestId)
        {
            await _friendRequestService.RejectFriendRequestAsync(requestId);
            return RedirectToAction("Details");
        }
    }

   
}
