using DomainLayer.Interfaces.IService.IuserServices;
using DomainLayer.ViewModels.UserVIewmodels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

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
        [OutputCache(Duration = 60)]
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
        [OutputCache(Duration = 60)]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginView model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var response = _userService.LoginUserAsync(model);
                    if (response != null)
                    {
                        Response.Cookies.Append("AccessToken", response.TokenInfo.AcessTokens, new CookieOptions
                        {
                            HttpOnly = true, // Prevents JavaScript access
                            Secure = true, // Ensures cookies are sent over HTTPS
                            SameSite = SameSiteMode.Strict, // Prevents CSRF attacks
                        });
                        Response.Cookies.Append("RefreshToken", response.TokenInfo.RefreshTokens, new CookieOptions
                        {
                            HttpOnly = true,
                            Secure = true,
                            SameSite = SameSiteMode.Strict,
                        });

                        return Redirect(response.RedirectUrl);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View(model);
        }

      
    }
}
