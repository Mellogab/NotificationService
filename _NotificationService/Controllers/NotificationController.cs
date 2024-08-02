using _NotificationService.Presenters;
using Microsoft.AspNetCore.Mvc;
using NotificationService.Core;
using NotificationService.Core.UseCases.Notification;
using System.Net;
using System.Reflection;

namespace _NotificationService.Controllers 
{
    [ApiController]
    [Route("api/notification")]
    public class NotificationController : ControllerBase
    {
        private readonly ILogger<NotificationController> _logger;
        public NotificationController(ILogger<NotificationController> logger) => _logger = logger;

        [HttpPost]
        [Route("send-async")]
        public async Task<ActionResult> ExeuteTransfersAsync(
            [FromServices] INotificationUseCase useCase,
            [FromServices] DefaultPresenter<UseCaseResponseMessage> presenter,
            [FromBody] NotificationUseCaseInput input
        )
        {
            try
            {
                await useCase.Handle(input, presenter);
                return presenter.GetJsonResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error On {MethodBase.GetCurrentMethod().Name}. Ex {ex.Message}");
                return StatusCode((int)HttpStatusCode.InternalServerError, ex);
            }
        }
    }
}
