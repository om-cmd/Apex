using DomainLayer.ViewModels;
using DomainLayer.ViewModels.UserVIewmodels;
using PresentationLayer.Models;

namespace DomainLayer.Interfaces.IRepo.IuserRepos
{
    public interface IUserRepo
    {

        public ApplicationUser RegisterUserAsync(RegisterView model);
        public JWTTokenViewModels LoginUserAsync(LoginView model);
      

    }
}
