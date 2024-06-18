using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace PresentationLayer.Controllers.MessageControllers
{

    public class MessageController : Controller
    {
        private readonly IHubContext<MessageHub> _hubContext;

        public MessageController(IHubContext<MessageHub> hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMessage(string user, string message)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", user, message);
            return Ok();
        }

        [HttpPost("reply")]
        public async Task<IActionResult> ReplyMessage(string user, string message, string replyToUser)
        {
            await _hubContext.Clients.User(replyToUser).SendAsync("ReceiveReply", user, message);
            return Ok();
        }
    }
}
