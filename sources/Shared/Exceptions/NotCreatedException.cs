namespace Shared.Exceptions
{
    /// <summary>
    /// Exception thrown when a requested resource is not found.
    /// </summary>
    public class NotCreatedExecption : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotCreatedExecption"/> class.
        /// </summary>
        public NotCreatedExecption() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotCreatedExecption"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public NotCreatedExecption(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotCreatedExecption"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public NotCreatedExecption(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}
