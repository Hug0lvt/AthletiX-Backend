namespace API.RealTime
{
    using Microsoft.AspNetCore.SignalR;
    using System.Threading.Tasks;
    using System.Linq;
    using Dommain.Entities;
    using Shared.Exceptions;

    public class ChatHub : Hub
    {
        private readonly IdentityAppDbContext _context;

        public ChatHub(IdentityAppDbContext context)
        {
            _context = context;
        }

        public async Task SendMessage(int conversationId, int senderId, string message)
        {
            var isMember = _context.ConversationMembers
                .Any(cm => cm.ConversationId == conversationId && cm.ProfileId == senderId);

            if (!isMember) throw new ConflictExecption("Sender does not in a Conversation");

            var messageEntity = new MessageEntity
            {
                Content = message,
                DateSent = DateTime.UtcNow,
                ConversationId = conversationId,
                ProfileId = senderId
            };

            _context.Messages.Add(messageEntity);
            await _context.SaveChangesAsync();

            await Clients.Group(conversationId.ToString()).SendAsync("ReceiveMessage", senderId, message);
        }

        public async Task JoinConversation(int conversationId, int profileId)
        {
            var isMember = _context.ConversationMembers
                .Any(cm => cm.ConversationId == conversationId && cm.ProfileId == profileId);

            if (!isMember)
            {
                var conversationMember = new ConversationMembersEntity
                {
                    ConversationId = conversationId,
                    ProfileId = profileId
                };

                _context.ConversationMembers.Add(conversationMember);
                await _context.SaveChangesAsync();
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, conversationId.ToString());
        }

        public async Task LeaveConversation(int conversationId, int profileId)
        {
            var conversationMember = _context.ConversationMembers
                .FirstOrDefault(cm => cm.ConversationId == conversationId && cm.ProfileId == profileId);

            if (conversationMember != null)
            {
                _context.ConversationMembers.Remove(conversationMember);
                await _context.SaveChangesAsync();
            }

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, conversationId.ToString());
        }
    }

}
