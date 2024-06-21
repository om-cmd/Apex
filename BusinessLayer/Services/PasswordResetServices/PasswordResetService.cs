using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using BusinessLayer.Helper;
using DomainLayer.DataAcess;
using DomainLayer.ViewModels.PasswordResetViewModels;
using DomainLayer.Interfaces.IService.IEmailSender;
using Microsoft.Extensions.Options;
using static EmailSender;

public class PasswordResetService
{
    private readonly IUnitOfWork _context;
    private readonly IEmailService _emailSender;
    private readonly OtpGenerator _otpService;

    public PasswordResetService(IUnitOfWork context, IEmailService emailSender, IOptions<SmtpConfiguration> smtpConfig, OtpGenerator otpService)
    {
        _context = context;
        _otpService = otpService;
        _emailSender = new EmailSender(smtpConfig);
    }

    public async Task SendPasswordResetOtpAsync(string email)
    {
        var user = await _context.Context.Users.SingleOrDefaultAsync(u => u.Email == email);
        if (user == null)
        {
            throw new Exception("User not found.");
        }

        string otp = _otpService.GenerateOtp();

        user.PasswordResetToken = otp;
        user.PasswordResetTokenExpiry = DateTime.UtcNow.AddMinutes(1);

        await _context.Context.SaveChangesAsync();

        var subject = "Your Password Reset OTP Code";
        var message = $"Your OTP code is: {otp}. It is valid for the next 10 minutes.";

        await _emailSender.SendEmailAsync(email, subject, message);
    }

    public async Task<bool> VerifyOtpAsync(string email, string otp)
    {
        var user = await _context.Context.Users.SingleOrDefaultAsync(u => u.Email == email);
        if (user == null || user.PasswordResetToken != otp || user.PasswordResetTokenExpiry < DateTime.UtcNow)
        {
            return false;
        }

        // OTP is valid, clear it
        user.PasswordResetToken = null;
        user.PasswordResetTokenExpiry = null;
        await _context.Context.SaveChangesAsync();

        return true;
    }

    public async Task ResetPasswordAsync(ResetPasswordModel reset)
    {
        var user = await _context.Context.Users.SingleOrDefaultAsync(u => u.Email == reset.Email);
        if (user == null)
        {
            throw new Exception("User not found.");
        }

        string hashedPassword = PasswordHash.Hashing(reset.NewPassword);
        user.Password = hashedPassword;

        await _context.Context.SaveChangesAsync();
    }
}
