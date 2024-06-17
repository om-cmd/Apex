using DomainLayer.ViewModels.BranchViewModels;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers.BranchControllers
{
    public class BranchController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult BranchList()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateBranch(BranchAdd model)
        {
            return View();
        }

        [HttpPut]
        public IActionResult EditBranch(BranchEdit edit , int id)
        {
            return View();
        }
        [HttpDelete]
        public IActionResult DeleteBranch(int id)
        {
            return View();
        }
    }
}
