using NotificationService.Core.Enums;
using static NotificationService.Core.IUseCaseRequest;

namespace NotificationService.Core.UseCases.Notification
{
    public class NotificationUseCaseInput : IUseCaseRequest<NotificationUseCaseOutput>
    {
        public required NotificationTypeEnum NotificationType { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string PhoneNumber { get; set; }
    }
}
