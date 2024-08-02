using NotificationService.Core.Configurations;
using NotificationService.Core.Patterns.Factory;
using System.Net;
using System.Net.Mail;

namespace NotificationService.Infrastructure.Services
{
    public class EmailService : INotificationSender
    {
        private SMPTConfigurations SMPTConfigurations { get; set; }
        public EmailService(SMPTConfigurations _SMPTConfigurations) => SMPTConfigurations = _SMPTConfigurations;
        

        public async Task<Tuple<bool, string>> Send(string from, string to, string subject, string body, string phoneNumber)
        {
            try
            {
                using (SmtpClient client = new SmtpClient(SMPTConfigurations.Host, SMPTConfigurations.Port))
                {
                    client.EnableSsl = true;
                    client.Credentials = new NetworkCredential(SMPTConfigurations.Login, SMPTConfigurations.Password);

                    MailMessage mailMessage = new MailMessage();
                    mailMessage.From = new MailAddress(from);
                    mailMessage.To.Add(to);
                    mailMessage.Subject = subject;
                    mailMessage.Body = body;

                    client.SendAsync(mailMessage, null);
                }

                return Tuple.Create(true, string.Empty);
            }
            catch (Exception ex)
            {
                return Tuple.Create(false, ex.StackTrace.ToString());
            }
        }
    }
}