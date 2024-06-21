using BusinessLayer.Services.NotificationServices;
using DomainLayer.DataAcess;
using Microsoft.AspNetCore.SignalR;
using PresentationLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services.LikeServices
{
    public class LikeHub : Hub
    {
       
        public async Task SendLike(int postId, int userId)
        {
            await Clients.User(userId.ToString()).SendAsync("ReceiveLike", postId, userId);
          
        }
    }
}
