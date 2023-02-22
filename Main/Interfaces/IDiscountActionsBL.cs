using System;
using WebShop.Main.Conext;

namespace WebShop.Main.Interfaces
{
	public interface IDiscountActionsBL
	{
        Task<User> GetUser(Guid userId);

        Task<Product> GetProduct(Guid productId);

        Task<bool> UsePromocode(Product product, int discountType, int discount);

        Task<string> ClearDiscount(Product product);


    }
}

