using System;
using System.Linq;
using System.Security.Claims;
using System.Xml.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebShop.Main.Conext;
using WebShop.Main.DBContext;
using WebShop.Main.Interfaces;
using WebShop.Models;

namespace Shop.Main.Actions
{
    [ApiController]
    [Route("[controller]")]
    public class PromocodeAction : ControllerBase
    {
        private ShopContext _context;
        public PromocodeAction(ShopContext context)
        {
            _context = context;
        }

            private Guid UserId => Guid.Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);


        [HttpPut("AddPromocode")]
        public IActionResult AddPromocode(int _discount, string _code, Guid _userId)
        {
            var user = _context.users.FirstOrDefault(z => z.UserId == _userId);

            if (user != null)
            {
                if (user.Role == UserRole.Admin)
                {
                    _context.promocodes.Add(new Promocode()
                    {
                        PromocodetId = Guid.NewGuid(),
                        Code = _code,
                        Discount = _discount
                    });
                    _context.SaveChanges();
                    return Ok($"Promocode: {_code}, added to promolist");
                }
                else
                    return Unauthorized($"Error {user.Name}!");
            }
            else
                return Unauthorized($"Error {user.Name}!");
        }

        [HttpPost("UsePromocode")]
        [Authorize]
        public int UsePromocode([FromBody] PromocodeModel model)
        {
            if (_context.promocodes.Any(x => x.Code == model.Code))
            {
                var promo = _context.promocodes.FirstOrDefault(x => x.Code == model.Code);

                if (promo != null)
                {
                    return promo.Discount;
                }
                else
                    return 0;
            }
            else
                return 0;
        }
    }
}

