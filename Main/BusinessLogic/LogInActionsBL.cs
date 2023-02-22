using System;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WebShop.Main.Conext;
using WebShop.Main.DBContext;
using WebShop.Main.Interfaces;
using BC = BCrypt.Net.BCrypt;
using Authenticate;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;

namespace WebShop.Main.BusinessLogic
{
	public class LogInActionsBL : ILogInActionsBL
    {
        private ShopContext _context;

        private readonly IOptions<AuthOptions> authOptions;


        public LogInActionsBL(ShopContext context, IOptions<AuthOptions> authOptions)
        {
            _context = context;
            this.authOptions = authOptions;
        }

        public async Task<User> AuthenticateUser(string name, string password)
        {
            var user = await _context.users.SingleOrDefaultAsync(u => u.Name == name);

            if(user != null)
            {
                if (BC.Verify(password, user.Password))
                {
                    user.Password = BC.HashPassword(password);

                    return user;
                }
                else
                    return null;
            }
            else return null;
        }

        public string GenerateJWT(User user)
        {
            var authParams = authOptions.Value;

            var securitykey = authParams.GetSymmetricSecuritykey();
            var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>() {
                new Claim (JwtRegisteredClaimNames.Name ,user.Name),
                new Claim (JwtRegisteredClaimNames.Sub, user.UserId.ToString())
            };

            claims.Add(new Claim("role", user.Role.ToString()));

            var token = new JwtSecurityToken(authParams.Issuer,
                authParams.Audience,
                claims,
                expires: DateTime.Now.AddSeconds(authParams.TokenLifetime),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


    }
}

