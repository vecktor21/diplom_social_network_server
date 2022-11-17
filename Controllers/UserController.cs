using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using server;
using server.Models;
using server.ViewModels;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly ApplicationContext db;

        public UserController(ApplicationContext context)
        {
            db = context;
        }
        [HttpGet("users")]
        //[Authorize(Roles = "ADMIN")]
        public async Task<IEnumerable<User>> GetUsers()
        {
            return await db.Users.ToListAsync();
        }
        [HttpGet]
        public async Task<IActionResult> GetUser(int userId)
        {
            try
            {
                User user = db.Users
                    .Include(x => x.Role)
                    .Include(x => x.Image)
                    .First(x => x.UserId == userId);
                return Json(new UserViewModel(user));

            }catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        /*public async Task<IActionResult> GetUserInfo()
        {

        }*/
        //public async Task<UseIActionResultrInfo> ChangeUserInfo()

    }
}
