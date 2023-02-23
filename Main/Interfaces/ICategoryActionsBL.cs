using System;
using WebShop.Main.Conext;
using WebShop.Main.DTO;

namespace WebShop.Main.Interfaces
{
	public interface ICategoryActionsBL
	{
        Task<User> GetUser(Guid userId);

        List<CategoriesDTO> CategoriesDTO();

        Task<string> AddCategory(string categoryName);

        Task<bool> CheckCategory(string categoryName);
    }
}

