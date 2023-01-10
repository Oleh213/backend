using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using WebShop.Main.Conext;
using WebShop.Main.DBContext;
using WebShop.Main.Interfaces;
using static Azure.Core.HttpHeader;

namespace Shop.Main.Actions
{
    [ApiController]
    [Route("[controller]")]
    public class RegistAction : ControllerBase
    {

        private ShopContext _context;

        public RegistAction(ShopContext context)
        {
            _context = context;
        }

        [HttpGet(Name = "RegistAction")]
        public IActionResult Start(string _name, string _password)
        {
            var user = _context.users.Where(s => s.Name == _name);

            if (user.Any())
            {
                return Unauthorized("Error! Enter another name!");
            }
            else
                {
                var id = Guid.NewGuid();
                _context.users.Add(new User
                {
                    Name = _name,
                    Password = _password,
                    Online = false,
                    UserId = id,
                    Role = UserRole.User
                });
                _context.SaveChanges();

                return Ok($"Registration successful! {_name}, Welcome to our shop!");
                }
            }
        }
    }
