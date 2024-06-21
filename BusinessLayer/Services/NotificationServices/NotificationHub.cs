using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services.NotificationServices
{
    public class NotificationHub : Hub
    {
        public async Task SendNotification(int userId , string message)
        {
            await Clients.User(userId.ToString()).SendAsync("ReceiveNotification", message);

        }

    }
}
