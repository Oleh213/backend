using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
//using WebShop.Auth;
using WebShop.Main.Conext;
using WebShop.Main.DBContext;
using WebShop.Main.Interfaces;
using WebShop.Models;
using static Azure.Core.HttpHeader;

namespace Shop.Main.Actions
{
    [ApiController]
    [Route("[controller]")]
    public class RegistAction : ControllerBase
    {
        //private readonly IOptions<AuthOptions> authOptions;


        //public RegistAction(IOptions<AuthOptions> authOptions)
        //{
        //    this.authOptions = authOptions;
        //}

        private ShopContext _context;

        public RegistAction(ShopContext context)
        {
            _context = context;
        }

        [HttpPost(Name = "RegistAction")]
        public IActionResult Start([FromBody] RegisterModel model)
        {
            var user = _context.users.Where(s => s.Name == model.Name);

            if (user.Any())
            {
                var resEr = new Response<string>()
                {
                    IsError = true,
                    ErrorMessage ="401",
                    Data = $"* Enter another username! *"
                };

                return Unauthorized(resEr);
            }
            else
                {
                var id = Guid.NewGuid();
                _context.users.Add(new User
                {
                    Name = model.Name,
                    Password = model.Password,
                    UserId = id,
                    Role = UserRole.Admin,
                    RegistData = DateTime.Now

                });
                _context.SaveChanges();

                var res = new Response<string>()
                {
                    IsError = false,
                    ErrorMessage = null,
                    Data = $"Registration successful! {model.Name}, Welcome to our shop!"
                };
                return Ok(res);
            }
        }
    }
}
