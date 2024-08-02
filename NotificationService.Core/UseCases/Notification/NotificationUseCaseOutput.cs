namespace NotificationService.Core.UseCases.Notification
{
    public class NotificationUseCaseOutput : UseCaseResponseMessage
    {
        public NotificationUseCaseOutput(
            bool success,
            string message = null,
            string error = null
        ) : base(success, message, error)
        {

        }
    }
}
