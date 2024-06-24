using ApplicationSecretKeys;

namespace EmailSender.Services
{
    public class EmailService : IEmailService
    {
        // private readonly SmtpClient _smtpClient;

        public EmailService()
        {
            // var emailKeyValues = new EmailSenderKeyValues();

            // _smtpClient = new SmtpClient("smtp.gmail.com", 587)
            // {
            //     EnableSsl = true,
            //     Timeout = 50000,
            //     UseDefaultCredentials = false,
            //     Credentials = new NetworkCredential(emailKeyValues.From, emailKeyValues.SecretKey)
            // };
        }

        public void SendEmail(string email, string subject) 
        {
            // MailMessage message = new MailMessage(emailFrom, emailTo);

            // message.Subject = subject;

            // if (!string.IsNullOrWhiteSpace(emailCopy)) 
            // {
            //     MailAddress copy = new MailAddress(emailCopy);

            //     message.CC.Add(copy);
            // }

            // message.Body = emailHtml;

            // message.IsBodyHtml = true;

            // client.Send(message);
        }
    }
}