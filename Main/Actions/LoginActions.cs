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
using WebShop.Main.Context;

namespace Shop.Main.Actions
{

    [ApiController]
    [Route("[controller]")] 
    public class LogInActions : ControllerBase  
    {
        private readonly ILoggerBL _loggerBL;

        private ILogInActionsBL _logInActionsBL;

        public LogInActions(ILogInActionsBL logInActionsBL, ILoggerBL loggerBL)
        {
            _logInActionsBL = logInActionsBL;
            _loggerBL = loggerBL;
        }

        [HttpPost("LogIn")]
        public async Task<IActionResult> LogIn([FromBody] LoginModule model)
        {
            try
            {
                var user = await _logInActionsBL.AuthenticateUser(model.Name, model.Password);

                if (user != null)
                {
                    var token = _logInActionsBL.GenerateJWT(user);

                    _loggerBL.AddLog(LoggerLevel.Info, $"User:'{user.UserId}' login to account");

                    return Ok(new
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
                    _loggerBL.AddLog(LoggerLevel.Warn, $"User:'{model.Name}' enter incorrect info");
                    return Unauthorized(resEr);
                }
            }
            catch (Exception ex)
            {
                _loggerBL.AddLog(LoggerLevel.Error, ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        
    }
}


