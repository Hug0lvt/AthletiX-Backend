using API.Exceptions;
using FirebaseAdmin.Messaging;


namespace API.Services.Notification
{
    public class FCMService
    {
        private readonly ILogger<FCMService> _logger;

        public FCMService(ILogger<FCMService> logger) 
        { 
            _logger = logger;
        }

        public async Task SendNotification(string title, string body, string token)
        {

            try
            {
                var message = new Message
                {
                    Token = token,
                    Notification = new FirebaseAdmin.Messaging.Notification
                    {
                        Title = title,
                        Body = body
                    }
                };

                var response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
            }
            catch (Exception ex)
            {
                _logger.LogError("[LOG | FCMService] - (SendNotification): Error sending FCM notification");
                throw new NotificationException("[LOG | FCMService] - (SendNotification): Error sending FCM notification");
            }
        }

    }
}
