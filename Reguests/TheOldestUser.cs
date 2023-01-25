using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using WebShop.Main.Conext;
using WebShop.Main.DBContext;

namespace WebShop.Reguests
{
    [ApiController]
    [Route("Reguests")]
    public class TheOldestUser : ControllerBase
    {
        private ShopContext _context;
        public TheOldestUser(ShopContext context)
        {
            _context = context;
        }

        [HttpGet("The oldest users")]
        public List<User> Reguest(Guid _userId)
        {
            var user = _context.users.FirstOrDefault(z => z.UserId == _userId);

            if (user != null)
            {
                if (user.Role == UserRole.Admin)
                {
                    _context.orders.Load();

                    var top5 = _context.users.OrderBy(x => x.RegistData).Take(5);

                    Console.WriteLine("Top 5 the oldest users in shop!");

                    foreach (var i in top5)
                    {
                        Console.WriteLine($"UserId: {i.UserId}, Regist Date: {i.RegistData}");
                    }
                    return top5.ToList();
                }
                else
                    return null;
            }
            else
                return null;
        }
    }
}

