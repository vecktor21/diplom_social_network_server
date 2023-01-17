using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace server.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Login{ get; set; }
        public string Password{ get; set; }
        public string Email { get; set; }
        public bool IsVerified { get; set; }
        public string Nickname { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime RegistrationDate { get; set; }
        public int TokenId { get; set; }
        public UserToken Token { get; set; }
        public int ImageId { get; set; }
        public Models.File Image { get; set; }
        public int UserInfoId { get; set; }
        public UserInfo UserInfo { get; set; }
        public int RoleId { get; set; }
        public UserRole Role { get; set; } 
        public int UserStatusId { get; set; }
        public UserStatus UserStatus { get; set; }
        //foreign keys
        public List<Article> Articles { get; set; }
        public List<BlockList> BlockedUsers{ get; set; }
        public List<Favorite> Favorites { get; set; }
        public List<UserFile> UserFile { get; set; }
        public virtual List<Friend> Friends1 { get; set; }
        public virtual List<Friend> Friends2 { get; set; }
        public List<FriendRequest> FriendRequestsSenders { get; set; }
        public List<FriendRequest> FriendRequestsReceivers { get; set; }
        public List<GroupMember> UserGroupMembership { get; set; }
        public List<Like> Likes { get; set; }
        public List<Notification> Notifications { get; set; }
        public List<Subscribe> SubUser{ get; set; }
        public List<Subscribe> SubSub { get; set; }
        public List<UserInterest> UserInterests { get; set; }
        public List<UserNote> UserNotes { get; set; }
        public List<UserChatRoom> UserChatRooms { get; set; }



    }
}
