using DomainLayer.Interfaces.IService.IuserServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using PresentationLayer.Models;

namespace PresentationLayer.Controllers.UserControllers
{
    [Authorize]
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [OutputCache(Duration = 60)]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [OutputCache(Duration = 60)]

        public IActionResult UserList()
        {
            var list = _customerService.Userlist();
            return View(list);
        }

        [HttpPut]
        public IActionResult Edit(int id, EditProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _customerService.EditUserProfileAsync(model, id);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }


        [HttpDelete, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var user = _customerService.DeleteUserAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
