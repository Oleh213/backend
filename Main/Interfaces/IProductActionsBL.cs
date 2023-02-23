using System;
using WebShop.Main.Conext;
using WebShop.Main.Context;
using WebShop.Main.DTO;
using WebShop.Models;

namespace WebShop.Main.Interfaces
{
	public interface IProductActionsBL
	{
        Task<User> GetUser(Guid userId);

        Task<bool> CheckCategory(Guid categoryId);

        Task<string> AddProduct(ProductModel model);

        Task<string> UpdateProduct(UpdateProductModel model, Product product);

        Task<Product> GetProduct(Guid productId);

        List<ProductDTO> AllProductsDTO();

        Task<Product> GetOneProductWithAll(Guid productId);

        ProductDTO OneProductsDTO(Product product);

        List<ProductDTO> GetFavouriteProducts();

        Task<Characteristics> CheckCharacteristic(string name, string value);

        Task<string> AddNewCharacteristic(string name, string value);

        Task<string> AddCharacteristicToProduct(Characteristics characteristics, Guid productId);

        Dictionary<string, List<Characteristics>> GetChatacteristics(Product product);

        Dictionary<string, List<string>> FirlterDTO(Dictionary<string, List<Characteristics>> chatacteristics);
    }
}