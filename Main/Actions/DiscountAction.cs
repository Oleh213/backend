using System;
using Microsoft.AspNetCore.Mvc;
using WebShop.Main.Conext;
using WebShop.Main.DBContext;
using WebShop.Main.Interfaces;
using Microsoft.IdentityModel.Tokens;
using WebShop.Models;
using System.Security.Claims;

namespace WebShop.Main.Actions
{

    [ApiController]
    [Route("[controller]")]
    public class DiscountAction : ControllerBase
    {
        private ShopContext _context;
        public DiscountAction(ShopContext context)
        {
            _context = context;
        }

        private Guid UserId => Guid.Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        [HttpPost("AddDiscount")]
        public IActionResult AddDiscount([FromBody] AddProductDiscountModel model)
        {
            var user = _context.users.FirstOrDefault(user => user.UserId == UserId);

            if (user != null)
            {
                if (user.Role == UserRole.Admin)
                {
                    var product = _context.products.FirstOrDefault(id => id.ProductId == model.ProductId);

                    if (product != null)
                    {
                        if (model.DiscountType == 1)
                        {
                            double price = Convert.ToDouble(product.Price) * (Convert.ToDouble(model.Discount) / 100);
                            product.Price = product.Price - Convert.ToInt32(price);
                            product.Discount = Convert.ToInt32(price);
                        }
                        else if (model.DiscountType == 2)
                        {
                            if (product.Price > model.Discount)
                            {
                                product.Price -= model.Discount;
                                product.Discount = model.Discount;
                            }
                            else
                            {
                                product.Price = 1;
                            }
                        }
                        else
                        {
                            return NotFound();
                        }
                    }

                    _context.SaveChanges();

                    var resOk = new Response<string>()
                    {
                        IsError = false,
                        ErrorMessage = "",
                        Data = "Discount successfully added!"
                    };

                    return Ok(resOk);
                }
                else
                {
                    var resEr = new Response<string>()
                    {
                        IsError = true,
                        ErrorMessage = "401",
                        Data = $"* Error *, You can't do it!"
                    };
                    return Unauthorized(resEr);
                }
                    
            }
            else
                return NotFound();
        }
        [HttpPost("ClearDiscount")]
        public IActionResult ClearDiscount([FromBody] ClearDiscountProduct model)
        {
            var user = _context.users.FirstOrDefault(user => user.UserId == UserId);

            if (user != null)
            {
                if (user.Role == UserRole.Admin)
                {
                    var product = _context.products.FirstOrDefault(id => id.ProductId == model.ProductId);

                    if (product != null)
                    {
                        if (product.Discount != 0)
                        {
                            product.Price += product.Discount;
                            product.Discount = 0;

                            _context.SaveChanges();

                            var resOk = new Response<string>()
                            {
                                IsError = false,
                                ErrorMessage = "",
                                Data = "Discount successfully cleaned!"
                            };

                            return Ok(resOk);
                        }
                        else
                            return NotFound();
                    }
                    else
                        return NotFound();
                }
                else
                {
                    var resEr = new Response<string>()
                    {
                        IsError = true,
                        ErrorMessage = "401",
                        Data = $"* Error *, You can't do it!"
                    };
                    return Unauthorized(resEr);
                }
            }
            else
                return NotFound();
        }
    }
}


