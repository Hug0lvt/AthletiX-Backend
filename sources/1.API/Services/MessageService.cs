using Model;
using Repositories;
using API.Exceptions;

namespace API.Services
{
    public class MessageService
    {
        private readonly AppDbContext _dbContext;

        public MessageService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Message CreateMessage(Message message)
        {
            _dbContext.Messages.Add(message);
            _dbContext.SaveChanges();
            return message;
        }

        public List<Message> GetAllMessages()
        {
            return _dbContext.Messages.ToList();
        }

        public Message GetMessageById(int messageId)
        {
            return _dbContext.Messages.FirstOrDefault(m => m.Id == messageId);
        }

        public Message UpdateMessage(Message updatedMessage)
        {
            var existingMessage = _dbContext.Messages.Find(updatedMessage.Id);

            if (existingMessage != null)
            {
                existingMessage.Content = updatedMessage.Content;
                _dbContext.SaveChanges();
                return existingMessage;
            }

            throw new NotFoundException("[LOG | MessageService] - (UpdateMessage): Message not found");
        }

        public Message DeleteMessage(int messageId)
        {
            var messageToDelete = _dbContext.Messages.Find(messageId);

            if (messageToDelete != null)
            {
                _dbContext.Messages.Remove(messageToDelete);
                _dbContext.SaveChanges();
                return messageToDelete;
            }

            throw new NotFoundException("[LOG | MessageService] - (DeleteMessage): Message not found");
        }
    }
}
