//using System;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.AspNetCore.Mvc;
//using WebShop.Main.Conext;
//using WebShop.Main.DBContext;
//using System.Linq;
//using WebShop.Main.Context;

//namespace WebShop.Reguests
//{
//    [ApiController]
//    [Route("Reguests")]
//    public class LastOrders : ControllerBase
//    {
//        private ShopContext _context;
//        public LastOrders(ShopContext context)
//        {
//            _context = context;
//        }

//        [HttpGet("Last orders")]
//        public List<Order> Reguest(Guid _userId)
//        {
//            var user = _context.users.FirstOrDefault(z => z.UserId == _userId);

//            if (user != null)
//            {
//                if (user.Role == UserRole.Admin)
//                {
//                    _context.orders.Load();

//                    var top5 = _context.orders.OrderByDescending(x => x.OrderTime).Take(5);

//                    Console.WriteLine("Five last orders");

//                    foreach (var i in top5)
//                    {
//                        Console.WriteLine($"UserId: {i.UserId}, Order time: {i.OrderTime}");
//                    }
//                    return top5.ToList();
//                }
//                else
//                    return null;
//            }
//            else
//                return null;
//        }
//    }
//}

