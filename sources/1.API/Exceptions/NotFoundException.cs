﻿namespace API.Exceptions
{
    /// <summary>
    /// Exception thrown when a requested resource is not found.
    /// </summary>
    public class NotFoundException : System.Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotFoundException"/> class.
        /// </summary>
        public NotFoundException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotFoundException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public NotFoundException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotFoundException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public NotFoundException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}
