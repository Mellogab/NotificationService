using NotificationService.Core.Configurations;
using NotificationService.Core.Enums;
using NotificationService.Core.Patterns.Factory;
using NotificationService.Infrastructure.Services;

namespace NotificationService.Infrastructure.Patterns.Factory
{
    public class NotificationSenderFactory : INotificationSenderFactory
    {

        private SMPTConfigurations SMPTConfigurations { get; set; }
        public NotificationSenderFactory(SMPTConfigurations _SMPTConfigurations) => SMPTConfigurations = _SMPTConfigurations;

        public INotificationSender CreateSender(PlatformNotificationTypeEnum notificationType)
        {
            switch (notificationType)
            {
                case PlatformNotificationTypeEnum.EMAIL:
                    return new EmailService(SMPTConfigurations);
                case PlatformNotificationTypeEnum.WHATSAPP:
                    break; throw new NotImplementedException();
                case PlatformNotificationTypeEnum.TELEGRAM:
                    break; throw new NotImplementedException();
                case PlatformNotificationTypeEnum.PUSH_NOTIFICATION:
                    break; throw new NotImplementedException();
                default:
                    break;
            }

            throw new NotImplementedException();
        }
    }
}