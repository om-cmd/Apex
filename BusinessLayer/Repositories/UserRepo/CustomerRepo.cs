using DomainLayer.DataAcess;
using DomainLayer.Interfaces.IRepo.IuserRepos;
using PresentationLayer.Models;

namespace BusinessLayer.Repositories.UserRepo
{
    public class CustomerRepo : ICustomerRepo
    {
        private readonly IUnitOfWork _unitOfWork;

        public CustomerRepo(IUnitOfWork unit)
        {
            _unitOfWork = unit;
        }

        public ApplicationUser DeleteUserAsync(int id)
        {
            var existingUser = _unitOfWork.Context.Users.FirstOrDefault(x => x.Id == id);
            if (existingUser == null)
            {
                throw new Exception("User does not exist");
            }
            existingUser.IsActive = true;
            _unitOfWork.Context.SaveChanges();
            return existingUser;
        }

        public ApplicationUser EditUserProfileAsync(EditProfileViewModel model, int id)
        {
            var user = _unitOfWork.Context.Users.FirstOrDefault(x => x.Id == id);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images");



            FileInfo fileInfo = new FileInfo(model.ProfilePictureUrl.FileName);
            string fileName = model.ProfilePictureUrl.FileName + fileInfo.Extension;

            string fileNameWithPath = Path.Combine(path, fileName);

            using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
            {
                model.ProfilePictureUrl.CopyTo(stream);
            }

            user.FullName = model.FullName;
            user.Email = model.Email;
            user.Gender = model.Gender;
            user.Bio = model.Bio;
            user.PhoneNumber = model.PhoneNumber;
            user.Roles = model.Roles;

            _unitOfWork.Context.Update(user);
            _unitOfWork.Context.SaveChanges();
            return user;
        }

        public ApplicationUser GetUser(int id)
        {
            return _unitOfWork.Context.Users.FirstOrDefault(x => x.Id == id);
        }

        public ICollection<ApplicationUser> Userlist()
        {
            return _unitOfWork.Context.Users.ToList();
        }
    }
}
