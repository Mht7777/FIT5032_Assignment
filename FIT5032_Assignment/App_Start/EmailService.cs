using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace FIT5032_Assignment.Utility
{
    public class EmailService : IIdentityMessageService
    {
        public async Task SendAsync(IdentityMessage message)
        {
            await configSendGridasync(message);
        }

        private async Task configSendGridasync(IdentityMessage message)
        {
            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("UltraSound@notify.com", "Ultra Sound"),
                Subject = message.Subject,
                PlainTextContent = message.Body,
                HtmlContent = message.Body
            };
            msg.AddTo(new EmailAddress(message.Destination));
            await client.SendEmailAsync(msg);
        }
    }

}