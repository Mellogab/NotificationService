using System.Net.Mail;

namespace NotificationService.Core.Services
{
    public interface IEmailService
    {
        public Task<Tuple<bool, string>> SendEmail(string from, string to, string subject, string body);
    }
}