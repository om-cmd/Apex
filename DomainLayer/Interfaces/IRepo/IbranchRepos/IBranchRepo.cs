using DomainLayer.ViewModels.BranchViewModels;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Models;

namespace DomainLayer.Interfaces.IRepo.IbranchRepos
{
    public interface IBranchRepo
    {
        ICollection<Branch> BranchList();
        Branch CreateBranch(BranchAdd model);

        Branch EditBranch(BranchEdit edit, int id);

        Branch DeleteBranch(int id);

    }
}
