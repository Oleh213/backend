using System;
using WebShop.Main.Conext;
using WebShop.Main.DTO;

namespace WebShop.Main.Interfaces
{
	public interface ICartItemActionsBL
	{
        Task<bool> CheckIfCartInProduct(Guid productId, Guid UserId);

        Task<Product> GetProduct(Guid productId);

        Task<string> AddProductToCart(Guid productId, int count, Guid userId);

        Task<CartItems> GetCartItem(Guid productId, Guid userId);

        Task<string> DellCartItem(CartItems cartItem);

        Task<List<CartItems>> GetCart(Guid userId);

        List<CartItemDTO> CartItemsDTO(List<CartItems> cartOfUser);

        Task<string> ChangeCoutOfCartItem(CartItems cartItem, int count);

    }
}

