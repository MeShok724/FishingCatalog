using MimeKit;
using MailKit.Net.Smtp;
using System.Threading.Tasks;

namespace FishingCatalog.msNotification.Infrastructure
{
    public class EmailService
    {
        public static async Task SendEmailAsync(string email, string subject, string message)
        {
            using var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Администрация сайта", "mishakopytok@gmail.com"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 465, true);

                await client.AuthenticateAsync("mishakopytok@gmail.com", "arxe oqmp ohhv zkmz");

                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }
    }
}
