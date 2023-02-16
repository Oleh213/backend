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
    public class GetUsers : ControllerBase
    {
        private readonly ShopContext _context;

        private Guid UserId => Guid.Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public GetUsers(ShopContext context) => _context = context;


        [HttpGet(Name = "GetUser")]
        public IActionResult GetUser()
        {
            var user = _context.users.FirstOrDefault(x => x.UserId == UserId);

            byte permition = 1;

            if(user.Role == UserRole.Admin)
            {
                permition = 0;
            }
            else if(user.Role == UserRole.Manager)
            {
                permition = 2;
            }


            var userId = new UsersId{ UserId = user.UserId, UserRole = permition};

            return Ok(userId);
        }
    }
    public class UsersId
    {
        public Guid UserId { get; set; }

        public byte UserRole { get; set; }
    }
}

