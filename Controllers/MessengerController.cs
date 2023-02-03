using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
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

        //работа с сообщениями
        //удаление сообщения
        [HttpDelete("[action]/{messageId}")]
        public async Task<IActionResult> RemoveMessage(int messageId)
        {
            try
            {
                Message message = db.Messages.FirstOrDefault(x => x.MessageId == messageId);
                if (message == null)
                {
                    return NotFound("сообщение не найдено");
                }
                db.Messages.Remove(message);
                await db.SaveChangesAsync();
                return Ok();
            }catch
            {
                return BadRequest();
            }
        }


        //работа с чат комнатами

        //создать чат
        [HttpPost("[action]")]
        public async Task<ActionResult> CreateChatRoom(ChatRoomCreateViewModel chatRoomCreateViewModel)
        {
            ChatRoomType chatRoomType = db.ChatRoomTypes.FirstOrDefault(x=>x.ChatRoomTypeId==chatRoomCreateViewModel.ChatRoomTypeId);
            if(chatRoomType == null)
            {
                return BadRequest("ошибка. указан не верный тип чата");
            }
            ChatRoom chatRoom = new ChatRoom
            {
                ChatRoomImageId = 2,
                ChatRoomName = chatRoomCreateViewModel.ChatRoomName,
                ChatRoomType = chatRoomType,
            };
            try
            {
                db.ChatRooms.Add(chatRoom);
                await db.SaveChangesAsync();
                List<UserChatRoom> chatRoomMembers = AddUsersToChatRoom(chatRoom, chatRoomCreateViewModel.ChatRoomMembers);
                if (chatRoomMembers.Any(x => x.User == null))
                {
                    db.ChatRooms.Remove(chatRoom);
                    await db.SaveChangesAsync();
                    return BadRequest("невозможно создать чат. один или несколько пользоватеей не найдены");
                }
                if(chatRoomMembers.Count < 2)
                {
                    db.ChatRooms.Remove(chatRoom);
                    await db.SaveChangesAsync();
                    return BadRequest("невозможно создать чат менее чем для двух пользователей");
                }
                
                if (chatRoomType.ChatRoomTypeId == 2)
                {
                    if (chatRoomCreateViewModel.AdminId == null || !chatRoomMembers.Any(x => x.UserId == chatRoomCreateViewModel.AdminId))
                    {
                        db.ChatRooms.Remove(chatRoom);
                        await db.SaveChangesAsync();
                        return BadRequest("ошибка, указан не верный ID админа");
                    }
                    chatRoomMembers.First(x => x.UserId == chatRoomCreateViewModel.AdminId).UserChatRoomRoleId = 1;

                }
                db.UserChatRooms.AddRange(chatRoomMembers);
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
            return new JsonResult(chatRoom.ChatRoomId);
        }

        //удаление чата
        [HttpDelete("[action]/{chatRoomId}")]
        [Authorize]
        public async Task<IActionResult> DeleteChatRoom(int chatRoomId)
        {
            ChatRoom chatRoom = IncludeChatRoomData()
                .FirstOrDefault(x => x.ChatRoomId == chatRoomId);
            if (chatRoom == null)
            {
                return NotFound();
            }
            try
            {
                //если публичный - дополнительно проверяем роль пользователя
                if (chatRoom.ChatRoomTypeId == 2)
                {
                    User currentUser = db.Users.FirstOrDefault(x => x.Login == HttpContext.User.Identity.Name);
                    UserChatRoomRole userChatRoomRole = chatRoom.UserChatRooms.FirstOrDefault(x => x.UserId == currentUser.UserId).UserChatRoomRole;
                    if (userChatRoomRole == null || userChatRoomRole.UserChatRoomRoleId != 1)
                    {
                        return Forbid("у вас не достаточно прав для удаления чата");
                    }
                }
                //если чат приватный - удаляем спокойно
                //сначала удаляем все сообщения
                List<Message> messages = db.Messages.Where(x => x.ChatRoomId == chatRoom.ChatRoomId).ToList();
                db.Messages.RemoveRange(messages);
                //удаляем пользователей из чата
                List<UserChatRoom> userChatRooms = chatRoom.UserChatRooms.ToList();
                db.UserChatRooms.RemoveRange(userChatRooms);
                //удаляем сам чат
                db.ChatRooms.Remove(chatRoom);
                await db.SaveChangesAsync();
                return Ok();
            }
            catch (Exception e)
            {

                return BadRequest();
            }
        }

        //получить все чаты пользователя
        [HttpGet("[action]/{userId}")]
        public ActionResult GetUserChatRooms(int userId)
        {
            List<ChatRoomViewModel> chatRooms = IncludeUserChatRoomsData()
                .Where(x => x.UserId == userId)
                .Select(x=>new ChatRoomViewModel(x.ChatRoom))
                .ToList();
            return new JsonResult(chatRooms);
        }

        //получить 1 чат
        [HttpGet("[action]/{chatRoomId}")]
        public ActionResult GetChatRoom(int chatRoomId)
        {
            ChatRoom chatRoom = this.IncludeChatRoomData()                
                .FirstOrDefault(x=>x.ChatRoomId==chatRoomId);
            if (chatRoom == null)
            {
                return NotFound();
            }
            ChatRoomViewModel chatRoomViewModel = new ChatRoomViewModel(chatRoom);
            return new JsonResult(chatRoomViewModel);
            
        }

        //добавление пользователей в чат
        private List<UserChatRoom> AddUsersToChatRoom(ChatRoom chatRoom, List<int> userIds)
        {
            List<UserChatRoom> chatRoomMembers = db.Users
                .Where(x => userIds.Contains(x.UserId))
                .Select(x => new UserChatRoom
                {
                    User = x,
                    ChatRoom = chatRoom,
                    UserChatRoomRoleId = 2,
                    UserId = x.UserId
                })
                .ToList();
            return chatRoomMembers;
        }
        //подгрузка данных чата
        private  List<ChatRoom> IncludeChatRoomData()
        {
            return db.ChatRooms
                .Include(x=>x.ChatRoomType)
                .Include(x=>x.ChatRoomImage)
                .Include(x => x.UserChatRooms)
                .ThenInclude(x => x.User.Image)
                .Include(x => x.UserChatRooms)
                .ThenInclude(x => x.User.Role)
                .Include(x => x.UserChatRooms)
                .ThenInclude(x => x.UserChatRoomRole)
                .Include(x => x.Messages)
                .ThenInclude(x => x.MessageAttachments)
                .ThenInclude(x => x.File)
                .Include(x => x.Messages)
                .ThenInclude(x => x.Sender.Image)
                .Include(x => x.Messages)
                .ThenInclude(x => x.Sender.Role)
                .ToList();
        }
        //подгрузка данных чата для случая UserChatRooms
        private List<UserChatRoom> IncludeUserChatRoomsData()
        {
            return db.UserChatRooms
                .Include(x => x.User.Image)
                .Include(x => x.User.Role)
                .Include(x => x.UserChatRoomRole)
                .Include(x=>x.ChatRoom)
                .ThenInclude(x => x.Messages)
                .ThenInclude(x => x.MessageAttachments)
                .ThenInclude(x => x.File)
                .Include(x => x.ChatRoom)
                .ThenInclude(x => x.Messages)
                .ThenInclude(x => x.Sender.Image)
                .Include(x => x.ChatRoom)
                .ThenInclude(x => x.Messages)
                .ThenInclude(x => x.Sender.Role)
                .Include(x => x.ChatRoom)
                .ThenInclude(x => x.ChatRoomType)
                .Include(x => x.ChatRoom)
                .ThenInclude(x => x.ChatRoomImage)
                .ToList();
        }
    }
}
