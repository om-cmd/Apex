using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services.CommentService
{
    public class SendCommentHub : Hub
    {
        public async Task SendComment(int userId, string message)
        {
            await Clients.User(userId.ToString()).SendAsync("ReceiveCommentNotification", message);
        }
    }
}
