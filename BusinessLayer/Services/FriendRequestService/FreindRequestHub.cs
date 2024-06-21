using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services.FriendRequestService
{
    public class FreindRequestHub : Hub
    {
        public async Task ReceiveFriendRequest()
        {
            await Clients.Caller.SendAsync("FriendRequestReceived");
        }
    }
}
