using DomainLayer.ViewModels.BranchViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DomainLayer.Interfaces.IService.IbranchServices
{
    public interface IBranchService
    {
        public IActionResult BranchList();
        public IActionResult CreateBranch(BranchAdd model);

        public IActionResult EditBranch(BranchEdit edit, int id);

        public IActionResult DeleteBranch(int id);
    }
}
