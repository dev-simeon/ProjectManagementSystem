using Microsoft.Extensions.Options;
using ProjectManagementSystem.Models;
using System;
using System.Net.Mail;

namespace ProjectManagementSystem.Services
{
    public class EmailService
    {
        private readonly SmtpClient _client;
        private readonly MailMessage _message;
        private readonly SmtpSettings _smtpSettings;

        public EmailService(IOptions<SmtpSettings> smtpSettings)
        {
            _smtpSettings = smtpSettings.Value;
            var smtp = _smtpSettings;
            var (host, port) = ParseEndpoint(smtp.ServerEndpoint);

            _client = new SmtpClient(host, port)
            {
                Credentials = new System.Net.NetworkCredential(smtp.Username, smtp.Password),
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = smtp.UseSSL,
                Timeout = 300000
            };

            _message = new MailMessage
            {
                IsBodyHtml = true,
                From = new MailAddress(smtp.Username, smtp.DisplayName),
            };
        }

        private static (string Host, int Port) ParseEndpoint(string serverEndpoint)
        {
            var port = 25;
            var parts = serverEndpoint.Split(':');

            if (parts.Length > 1)
                port = int.Parse(parts[1]);

            return (parts[0], port);
        }

        public void SendPasswordResetEmail(User user)
        {
            _message.To.Clear(); // Clear previous recipients
            _message.To.Add(user.Email);
            _message.Subject = "Password Reset Request";
            _message.Body = $@"
            PASSWORD RECOVERY

            Dear {user.FirstName},

            You have requested that your password be sent to you by email.

            Username/Email.......: {user.Email}
            Password.............: {user.Password}

            IMPORTANT: To ensure the safety of your account, we strongly recommend
            you change your password the next time you login to the website.";
            try
            {
                _client.Send(_message);
                Console.WriteLine("Password reset email sent successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send password reset email: {ex.Message}");
            }
        }
    }
}
