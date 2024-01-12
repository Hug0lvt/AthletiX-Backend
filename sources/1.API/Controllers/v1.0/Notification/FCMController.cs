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
    }
}
