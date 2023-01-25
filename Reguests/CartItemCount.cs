using System;
using System.Data.Entity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebShop.Main.Conext;
using WebShop.Main.DBContext;
using WebShop.Main.DTO;

namespace WebShop.Reguests
{
    [ApiController]
    [Route("[controller]")]
    public class CartItemCount : ControllerBase
    {
        private ShopContext _context;

        public CartItemCount(ShopContext context)
        {
            _context = context;
        }

        private Guid UserId => Guid.Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        [HttpGet("GetCartItemCount")]
        [Authorize]
        public int GetCartItemCount()
        {
            _context.users.Load();
            _context.cartItems.Load();

            var cartOfUser = _context.cartItems.Where(x => x.UserId == UserId);

            if (cartOfUser != null)
            {
                return cartOfUser.Count();
            }
            else
            {
                return 0;
            }
        }
    }
}

