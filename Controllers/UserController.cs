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
using server.Services;
using server.ViewModels;
using server.ViewModels.Additional;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly ApplicationContext db;
        private readonly AccountService accountService;
        private readonly GroupService groupService;

        public UserController(ApplicationContext context, AccountService accountService, GroupService groupService)
        {
            db = context;
            this.accountService = accountService;
            this.groupService = groupService;
        }
        [HttpGet("users")]
        //[Authorize(Roles = "ADMIN")]
        public async Task<IEnumerable<User>> GetUsers()
        {
            return await db.Users.ToListAsync();
        }
        //проверка доступа к странице
        [HttpGet("[action]")]
        public IActionResult IsAllowed(int targetUserId, int? currentUserId)
        {
            if(accountService.IsAllowed(targetUserId, currentUserId, out string mes)){
                return Ok();
            }
            else
            {
                return BadRequest(mes);
            }
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
        /// <summary>
        /// изменение информации о пользователе
        /// </summary>
        /// <param name="newUserInfo"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        [Authorize]
        public async Task<IActionResult> ChangeUser(UserChangeViewModel newUserInfo)
        {
            try
            {
                User user = db.Users.First(x=>x.UserId==newUserInfo.UserId);
                user.Name=newUserInfo.Name;
                user.Surname=newUserInfo.Surname;
                user.Nickname=newUserInfo.Nickname;
                user.Email=newUserInfo.Email;
                db.Users.Update(user);
                await db.SaveChangesAsync();
                return Ok();

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        /// <summary>
        /// изменение аватарки
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="newImageId"></param>
        /// <returns></returns>

        [HttpPost("[action]")]
        public async Task<IActionResult> ChangeUserProfileImage(int userId, int newImageId)
        {
            
            try
            {
                User? user = db.Users
                .Include(x => x.Image)
                .FirstOrDefault(x => x.UserId == userId);
                if (user == null)
                {
                    return NotFound("пользователь не найден");
                }
                Models.File? newImage = db.Files.FirstOrDefault(x => x.FileId == newImageId);
                if (newImage == null)
                {
                    return NotFound("файл не найден");
                }
                if(newImage.FileType.ToLower()!= "image")
                {
                    return BadRequest("необходимо загружать изображения");
                }
                user.Image.LogicalName = newImage.LogicalName;
                user.Image.PhysicalName = newImage.PhysicalName;
                user.Image.FileLink = newImage.FileLink;
                user.Image.PublicationDate = newImage.PublicationDate;
                db.Files.Update(user.Image);
                await db.SaveChangesAsync();
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }
        /// <summary>
        /// метод возвращает инфу о пользователе
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("[action]/{userId}")]
        public IActionResult GetUserInfo(int userId, int? currentUserId)
        {
            User? user = db.Users
                .Include(x=>x.BlockedUsers)
                .Include(x=>x.UserInfo)
                .ThenInclude(x=>x.UserInfoPrivacyType)
                .Include(x => x.UserInfo)
                .ThenInclude(x => x.Country)
                .FirstOrDefault(x=>x.UserId==userId);
            if (user == null)
            {
                return NotFound("пользователь не найден");
            }
            if (currentUserId != null)
            {
                if (user.BlockedUsers.Exists(x => (x.BlockedUserId == currentUserId) && (x.UserId == userId)))
                {
                    return Json(null);
                }
            }
            return Json(new UserInfoViewModel(user));
        }

        /// <summary>
        /// изменение общей информации о пользователе
        /// </summary>
        /// <param name="newUserInfo"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        [Authorize]
        public async Task<IActionResult> ChangeUserInfo(UserInfoUpdateViewModel newUserInfo)
        {
            try
            {
                newUserInfo.UserInfoPrivacyType = db.UserInfoPrivacyTypes.First(x => x.UserInfoPrivacyTypeId == newUserInfo.UserInfoPrivacyType.UserInfoPrivacyTypeId);
                newUserInfo.Country = db.Countries.First(x => x.CountryID == newUserInfo.Country.CountryID);
                UserInfo oldInfo = db.UserInfo.First(x => x.UserInfoId == newUserInfo.UserInfoId);
                if (oldInfo == null)
                {
                    return NotFound("не найдена информация о пользователе");
                }
                oldInfo.Status = newUserInfo.Status;
                oldInfo.Age = newUserInfo.Age;
                oldInfo.DateOfBirth = newUserInfo.DateOfBirth;
                oldInfo.City = newUserInfo.City;
                oldInfo.Country = newUserInfo.Country;
                oldInfo.CountryId = newUserInfo.Country.CountryID;
                oldInfo.Education = newUserInfo.Education;
                oldInfo.UserInfoPrivacyType = newUserInfo.UserInfoPrivacyType;
                oldInfo.UserInfoPrivacyTypeId = newUserInfo.UserInfoPrivacyType.UserInfoPrivacyTypeId;

                db.UserInfo.Update(oldInfo);
                await db.SaveChangesAsync();
                return Ok();
            }
            catch(Exception e)
            {
                return BadRequest();
            }
        }


        /// <summary>
        /// поиск людей
        /// </summary>
        /// <param name="page">номер страницы
        /// рассчет по формуле Count/Take (округлить вверх)
        /// </param>
        /// <param name="page">номер страницы
        /// рассчет по формуле Count/Take (округлить вверх)
        /// </param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public IActionResult FindUsers(string searchString, int? page, int? take)
        {
            if (String.IsNullOrEmpty(searchString))
            {
                return BadRequest("строка поиска не должна быть пустой ");
            }
            List<UserShortViewModel> users = accountService.FindUsers(searchString);
            int total = users.Count;
            if (page != null && take != null)
            {
                users = users.Paginate((int)page, (int)take).ToList();
            }
            PaginationParams pgParams = new PaginationParams
            {
                total = total,
                page = page,
                skip = (page - 1) * take,
                take = take,
                totalPages = (int)Math.Ceiling((decimal)total / (take ?? 10))
            };
            return Json(new PaginationViewModel<UserShortViewModel>
            {
                values = users,
                paginationParams = pgParams
            });
        }

    }
}
