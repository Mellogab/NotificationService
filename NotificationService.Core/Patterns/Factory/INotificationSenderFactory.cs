using NotificationService.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Core.Patterns.Factory
{
    public interface INotificationSenderFactory
    {
        public INotificationSender CreateSender(PlatformNotificationTypeEnum notificationType);
    }
}
