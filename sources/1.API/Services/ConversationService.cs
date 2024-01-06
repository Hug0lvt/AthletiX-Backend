using Model;
using Repositories;
using API.Exceptions;

namespace API.Services
{
    public class ConversationService
    {
        private readonly AppDbContext _dbContext;

        public ConversationService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Conversation CreateConversation(Conversation conversation)
        {
            _dbContext.Conversations.Add(conversation);
            _dbContext.SaveChanges();
            return conversation;
        }

        public List<Conversation> GetAllConversations()
        {
            return _dbContext.Conversations.ToList();
        }

        public Conversation GetConversationById(int conversationId)
        {
            return _dbContext.Conversations.FirstOrDefault(c => c.Id == conversationId);
        }

        public Conversation DeleteConversation(int conversationId)
        {
            var conversationToDelete = _dbContext.Conversations.Find(conversationId);

            if (conversationToDelete != null)
            {
                _dbContext.Conversations.Remove(conversationToDelete);
                _dbContext.SaveChanges();
                return conversationToDelete;
            }

            throw new NotFoundException("[LOG | ConversationService] - (DeleteConversation): Conversation not found");
        }
    }
}
