using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Models;
using server.Services;
using server.ViewModels;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessengerController : ControllerBase
    {
        private ApplicationContext db;
        public MessengerController(ApplicationContext applicationContext) 
        {
            this.db = applicationContext;

        
        }

        [HttpGet("[action]/{chatRoomId}")]
        public ActionResult GetChatRoom(int chatRoomId)
        {
            ChatRoom chatRoom = db.ChatRooms
                .Include(x=>x.UserChatRooms)
                .ThenInclude(x=>x.User.Image)
                .Include(x => x.UserChatRooms)
                .ThenInclude(x => x.User.Role)
                .Include(x=>x.UserChatRooms)
                .ThenInclude(x=>x.UserChatRoomRole)
                .Include(x=>x.Messages)
                .ThenInclude(x=>x.MessageAttachments)
                .ThenInclude(x=>x.File)
                .Include(x => x.Messages)
                .ThenInclude(x=>x.Sender.Image)
                .Include(x => x.Messages)
                .ThenInclude(x => x.Sender.Role)
                .FirstOrDefault(x=>x.ChatRoomId==chatRoomId);
            if (chatRoom == null)
            {
                return NotFound();
            }
            ChatRoomViewModel chatRoomViewModel = new ChatRoomViewModel(chatRoom);
            return new JsonResult(chatRoomViewModel);
            
        }
    }
}
