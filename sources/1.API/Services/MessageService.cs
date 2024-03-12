using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Model;
using API.Exceptions;
using API.Repositories;

namespace API.Services
{
    /// <summary>
    /// Service for managing messages.
    /// </summary>
    public class MessageService
    {
        private readonly ILogger<MessageService> _logger;
        private readonly IdentityAppDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageService"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="logger">The logger instance.</param>
        public MessageService(IdentityAppDbContext dbContext, ILogger<MessageService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new message.
        /// </summary>
        /// <param name="message">The message to be created.</param>
        /// <returns>The created message.</returns>
        public Message CreateMessage(Message message)
        {
            _dbContext.Messages.Add(message);
            _dbContext.SaveChanges();
            return message;
        }

        /// <summary>
        /// Gets all messages.
        /// </summary>
        /// <returns>A list of all messages.</returns>
        public List<Message> GetAllMessages()
        {
            return _dbContext.Messages.ToList();
        }

        /// <summary>
        /// Gets a message by its identifier.
        /// </summary>
        /// <param name="messageId">The identifier of the message.</param>
        /// <returns>The message with the specified identifier.</returns>
        public Message GetMessageById(int messageId)
        {
            return _dbContext.Messages.FirstOrDefault(m => m.Id == messageId);
        }

        /// <summary>
        /// Updates an existing message.
        /// </summary>
        /// <param name="updatedMessage">The updated message.</param>
        /// <returns>The updated message.</returns>
        public Message UpdateMessage(Message updatedMessage)
        {
            var existingMessage = _dbContext.Messages.Find(updatedMessage.Id);

            if (existingMessage != null)
            {
                existingMessage.Content = updatedMessage.Content;
                _dbContext.SaveChanges();
                return existingMessage;
            }

            _logger.LogTrace("[LOG | MessageService] - (UpdateMessage): Message not found");
            throw new NotFoundException("[LOG | MessageService] - (UpdateMessage): Message not found");
        }

        /// <summary>
        /// Deletes a message by its identifier.
        /// </summary>
        /// <param name="messageId">The identifier of the message to be deleted.</param>
        /// <returns>The deleted message.</returns>
        public Message DeleteMessage(int messageId)
        {
            var messageToDelete = _dbContext.Messages.Find(messageId);

            if (messageToDelete != null)
            {
                _dbContext.Messages.Remove(messageToDelete);
                _dbContext.SaveChanges();
                return messageToDelete;
            }

            _logger.LogTrace("[LOG | MessageService] - (DeleteMessage): Message not found");
            throw new NotFoundException("[LOG | MessageService] - (DeleteMessage): Message not found");
        }
    }
}
