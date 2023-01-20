using Microsoft.VisualBasic.FileIO;
using server.Models;
using System.Net.Mail;

namespace server.ViewModels
{
    public class MessageViewModel
    {
        public int MessageId { get; set; }
        public DateTime SendingTime { get; set; }
        public UserViewModel Sender { get; set; }
        public int ChatRoomId { get; set; }
        public string Text { get; set; }
        public List<AttachmentViewModel> MessageAttachments { get; set; }
        public MessageViewModel(Message message)
        {
            this.SendingTime = message.SendingTime;
            this.MessageId = message.MessageId;
            this.Sender = new UserViewModel(message.Sender);
            this.Text = message.Text;
            this.ChatRoomId = message.ChatRoomId;
            this.MessageAttachments = message.MessageAttachments.Select(x=>new AttachmentViewModel
            {
                attachmentId = x.FileId,
                fileLink = x.File.FileLink,
                fileName = x.File.LogicalName,
                fileType = x.File.FileType
            }
            ).ToList();
        }
    }
}
