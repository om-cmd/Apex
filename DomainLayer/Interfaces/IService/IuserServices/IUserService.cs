using DomainLayer.ViewModels;
using DomainLayer.ViewModels.UserVIewmodels;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Models;

namespace DomainLayer.Interfaces.IService.IuserServices
{
    public interface IUserService
    {

        public ApplicationUser RegisterUserAsync(RegisterView model);
        public JWTTokenViewModels LoginUserAsync(LoginView model);
     

    }
}
