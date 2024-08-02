using AutoMapper;
using NotificationService.Core.Entities;
using NotificationService.Core.Enums;
using NotificationService.Core.Patterns.Factory;
using NotificationService.Core.Services;
using NotificationService.Core.UseCases.Notification;
using Moq;
using Moq.AutoMock;
using _NotificationService.Presenters;
using NotificationService.Core.Configurations;

namespace NotificationService.Test.UseCases
{
    public class NotificationUseCasesTests
    {
        private readonly Mock<INotificationSenderFactory> mockFactory;
        private readonly Mock<ICacheService> mockCache;
        private readonly Mock<IMapper> mockMapper;
        
        public NotificationUseCasesTests()
        {
            mockFactory = new Mock<INotificationSenderFactory>();
            mockCache = new Mock<ICacheService>();
            mockMapper = new Mock<IMapper>();    
        }

        [InlineData(NotificationTypeEnum.NEWS, "emailA@gmail.com", "emailB@gmail.com", "C# Template Sender", "It works", "", 1)]
        [InlineData(NotificationTypeEnum.STATUS, "emailA@gmail.com", "emailB@gmail.com", "C# Template Sender", "It works", "", 2)]
        [InlineData(NotificationTypeEnum.MARKETING, "emailA@gmail.com", "emailB@gmail.com", "C# Template Sender", "It works", "", 3)]
        public async Task Handle_NotificationWithinLimit_NotShouldSend(NotificationTypeEnum notificationType, string from, string to, string subject, string body, string phoneNumber, int existingOnCache)
        {
            // Arrange
            var _mocker = new AutoMocker();
            _mocker.CreateInstance<NotificationUseCase>();

            var presenter = new DefaultPresenter<NotificationUseCaseOutput>();

            var smtpConfigurarions = new SMPTConfigurations();
            smtpConfigurarions.Login = "login";
            smtpConfigurarions.Host = "host";
            smtpConfigurarions.Password = "password";
            smtpConfigurarions.Port = 587;

            _mocker
              .GetMock<INotificationSenderFactory>()
              .Setup(x => x.CreateSender(
                  It.IsAny<PlatformNotificationTypeEnum>()
                ))
              .Returns(() => new EmailServiceMock(smtpConfigurarions));

            _mocker
              .GetMock<INotificationSender>()
              .Setup(x => x.Send(
                  It.IsAny<string>(),
                  It.IsAny<string>(),
                  It.IsAny<string>(),
                  It.IsAny<string>(),
                  It.IsAny<string>()
                ))
              .Returns(() => Task.FromResult(Tuple.Create(true, string.Empty)));

            int existingCount = 1;

            _mocker
              .GetMock<ICacheService>()
              .Setup(x => x.TryGetValue(
                  It.IsAny<string>(),
                  out existingCount
                ))
              .Returns(() => true);

            _mocker
              .GetMock<ICacheService>()
              .Setup(x => x.Set<int>(
                  It.IsAny<string>(),
                  It.IsAny<int>(),
                  It.IsAny<TimeSpan>()
                ));

            _mocker
              .GetMock<IMapper>()
              .Setup(x => x.Map<Notification>(
                  It.IsAny<NotificationUseCaseInput>()
                ))
              .Returns(() => new Notification(NotificationTypeEnum.NEWS, from, to, subject, body, string.Empty));

            var notificationOutputpresenter = new Core.Presenters.DefaultPresenter<NotificationUseCaseOutput>();
            var notificationUseCaseOutputpresenter = new Core.Presenters.DefaultPresenter<NotificationUseCaseOutput>();
            notificationUseCaseOutputpresenter.Handle(new NotificationUseCaseOutput(true));

            // Act
            var sut = new NotificationUseCase(
                _mocker.GetMock<INotificationSenderFactory>().Object,
                _mocker.GetMock<ICacheService>().Object,
                _mocker.GetMock<IMapper>().Object
            );

            var useCaseInput = new NotificationUseCaseInput()
            {
                From = "emailA@gmail.com",
                To = "emaiB@gmail.com",
                Subject = "C# Email Sender",
                Body = "It works",
                NotificationType = NotificationTypeEnum.NEWS,
                PhoneNumber = string.Empty
            };


            await sut.Handle(useCaseInput, presenter);

            var result = presenter.GetJsonResult();
            var executeTransferOutput = (NotificationUseCaseOutput)result.Value;

            // Assert
            Assert.Equal(false, executeTransferOutput.Success);
            Assert.Equal(executeTransferOutput.Message, "Limit sent for this recipient considering this type of email was exceed!");
        }

        
        [InlineData(NotificationTypeEnum.NEWS, "emailA@gmail.com", "emailB@gmail.com", "C# Template Sender", "It works", "", 0)]
        [InlineData(NotificationTypeEnum.STATUS, "emailA@gmail.com", "emailB@gmail.com", "C# Template Sender", "It works", "", 0)]
        [InlineData(NotificationTypeEnum.MARKETING, "emailA@gmail.com", "emailB@gmail.com", "C# Template Sender", "It works", "", 0)]
        [InlineData(NotificationTypeEnum.STATUS, "emailA@gmail.com", "emailB@gmail.com", "C# Template Sender", "It works", "", 1)]
        [InlineData(NotificationTypeEnum.MARKETING, "emailA@gmail.com", "emailB@gmail.com", "C# Template Sender", "It works", "", 1)]
        [InlineData(NotificationTypeEnum.MARKETING, "emailA@gmail.com", "emailB@gmail.com", "C# Template Sender", "It works", "", 2)]
        public async Task Handle_NotificationWithinLimit_ShouldSend(NotificationTypeEnum notificationType, string from, string to, string subject, string body, string phoneNumber, int existingOnCache)
        {
            // Arrange
            var _mocker = new AutoMocker();
            _mocker.CreateInstance<NotificationUseCase>();

            var presenter = new DefaultPresenter<NotificationUseCaseOutput>();

            var smtpConfigurarions = new SMPTConfigurations();
            smtpConfigurarions.Login = "login";
            smtpConfigurarions.Host = "host";
            smtpConfigurarions.Password = "password";
            smtpConfigurarions.Port = 587;

            _mocker
              .GetMock<INotificationSenderFactory>()
              .Setup(x => x.CreateSender(
                  It.IsAny<PlatformNotificationTypeEnum>()
                ))
              .Returns(() => new EmailServiceMock(smtpConfigurarions));

            _mocker
              .GetMock<INotificationSender>()
              .Setup(x => x.Send(
                  It.IsAny<string>(),
                  It.IsAny<string>(),
                  It.IsAny<string>(),
                  It.IsAny<string>(),
                  It.IsAny<string>()
                ))
              .Returns(() => Task.FromResult(Tuple.Create(true, string.Empty)));

            _mocker
              .GetMock<ICacheService>()
              .Setup(x => x.TryGetValue(
                  It.IsAny<string>(),
                  out existingOnCache
                ))
              .Returns(() => true);

            _mocker
              .GetMock<ICacheService>()
              .Setup(x => x.Set<int>(
                  It.IsAny<string>(),
                  It.IsAny<int>(),
                  It.IsAny<TimeSpan>()
                ));

            _mocker
              .GetMock<IMapper>()
              .Setup(x => x.Map<Notification>(
                  It.IsAny<NotificationUseCaseInput>()
                ))
              .Returns(() => new Notification(NotificationTypeEnum.NEWS, from, to, subject, body, string.Empty));

            var notificationOutputpresenter = new Core.Presenters.DefaultPresenter<NotificationUseCaseOutput>();
            var notificationUseCaseOutputpresenter = new Core.Presenters.DefaultPresenter<NotificationUseCaseOutput>();
            notificationUseCaseOutputpresenter.Handle(new NotificationUseCaseOutput(true));

            // Act
            var sut = new NotificationUseCase(
                _mocker.GetMock<INotificationSenderFactory>().Object,
                _mocker.GetMock<ICacheService>().Object,
                _mocker.GetMock<IMapper>().Object
            );

            var useCaseInput = new NotificationUseCaseInput()
            {
                From = from,
                To = to,
                Subject = subject,
                Body = body,
                NotificationType = NotificationTypeEnum.NEWS,
                PhoneNumber = string.Empty
            };

            await sut.Handle(useCaseInput, presenter);

            var result = presenter.GetJsonResult();
            var executeTransferOutput = (NotificationUseCaseOutput)result.Value;

            // Assert
            Assert.Equal(true, executeTransferOutput.Success);
        }
    }

    public class EmailServiceMock : INotificationSender
    {
        private SMPTConfigurations SMPTConfigurations { get; set; }
        public EmailServiceMock(SMPTConfigurations _SMPTConfigurations) => SMPTConfigurations = _SMPTConfigurations;

        public async Task<Tuple<bool, string>> Send(string from, string to, string subject, string body, string phoneNumber)
        {
            return Tuple.Create(true, string.Empty);
        }
    }
}
