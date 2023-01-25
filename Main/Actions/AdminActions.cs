using System;
using WebShop.Main.Conext;
using WebShop.Main.DBContext;
using WebShop.Main.Interfaces;
using System.Data.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace Shop.Main.Actions
{
    [ApiController]
    [Route("AddAdminAction")]
    public class AddAdminAction : ControllerBase
    {
        private ShopContext _context;

        public AddAdminAction(ShopContext context)
        {
            _context = context;
        }
        [HttpPut(Name = "AddAdmin")]
        public IActionResult AddAdmin(Guid _userId)
        {
            var user = _context.users.FirstOrDefault(x => x.UserId == _userId);

            user.Role = UserRole.Admin;

            _context.SaveChanges();

            return Ok($"{user.Name} added to admins team");
        }
    }
}

