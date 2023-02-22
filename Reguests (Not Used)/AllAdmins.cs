//using System;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.AspNetCore.Mvc;
//using WebShop.Main.Conext;
//using WebShop.Main.DBContext;

//namespace WebShop.Reguests
//{
//    [ApiController]
//    [Route("Reguests")]
//    public class AllAdmins : ControllerBase
//    {
//        private ShopContext _context;
//        public AllAdmins(ShopContext context)
//        {
//            _context = context;
//        }

//        [HttpGet("All admins in")]
//        public List<User> Reguest(Guid _userId)
//        {
//            var user = _context.users.FirstOrDefault(z => z.UserId == _userId);

//            if (user != null)
//            {
//                if (user.Role == UserRole.Admin)
//                {
//                    _context.orders.Load();

//                    var top5 = _context.users.Where(x => x.Role == UserRole.Admin);

//                    Console.WriteLine("Admins in shop!");

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

