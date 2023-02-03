using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebShop.Main.Conext;
using WebShop.Main.DBContext;
using WebShop.Models;

namespace WebShop.Reguests
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class GetUserInfo : ControllerBase
    {
        private readonly ShopContext _context;

        private Guid UserId => Guid.Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public GetUserInfo(ShopContext context) => _context = context;


        [HttpGet(Name = "GetInfo")]
        [Authorize]
        public IActionResult GetInfo()
        {
            var user = _context.users.FirstOrDefault(x => x.UserId == UserId);

            if (user != null)
            {
                if(user.LastName.Length == 0)
                {
                    user.LastName = "Not specified";
                }
                if(user.Email.Length == 0)
                {
                    user.Email = "Not specified";
                }
                if(user.PhoneNumber.Length == 0)
                {
                    user.PhoneNumber = "Not specified";
                }
                if(user.Birthday.Length == 0)
                {
                    user.Birthday = "Not specified";
                }

                var outUser = new UserInfoModel()
                {
                    Name = user.Name,
                    LastName = user.LastName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Birthday = user.Birthday
                };
                return Ok(outUser);
            }
            else
                return NotFound();
        }
    }
}

