using NotificationService.Core.Entities;
using NotificationService.Core.Enums;

namespace NotificationService.Test.Entities
{
    public class NotificationTests
    {
        [Fact]
        public void Constructor_ValidParameters_ShouldCreateInstance()
        {
            // Arrange
            var notificationType = NotificationTypeEnum.STATUS;
            var from = "from@example.com";
            var to = "to@example.com";
            var subject = "Test Subject";
            var body = "Test Body";
            var phoneNumber = "1234567890";

            // Act
            var notification = new Notification(notificationType, from, to, subject, body, phoneNumber);

            // Assert
            Assert.NotNull(notification);
            Assert.Equal(from, notification.From);
            Assert.Equal(to, notification.To);
            Assert.Equal(subject, notification.Subject);
            Assert.Equal(body, notification.Body);
            Assert.Equal(phoneNumber, notification.PhoneNumber);
        }

        [Theory]
        [InlineData("", "", "", "", "")]
        [InlineData("from@example.com", "", "", "", "")]
        [InlineData("", "to@example.com", "", "", "")]
        [InlineData("", "", "subject", "", "")]
        [InlineData("", "", "", "body", "")]
        public void Constructor_MissingRequiredFields_ShouldThrowArgumentNullException(string from, string to, string subject, string body, string phoneNumber)
        {
            // Arrange
            var notificationType = NotificationTypeEnum.STATUS;

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => new Notification(notificationType, from, to, subject, body, phoneNumber));
            Assert.Equal("You must be fill the required fields to send an email", exception.ParamName);
        }

        [Theory]
        [InlineData(NotificationTypeEnum.STATUS, 1, 2)]
        [InlineData(NotificationTypeEnum.NEWS, 1440, 1)]
        [InlineData(NotificationTypeEnum.MARKETING, 60, 3)]
        public void GetRateLimits_WithDifferentNotificationTypes_ShouldReturnCorrectValues(NotificationTypeEnum type, double expectedMinutes, int expectedLimit)
        {
            // Arrange
            var notification = new Notification(type, "from@example.com", "to@example.com", "Test Subject", "Test Body", "1234567890");

            // Act
            var (timeSpan, limit) = notification.GetRateLimits();

            // Assert
            Assert.Equal(TimeSpan.FromMinutes(expectedMinutes), timeSpan);
            Assert.Equal(expectedLimit, limit);
        }

        [Fact]
        public void GetRateLimits_UnsupportedType_ShouldThrowNotSupportedException()
        {
            // Arrange
            var notification = new Notification((NotificationTypeEnum)999, "from@example.com", "to@example.com", "Test Subject", "Test Body", "1234567890");

            // Act & Assert
            Assert.Throws<NotSupportedException>(() => notification.GetRateLimits());
        }
    }
}
