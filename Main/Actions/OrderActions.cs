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

        [HttpGet("Buy")]
        public IActionResult MakeOrder(Guid _userId, string _deliveryOptions, string _promocode)
        {
            var user = _context.users
                .Where(x => x.UserId == _userId)
                .Include(u => u.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefault();
             
            
            if (user.Online && user != null)
            {
                var products = user.CartItems.Select(ci => ci.Product);

                int? totalPrice = 0;

                foreach(var price in products)
                {
                    totalPrice += price.Price;
                }

                var promo = _context.promocodes.FirstOrDefault(x => x.Code == _promocode);

                if (promo != null)
                {
                    totalPrice -= promo.Discount;
                }

                var newOrder = new Order()
                {
                    UserId = user.UserId,
                    TotalPrice = totalPrice,
                    DeliveryOptions = _deliveryOptions
                };
                _context.orders.Add(newOrder);

                _context.SaveChanges();
                
                foreach (var product in products)
                {
                    _context.orderLists.Add(new OrderList
                    {
                        Product = product,
                        Order = newOrder
                    });
                  
                }

                _context.SaveChanges();

                return Ok($"{user.Name}, You successful bought!");
            }
            else
                return Unauthorized($"{user.Name} Error!");
        }

        [HttpGet("ShowBuyList")]
        public void ShowBuyList(Guid _userId)
        {
            var user = _context.orders.FirstOrDefault(x => x.UserId == _userId);

            _context.orders.Load();
            _context.cartItems.Load();
            var cartOfUsers = _context.orders.Where(x => x.UserId == _userId).Include(u => u.OrderLists).FirstOrDefault();

            Console.WriteLine();

        }
    }
}





