using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SendGrid
{
    public class Grid
    {
        public async Task Execute()
        {
            var apiKey = Environment.GetEnvironmentVariable("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");
            //var apiKey = "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");
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("EIM_IPSD@cat.com", "manohar_yeresi@srivensolutions.com");
            var subject = "Sending with SendGrid is Fun";
            var to = new EmailAddress("test@example.com", "Example User");
            var plainTextContent = "and easy to do anywhere, even with C#";
            var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }
    }
}
