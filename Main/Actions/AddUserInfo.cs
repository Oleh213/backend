using System;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebShop.Main.Conext;
using WebShop.Main.DBContext;
using WebShop.Models;
using Microsoft.AspNetCore.Authorization;

namespace WebShop.Main.Actions
{
    [ApiController]
    [Route("AddUserInfo")]
    public class AddUserInfo : ControllerBase
    {
        private ShopContext _context;
        public AddUserInfo(ShopContext context)
        {
            _context = context;
        }

        private Guid UserId => Guid.Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        [HttpPost("AddInfo")]
        [Authorize]
        public IActionResult AddInfo([FromBody] UserInfoModel model)
        {

            var user = _context.users.FirstOrDefault(user => user.UserId == UserId);

            if (user != null)
            {

                var users = _context.users.Where(user=> user.Name != model.Name);

                if(users.FirstOrDefault(x=> x.Name == model.Name) == null)
                {
                    if(users.FirstOrDefault(email=> email.Email == model.Email) == null)
                    {
                        user.Name = model.Name;
                        user.Email = model.Email;
                        user.LastName = model.LastName;
                        user.PhoneNumber = model.PhoneNumber;
                        user.Birthday = model.Birthday;

                        _context.SaveChanges();

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
    }

}