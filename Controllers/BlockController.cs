using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Models;
using server.ViewModels;
using System.Web.Http.Controllers;

namespace server.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class BlockController : ControllerBase
    {

        private readonly ApplicationContext db;

        public BlockController(ApplicationContext context)
        {
            db = context;
        }
        //просмотр заблокированных пользователей

        [HttpGet("user")]
        public IActionResult GetUserBlockList(int userId)
        {
            return Ok(db.UserBlockList.Include(x => x.BlockedUser)
                .ThenInclude(x => x.Image).Where(x => x.UserId == userId).Select(x => new BlockUserViewModel(x)).ToList());
        }
        //просмотр заблокированных пользователей в группах
        [HttpGet("group")]
        public IActionResult GetGroupBlockList(int groupId)
        {
            return Ok(db.GroupBlockList.Include(x => x.BlockedUser)
                .ThenInclude(x => x.Image).Where(x => x.GroupId == groupId).Select(x => new BlockUserViewModel(x)).ToList());
        }
        //бан пользователя
        [HttpPut("user")]
        [Authorize]
        public async Task<IActionResult> AddUserToBlockList(BlockUserAddViewModel model)
        {
            try
            {
                User? user = db.Users.FirstOrDefault(x => x.UserId == model.UserId);
                User? blockedUser = db.Users.FirstOrDefault(x => x.UserId == model.BlockedUserId);
                bool isUserBlocked = db.UserBlockList.FirstOrDefault(x => x.BlockedUserId == model.BlockedUserId && x.UserId == model.UserId) != null;
                if (isUserBlocked) { return Ok("пользователь уже заблокирован"); }
                if (user == null || blockedUser == null) { return NotFound("пользователь не найден"); }
                db.UserBlockList.Add(new UserBlockList
                {
                    BlockedUserId = model.BlockedUserId,
                    UserId = model.UserId,
                    DateFrom = DateTime.Now,
                    DateTo = model.DateTo ?? DateTime.Now.AddYears(2),
                    Reason=model.Reason
                });
                await db.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        //бан пользователя в группе
        [HttpPut("group")]
        [Authorize]
        public async Task<IActionResult> AddGroupUserToBlockList(BlockGroupAddViewModel model)
        {
            try
            {
                Group? group = db.Groups.FirstOrDefault(x => x.GroupId == model.GroupId);
                User? blockedUser = db.Users.FirstOrDefault(x => x.UserId == model.BlockedUserId);
                bool isUserBlocked = db.GroupBlockList.FirstOrDefault(x => x.BlockedUserId == model.BlockedUserId && x.GroupId == model.GroupId) != null;
                if (isUserBlocked) { return Ok("пользователь уже заблокирован"); }
                if (group == null || blockedUser == null) { return NotFound("не найден"); }
                db.GroupBlockList.Add(new GroupBlockList
                {
                    BlockedUserId = model.BlockedUserId,
                    GroupId = model.GroupId,
                    DateFrom = DateTime.Now,
                    DateTo = model.DateTo ?? DateTime.Now.AddYears(2),
                    Reason = model.Reason
                });
                await db.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        //удаление пользователя из черного списка 
        [HttpDelete("user")]
        [Authorize]
        public async Task<IActionResult> RemoveFromUserBlockist(int userId, int blockUserId)
        {
            UserBlockList? user = db.UserBlockList.FirstOrDefault(x => x.UserId == userId && x.BlockedUserId == blockUserId);
            if (user == null)
            {
                return NotFound("пользователь не найден");
            }
            db.UserBlockList.Remove(user);
            await db.SaveChangesAsync();
            return Ok();
        }

        //удаление пользователя из черного списка группы
        //удаление пользователя из черного списка 
        [HttpDelete("group")]
        [Authorize]
        public async Task<IActionResult> RemoveFromGroupBlockist(int groupId, int blockUserId)
        {
            GroupBlockList? user = db.GroupBlockList.FirstOrDefault(x => x.GroupId == groupId && x.BlockedUserId == blockUserId);
            if (user == null)
            {
                return NotFound("пользователь не найден");
            }
            db.GroupBlockList.Remove(user);
            await db.SaveChangesAsync();
            return Ok();

        }
    }

}