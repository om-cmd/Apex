using System.Net;
using System.Net.Mail;
using DomainLayer.Interfaces.IService.IEmailSender;
using DomainLayer.ViewModels.PasswordResetViewModels;
using Microsoft.Extensions.Options;

public class EmailSender : IEmailService
{
    private readonly SmtpConfiguration _smtpConfig;

    public EmailSender(IOptions<SmtpConfiguration> smtpConfig)
    {
        _smtpConfig = smtpConfig.Value ?? throw new ArgumentNullException(nameof(smtpConfig), "SMTP configuration is not complete.");
    }

    public async Task SendEmailAsync(string email, string subject, string message)
    {
        if (string.IsNullOrWhiteSpace(email)) throw new ArgumentNullException(nameof(email));
        if (string.IsNullOrWhiteSpace(subject)) throw new ArgumentNullException(nameof(subject));
        if (string.IsNullOrWhiteSpace(message)) throw new ArgumentNullException(nameof(message));

        using (var client = new SmtpClient(_smtpConfig.Server, _smtpConfig.Port))
        using (var mailMessage = new MailMessage())
        {
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(_smtpConfig.Username, _smtpConfig.Password);
            client.EnableSsl = _smtpConfig.EnableSsl;

            mailMessage.From = new MailAddress(_smtpConfig.SenderEmail, _smtpConfig.SenderName);
            mailMessage.Subject = subject;
            mailMessage.Body = message;
            mailMessage.IsBodyHtml = true;
            mailMessage.To.Add(email);

            try
            {
                await client.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to send email: {ex.Message}", ex);
            }
        }
    }

    public async Task SendOtpEmailAsync(string email, string otp)
    {
        var subject = "Your OTP Code";
        var message = $"Your OTP code is: <b>{otp}</b>";
        await SendEmailAsync(email, subject, message);
    }

    public  class OtpGenerator
    {
        public string GenerateOtp(int length = 6)
        {
            var random = new Random();
            var otp = new char[length];
            for (int i = 0; i < length; i++)
            {
                otp[i] = (char)('0' + random.Next(0, 10));
            }
            return new string(otp);
        }
    }

}
