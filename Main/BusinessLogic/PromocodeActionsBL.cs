using System;
using Microsoft.EntityFrameworkCore;
using WebShop.Main.Conext;
using WebShop.Main.DBContext;
using WebShop.Main.Interfaces;

namespace WebShop.Main.BusinessLogic
{
    public class PromocodeActionsBL : IPromocodeActionsBL
    {
        private ShopContext _context;

        public PromocodeActionsBL(ShopContext context)
        {
            _context = context;
        }

        public async Task<User> GetUser(Guid userId)
        {
            return await _context.users.FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public async Task<Promocode> GetPromocode(string code)
        {
            return await _context.promocodes.FirstOrDefaultAsync(x => x.Code == code);
        }
    }
}

