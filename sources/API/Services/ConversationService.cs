using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Model;
using Shared.Exceptions;
using System.Linq;
using Shared;
using AutoMapper;
using Dommain.Entities;
using Microsoft.EntityFrameworkCore;

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
        /// Gets user conversations.
        /// </summary>
        /// <returns>A list of user conversations.</returns>
        public PaginationResult<Conversation> GetConversationsForUser(int userId, bool includeProfiles = false, bool includeMessages = false)
        {
            
            var items = _dbContext.ConversationMembers
                .Where(q => q.ProfileId == userId)
                .ToList();
            var conversationIds = items.Select(cm => cm.ConversationId).ToList();
            var userConversations = _mapper.Map<List<Conversation>>(_dbContext.Conversations
                .Where(c => conversationIds.Contains(c.Id))
                .ToList());

            if (includeProfiles)
            {
                foreach(var conversation in userConversations)
                {
                    if(conversation != null)
                    {
                        List<Model.Profile> profiles = _mapper.Map<List<Model.Profile>>(_dbContext.ConversationMembers
                           .Include(cm => cm.Profile)
                           .Where(cm => cm.ConversationId == conversation.Id)
                           .Select(cm => cm.Profile)
                           .ToList());
                        conversation.Profiles = profiles;
                    }
                }
            }

            if (includeMessages)
            {
                foreach (var conversation in userConversations)
                {
                    if (conversation != null)
                    {
                        List<Message> messages = _mapper.Map<List<Message>>(_dbContext.Messages
                           .Include(m => m.Sender)
                           .Where(m => m.ConversationId == conversation.Id)
                           .ToList());
                        conversation.Messages = messages;
                    }
                }
            }

            var totalItems = items.Count();

            return new PaginationResult<Conversation>
            {
                Items = userConversations,
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
            Conversation conversation = _mapper.Map<Conversation>(_dbContext.Conversations.FirstOrDefault(c => c.Id == conversationId));

            List<Model.Profile> profiles = _mapper.Map<List<Model.Profile>>(_dbContext.ConversationMembers
                           .Include(cm => cm.Profile)
                           .Where(cm => cm.ConversationId == conversationId)
                           .Select(cm => cm.Profile)
                           .ToList());
            conversation.Profiles = profiles;
            List<Message> messages = _mapper.Map<List<Message>>(_dbContext.Messages
                           .Include(m => m.Sender)
                           .Where(m => m.ConversationId == conversationId)
                           .ToList());
            conversation.Messages = messages;
            return conversation;
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
