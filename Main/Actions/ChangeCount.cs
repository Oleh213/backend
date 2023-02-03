using System;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebShop.Main.Conext;
using WebShop.Main.DBContext;
using WebShop.Models;

namespace WebShop.Main.Actions
{

    [ApiController]
    [Route("ChangeCount")]
    public class ChangeCount : ControllerBase
    {
        private ShopContext _context;
        public ChangeCount(ShopContext context)
        {
            _context = context;
        }
        private Guid UserId => Guid.Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);


        [HttpPost(Name = "ChangeCount")]
        public IActionResult ChangeCountOfItemInCart([FromBody] GetCountModel model)
        {
            var count = _context.cartItems.FirstOrDefault(user => user.UserId == UserId && user.ProductId == model.ProductId);

            if (count != null)
            {
                if(_context.products.FirstOrDefault(prod=> prod.ProductId == model.ProductId).Available >= model.Count)
                {
                    var res = new Response<string>()
                    {
                        IsError = false,
                        ErrorMessage = "",
                        Data = "Your information successful update"
                    };

                    count.Count = model.Count;
                    _context.SaveChanges();
                    return Ok(res);
                }
                else
                {
                    var resError = new Response<string>()
                    {
                        IsError = true,
                        ErrorMessage = "CountUnAvailable",
                        Data = "Sorry, we don't have available count of this product"
                    };

                    return NotFound(resError);
                }

                
            }
            else
                return NotFound();
        }
    }

    public class GetCountModel
    {
        public Guid ProductId { get; set; }

        public int Count { get; set; }
    }
}

