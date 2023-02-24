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
    public class PromocodeActions : ControllerBase
    {
        private ShopContext _context;
        private IPromocodeActionsBL _promocodeActionsBL;

        public PromocodeActions(ShopContext context, IPromocodeActionsBL promocodeActionsBL)
        {
            _context = context;
            _promocodeActionsBL = promocodeActionsBL;
        }

        private Guid UserId => Guid.Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        //[HttpPost("AddPromocode")]
        //public IActionResult AddPromocode(int _discount, string _code, Guid _userId)
        //{
        //    var user = _context.users.FirstOrDefault(z => z.UserId == _userId);

        //    if (user != null)
        //    {
        //        if (user.Role == UserRole.Admin)
        //        {
        //            _context.promocodes.Add(new Promocode()
        //            {
        //                PromocodetId = Guid.NewGuid(),
        //                Code = _code,
        //                Discount = _discount
        //            });
        //            _context.SaveChanges();
        //            return Ok($"Promocode: {_code}, added to promolist");
        //        }
        //        else
        //            return Unauthorized($"Error {user.Name}!");
        //    }
        //    else
        //        return Unauthorized($"Error {user.Name}!");
        //}

        [HttpPost("UsePromocode")]
        public async Task<int> UsePromocode([FromBody] PromocodeModel model)
        {
            var promo =  await _promocodeActionsBL.GetPromocode(model.Code);

            if (promo != null)
            {
                return promo.Discount;
            }
            else
                return 0;
        }
    }
}

