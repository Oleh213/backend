using System;
using Microsoft.EntityFrameworkCore;
using WebShop.Main.Conext;
using WebShop.Main.Context;
using WebShop.Main.DBContext;
using WebShop.Main.Interfaces;
using BC = BCrypt.Net.BCrypt;


namespace WebShop.Main.BusinessLogic
{
	public class RegistActionsBL : IRegistActionsBL
    {
        private ShopContext _context;

        public RegistActionsBL(ShopContext context)
        {
            _context = context;
        }

        public async Task<bool> CheckName(string name)
        {
            return await _context.users.AnyAsync(x => x.Name == name);
        }

        public async Task<bool> CheckEmail(string email)
        {
            return await _context.users.AnyAsync(x => x.Email == email);
        }

        public async Task<string> Regist(string name, string email, string password)
        {
            password = BC.HashPassword(password);

            var id = Guid.NewGuid();
            _context.deliveryOptions.Add(new DeliveryOptions { UserId = id });
            _context.users.Add(new User
            {
                Name = name,
                Email = email,
                Password = password,
                UserId = id,
                AccountBalance = 0,
                Role = UserRole.User,
                RegistData = DateTime.Now,
                DeliveryOptions = new List<DeliveryOptions>()
            });
            await _context.SaveChangesAsync();

            return "Ok";
        }

    }
}

