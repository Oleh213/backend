using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Xml.Linq;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using WebShop.Main.BusinessLogic;
using WebShop.Main.Conext;
using WebShop.Main.Context;
using WebShop.Main.DBContext;
using WebShop.Main.Interfaces;
using WebShop.Models;
using static Azure.Core.HttpHeader;

namespace Shop.Main.Actions
{
    [ApiController]
    [Route("[controller]")]
    public class RegistActions : ControllerBase
    {
        private IRegistActionsBL _registActionsBL;

        private readonly ILoggerBL _loggerBL;

        public RegistActions(IRegistActionsBL registActionsBL, ILoggerBL loggerBL)
        {
            _registActionsBL = registActionsBL;
            _loggerBL = loggerBL;
        }

        [HttpPost("Registration")]
        public async Task<IActionResult> Registration([FromBody] RegisterModel model)
        {
            try {
                if (await _registActionsBL.CheckName(model.Name))
                {
                    var resEr = new Response<string>()
                    {
                        IsError = true,
                        ErrorMessage = "401",
                        Data = $"Enter another username!"
                    };
                    return Unauthorized(resEr);
                }
                else if (await _registActionsBL.CheckEmail(model.Email))
                {
                    var resEr2 = new Response<string>()
                    {
                        IsError = true,
                        ErrorMessage = "401",
                        Data = $"This email connect to other user!"
                    };
                    return Unauthorized(resEr2);
                }
                else
                {
                    await _registActionsBL.Regist(model.Name, model.Email, model.Password);

                    var res = new Response<string>()
                    {
                        IsError = false,
                        ErrorMessage = null,
                        Data = $"Registration successful! {model.Name}, Welcome to our shop!"
                    };

                    _loggerBL.AddLog(LoggerLevel.Info, $"User:'{model.Name}' register new account");
                    return Ok(res);
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
