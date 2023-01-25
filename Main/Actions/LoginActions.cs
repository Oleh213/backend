using System;
using System.Collections.Generic;
using Shop.Main.Actions;
using System.Linq;
using System.IO;
using WebShop.Main.Conext;
using WebShop.Main.DBContext;
using WebShop.Main.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebShop.Models;
using Microsoft.Extensions.Options;
using Authenticate;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.OData.UriParser;

namespace Shop.Main.Actions
{

    [ApiController]
    [Route("[controller]")] 
    public class LogInAction : ControllerBase  
    {
        private readonly IOptions<AuthOptions> authOptions;

        private ShopContext _context;

        public LogInAction(ShopContext context, IOptions<AuthOptions> authOptions)
        {
            _context = context;
            this.authOptions = authOptions;
        }

        [HttpPost(Name = "LogIn")]
        public IActionResult LogIn([FromBody] LoginModule model)
        {

            var user = AuthenticateUser(model.Name, model.Password);

            if (user != null)
            {
                var token = GenerateJWT(user);

                return Ok( new
                {
                    access_token = token
                });
            }
            else
            {
                return Unauthorized();
            }
        }
        private User AuthenticateUser(string name, string password)
        {
            return _context.users.SingleOrDefault(u => u.Name == name && u.Password == password);
        }

        private string GenerateJWT(User user)
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


