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
                if(user.LastName == null)
                {
                    user.LastName = "Not specified";
                }
                if(user.Email== null)
                {
                    user.Email = "Not specified";
                }
                if(user.PhoneNumber== null)
                {
                    user.PhoneNumber = "Not specified";
                }
                if(user.Birthday == null)
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

