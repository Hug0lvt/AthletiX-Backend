using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Model;
using Shared.Exceptions;
using Shared;
using AutoMapper;
using Dommain.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    /// <summary>
    /// Service for managing messages.
    /// </summary>
    public class MessageService
    {
        private readonly ILogger<MessageService> _logger;
        private readonly IdentityAppDbContext _dbContext;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageService"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="logger">The logger instance.</param>
        public MessageService(IdentityAppDbContext dbContext, ILogger<MessageService> logger, IMapper mapper)
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Creates a new message.
        /// </summary>
        /// <param name="message">The message to be created.</param>
        /// <returns>The created message.</returns>
        public async Task<Message> CreateMessageAsync(Message message)
        {
            try
            {
                var existingProfile = await _dbContext.Profiles.FirstOrDefaultAsync(p => p.Id == message.Sender.Id);
                if (existingProfile == null)
                    throw new NotCreatedExecption("Profile does not exist.");

                var existingConversation = await _dbContext.Conversations.FirstOrDefaultAsync(c => c.Id == message.ConversationId);
                if (existingConversation == null)
                    throw new NotCreatedExecption("Conversation does not exist.");

                var entity = _mapper.Map<MessageEntity>(message);

                entity.ConversationId = existingConversation.Id;
                entity.ProfileId = existingProfile.Id;
                entity.Sender = null;
                entity.Conversation = null;

                _dbContext.Entry(existingConversation).State = EntityState.Unchanged;
                _dbContext.Entry(existingProfile).State = EntityState.Unchanged;

                _dbContext.Messages.Add(entity);
                await _dbContext.SaveChangesAsync();

                return _mapper.Map<Message>(entity);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to create message.", ex);
            }

            // TODO Send Notification
        }

        /// <summary>
        /// Gets all messages.
        /// </summary>
        /// <returns>A list of all messages.</returns>
        public List<Message> GetAllMessages()
        {
            return _mapper.Map<List<Message>>(_dbContext.Messages.ToList());
        }

        /// <summary>
        /// Gets all Message (with pages).
        /// </summary>
        /// <returns>A list of all Message.</returns>
        public PaginationResult<Message> GetAllMessagesWithPages(
            int pageSize = 10,
            int pageNumber = 0)
        {
            var totalItems = _dbContext.Messages.Count();
            var items = _mapper.Map<List<Message>>(_dbContext.Messages
                .Include(m => m.Sender)
                .Skip(pageNumber * pageSize)
                .Take(pageSize)
                .ToList());

            return new PaginationResult<Message>
            {
                Items = items,
                NextPage = (pageNumber + 1) * pageSize < totalItems ? pageNumber + 1 : -1,
                TotalItems = totalItems
            };
        }

        /// <summary>
        /// Gets all Message (with pages).
        /// </summary>
        /// <returns>A list of all Message.</returns>
        public PaginationResult<Message> GetAllMessagesWithPagesForConversation(
            int conversationId,
            int pageSize = 10,
            int pageNumber = 0)
        {
            var totalItems = _dbContext.Messages.Count();
            var items = _mapper.Map<List<Message>>(_dbContext.Messages
                .Where(c => c.ConversationId == conversationId)
                .Include(m => m.Sender)
                .Skip(pageNumber * pageSize)
                .Take(pageSize)
                .ToList());

            return new PaginationResult<Message>
            {
                Items = items,
                NextPage = (pageNumber + 1) * pageSize < totalItems ? pageNumber + 1 : -1,
                TotalItems = totalItems
            };
        }

        /// <summary>
        /// Gets a message by its identifier.
        /// </summary>
        /// <param name="messageId">The identifier of the message.</param>
        /// <returns>The message with the specified identifier.</returns>
        public Message GetMessageById(int messageId)
        {
            return _mapper.Map<Message>(_dbContext.Messages
                .Include(m => m.Sender)
                .FirstOrDefault(m => m.Id == messageId));
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
                return _mapper.Map<Message>(existingMessage);
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
                return _mapper.Map<Message>(messageToDelete);
            }

            _logger.LogTrace("[LOG | MessageService] - (DeleteMessage): Message not found");
            throw new NotFoundException("[LOG | MessageService] - (DeleteMessage): Message not found");
        }
    }
}
