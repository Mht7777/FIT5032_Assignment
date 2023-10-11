using SendGrid.Helpers.Mail;
using SendGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;


namespace FIT5032_Assignment.Utils
{
    public class EmailSender
    {
        private const String API_KEY = "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";

        public void Send(List<string> toEmails, string subject, string contents, HttpPostedFileBase attachment)
        {
            var client = new SendGridClient(API_KEY);
            var from = new EmailAddress("miusi960829@gmail.com", "FIT5032 Example Email User");
            var plainTextContent = contents;
            var htmlContent = "<p>" + contents + "</p>";


            var tos = toEmails.Select(email => new EmailAddress(email,"")).ToList();
            var showAllRecipients = false;
            var msg = MailHelper.CreateSingleEmailToMultipleRecipients(from,
                                                           tos,
                                                           subject,
                                                           plainTextContent,
                                                           htmlContent,
                                                           showAllRecipients
                                                           );

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