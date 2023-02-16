using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using WebShop.Main.Conext;
using WebShop.Main.DBContext;
using WebShop.Main.DTO;

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

        [HttpGet("FavouriteProductReguest")]
        public IActionResult Reguest()
        {
            _context.products.Load();
            _context.categories.Load();
            _context.cartItems.Load();

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

            var list = topProducts.OrderByDescending(c => c.Value).Take(6);

            var productDPOs = new List<ProductDTO>();

            foreach (var item in list)
            {
                var prod = _context.products.FirstOrDefault(x => x.ProductId == item.Key);

                productDPOs.Add(new ProductDTO
                {
                    ProductId = prod.ProductId,
                    Price = prod.Price,
                    CategoryName = prod.Category.Name,
                    CategoryId = prod.CategorytId,
                    ProductName = prod.Name,
                    Available = prod.Available,
                    Discount = prod.Discount,
                    Description = prod.Description,
                    Img = prod.Img
                });
            }
            return Ok(productDPOs);
        }
    }
}

