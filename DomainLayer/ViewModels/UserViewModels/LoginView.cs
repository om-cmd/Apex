namespace DomainLayer.ViewModels.UserVIewmodels
{
    public class LoginView
    {
        public string Email { get; set; }
        public string Password { get; set; }

    }
    public class LoginResponse
    {
        public JWTTokenViewModels TokenInfo { get; set; }
        public string RedirectUrl { get; set; }
    }
    public class JWTTokenViewModels
    {
        public string AcessTokens { get; set; }
        public string RefreshTokens { get; set; }
    }

}

