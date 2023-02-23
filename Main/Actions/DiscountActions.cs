using System;
using Microsoft.AspNetCore.Mvc;
using WebShop.Main.Conext;
using WebShop.Main.DBContext;
using WebShop.Main.Interfaces;
using Microsoft.IdentityModel.Tokens;
using WebShop.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace WebShop.Main.Actions
{

    [ApiController]
    [Route("[controller]")]
    public class DiscountActions : ControllerBase
    {
        private ShopContext _context;
        private IDiscountActionsBL _discountActionsBL;

        public DiscountActions(ShopContext context, IDiscountActionsBL discountActionsBL)
        {
            _context = context;
            _discountActionsBL = discountActionsBL;
        }

        private Guid UserId => Guid.Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        [HttpPost("AddDiscount")]
        [Authorize]
        public async Task<IActionResult> AddDiscount([FromBody] AddProductDiscountModel model)
        {
            var user = await _discountActionsBL.GetUser(UserId);

            if (user != null)
            {
                if (user.Role == UserRole.Admin)
                {
                    var product = await _discountActionsBL.GetProduct(model.ProductId);

                    if (product != null)
                    {
                        if(await _discountActionsBL.UsePromocode(product, model.DiscountType, model.Discount))
                        {
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
                            return NotFound();
                        }
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
        [HttpPost("ClearDiscount")]
        [Authorize]
        public async Task<IActionResult> ClearDiscount([FromBody] ClearDiscountProduct model)
        {
            var user =  await _discountActionsBL.GetUser(UserId);

            if (user != null)
            {
                if (user.Role == UserRole.Admin)
                {
                    var product = await _discountActionsBL.GetProduct(model.ProductId);

                    if (product != null)
                    {
                        if (product.Discount != 0)
                        {
                            await _discountActionsBL.ClearDiscount(product);

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


