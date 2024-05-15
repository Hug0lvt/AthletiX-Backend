using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Model;
using API.Exceptions;
using API.Repositories;
using Shared.Mappers;

namespace API.Services
{
    /// <summary>
    /// Service for managing conversations.
    /// </summary>
    public class ConversationService
    {
        private readonly ILogger<ConversationService> _logger;
        private readonly IdentityAppDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConversationService"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="logger">The logger instance.</param>
        public ConversationService(IdentityAppDbContext dbContext, ILogger<ConversationService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new conversation.
        /// </summary>
        /// <param name="conversation">The conversation to be created.</param>
        /// <returns>The created conversation.</returns>
        public Conversation CreateConversation(Conversation conversation)
        {
            _dbContext.Conversations.Add(conversation);
            _dbContext.SaveChanges();
            return conversation;
        }

        /// <summary>
        /// Gets all conversations.
        /// </summary>
        /// <returns>A list of all conversations.</returns>
        public List<Conversation> GetAllConversations()
        {
            return _dbContext.Conversations.ToList();
        }

        /// <summary>
        /// Gets all conversations (with pages).
        /// </summary>
        /// <returns>A list of all conversations.</returns>
        public PaginationResult<Conversation> GetAllConversationsWithPages(
            int pageSize = 10,
            int pageNumber = 0)
        {
            var totalItems = _dbContext.Conversations.Count();
            var items = _dbContext.Conversations
                .Skip(pageNumber * pageSize)
                .Take(pageSize)
                .ToList();

            return new PaginationResult<Conversation>
            {
                Items = items,
                NextPage = (pageNumber + 1) * pageSize < totalItems ? pageNumber + 1 : -1,
                TotalItems = totalItems
            };
        }

        /// <summary>
        /// Gets user conversations.
        /// </summary>
        /// <returns>A list of user conversations.</returns>
        public PaginationResult<Conversation> GetConversationsForUser(int userId)
        {
            
            var items = _dbContext.Conversations
                .Where(q => q.Profiles.Any(p => p.Id == userId))
                .ToList();

            var totalItems = items.Count();

            return new PaginationResult<Conversation>
            {
                Items = items,
                NextPage = -1,
                TotalItems = totalItems
            };
        }

        /// <summary>
        /// Gets a conversation by its identifier.
        /// </summary>
        /// <param name="conversationId">The identifier of the conversation.</param>
        /// <returns>The conversation with the specified identifier.</returns>
        public Conversation GetConversationById(int conversationId)
        {
            return _dbContext.Conversations.FirstOrDefault(c => c.Id == conversationId);
        }

        /// <summary>
        /// Deletes a conversation by its identifier.
        /// </summary>
        /// <param name="conversationId">The identifier of the conversation to be deleted.</param>
        /// <returns>The deleted conversation.</returns>
        public Conversation DeleteConversation(int conversationId)
        {
            var conversationToDelete = _dbContext.Conversations.Find(conversationId);

            if (conversationToDelete != null)
            {
                _dbContext.Conversations.Remove(conversationToDelete);
                _dbContext.SaveChanges();
                return conversationToDelete;
            }

            _logger.LogTrace("[LOG | ConversationService] - (DeleteConversation): Conversation not found");
            throw new NotFoundException("[LOG | ConversationService] - (DeleteConversation): Conversation not found");
        }
    }
}
