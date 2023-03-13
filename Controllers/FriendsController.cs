using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using server.Models;
using server.Services;
using server.ViewModels;
using System.Linq;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FriendsController : Controller
    {
        private readonly ApplicationContext db;
        private readonly AccountService accountService;

        public FriendsController(ApplicationContext context, AccountService accountService)
        {
            db = context;
            this.accountService = accountService;
        }
        [HttpGet]
        public async Task<IActionResult> GetFriends(int userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            List<UserShortViewModel> friends = new List<UserShortViewModel>();
            friends.AddRange(db.Friends
                .Where(x => x.User1Id != userId && x.User2Id == userId)
                .Include(x=>x.User1)
                .ThenInclude(x=>x.Role)
                .Include(x => x.User1)
                .ThenInclude(x => x.Image)
                .Select(x => new UserShortViewModel(x.User1.UserId, x.User1.Name + " " + x.User1.Surname, x.User1.Image.FileLink))
                .ToList());
            friends.AddRange(
                db.Friends
                .Where(x => x.User2Id != userId && x.User1Id == userId)
                .Include(x => x.User2)
                .ThenInclude(x => x.Role)
                .Include(x => x.User2)
                .ThenInclude(x => x.Image)
                .Select(x => new UserShortViewModel(x.User2.UserId, x.User2.Name + " " + x.User2.Surname, x.User2.Image.FileLink))
                .ToList());
            return Json(friends);
        }


        [HttpGet("[action]")]
        public IActionResult GetAllFriends()
        {
            return Json(db.Friends.ToList());
        }


        [HttpGet("[action]")]
        public async Task<IActionResult> FindUsers(string search)
        {
            if (string.IsNullOrEmpty(search))
            {
                return Json(Array.Empty<string>());
            }
            List<UserShortViewModel> foundUsers = accountService.FindUsers(search);
            return Json(foundUsers);
        }


        [Authorize]
        [HttpPost("[action]")]
        public async Task<IActionResult> FriendRequest(FriendRequestViewModel request)
        {
            try
            {
                User sender = db.Users.Single(x => x.UserId == request.SenderId);
                User receiver = db.Users.Single(x => x.UserId == request.UserId);
                db.FriendRequests.Add(new FriendRequest
                {
                    SenderId = request.SenderId,
                    UserId = request.UserId,
                    Message = request.Message,
                });
                await db.SaveChangesAsync();
                return Ok();
            }
            catch(Exception e)
            {
                return BadRequest("ошибка");
            }
            return Ok();
            
        }


        [HttpGet("[action]")]
        public async Task<IActionResult> GetFriendRequests()
        {
            return Json(db.FriendRequests.ToList());
        }



        [Authorize]
        [HttpGet("[action]/{userId}")]
        public async Task<IActionResult> GetFriendRequests(int userId)
        {
            var res = db.FriendRequests
                .Where(x => x.UserId == userId)
                .Include(x => x.Sender)
                .Select(x => new {
                    requestID = x.RequestID,
                    userId = x.UserId,
                    senderId =  x.SenderId,
                    message = x.Message,
                    sender = x.Sender
                })
                .ToList();
            return Json(res);
        }



        //[Authorize]
        [HttpGet("FriendRequest")]
        public async Task<IActionResult> FriendRequestReact(int requestId, bool isAccepted) 
        {
            try
            {
                FriendRequest req = db.FriendRequests.Include(x => x.User).Include(x => x.Sender).FirstOrDefault(x => x.RequestID == requestId);
                if (!isAccepted)
                {
                    db.FriendRequests.Remove(req);
                    await db.SaveChangesAsync();
                    return Ok();
                }
                Friend friend = new Friend
                {
                    User1Id = req.SenderId,
                    User2Id = req.UserId
                };
                db.Friends.Add(friend);
                db.FriendRequests.Remove(req);
                await db.SaveChangesAsync();
                return Ok();
            }
            catch(Exception e)
            {
                return BadRequest();
            }
        }


        [HttpGet("[action]")]
        public bool HasFriendRequest(int currentUserId, int targetUserId)
        {
            FriendRequest req = db.FriendRequests.FirstOrDefault(x => x.UserId == targetUserId & x.SenderId == currentUserId);
            if(req == null)
            {
                return false;
            }
            return true;
        }


        [HttpGet("[action]")]
        public bool IsFriend(int currentUserId, int targetUserId)
        {
            Friend friend = db.Friends.FirstOrDefault(x=> (x.User1Id == currentUserId && x.User2Id == targetUserId) || (x.User2Id == currentUserId && x.User1Id == targetUserId));
            if (friend == null)
            {
                return false;
            }
            return true;
        }


        [HttpDelete("[action]")]
        public async Task<IActionResult> RemoveFromFriends(int currentUserId, int targetUserId)
        {
            Friend friend = db.Friends.FirstOrDefault(x => (x.User1Id == currentUserId && x.User2Id == targetUserId) || (x.User2Id == currentUserId && x.User1Id == targetUserId));
            if (friend == null)
            {
                return BadRequest();
            }
            db.Friends.Remove(friend);
            await db.SaveChangesAsync();
            return Ok();
        }


        [HttpDelete("[action]")]
        public async Task<IActionResult> RemoveFriendRequest(int currentUserId, int targetUserId)
        {
            FriendRequest req = db.FriendRequests.FirstOrDefault(x => x.UserId == targetUserId & x.SenderId == currentUserId);
            if (req == null)
            {
                return BadRequest();
            }
            db.Remove(req);
            await db.SaveChangesAsync();
            return Ok();
        }
    }
}
