using API.Exceptions;
using API.Services;
using API.Services.Notification;
using Microsoft.AspNetCore.Mvc;
using Model;
using Microsoft.AspNetCore.Http;

namespace API.Controllers.v1_0.Notification
{
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "Notifications APIs (with FCM)")]
    [ApiController]
    [Route("api/notification/fcm")]
    public class FCMController : ControllerBase
    {
        private readonly FCMService _fcmService;

        public FCMController(FCMService fcmService)
        {
            _fcmService = fcmService;
        }

        /// <summary>
        /// Fix a UniqueNotificationToken.
        /// </summary>
        /// <param name="profile">The profile.</param>
        /// <returns>The updated profile.</returns>
        [HttpPatch(Name = "PATCH - Entrypoint for patch UniqueNotificationToken in Profile")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateUniqueNotificationToken([FromBody] Profile updatedProfile)
        {
            try
            {
                _fcmService.AutomaticRefreshTokenNotification(updatedProfile);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Sends a notification to a specific client.
        /// </summary>
        /// <param name="title">The title of the notification.</param>
        /// <param name="body">The body of the notification.</param>
        /// <param name="token">The device token of the target client.</param>
        /// <returns>An IActionResult indicating the result of the notification sending operation.</returns>
        [HttpPost(Name = "POST - Entrypoint for sending a notification to a specific client")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SendNotification(string title, string body, string token)
        {
            try
            {
                await _fcmService.SendNotification(title, body, token);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
