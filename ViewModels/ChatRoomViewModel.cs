using server.Models;

namespace server.ViewModels
{
    public class ChatRoomViewModel
    {
        public int ChatRoomId { get; set; }
        public int ChatRoomTypeId { get; set; }
        public string ChatRoomType { get; set; }
        public string ChatRoomImage { get; set; }
        public string ChatRoomName { get; set; }
        public MessageViewModel LastMessage{ get; set; }
        public List<MessageViewModel> Messages { get; set; }
        public List<UserViewModel> Members { get; set; }
        public int ChatRoomAdminId { get; set; }
        public ChatRoomViewModel(ChatRoom chatRoom) { 
            this.ChatRoomId= chatRoom.ChatRoomId;
            this.ChatRoomTypeId = chatRoom.ChatRoomTypeId;
            this.ChatRoomType = chatRoom.ChatRoomType.ChatRoomTypeName;
            this.ChatRoomImage = chatRoom.ChatRoomImage.FileLink;
            this.ChatRoomName = chatRoom.ChatRoomName;
            this.Messages = chatRoom.Messages.Select(x=>new MessageViewModel(x)).ToList();
            this.LastMessage = this.Messages.Count >0 ? this.Messages.OrderByDescending(x => x.SendingTime).First() : null;
            this.Members = chatRoom.UserChatRooms.Select(x => new UserViewModel(x.User)).ToList();
            foreach (var UserChatRoom in chatRoom.UserChatRooms)
            {
                if (UserChatRoom.UserChatRoomRoleId == 1)
                {
                    this.ChatRoomAdminId = UserChatRoom.UserChatRoomRoleId;
                }
                else
                {
                    this.ChatRoomAdminId = 0;
                }
            }
        }
    }
}
