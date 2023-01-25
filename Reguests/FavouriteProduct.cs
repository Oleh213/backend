using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using WebShop.Main.Conext;
using WebShop.Main.DBContext;

namespace WebShop.Reguests
{
    [ApiController]
    [Route("Reguests")]
    public class FavouriteProduct : ControllerBase
    {
        private ShopContext _context;
        public FavouriteProduct(ShopContext context)
        {
            _context = context;
        }

        [HttpGet("Favourite products by count")]
        public IActionResult Reguest(Guid _userId)
        {
            var user = _context.users.FirstOrDefault(z => z.UserId == _userId);

            if (user != null)
            {
                if (user.Role == UserRole.Admin)
                {
                    var products = _context.orderLists.GroupBy(ol => ol.ProductId);

                    Dictionary<Guid, int> topProducts = new Dictionary<Guid, int>();

                    foreach(var item in products)
                    {
                        int sum = 0;

                        item.ToList().ForEach(x =>
                        {
                            sum += x.Count;
                        });

                        topProducts.Add(item.Key, sum);
                    }
                     
                    var list = topProducts.OrderByDescending(c => c.Value).Take(5);

                    Console.WriteLine("Top 5 favourite products by count!");

                    foreach (var i in list)
                    {
                        Console.WriteLine($"Product: {i.Key} Count: {i.Value}");
                    }

                    return Ok($"Ok");
                }
                else
                    return Unauthorized($"Error {user.Name}!");
            }
            else
                return Unauthorized($"Error {user.Name}!");
        }
        [HttpGet("Favourite products")]
        public IActionResult Reguest2(Guid _userId)
        {
            var user = _context.users.FirstOrDefault(z => z.UserId == _userId);

            if (user != null)
            {
                if (user.Role == UserRole.Admin)
                {
                    var products = _context.orderLists.GroupBy(ol => ol.ProductId);

                    Dictionary<Guid, int> topProducts = new Dictionary<Guid, int>();

                    foreach (var item in products)
                    {
                        int count = 0;

                        item.ToList().ForEach(x =>
                        {
                            count++;
                        });
                        topProducts.Add(item.Key, count);
                    }

                    var list = topProducts.OrderByDescending(c => c.Value).Take(5);

                    Console.WriteLine("Top 5 favourite products by count!");

                    foreach (var i in list)
                    {
                        Console.WriteLine($"Product: {i.Key} Count: {i.Value}");
                    }

                    return Ok($"Ok");
                }
                else
                    return Unauthorized($"Error {user.Name}!");
            }
            else
                return Unauthorized($"Error {user.Name}!");
        }
    }
}

