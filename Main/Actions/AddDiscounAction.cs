using System;
using Microsoft.AspNetCore.Mvc;
using WebShop.Main.Conext;
using WebShop.Main.DBContext;
using WebShop.Main.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace WebShop.Main.Actions
{

    [ApiController]
    [Route("[controller]")]
    public class AddDiscounAction : ControllerBase
    {
        private ShopContext _context;
        public AddDiscounAction(ShopContext context)
        {
            _context = context;
        }

        [HttpPut(Name = "AddDiscounAction")]
        public IActionResult AddPromocode(Guid _productId, int _discount, Guid _userId, int _discountType)
        {
            var user = _context.users.FirstOrDefault(user => user.UserId == _userId);

            if (user != null)
            {
                if (user.Role == UserRole.Admin)
                {
                    var product = _context.products.FirstOrDefault(id => id.ProductId == _productId);

                    if (product != null)
                    {
                        if (_discountType == 1)
                        {
                            product.DiscountType = DiscountType.Percent;
                            double price = Convert.ToDouble(product.Price) * (Convert.ToDouble(_discount) / 100);
                            product.Price = product.Price - Convert.ToInt32(price);
                            product.Discount = _discount;
                        }
                        else if (_discountType == 2)
                        {
                            product.DiscountType = DiscountType.Money;

                            if (product.Price > _discount)
                            {
                                product.Price -= _discount;
                                product.Discount = _discount;
                            }
                            else
                            {
                                product.Price = 1;
                            }
                        }
                        else
                        {
                            return Unauthorized($"Error {user.Name}! Discount type didn't find");
                        }
                    }
                    _context.SaveChanges();
                    return Ok($"Discount added to product");
                }
                else
                    return Unauthorized($"Error {user.Name}!");
            }
            else
                return Unauthorized($"Error {user.Name}!");
        }
    }
}


