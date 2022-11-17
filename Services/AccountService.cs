using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using server.ViewModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;

namespace server.Services
{
    public class AccountService
    {

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
    }
}
