using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using DomainLayer.DataAcess;
using Microsoft.EntityFrameworkCore;
using PresentationLayer.Models;
using BusinessLayer.Services.NotificationServices;

namespace PresentationLayer.Controllers.MessageControllers
{
    public class MessageHub : Hub
    {
        private readonly IUnitOfWork _context;
        private readonly IHubContext<NotificationHub> _notificationHubContext;

        public MessageHub(IUnitOfWork context, IHubContext<NotificationHub> notificationHubContext)
        {
            _context = context;
            _notificationHubContext = notificationHubContext;
        }

        public async Task SendMessage(string user, string message)
        {
            var sender = await _context.Context.Users.FirstOrDefaultAsync(u => u.FullName == user);
            if (sender != null)
            {
                var friends = await GetFriendsAsync(sender.Id);
                foreach (var friend in friends)
                {
                    await Clients.User(friend.FullName).SendAsync("ReceiveMessage", user, message);

                    // Create and save notification
                    var notification = new Notification
                    {
                        Message = $"{user} sent you a message: {message}",
                        CreatedAt = DateTime.UtcNow,
                        IsRead = false,
                        UserId = friend.Id
                    };
                    _context.Context.Notifications.Add(notification);
                    await _context.Context.SaveChangesAsync();

                    await _notificationHubContext.Clients.User(friend.FullName).SendAsync("ReceiveNotification", notification.Message);
                }
            }
        }

        public async Task ReplyMessage(string user, string message, string replyToUser)
        {
            var sender = await _context.Context.Users.FirstOrDefaultAsync(u => u.FullName == user);
            var receiver = await _context.Context.Users.FirstOrDefaultAsync(u => u.FullName == replyToUser);

            if (sender != null && receiver != null && await AreFriendsAsync(sender.Id, receiver.Id))
            {
                await Clients.User(replyToUser).SendAsync("ReceiveReply", user, message);

                // Create and save notification
                var notification = new Notification
                {
                    Message = $"{user} replied to your message: {message}",
                    CreatedAt = DateTime.UtcNow,
                    IsRead = false,
                    UserId = receiver.Id
                };
                _context.Context.Notifications.Add(notification);
                await _context.Context.SaveChangesAsync();

                await _notificationHubContext.Clients.User(replyToUser).SendAsync("ReceiveNotification", notification.Message);
            }
        }

        private async Task<bool> AreFriendsAsync(int userId1, int userId2)
        {
            return await _context.Context.FriendRequests.AnyAsync(fr =>
                ((fr.SenderId == userId1 && fr.ReceiverId == userId2) ||
                (fr.SenderId == userId2 && fr.ReceiverId == userId1)) &&
                fr.IsAccepted);
        }

        private async Task<List<ApplicationUser>> GetFriendsAsync(int userId)
        {
            var friends = await _context.Context.FriendRequests
                .Where(fr => (fr.SenderId == userId || fr.ReceiverId == userId) && fr.IsAccepted)
                .Select(fr => fr.SenderId == userId ? fr.Receiver : fr.Sender)
                .ToListAsync();

            return friends;
        }
    }
}
