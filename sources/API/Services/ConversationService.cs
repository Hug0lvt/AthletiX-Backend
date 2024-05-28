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
        /// Add a new member at conversation.
        /// </summary>
        /// <param name="conversation">The conversation to be created.</param>
        /// <returns>The created conversation.</returns>
        public async Task<Conversation> AddMemberToConversation(int conversationId, int profileId)
        {
            var existingConversation = await _dbContext.Conversations.FirstOrDefaultAsync(c => c.Id == conversationId);
            if (existingConversation == null)
                throw new NotCreatedExecption("Conversation does not exist.");

            var existingProfile = await _dbContext.Profiles.FirstOrDefaultAsync(p => p.Id == profileId);
            if (existingProfile == null)
                throw new NotCreatedExecption("Profile does not exist.");

            _dbContext.ConversationMembers.Add(new ConversationMembersEntity { Id = 0, ConversationId = conversationId, ProfileId = profileId });
            _dbContext.SaveChanges();
            return GetConversationById(conversationId);
        }

        /// <summary>
        /// Remove a member at conversation.
        /// </summary>
        /// <param name="conversation">The conversation to be created.</param>
        /// <returns>The created conversation.</returns>
        public async Task<Conversation> RemoveMemberToConversation(int conversationId, int profileId)
        {
            var existingConversation = await _dbContext.Conversations.FirstOrDefaultAsync(c => c.Id == conversationId);
            if (existingConversation == null)
                throw new NotCreatedExecption("Conversation does not exist.");

            var existingProfile = await _dbContext.Profiles.FirstOrDefaultAsync(p => p.Id == profileId);
            if (existingProfile == null)
                throw new NotCreatedExecption("Profile does not exist.");

            var existingProfileInConveresation = await _dbContext.ConversationMembers
                .FirstOrDefaultAsync(c => c.ConversationId == conversationId && c.ProfileId == profileId);
            if (existingProfileInConveresation == null)
                throw new NotFoundException("Profile in this conversation does not exist.");

            _dbContext.ConversationMembers.Remove(existingProfileInConveresation);
            _dbContext.SaveChanges();
            return GetConversationById(conversationId);
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
            int pageNumber = 0,
            bool includeProfiles = false,
            bool includeMessages = false)
        {
            var totalItems = _dbContext.Conversations.Count();
            var items = _mapper.Map<List<Conversation>>(_dbContext.Conversations
                .Skip(pageNumber * pageSize)
                .Take(pageSize)
                .ToList());

            if (includeProfiles)
            {
                foreach (var conversation in items)
                {
                    if (conversation != null)
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
                foreach (var conversation in items)
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
