using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;

namespace WebShopApp.Services
{

    public class AuthMessageSender : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string message, string userName)
        {
            // Plug in your email service here to send an email.
            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(new MailboxAddress("Emil Hedström Södergren", "kontakt@emilsodergren.se"));
            mimeMessage.To.Add(new MailboxAddress(userName, email));
            mimeMessage.Subject = subject;

            mimeMessage.Body = new TextPart("plain")
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                await client.ConnectAsync("smtp01.binero.se", 587, false);
                client.AuthenticationMechanisms.Remove("XOAUTH2");

                await client.AuthenticateAsync("kontakt@emilsodergren.se", "Emil1100");

                await client.SendAsync(mimeMessage);
                await client.DisconnectAsync(true);
            }
        }
    }
}
