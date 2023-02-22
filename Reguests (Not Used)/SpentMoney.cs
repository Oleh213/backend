//using System;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.AspNetCore.Mvc;
//using WebShop.Main.Conext;
//using WebShop.Main.DBContext;

//namespace WebShop.Reguests
//{

//    public class ForList2
//    {
//        public Guid UserId { get; set; }

//        public int Spent { get; set; }
//    }
//    [ApiController]
//    [Route("Reguest")]
//    public class SpentMoreMoney : ControllerBase
//    {
//        private ShopContext _context;
//        public SpentMoreMoney(ShopContext context)
//        {
//            _context = context;
//        }

//        [HttpGet("Users who sent more money")]
//        public List<ForList2> Reguest(Guid _userId)
//        {
//            var user = _context.users.FirstOrDefault(z => z.UserId == _userId);

//            if (user != null)
//            {
//                if (user.Role == UserRole.Admin)
//                {

//                    var products = _context.orders.GroupBy(ol => ol.UserId);

//                    List<ForList2> topUsers = new List<ForList2>();

//                    foreach (var item in products)
//                    {
//                        int sum = 0;

//                        item.ToList().ForEach(x =>
//                        {
//                            sum += x.TotalPrice;
//                        });
//                        topUsers.Add(new ForList2 {
//                            UserId = item.Key,
//                            Spent = sum
//                        });
//                    }

//                    var list = topUsers.OrderByDescending(c => c.Spent).Take(5);

//                    Console.WriteLine("Users who sent more money");

//                    return list.ToList();
//                }
//                else
//                    return null;
//            }
//            else
//                return null;
//        }
//    }
//}