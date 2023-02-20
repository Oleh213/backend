using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Xml.Linq;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
//using WebShop.Auth;
using WebShop.Main.Conext;
using WebShop.Main.Context;
using WebShop.Main.DBContext;
using WebShop.Main.Interfaces;
using WebShop.Models;
using static Azure.Core.HttpHeader;

using BC = BCrypt.Net.BCrypt;

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

            if (_context.users.Any(x => x.Name == model.Name))
            {

                var resEr = new Response<string>()
                {
                    IsError = true,
                    ErrorMessage = "401",
                    Data = $"Enter another username!"
                };
                return Unauthorized(resEr);
            }
            else if (_context.users.Any(x => x.Email == model.Email))
            {
                var resEr2 = new Response<string>()
                {
                    IsError = true,
                    ErrorMessage = "401",
                    Data = $"This email connect to other user, enter other email!"
                };
                return Unauthorized(resEr2);
            }
            else
                {
                model.Password = BC.HashPassword(model.Password);

                var id = Guid.NewGuid();
                _context.deliveryOptions.Add(new DeliveryOptions { UserId = id });
                _context.users.Add(new User
                {
                    Name = model.Name,
                    Email = model.Email,
                    Password = model.Password,
                    UserId = id,
                    AccountBalance = 0,
                    Role = UserRole.User,
                    RegistData = DateTime.Now,
                    DeliveryOptions =  new List<DeliveryOptions>()
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
