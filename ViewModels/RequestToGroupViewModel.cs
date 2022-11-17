using server.Models;

namespace server.ViewModels
{
    public class RequestToGroupViewModel
    {
        public int GroupId { get; set; }
        public int UserId { get; set; }
        public string UserNickname { get; set; }
        public string GroupName { get; set; }
        public int RequestId { get; set; }
        public RequestToGroupViewModel(RequestToGroup request)
        {
            this.GroupId = request.GroupId;
            this.UserId = request.UserId;
            this.GroupName= request.Group.GroupName;
            this.UserNickname = request.User.Nickname;
            this.RequestId = request.RequestToGroupId;
        }

    }
}
