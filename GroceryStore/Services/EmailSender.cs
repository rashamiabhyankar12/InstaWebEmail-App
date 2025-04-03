using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace GroceryStore.Services
{
    public class EmailSender : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var fromEmail = "rashamirox@gmail.com";
            //var fromPassword = @":?\P~e8&+(P6Bkm2";
            var fromPassword = "rzyg mfzg eurb dkqy";
            MailMessage message = new();
            message.From = new MailAddress(fromEmail);
            message.To.Add(new MailAddress(email));
            message.Subject = subject;
            message.Body = htmlMessage;
            message.IsBodyHtml = true;

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(fromEmail, fromPassword),
                EnableSsl = true
            };

            smtpClient.Send(message);
        }
    }
}
