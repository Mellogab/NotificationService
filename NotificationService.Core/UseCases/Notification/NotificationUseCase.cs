using AutoMapper;
using NotificationService.Core.Patterns.Factory;
using NotificationService.Core.Services;
using NotificationService.Core.Enums;

namespace NotificationService.Core.UseCases.Notification
{
    public class NotificationUseCase : INotificationUseCase
    {
        private INotificationSenderFactory NotificationSenderFactory;
        private ICacheService CacheService;
        private IMapper IMapper;

        public NotificationUseCase(
            INotificationSenderFactory _notificationSenderFactory,
            ICacheService _cacheService,
            IMapper _iMapper
        ) 
        {
            NotificationSenderFactory = _notificationSenderFactory;
            CacheService = _cacheService;
            IMapper = _iMapper;
        }

        public async Task<bool> Handle(NotificationUseCaseInput message, IOutputPort<NotificationUseCaseOutput> outputPort)
        {
            try
            {
                var cacheKey = $"from:{message.From}-to:{message.To}:email-type:{(int)message.NotificationType}";
                var notification = IMapper.Map<Entities.Notification>(message);

                if(LimitIsExceed(message, cacheKey, notification))
                {
                    outputPort.Handle(new NotificationUseCaseOutput(false, "Limit sent for this recipient considering this type of email was exceed!"));
                    return false;
                }

                var notificationsService = NotificationSenderFactory.CreateSender(PlatformNotificationTypeEnum.EMAIL);
                await notificationsService.Send(notification.From, notification.To, notification.Subject, notification.Body, notification.PhoneNumber);

                UpdateRateLimit(cacheKey, notification);

                outputPort.Handle(new NotificationUseCaseOutput(true));
                return true;
            }
            catch (ArgumentException businessRuleError)
            {
                outputPort.Handle(new NotificationUseCaseOutput(false, businessRuleError.Message));
                return false;
            }
            catch (Exception)
            {
                outputPort.Handle(new NotificationUseCaseOutput(false, "An error was ocurred when the request is processing."));
                return false;
            }
        }

        private bool LimitIsExceed(NotificationUseCaseInput message, string cacheKey, Entities.Notification notification)
        {
            if (CacheService.TryGetValue(cacheKey, out int lastSent))
            {
                var limitsBasedOnNotificationType = notification.GetRateLimits();

                if (lastSent >= limitsBasedOnNotificationType.Item2)
                    return true;
            }

            return false;
        }

        private void UpdateRateLimit(string cacheKey, Entities.Notification notification)
        {
            var limitsBasedOnNotificationType = notification.GetRateLimits();

            if (CacheService.TryGetValue(cacheKey, out int currentCount))
                CacheService.Set(cacheKey, currentCount+1, limitsBasedOnNotificationType.Item1);
            else
                CacheService.Set(cacheKey, 1, limitsBasedOnNotificationType.Item1);
        }
    }
}