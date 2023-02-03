using Microsoft.EntityFrameworkCore;
using server.Models;
using server.ViewModels;

namespace server.Services
{
    public class MessageService
    {
        private readonly ApplicationContext db;
        public MessageService(ApplicationContext db)
        {
            this.db = db;
        }

        //создание сообщения
        public async Task<Message?> AddMessage(MessageCreateViewModel newMesage)
        {
            try
            {
                User sernder = db.Users
                    .Include(x=>x.Role)
                    .Include(x=>x.Image)
                    .FirstOrDefault(x => x.UserId == newMesage.SenderId);
                if(sernder == null)
                {
                    return null;
                }
                Message message = new Message
                {
                    ChatRoomId = newMesage.ChatRoomId,
                    SenderId = newMesage.SenderId,
                    Sender = sernder,
                    Text = newMesage.Text,
                    MessageAttachments = new List<MessageAttachment>()
                };
                await db.Messages.AddAsync(message);

                message.MessageAttachments = db.Files
                    .Where(x => newMesage.MessageAttachmentIds.Contains(x.FileId))
                    .Select(x => new MessageAttachment
                    {
                        Message = message,
                        File = x
                    })
                    .ToList();
                await db.SaveChangesAsync();
                return message;
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        
    }
}
