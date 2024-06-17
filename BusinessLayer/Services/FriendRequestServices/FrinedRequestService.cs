using DomainLayer.DataAcess;
using DomainLayer.Interfaces.IService.IFriendRequestServices;
using DomainLayer.ViewModels.FriendRequestViewModels;
using Microsoft.EntityFrameworkCore;
using PresentationLayer.Models;

namespace BusinessLayer.Services.FriendRequestServices
{
    public class FrinedRequestService : IFriendRequestService
    {
        private readonly IUnitOfWork _unitOfWork;

        public FrinedRequestService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AcceptFriendRequestAsync(int requestId)
        {
            var friendRequest = await _unitOfWork.Context.FriendRequests.FindAsync(requestId);
            if (friendRequest != null)
            {
                friendRequest.IsAccepted = true;
                await _unitOfWork.Context.SaveChangesAsync();
            }
        }

        public async Task<List<FreindRequestView>> GetFriendRequestsAsync(int userId)
        {
            return await _unitOfWork.Context.FriendRequests
                .Where(fr => fr.ReceiverId == userId && !fr.IsAccepted)
                .Select(fr => new FreindRequestView
                {
                    SenderId = fr.SenderId,
                    ReceiverId = fr.ReceiverId,
                    SentAt = fr.SentAt,
                    IsAccepted = fr.IsAccepted
                }).ToListAsync();
        }

        public async Task RejectFriendRequestAsync(int requestId)
        {
            var friendRequest = await _unitOfWork.Context.FriendRequests.FindAsync(requestId);
            if (friendRequest != null)
            {
                _unitOfWork.Context.FriendRequests.Remove(friendRequest);
                await _unitOfWork.Context.SaveChangesAsync();
            }
        }

        public async Task SendFriendRequestAsync(int senderId, int receiverId)
        {
            var friendRequest = new FriendRequest
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                SentAt = DateTime.UtcNow,
                IsAccepted = false
            };
            _unitOfWork.Context.FriendRequests.Add(friendRequest);
            await _unitOfWork.Context.SaveChangesAsync();
        }
    }
}
