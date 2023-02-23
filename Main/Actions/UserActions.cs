using System;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebShop.Main.Conext;
using WebShop.Main.DBContext;
using WebShop.Models;
using Microsoft.AspNetCore.Authorization;
using WebShop.Main.DTO;
using WebShop.Main.Interfaces;

namespace WebShop.Main.Actions
{
    [ApiController]
    [Route("UserActions")]
    public class UserActions : ControllerBase
    {
        private ShopContext _context;
        private IUserActionsBL _userActionsBL;

        public UserActions(ShopContext context, IUserActionsBL userActionsBL)
        {
            _context = context;
            _userActionsBL = userActionsBL;
        }

        private Guid UserId => Guid.Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        [HttpPost("AddUserInfo")]
        [Authorize]
        public async Task<IActionResult> AddUserInfo([FromBody] UserInfoModel model)
        {
            var user = await _userActionsBL.GetUser(UserId);

            if (user != null)
            {
                if(!await _userActionsBL.CheckName(model.Name))
                {
                    if(!await _userActionsBL.CheckEmail(model.Email))
                    {

                        await _userActionsBL.ChangeUserInfo(model,user);

                        var res = new Response<string>()
                        {
                            IsError = false,
                            ErrorMessage = "",
                            Data = "Your information successful update"
                        };

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

        [HttpGet("GetNameOfUser")]
        [Authorize]
        public async Task<IActionResult> GetNameOfUser()
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

        [HttpPost("AddDeliveryInfo")]
        [Authorize]
        public async Task<IActionResult> AddDeliveryInfo([FromBody] DeliveryOptionsModel model)
        {
            var user = await _userActionsBL.GetUserDeliveryInfo(UserId);

            if (user != null)
            {
                await _userActionsBL.ChangeDeliveryInfo(model,user);

                var res = new Response<string>()
                {
                    IsError = false,
                    ErrorMessage = "",
                    Data = "Your deliery information successful update"
                };

                return Ok(res);
            }
            else
                return Unauthorized();
        }

        [HttpGet("GetUser")]
        [Authorize]
        public async Task<IActionResult> GetUser()
        {
            var user = await _userActionsBL.GetUser(UserId);

            var userId = _userActionsBL.UserDTO(user);

            return Ok(userId);
        }

        [HttpGet("GetUserDeliveryInfo")]
        [Authorize]
        public async Task<IActionResult> GetUserDeliveryInfo()
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

        [HttpGet("GetUserInfo")]
        [Authorize]
        public async Task<IActionResult> GetUserInfo()
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
    }
}