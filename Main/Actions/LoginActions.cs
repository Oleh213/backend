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
    public class LogInActions : ControllerBase  
    {
        private ShopContext _context;

        private ILogInActionsBL _logInActionsBL;

        public LogInActions(ShopContext context, ILogInActionsBL logInActionsBL)
        {
            _context = context;

            _logInActionsBL = logInActionsBL;
        }

        [HttpPost("LogIn")]
        public async Task<IActionResult> LogIn([FromBody] LoginModule model)
        {
            var user = await _logInActionsBL.AuthenticateUser(model.Name, model.Password);

            if (user != null)
            {
                var token = _logInActionsBL.GenerateJWT(user);

                return Ok( new
                {
                    access_token = token
                });
            }
            else
            {
                var resEr = new Response<string>()
                {
                    IsError = true,
                    ErrorMessage = "401",
                    Data = "Check your name or password!"
                };
                return Unauthorized(resEr);
            }
        }
        
    }
}


