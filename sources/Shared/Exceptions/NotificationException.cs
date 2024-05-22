﻿namespace Shared.Exceptions
{
    /// <summary>
    /// Exception thrown when an error occurs related to notifications.
    /// </summary>
    public class NotificationException : System.Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationException"/> class.
        /// </summary>
        public NotificationException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public NotificationException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public NotificationException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}