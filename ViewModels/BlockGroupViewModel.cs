using server.Models;

namespace server.ViewModels
{
    public class BlockGroupViewModel
    {
        public int GroupId { get; set; }
        public int BlockedUserId { get; set; }
        public string? Reason { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string BlockedUserName { get; set; }
        public string BlockedUserImage { get; set; }
        public BlockGroupViewModel(GroupBlockList model)
        {
            this.BlockedUserId = model.BlockedUserId;
            this.Reason = model.Reason;
            this.DateFrom = model.DateFrom;
            this.DateTo = model.DateTo;
            this.GroupId = model.GroupId;
            this.BlockedUserName = model.BlockedUser.Name + " " + model.BlockedUser.Surname;
            this.BlockedUserImage = model.BlockedUser.Image.FileLink;
        }
    }
}
