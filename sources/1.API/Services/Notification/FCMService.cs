﻿using API.Exceptions;
using FirebaseAdmin.Messaging;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace API.Services.Notification
{
    /// <summary>
    /// Service for sending Firebase Cloud Messaging (FCM) notifications.
    /// </summary>
    public class FCMService
    {
        private readonly ILogger<FCMService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="FCMService"/> class.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        public FCMService(ILogger<FCMService> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Sends a Firebase Cloud Messaging (FCM) notification.
        /// </summary>
        /// <param name="title">The title of the notification.</param>
        /// <param name="body">The body/content of the notification.</param>
        /// <param name="token">The FCM token of the device to receive the notification.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
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