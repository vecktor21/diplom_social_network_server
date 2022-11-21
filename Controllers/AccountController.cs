using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using NuGet.Common;
using server.Models;
using server.Services;
using server.ViewModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {
        private ApplicationContext db;
        private AccountService accountService;
        public AccountController(ApplicationContext ct, AccountService accountService)
        {
            db = ct;
            this.accountService = accountService;
        }
        [HttpPost("[action]")]
        public async Task<ActionResult> Register(RegistrationModel newUser )
        {
            if (ModelState.IsValid)
            {
                Models.File default_image = new Models.File
                {
                    LogicalName = "default_avatar.png",
                    PhysicalName = "default_avatar.png",
                    FileType = "image",
                    PublicationDate = DateTime.Now,
                    FileLink = "/files/images/default_avatar.png"
                };
                List<JwtSecurityToken> tokens = accountService.CreateTokens(email: newUser.Email, login: newUser.Login, role: newUser.Role);
                UserToken userToken = new UserToken { RefreshToken = new JwtSecurityTokenHandler().WriteToken(tokens[1])};
                string access_token_encoded = new JwtSecurityTokenHandler().WriteToken(tokens[0]);
                
                Country country = db.Countries.Single(x => x.CountryID == newUser.UserInfo.Country.CountryID);
                UserRole role = db.UserRoles.Single(x => x.RoleName == newUser.Role);

                UserInfoPrivacyType privacy = db.UserInfoPrivacyTypes.FirstOrDefault(x => x.UserInfoPrivacyTypeName == newUser.UserInfo.UserInfoPrivacyType.UserInfoPrivacyTypeName);
                User user = new User
                {
                    Login = newUser.Login,
                    Email = newUser.Email,
                    Nickname = newUser.Nickname,
                    Name = newUser.Name,
                    Surname = newUser.Surname,
                    Password = BCrypt.Net.BCrypt.HashPassword(newUser.Password),
                    Image = default_image,
                    Token = userToken,
                    Role = role,
                    UserStatus = new UserStatus { StatusFrom = DateTime.Now, StatusName = "NORMAL"},
                    UserInfo = new UserInfo
                    {
                        Age = newUser.UserInfo.Age,
                        City = newUser.UserInfo.City,
                        Country = country,
                        Status = newUser.UserInfo.Status,
                        DateOfBirth = newUser.UserInfo.DateOfBirth,
                        Education = newUser.UserInfo.Education,
                        UserInfoPrivacyType = privacy
                    }
                };
                try
                {
                    await db.Users.AddAsync(user);
                    await db.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", e.Message);
                    return BadRequest(ModelState);
                }
                return Json(new
                {
                    Token = access_token_encoded,
                    User = new UserViewModel(user)
                });
            }
            return BadRequest(ModelState);
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> Login(LoginModel user)
        {
            User user_candidate;
            if (ModelState.IsValid)
            {
                user_candidate = db.Users.Include(x => x.Role).Include(x => x.Image).FirstOrDefault(x => x.Login == user.Login);
            }
            else
            {
                return BadRequest("введит логин или почту");
            }
            if(user_candidate == null)
            {
                return BadRequest("неверный логин или почта");
            }
            if(!BCrypt.Net.BCrypt.Verify(user.Password, user_candidate.Password))
            {
                return BadRequest("неверный пароль");
            }
            UserToken userToken = db.UserTokens.Include(x=>x.User).Single(x => x.User.UserId == user_candidate.UserId);
            List<JwtSecurityToken> tokens = accountService.CreateTokens(email: user_candidate.Email, login: user_candidate.Login, role: user_candidate.Role.RoleName);
            userToken.RefreshToken = new JwtSecurityTokenHandler().WriteToken(tokens[1]);
            db.UserTokens.Update(userToken);
            await db.SaveChangesAsync();
            return Json(new
            {
                Token = new JwtSecurityTokenHandler().WriteToken(tokens[0]),
                User = new UserViewModel(user_candidate)
            });
        }
        //логика:
        /*
         * 1) если HttpContext.User.Claims = null:
         * 2) выбрать нужный токен у пользователя из бд
         * 3) если токен валидный ???, то:
         * 4) создать новый access и refersh токены
         * 5) записать refresh в бд, отправить access в ответ
         * 6) если токен в бд не валидный:
         * 7) вернуть ошибку, либо статус 405
         * 
         * на клиенте:
         * при каждой отпарвке запроса на сервер:
         * добавлять header Authorization = Bearer + token
         * если статус ответа 200 - все хорошо
         * если статус ответа 405 - отправить запрос на api/account/checkauth с телом: {LoginModel user}
         */
        
        [HttpGet("[action]")]
        public async Task<IActionResult> CheckAuth(int userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("не найден пользователь");
            }
            User user = db.Users.Include(x => x.Token).Include(x=>x.Role).Include(x=>x.Image).FirstOrDefault(x => x.UserId == userId);
            UserToken userToken = db.UserTokens.Include(x => x.User).Single(x => x.User.UserId == userId);
            if (user == null)
            {
                return BadRequest("не найден пользователь");
            }
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                try
                {
                    var validatedToken = accountService.ValidateToken(userToken.RefreshToken);
                    if(DateTime.UtcNow <= validatedToken.ValidTo)
                    {
                        List<JwtSecurityToken> tokens = accountService.CreateTokens(email: user.Email, login: user.Login, role: user.Role.RoleName);
                        userToken.RefreshToken = new JwtSecurityTokenHandler().WriteToken(tokens[1]);
                        db.UserTokens.Update(userToken);
                        await db.SaveChangesAsync();
                        return Json(new
                        {
                            Token = new JwtSecurityTokenHandler().WriteToken(tokens[0]),
                            User = new UserViewModel(user)
                        });
                    }
                    else
                    {
                        return Unauthorized();
                    }
                }
                catch (Exception e)
                {
                    return Unauthorized();
                }
            }
            else
            {
                List<JwtSecurityToken> tokens = accountService.CreateTokens(email: user.Email, login: user.Login, role: user.Role.RoleName);
                userToken.RefreshToken = new JwtSecurityTokenHandler().WriteToken(tokens[1]);
                db.UserTokens.Update(userToken);
                await db.SaveChangesAsync(); 
                return Json(new
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(tokens[0]),
                    User = new UserViewModel(user)
                });
            }
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> Logout(int userId)
        {
            try
            {
                if (userId == null)
                {
                    return BadRequest("ошибка. не верный userId");
                }
                User user = db.Users.Include(x => x.Token).FirstOrDefault(x => x.UserId == userId);
                if (user == null)
                {
                    return BadRequest("ошибка. не верный userId");
                }
                user.Token.RefreshToken = "";
                db.Users.Update(user);
                await db.SaveChangesAsync();
                return Ok();
            }
            catch(Exception e)
            {
                return BadRequest("ошибка. не верный userId");
            }
        }
    }
}
