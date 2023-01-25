using System;
using System.Collections.Generic;
// using System.Data.Entity;
using System.Linq;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using WebShop.Main.Conext;
using WebShop.Main.Context;
using Microsoft.EntityFrameworkCore;
using WebShop.Main.DBContext;
using WebShop.Main.Interfaces;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using WebShop.Models;

namespace Shop.Main.Actions
{
    [ApiController]
    [Route("OrderAction")]
    public class OrderAction : ControllerBase
    {
        private ShopContext _context;
        public OrderAction(ShopContext context)
        {
            _context = context;
        }

        private Guid UserId => Guid.Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        [HttpPost("Buy")]
        [Authorize]
        public IActionResult MakeOrder([FromBody] OrderModel model)
        {
            _context.users.Load();
            _context.products.Load();
            _context.cartItems.Load();
            

            var user = _context.users
                .Where(x => x.UserId == UserId)
                .Include(u => u.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefault();

            if (user != null)
            {
                var products = user.CartItems.Select(ci => ci.Product);


                foreach (var item in products)
                {
                    var count = model.Cart.FirstOrDefault(ci => ci.ProductId == item.ProductId);

                    if (item.Available < count.Count)
                    {
                        return NotFound($"{user.Name} Error, count is no avialable!");
                    }
                    else
                    {
                        foreach (var i in _context.products)
                        {
                            if (i.ProductId == item.ProductId)
                            {
                                i.Available -= count.Count;
                            }
                        }
                    }
                }

                var newOrder = new Order()
                {
                    UserId = user.UserId,
                    TotalPrice = model.TotalPrice,
                    OrderTime = DateTime.Now,
                    DeliveryOptions = "Delivery Options"
                    
                };
                _context.orders.Add(newOrder);


                foreach (var product in products)
                {
                    var count = model.Cart.FirstOrDefault(x => x.ProductId == product.ProductId);

                    _context.orderLists.Add(new OrderList
                    {
                        Product = product,
                        Order = newOrder,
                        Count = count.Count,
                        Name = product.Name,
                        Img = product.Img,
                        Price = product.Price
                    }); ;


                    foreach (var i in _context.cartItems)
                    {
                        if (i.UserId == UserId)
                        {
                            _context.cartItems.Remove(i);
                        }
                    }
                }
                _context.SaveChanges();
                return Ok($"{user.Name}, You successful bought!");

            }
            else
                return Unauthorized($"{user.Name} Error!");
        }

        [Authorize]
        [HttpGet("ShowBuyList")]
        public IActionResult ShowBuyList()
        {
            _context.orders.Load();
            _context.orderLists.Load();
            if (!_context.orders.Any(x => x.UserId == UserId)) return Ok(Enumerable.Empty<Order>());

            var orderedProductIds = _context.orders.Where(id => id.UserId == UserId).Include(x=> x.OrderLists).ToList();

            return Ok(orderedProductIds);
        }
    }
}





