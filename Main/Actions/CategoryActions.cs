using System;
using System.Collections.Generic;
using Shop.Main.Actions;
using System.Xml.Linq;
using System.Linq;
using WebShop.Main.Conext;
using WebShop.Main.DBContext;
using WebShop.Main.Interfaces;
using Microsoft.AspNetCore.Mvc;
using static Azure.Core.HttpHeader;
using WebShop.Main.DTO;
using System.Data.Entity;

namespace Shop.Main.Actions
{
    [ApiController]
    [Route("CategoryAction")]
    public class CategoryAction : ControllerBase
    {
        private ShopContext _context;

        public CategoryAction(ShopContext context)
        {
            _context = context;
        }

        [HttpPut(Name = "AddCategory")]
        public IActionResult AddCategory(Guid _userId, string _name)
        {

            if (!_context.categories.Any(x => x.Name == _name))
            {
                var user = _context.users.FirstOrDefault(x => x.UserId == _userId);

                if (user.Role == UserRole.Admin)
                {

                    _context.categories.Add(new Category { Name = _name, CatId = Guid.NewGuid() });
                    _context.SaveChanges();

                    return Ok($"Category seccusful aded by {user.Name}");
                }
                else
                    return Unauthorized($"Error! {user.Name}, You cann't do it!");
            }
            else
                return Unauthorized($"Error! Category is added");
        }

        [HttpGet("ShowCategory")]
        public IActionResult ShowCategory()
        {
            _context.categories.Load();

            return Ok(_context.categories);
        }
    }
}