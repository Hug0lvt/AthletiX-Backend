using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Model;
using Shared.Exceptions;
using System.Linq;
using Shared;
using AutoMapper;
using Dommain.Entities;

namespace API.Services
{
    /// <summary>
    /// Service for managing conversations.
    /// </summary>
    public class ConversationService
    {
        private readonly ILogger<ConversationService> _logger;
        private readonly IdentityAppDbContext _dbContext;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConversationService"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="logger">The logger instance.</param>
        public ConversationService(IdentityAppDbContext dbContext, ILogger<ConversationService> logger, IMapper mapper)
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Creates a new conversation.
        /// </summary>
        /// <param name="conversation">The conversation to be created.</param>
        /// <returns>The created conversation.</returns>
        public Conversation CreateConversation(Conversation conversation)
        {
            _dbContext.Conversations.Add(_mapper.Map<ConversationEntity>(conversation));
            _dbContext.SaveChanges();
            return conversation;
        }

        /// <summary>
        /// Gets all conversations.
        /// </summary>
        /// <returns>A list of all conversations.</returns>
        public List<Conversation> GetAllConversations()
        {
            return _mapper.Map<List<Conversation>>(_dbContext.Conversations.ToList());
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
                Items = _mapper.Map<List<Conversation>>(items),
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
            
            var items = _dbContext.ConversationMembers
                .Where(q => q.ProfileId == userId)
                .ToList();
            var conversationIds = items.Select(cm => cm.ConversationId).ToList();
            var userConversations = _dbContext.Conversations
                .Where(c => conversationIds.Contains(c.Id))
                .ToList();

            var totalItems = items.Count();

            return new PaginationResult<Conversation>
            {
                Items = _mapper.Map<List<Conversation>>(userConversations),
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
            return _mapper.Map<Conversation>(_dbContext.Conversations.FirstOrDefault(c => c.Id == conversationId));
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
                return _mapper.Map<Conversation>(conversationToDelete);
            }

            _logger.LogTrace("[LOG | ConversationService] - (DeleteConversation): Conversation not found");
            throw new NotFoundException("[LOG | ConversationService] - (DeleteConversation): Conversation not found");
        }
    }
}
