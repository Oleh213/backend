using System;
using Microsoft.EntityFrameworkCore;
using WebShop.Main.Conext;
using WebShop.Main.DBContext;
using WebShop.Main.Interfaces;

namespace WebShop.Main.BusinessLogic
{
	public class DiscountActionsBL : IDiscountActionsBL
    {
        private readonly ShopContext _context;

        public DiscountActionsBL(ShopContext context)
        {
            _context = context;
        }

        public async Task<User> GetUser(Guid userId)
        {
            return await _context.users.FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public async Task<Product> GetProduct(Guid productId)
        {
            return await _context.products.FirstOrDefaultAsync(x => x.ProductId == productId);
        }

        public async Task<bool> UsePromocode(Product product, int discountType, int discount)
        {
            if (discountType == 1)
            {
                double price = Convert.ToDouble(product.Price) * (Convert.ToDouble(discount) / 100);
                product.Price = product.Price - Convert.ToInt32(price);
                product.Discount = Convert.ToInt32(price);

                await _context.SaveChangesAsync();
                return true;
            }
            else if (discountType == 2)
            {
                if (product.Price > discount)
                {
                    product.Price -= discount;
                    product.Discount = discount;
                }
                else
                {
                    product.Price = 1;
                }

                await _context.SaveChangesAsync();
                return true;
            }
            else return false;

        }
        public async Task<string> ClearDiscount(Product product)
        {
            product.Price += product.Discount;
            product.Discount = 0;

            await _context.SaveChangesAsync();

            return "Ok";
        }
    }
}

