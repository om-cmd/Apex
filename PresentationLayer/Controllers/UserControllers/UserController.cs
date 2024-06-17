using DNTCaptcha.Core;
using DomainLayer.Interfaces.IService.IuserServices;
using DomainLayer.ViewModels.UserVIewmodels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace PresentationLayer.Controllers.UserControllers
{

    public class UserController : Controller
    {

        private readonly IUserService _userService;


        public UserController(IUserService userService)
        {
            _userService = userService;

        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterView model)
        {
            if (ModelState.IsValid)
            {
                var user = _userService.RegisterUserAsync(model);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginView model)
        {

            if (ModelState.IsValid)
            {
                var user = _userService.LoginUserAsync(model);
                if (user != null)
                {
                    return RedirectToAction(nameof(Login));
                }
                ModelState.AddModelError("", "Invalid login attempt.");
            }
            return View(model);
        }

    }
}
