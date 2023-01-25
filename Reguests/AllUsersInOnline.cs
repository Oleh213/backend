//using System;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.AspNetCore.Mvc;
//using WebShop.Main.Conext;
//using WebShop.Main.DBContext;

//namespace WebShop.Reguests
//{
//    [ApiController]
//    [Route("Reguests1")]
//    public class AllUsersInOnline : ControllerBase
//    {
//        private ShopContext _context;
//        public AllUsersInOnline(ShopContext context)
//        {
//            _context = context;
//        }

//        [HttpGet("All users in online")]
//        public List<User> Reguest(Guid _userId)
//        {
//            var user = _context.users.FirstOrDefault(z => z.UserId == _userId);

//            if (user != null)
//            {
//                if (user.Role == UserRole.Admin)
//                {
//                    _context.orders.Load();


//                    Console.WriteLine("All users in online");

//                    foreach (var i in top5)
//                    {
//                        Console.WriteLine($"UserId: {i.UserId}");
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

