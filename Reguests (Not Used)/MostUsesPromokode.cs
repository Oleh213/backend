//using System;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.AspNetCore.Mvc;
//using WebShop.Main.Conext;
//using WebShop.Main.DBContext;
//using System.Linq;

//namespace WebShop.Reguests
//{
//    public class ForList
//    {
//        public string Name { get; set; }

//        public int Count { get; set; }
//    }
    
//    [ApiController]
//    [Route("Reguests")]
//    public class MostUserPromocode : ControllerBase
//    {
//        private ShopContext _context;
//        public MostUserPromocode(ShopContext context)
//        {
//            _context = context;
//        }

//        [HttpGet("Most used promocode")]
//        public List<ForList> Reguest(Guid _userId)
//        {
//            var user = _context.users.FirstOrDefault(z => z.UserId == _userId);

//            if (user != null)
//            {
//                if (user.Role == UserRole.Admin)
//                {
//                    var products = _context.orders.GroupBy(ol => ol.UsedPromocode);

//                    List<ForList> topPromocodes = new List<ForList>();

//                    foreach (var item in products)
//                    {
//                        if(item.Key!=null)
//                        {
//                            int count = 0;

//                            item.ToList().ForEach(x =>
//                            {
//                                count++;
//                            });
//                            topPromocodes.Add(new ForList
//                            {
//                                Name = item.Key,
//                                Count = count
//                            });
//                        }
//                    }
//                    var top = topPromocodes.OrderByDescending(c => c.Count).Take(5);

//                    Console.WriteLine("Most used promocode");

//                    return topPromocodes.ToList();
//                }
//                else
//                    return null;
//            }
//            else
//                return null;
//        }
//    }
//}

