using System;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebShop.Main.Conext;
using WebShop.Main.DBContext;
using WebShop.Main.DTO;
using System.Data.Entity;

namespace WebShop.Reguests
{
	public class GetCategories : ControllerBase
    {
        private readonly ShopContext _context;

        public GetCategories(ShopContext context) => _context = context;

        private Guid UserId => Guid.Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        [HttpGet("GetAllCategories")]
        public IActionResult GetAllCategories()
        {
            _context.categories.Load();

            var user = _context.users.FirstOrDefault(user => user.UserId == UserId);

            if(user.Role == UserRole.Admin)
            {
                var categoriesDTO = new List<CategoriesName>();

                foreach(var item in _context.categories)
                {
                    categoriesDTO.Add(new CategoriesName { CategoryName = item.Name, CategoryId = item.CatId});
                }

                return Ok(categoriesDTO);
            }
            else
            {
                return NotFound();
            }

            

        }
    }

    public class CategoriesName
    {
        public string CategoryName { get; set; }

        public Guid CategoryId { get; set; }
    }
}

