using System;
using WebShop.Main.Conext;
using WebShop.Main.DBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using WebShop.Main.BusinessLogic;

namespace Shop.Main.BusinessLogic
{
    public class AddAdminActionsBL : IAdminActionsBL
    {
        private ShopContext _context;

        public AddAdminActionsBL(ShopContext context)
        {
            _context = context;
        }

        public async Task<string> AddAdmin(Guid _userId)
        {
            var user = await _context.users.FirstOrDefaultAsync(x => x.UserId == _userId);

            user.Role = UserRole.Admin;

            _context.SaveChanges();

            return user.Name;
        }
    }
}

