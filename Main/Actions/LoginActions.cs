using System;
using System.Collections.Generic;
using Shop.Main.Actions;
using System.Linq;
using System.IO;
using WebShop.Main.Conext;
using WebShop.Main.DBContext;
using WebShop.Main.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Shop.Main.Actions
{

    [ApiController]
    [Route("[controller]")]
    public class LogInAction : ControllerBase
    {
        private ShopContext _context;
        public LogInAction(ShopContext context)
        {
            _context = context;
        }

        [HttpGet(Name = "LogIn")]
        public IActionResult LogIn(string _name, string _password)
        {
            if (_context.users.Any(x => x.Name == _name && x.Password == _password))
            {
                var user = _context.users.FirstOrDefault(x => x.Name == _name);

                user.Online = true;

                _context.SaveChanges();
                return Ok("Log in was sucsesful" + _name);
            }
            else
            {
                return Unauthorized("User is not exists" + _name);
            }
        }
    }
}


