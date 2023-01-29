using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Security.Claims;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using WebShop.Main.Conext;
using WebShop.Main.DTO;
using WebShop.Main.DBContext;
using WebShop.Models;
using Microsoft.AspNetCore.Authorization;

namespace WebShop.Reguests
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class GetNameOfUser : ControllerBase
    {

        private readonly ShopContext _context;

        private Guid UserId => Guid.Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);


        public GetNameOfUser(ShopContext context) => _context = context;


        [HttpGet(Name = "GetName")]
        [Authorize]
        public IActionResult GetName()
        {
            var name = _context.users.FirstOrDefault(x => x.UserId == UserId).Name;

            if (name != null)
            {
                var res = new Response<string>()
                {
                    IsError = false,
                    ErrorMessage = "",
                    Data = name
                };
                return Ok(res);
            }
            else
                return NotFound();
        }
    }
}

