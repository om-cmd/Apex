using BusinessLayer.Helper;
using BusinessLayer.Middleware;
using DomainLayer.DataAcess;
using DomainLayer.Interfaces.IRepo.IuserRepos;
using DomainLayer.ViewModels;
using DomainLayer.ViewModels.UserVIewmodels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Models;
using System.Drawing.Imaging;
using System.Drawing;
using DNTCaptcha.Core;
using Microsoft.Extensions.Options;

namespace BusinessLayer.Repositories.UserRepo
{
    public class UserRepository : IUserRepo
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly Authentication _middleware;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DNTCaptchaOptions _captchaOptions;
        private readonly IDNTCaptchaValidatorService _validatorService;

        public UserRepository(IUnitOfWork unit, Authentication authentication, IHttpContextAccessor accessor, IDNTCaptchaValidatorService validatorService, IOptions<DNTCaptchaOptions> options)
        {
            _unitOfWork = unit;
            _middleware = authentication;
            _httpContextAccessor = accessor;
            _validatorService = validatorService;
            _captchaOptions = options == null ? throw new ArgumentNullException(nameof(options)) : options.Value;
        }

        public ApplicationUser RegisterUserAsync(RegisterView model)
        {
            if (_unitOfWork.Context.Users.Any(u => u.Email == model.Email))
            {
                throw new Exception("User already exists");
            }

            string hashedPassword = PasswordHash.Hashing(model.Password);
            var userName = _httpContextAccessor.HttpContext?.User?.Identity?.Name;

            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images");

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            FileInfo fileInfo = new FileInfo(model.ProfilePictureUrl.FileName);
            string fileName = model.ProfilePictureUrl.FileName + fileInfo.Extension;

            string fileNameWithPath = Path.Combine(path, fileName);

            using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
            {
                model.ProfilePictureUrl.CopyTo(stream);
            }


            var application = new ApplicationUser
            {
                FullName = model.FullName,
                Email = model.Email,
                Password = hashedPassword,
                PhoneNumber = model.PhoneNumber,
                Gender = model.Gender,
                ConfirmEmail = model.ConfirmEmail,
                DateOfBirth = model.DateOfBirth,
                CreatedBy = userName
            };

            _unitOfWork.Context.Add(application);
            _unitOfWork.Context.SaveChanges();

            return application;
        }

        public JWTTokenViewModels LoginUserAsync(LoginView model)
        {
            if (!_validatorService.HasRequestValidCaptchaEntry())
            {
                throw new Exception("captcha error");
            }

            var user = _unitOfWork.Context.Users.FirstOrDefault(u => u.Email == model.Email);
            string hashedPassword = PasswordHash.Hashing(model.Password);

            if (user == null || user.Password != hashedPassword)
            {
                throw new Exception("Incorrect password or user does not exist.");
            }
            var token = _middleware.ProvideBothToken(user);
            var login = new JWTTokenViewModels
            {
                AcessTokens = token.AccessToken.ToString(),
                RefreshTokens = token.RefreshToken.ToString(),
            };
            return login;
        }

    }
}
