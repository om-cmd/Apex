﻿namespace DomainLayer.Interfaces.IService.IEmailSender
{
    public interface IEmailService
    {
        Task SendEmailAsync(string email, string subject, string message);
        Task SendOtpEmailAsync(string email, string otp);
    }
}
