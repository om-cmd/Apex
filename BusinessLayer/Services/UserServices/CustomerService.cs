using DomainLayer.Interfaces.IRepo.IuserRepos;
using DomainLayer.Interfaces.IService.IuserServices;
using PresentationLayer.Models;

namespace BusinessLayer.Services.UserServices
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepo _customerRepo;

        public CustomerService(ICustomerRepo customerRepo)
        {
            _customerRepo = customerRepo;
        }
        public ApplicationUser DeleteUserAsync(int id)
        {
            return _customerRepo.DeleteUserAsync(id);
        }

        public ApplicationUser EditUserProfileAsync(EditProfileViewModel model, int id)
        {
            return _customerRepo.EditUserProfileAsync(model, id);
        }

        public ApplicationUser GetUser(int id)
        {
            return _customerRepo.GetUser(id);
        }

        public ICollection<ApplicationUser> Userlist()
        {
            return _customerRepo.Userlist();
        }
    }
}
