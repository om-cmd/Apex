namespace DomainLayer.ViewModels.MessageViewModels
{
    public class MessageView
    {
        public string Content { get; set; }
        public DateTime SentAt { get; set; } = DateTime.Now;
    }
}
