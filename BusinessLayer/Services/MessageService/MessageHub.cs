using Microsoft.AspNetCore.SignalR;

namespace PresentationLayer.Controllers.MessageControllers
{
    public class MessageHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task ReplyMessage(string user, string message, string replyToUser)
        {
            await Clients.User(replyToUser).SendAsync("ReceiveReply", user, message);
        }
    }
}
