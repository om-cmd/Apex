using Microsoft.AspNetCore.Mvc;
using DomainLayer.Interfaces.IService.IEmailSender;
using static EmailSender;

public class PasswordResetController : Controller
{
    private readonly IEmailService _emailRepo;
    private readonly OtpGenerator _otpGenerator;

    private static readonly Dictionary<string, string> otpStorage = new Dictionary<string, string>();

    public PasswordResetController(IEmailService emailRepo, OtpGenerator otpGenerator)
    {
        _emailRepo = emailRepo;
        _otpGenerator = otpGenerator;
    }

    [HttpPost]
    [Route("api/password-reset/request")]
    public async Task<IActionResult> RequestPasswordReset(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return BadRequest("Email is required.");
        }

        var otp = _otpGenerator.GenerateOtp();

        otpStorage[email] = otp;

        await _emailRepo.SendOtpEmailAsync(email, otp);

        return Ok("OTP sent to your email.");
    }

    [HttpPost]
    [Route("api/password-reset/verify")]
    public IActionResult VerifyOtp(string email, string otp)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(otp))
        {
            return BadRequest("Email and OTP are required.");
        }

        // Retrieve and validate the OTP from the dictionary (In production, retrieve from database or cache)
        if (!otpStorage.TryGetValue(email, out var storedOtp) || storedOtp != otp)
        {
            return BadRequest("Invalid OTP.");
        }

        // Remove OTP after successful verification
        otpStorage.Remove(email);

        // Proceed with password reset process
        return Ok("OTP verified. Proceed to reset password.");
    }
}
