using System;
using WebShop.Main.Conext;
using WebShop.Main.DBContext;
using WebShop.Main.Interfaces;
using System.Data.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Shop.Main.BusinessLogic;
using WebShop.Main.BusinessLogic;

namespace Shop.Main.Actions
{
    [ApiController]
    [Route("AdminActions")]
    public class AddAdminActions : ControllerBase
    {
        private ShopContext _context;

        private IAdminActionsBL _addAdminActionsBL;

        public AddAdminActions(ShopContext context, IAdminActionsBL addAdminActionsBL)
        {
            _context = context;
            _addAdminActionsBL = addAdminActionsBL;
        }

        [HttpPut("AddAdmin")]
        public async Task<IActionResult> AddAdmin(Guid _userId)
        {
          var name = await _addAdminActionsBL.AddAdmin(_userId);

          return Ok($"User name is {name}");
        }
    }
}

