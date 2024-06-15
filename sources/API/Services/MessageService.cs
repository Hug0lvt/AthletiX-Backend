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
    }
}
