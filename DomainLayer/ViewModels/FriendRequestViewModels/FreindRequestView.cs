namespace DomainLayer.ViewModels.FriendRequestViewModels
{
    public class FreindRequestView
    {
        public int SenderId { get; set; }      
        public int ReceiverId { get; set; }
        public DateTime SentAt { get; set; }
        public bool IsAccepted { get; set; }
    }
}
