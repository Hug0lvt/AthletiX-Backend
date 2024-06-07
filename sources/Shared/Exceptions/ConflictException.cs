namespace Shared.Exceptions
{
    /// <summary>
    /// Exception thrown when a requested resource is not found.
    /// </summary>
    public class ConflictExecption : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConflictExecption"/> class.
        /// </summary>
        public ConflictExecption() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConflictExecption"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public ConflictExecption(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConflictExecption"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public ConflictExecption(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}
