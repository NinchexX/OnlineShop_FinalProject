using Microsoft.AspNetCore.Identity.UI.Services;
using Project.Interfaces;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace Project.Services
{
    public class EmailSender : Interfaces.IEmailSender
    {
        public async Task SendEmailAsync(string email, string token)
        {
            byte[] tokenBytes = System.Text.Encoding.UTF8.GetBytes(token);
            var encodedToken = Convert.ToBase64String(tokenBytes);
            var appPasword = "xqkb uvbo ocfx tvfy";
            var from = "ninchexxdarcho@gmail.com";
            var message = new MailMessage()
            {
                From = new MailAddress(from),
                Subject = "Email Verification Token",
                Body = $"Your Email Verification Token Is https://localhost:7002/api/Account/Verify/{email}/{encodedToken}",
                IsBodyHtml = false,
            };

            message.To.Add(email);

            using var smtp = new SmtpClient("smtp.gmail.com",587)
            {
                Credentials = new NetworkCredential(from, appPasword),
                EnableSsl = true
            };

            await smtp.SendMailAsync(message);
        }
    }
}
