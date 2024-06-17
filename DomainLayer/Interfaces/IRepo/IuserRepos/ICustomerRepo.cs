using PresentationLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Interfaces.IRepo.IuserRepos
{
    public interface ICustomerRepo
    {
        public ICollection<ApplicationUser> Userlist();
        ApplicationUser GetUser(int id);
        public ApplicationUser EditUserProfileAsync(EditProfileViewModel model, int id);
        public ApplicationUser DeleteUserAsync(int id);
    }
}
