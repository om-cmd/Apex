using DomainLayer.DataAcess;
using DomainLayer.Interfaces.IRepo.IbranchRepos;
using DomainLayer.ViewModels.BranchViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using PresentationLayer.Models;
using System.Drawing;

namespace BusinessLayer.Repositories.BranchRepo
{
    public class BranchRepo : IBranchRepo
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public BranchRepo(IUnitOfWork unitOfWork, IHttpContextAccessor accessor)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = accessor;
        }

        public ICollection<Branch> BranchList()
        {
            var list = _unitOfWork.Context.Branches.Where(x=>x.IsActive==true).ToList();
            _unitOfWork.Context.SaveChanges();
            return list;
        }


        Branch IBranchRepo.CreateBranch(BranchAdd model)
        {
            var creator = _httpContextAccessor.HttpContext?.User?.Identity?.Name;
            var branch = new Branch
            {
                Name = model.Name,
                DateCreated = DateTime.Now,
                CreatedBy = creator,

            };
            _unitOfWork.Context.Add(branch);
            _unitOfWork.Context.SaveChanges();
            return branch;
        }

        Branch IBranchRepo.EditBranch(BranchEdit edit, int id)
        {
            var branchId = _unitOfWork.Context.Branches.FirstOrDefault(x => x.TenantId == id);
            if(branchId==null)
            {
                throw new Exception("Id not found");
            }
            var creator = _httpContextAccessor.HttpContext?.User?.Identity?.Name;

            var branch = new Branch
            {
                CreatedBy = creator,
                Name = edit.Name,
                ModifiedDate = DateTime.Now,

            };
            _unitOfWork.Context.Update(edit);
            _unitOfWork.Context.SaveChanges();
            return branch;
        }

        Branch IBranchRepo.DeleteBranch(int id)
        {
            var branchId = _unitOfWork.Context.Branches.FirstOrDefault(x => x.TenantId == id);
            branchId.IsActive=false;
            _unitOfWork.Context.SaveChanges();
            return branchId;
        }
    }
}
