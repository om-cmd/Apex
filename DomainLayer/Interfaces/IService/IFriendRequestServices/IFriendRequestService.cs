using DomainLayer.ViewModels.FriendRequestViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Interfaces.IService.IFriendRequestServices
{
    public interface IFriendRequestService
    {
        public  Task SendFriendRequestAsync(int senderId, int receiverId);
        public  Task AcceptFriendRequestAsync(int requestId);
        public Task RejectFriendRequestAsync(int requestId);

        public Task<List<FreindRequestView>> GetFriendRequestsAsync(int userId);


    }
}
