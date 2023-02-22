using System;
using WebShop.Main.Conext;
using WebShop.Main.DBContext;
using WebShop.Main.DTO;
using WebShop.Main.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace WebShop.Main.BusinessLogic
{
	public class CategoryActionsBL : ICategoryActionsBL
    {
        private readonly ShopContext _context;

        public CategoryActionsBL(ShopContext context)
        {
            _context = context;
        }

        public async Task<User> GetUser(Guid userId)
        {
            return await _context.users.FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public List<CategoriesDTO> CategoriesDTO()
        {
            var categoriesDTO = new List<CategoriesDTO>();

            foreach (var item in _context.categories)
            {
                categoriesDTO.Add(new CategoriesDTO { CategoryName = item.Name, CategoryId = item.CatId });
            }

            return (categoriesDTO);
        }
    }
}

