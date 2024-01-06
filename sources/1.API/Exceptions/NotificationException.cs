namespace API.Exceptions
{
    public class NotificationException : Exception
    {
        public NotificationException() : base()
        {
        }

        public NotificationException(string message) : base(message)
        {
        }

        public NotificationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
