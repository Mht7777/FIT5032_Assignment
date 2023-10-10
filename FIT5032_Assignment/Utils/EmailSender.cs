using SendGrid.Helpers.Mail;
using SendGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Net.Mail;
using System.Runtime.InteropServices.ComTypes;

namespace FIT5032_Assignment.Utils
{
    public class EmailSender
    {
        private const String API_KEY = "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";

        public void Send(String toEmail, String subject, String contents, HttpPostedFileBase attachment)
        {

            var client = new SendGridClient(API_KEY);
            var from = new EmailAddress("miusi960829@gmail.com", "FIT5032 Example Email User");
            var to = new EmailAddress(toEmail, "");
            var plainTextContent = contents;
            var htmlContent = "<p>" + contents + "</p>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

            if (attachment != null)
            {
                using (var stream = new MemoryStream())
                {
                    attachment.InputStream.CopyTo(stream);
                    var bytes = stream.ToArray();
                    var file = Convert.ToBase64String(bytes);
                    msg.AddAttachment(attachment.FileName, file);
                }
            }
            var response = client.SendEmailAsync(msg);
        }
    }
}