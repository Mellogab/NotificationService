using NotificationService.Core.Enums;

namespace NotificationService.Core.Patterns.Factory
{
    public interface INotificationSender
    {
        Task<Tuple<bool, string>> Send(string from, string to, string subject, string body, string phoneNumber);
    }
}
