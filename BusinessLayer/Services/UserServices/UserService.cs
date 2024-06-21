using DomainLayer.Interfaces.IRepo.IuserRepos;
using DomainLayer.Interfaces.IService.IuserServices;
using DomainLayer.ViewModels;
using DomainLayer.ViewModels.UserVIewmodels;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Models;

namespace BusinessLayer.Services.UserServices
{
    public class UserService : IUserService
    {
        private readonly IUserRepo _userRepo;

        public UserService(IUserRepo userRepo)
        {
            _userRepo = userRepo;
        }

       

        public LoginResponse LoginUserAsync(LoginView model)
        {
            return _userRepo.LoginUserAsync(model);
        }

        public ApplicationUser RegisterUserAsync(RegisterView model)
        {
            return _userRepo.RegisterUserAsync(model);
        }


    }
}
