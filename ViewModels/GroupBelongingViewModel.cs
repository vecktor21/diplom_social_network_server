namespace server.ViewModels
{
    public class GroupBelongingViewModel
    {
        public bool IsMember { get; set; }
        public bool IsLeader { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsModerator{ get; set; }
        public bool IsBanned { get; set; }
        public bool IsRequestSent { get; set; }
        public GroupBelongingViewModel()
        {
            IsMember = false;
            IsLeader = false;
            IsAdmin = false;
            IsModerator = false;
            IsBanned = false;
            IsRequestSent = false;
        }
    }
}
