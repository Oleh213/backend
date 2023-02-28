using System;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebShop.Main.Conext;
using WebShop.Main.DBContext;
using WebShop.Models;
using Microsoft.AspNetCore.Authorization;
using WebShop.Main.DTO;
using WebShop.Main.Interfaces;
using WebShop.Main.BusinessLogic;
using WebShop.Main.Context;

namespace WebShop.Main.Actions
{
    [ApiController]
    [Route("UserActions")]
    public class UserActions : ControllerBase
    {
        private IUserActionsBL _userActionsBL;

        private readonly ILoggerBL _loggerBL;

        public UserActions(IUserActionsBL userActionsBL, ILoggerBL loggerBL)
        {
            _userActionsBL = userActionsBL;
            _loggerBL = loggerBL;
        }

        private Guid UserId => Guid.Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        [HttpPost("AddUserInfo")]
        [Authorize]
        public async Task<IActionResult> AddUserInfo([FromBody] UserInfoModel model)
        {
            try
            {
                var user = await _userActionsBL.GetUser(UserId);

                if (user != null)
                {
                    if (!await _userActionsBL.CheckName(model.Name))
                    {
                        if (!await _userActionsBL.CheckEmail(model.Email))
                        {

                            await _userActionsBL.ChangeUserInfo(model, user);

                            var res = new Response<string>()
                            {
                                IsError = false,
                                ErrorMessage = "",
                                Data = "Your information successful update"
                            };

                            _loggerBL.AddLog(LoggerLevel.Info, $"User:'{UserId}' update information");
                            return Ok(res);
                        }
                        else
                        {
                            var resError = new Response<string>()
                            {
                                IsError = true,
                                ErrorMessage = "",
                                Data = "Please enter another email!"
                            };

                            return NotFound(resError);
                        }
                    }
                    else
                    {
                        var resError = new Response<string>()
                        {
                            IsError = true,
                            ErrorMessage = "",
                            Data = "Please enter another name!"
                        };
                        return NotFound(resError);
                    }
                }
                else
                    return Unauthorized();
            }
            catch (Exception ex)
            {
                _loggerBL.AddLog(LoggerLevel.Error, ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("GetNameOfUser")]
        [Authorize]
        public async Task<IActionResult> GetNameOfUser()
        {
            try
            {
                var user = await _userActionsBL.GetUser(UserId);

                if (user != null)
                {
                    var res = new Response<string>()
                    {
                        IsError = false,
                        ErrorMessage = "",
                        Data = user.Name,
                    };
                    return Ok(res);
                }
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                _loggerBL.AddLog(LoggerLevel.Error, ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("AddDeliveryInfo")]
        [Authorize]
        public async Task<IActionResult> AddDeliveryInfo([FromBody] DeliveryOptionsModel model)
        {
            try
            {
                var user = await _userActionsBL.GetUserDeliveryInfo(UserId);

                if (user != null)
                {
                    await _userActionsBL.ChangeDeliveryInfo(model, user);

                    var res = new Response<string>()
                    {
                        IsError = false,
                        ErrorMessage = "",
                        Data = "Your delivery information successful update"
                    };

                    _loggerBL.AddLog(LoggerLevel.Info, $"User:'{UserId}' update delivery information");
                    return Ok(res);
                }
                else
                    return Unauthorized();
            }
            catch (Exception ex)
            {
                _loggerBL.AddLog(LoggerLevel.Error, ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("GetUser")]
        [Authorize]
        public async Task<IActionResult> GetUser()
        {
            try
            {
                var user = await _userActionsBL.GetUser(UserId);

                var userId = _userActionsBL.UserDTO(user);

                return Ok(userId);
            }
            catch (Exception ex)
            {
                _loggerBL.AddLog(LoggerLevel.Error, ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("GetUserDeliveryInfo")]
        [Authorize]
        public async Task<IActionResult> GetUserDeliveryInfo()
        {
            try
            {
                var user = await _userActionsBL.GetUserDeliveryInfo(UserId);

                if (user != null)
                {
                    var outUser = await _userActionsBL.DeliveryDTO(user);

                    return Ok(outUser);
                }
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                _loggerBL.AddLog(LoggerLevel.Error, ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("GetUserInfo")]
        [Authorize]
        public async Task<IActionResult> GetUserInfo()
        {
            try
            {
                var user = await _userActionsBL.GetUser(UserId);

                if (user != null)
                {
                    var outUser = await _userActionsBL.UserInfoDTO(user);

                    return Ok(outUser);
                }
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                _loggerBL.AddLog(LoggerLevel.Error, ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}