using System;
using System.Linq;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using WebShop.Main.Conext;
using WebShop.Main.DBContext;
using WebShop.Main.Interfaces;



namespace Shop.Main.Actions
{
    public class PromocodeActionArgs : IActionArgs
    {
        public int Discount { get; set; }
        public string? Code { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
    }

    [ApiController]
    [Route("[controller]")]
    public class PromocodeAction : ControllerBase
    {
        private ShopContext _context;
        public PromocodeAction(ShopContext context)
        {
            _context = context;
        }

        [HttpGet(Name = "AddPromocode")]
        public IActionResult AddPromocode(int _discount, string _code, Guid _userId)
        {
            var user = _context.users.FirstOrDefault(z => z.UserId == _userId);

            if (user != null)
            {
                if (user.Role == UserRole.Admin && user.Online)
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

        //[HttpGet("UsePromocode")]
        //public IActionResult UsePromocode(Guid _userId, string _code)
        //{
        //    var user = _context.users.FirstOrDefault(x => x.UserId == _userId);

        //    if (user.UserId == _userId && user.Online && _context.promocodes.Any(x => x.Code == _code))
        //    {
        //        var promo = _context.promocodes.FirstOrDefault(x => x.Code == _code);

        //        var cart = _context.carts.FirstOrDefault(x => x.CartId == _userId);

        //        if (cart.TotalPrice > promo.Discount)
        //        {
        //            cart.TotalPrice -= promo.Discount;
        //        }
        //        else
        //            cart.TotalPrice = 0;

        //        return Ok($"Promocode: {_code}, successful used!");
        //    }
        //    else
        //        return Unauthorized($"Error {user.Name}!");

        
    }
}


