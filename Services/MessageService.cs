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
                Message message = new Message
                {
                    ChatRoomId = newMesage.ChatRoomId,
                    SenderId = newMesage.SenderId,
                    Text = newMesage.Text
                };
                await db.Messages.AddAsync(message);

                List<MessageAttachment> messageAttachments = db.Files
                    .Where(x => newMesage.MessageAttachemntIds.Contains(x.FileId))
                    .Select(x=>new MessageAttachment
                    {
                        Message = message,
                        File = x
                    })
                    .ToList();

                return message;
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        
    }
}
