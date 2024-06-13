using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace API.Controllers.RealTime
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message) //Broadcast
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        
    }
}
