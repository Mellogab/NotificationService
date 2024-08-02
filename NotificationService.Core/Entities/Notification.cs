using NotificationService.Core.Enums;

namespace NotificationService.Core.Entities
{
    public class Notification
    {
        public Notification(NotificationTypeEnum notificationType, string from, string to, string subject, string body, string phoneNumber) 
        { 
            
            if (string.IsNullOrEmpty(phoneNumber) && (string.IsNullOrEmpty(from) || string.IsNullOrEmpty(to) || string.IsNullOrEmpty(subject) || string.IsNullOrEmpty(body)))
                throw new ArgumentNullException("You must be fill the required fields to send an email");

            this.From = from;
            this.To = to;
            this.Subject = subject;
            this.Body = body;
            this.PhoneNumber = phoneNumber;
            this.NotificationType = notificationType;
        }

        public NotificationTypeEnum NotificationType { get; set; }

        public string From { get; private set; }
        public string To { get; private set; }
        public string Subject { get; private set; }
        public string Body { get; private set; }
        public string PhoneNumber { get; private set; }

        public (TimeSpan, int) GetRateLimits()
        {
            switch (NotificationType)
            {
                case NotificationTypeEnum.STATUS:
                    return (TimeSpan.FromMinutes(1), 2);
                case NotificationTypeEnum.NEWS:
                    return (TimeSpan.FromDays(1), 1);
                case NotificationTypeEnum.MARKETING:
                    return (TimeSpan.FromHours(1), 3);
                default:
                    throw new NotSupportedException($"Unsupported notification type: {NotificationType}");
            }
        }
    }
}