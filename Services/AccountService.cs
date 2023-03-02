using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using server.Models;
using server.ViewModels;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Security.Principal;

namespace server.Services
{
    public class AccountService
    {
        private readonly ApplicationContext db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AccountService(ApplicationContext db, IHttpContextAccessor httpContextAccessor)
        {
            this.db = db;
            _httpContextAccessor = httpContextAccessor;
        }

        public ClaimsIdentity GetIdentity(TokenAuthModel user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
                new Claim("email", user.Email),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role)
            };

            return new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
        }
        public JwtSecurityToken CreateToken(ClaimsIdentity claims, int lifeTime)
        {
            var now = DateTime.UtcNow;
            return new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                notBefore: now,
                claims: claims.Claims,
                expires: now.Add(TimeSpan.FromMinutes(lifeTime)),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
            );
        }
        public SecurityToken ValidateToken(string token)
        {
            ClaimsPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(token, AuthOptions.GetTokenValidationParameters(), out SecurityToken validatedToken);
            
            return validatedToken;
        }
        public List<JwtSecurityToken> CreateTokens(string email, string login, string role)
        {
            ClaimsIdentity identity = GetIdentity(new TokenAuthModel { Email = email, Login = login, Role = role });
            JwtSecurityToken access_token = CreateToken(identity, 60*24);
            JwtSecurityToken refresh_token = CreateToken(identity, 3*60 * 24);
            return new List<JwtSecurityToken> { access_token, refresh_token };
        }

        //проверка авторизации
        public bool IsCurrentUserAdmin()
        {
            string userName = _httpContextAccessor.HttpContext.User.Identity.Name;
            //проверка авторизации пользователя
            if (string.IsNullOrEmpty(userName))
            {
                return false;
            }
            User user = db.Users.Include(x=>x.Role).FirstOrDefault(x => x.Login == userName);
            
            //проверка существования пользователя
            if (user == null)
            {
                return false;
            }
            //проверка роли
            if (user.RoleId != 1)
            {
                return false;
            }

            return true;
        }
        //проверка доступа к странице пользователя
        public bool IsAllowed(int targetUserId, int? currentUserId, out string message)
        {
            User? targetUser = db.Users
                .Include(x=>x.UserInfo.UserInfoPrivacyType)
                .Include(x=>x.BlockedUsers)
                .FirstOrDefault(x => x.UserId == targetUserId);
            //проверка на то, существует ли искомый пользователь
            if (targetUser == null)
            {
                message = "пользователь не найден";
                return false;
            }
            //проверка, если приватная страница - то нельзя 
            if (targetUser.UserInfo.UserInfoPrivacyTypeId == 2)
            {
                message = "вы не можете просматривать приватную страницу";
                return false;
            }
            //проверка, есть ли текущий пользователь
            User? currentUser = db.Users.FirstOrDefault(x => x.UserId == currentUserId);
            if (currentUser == null)
            {
                message = "пользователь не найден";
                return false;
            }
            //проверка, заблокированы или нет
            UserBlockList? userBlock = db.UserBlockList.FirstOrDefault(x=>x.UserId==targetUserId && x.BlockedUserId==currentUserId);
            if (userBlock!=null)
            {
                message = "вы в черном списке у данного пользователя";
                return false;
            }
            //если страница для друзей, то проверить - друзья вы или нет
            if(targetUser.UserInfo.UserInfoPrivacyTypeId == 3)
            {
                Friend? friend = db.Friends.FirstOrDefault(x => (x.User1Id == currentUserId && x.User2Id == targetUserId) || (x.User2Id == currentUserId && x.User1Id == targetUserId));
                if (friend == null)
                {
                    message = "данную страницу могут просматривать только друзья";
                    return false;
                }
                message = "доступ есть";
                return true;
            }
            //если страница публичная - можно посмотреть
            message = "доступ есть";
            return true;
        }
    }
}
