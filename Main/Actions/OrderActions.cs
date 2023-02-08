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


            if (model.Card != null)
            {
                var card = _context.cards.FirstOrDefault(x => x.CardNumber == model.Card.CardNumber && x.Cvv == model.Card.Cvv && x.ExpiredDate == model.Card.ExpiredDate);

                if (card != null)
                {
                    if (card.Balance >= model.TotalPrice)
                    {
                        card.Balance -= model.TotalPrice;
                    }
                    else
                    {
                        var resError = new Response<string>()
                        {
                            IsError = true,
                            ErrorMessage = "Error ",
                            Data = "You don't have enough amount on your card's balance"
                        };
                        return NotFound(resError);
                    }
                }
                else
                {
                    var resError = new Response<string>()
                    {
                        IsError = true,
                        ErrorMessage = "Error ",
                        Data = "Check your credit card information!"
                    };
                    return NotFound(resError);
                }

            }
            else
            {
                if (user.AccountBalance >= model.TotalPrice)
                {
                    user.AccountBalance -= model.TotalPrice;
                }
                else
                {
                    var resError = new Response<string>()
                    {
                        IsError = true,
                        ErrorMessage = "Error ",
                        Data = "You don't have enough amount on your account balance"
                    };
                    return NotFound(resError);
                }
            }


            var products = user.CartItems.Select(ci => ci.Product);


            foreach (var item in products)
            {

                var count = user.CartItems.FirstOrDefault(id => id.ProductId == item.ProductId).Count;

                if (item.Available <= count)
                {
                    var resError = new Response<string>()
                    {
                        IsError = true,
                        ErrorMessage = "Error ",
                        Data = $"Error, count of {item.Name} is no avialable!"
                    };

                    return NotFound(resError);
                }
                else
                {
                    foreach (var i in _context.products)
                    {
                        if (i.ProductId == item.ProductId)
                        {
                            i.Available -= count;
                        }
                    }
                }
            }
            var id = Guid.NewGuid();

            var newOrder = new Order()
            {
                OrderId = id,
                UserId = user.UserId,
                TotalPrice = model.TotalPrice,
                OrderTime = DateTime.Now,
                Info = new Info
                {
                    OrderId = id,
                    Address = model.DeliveryOptions.Address,
                    City = model.DeliveryOptions.City,
                    Country = model.DeliveryOptions.Country,
                    Region = model.DeliveryOptions.Region,
                    ZipCode = model.DeliveryOptions.ZipCode,
                    Address2 = model.DeliveryOptions.Address2,
                    Name = model.contactInfo.Name,
                    LastName = model.contactInfo.LastName,
                    Email = model.contactInfo.Email,
                    PhoneNumber = model.contactInfo.PhoneNumber,
                }
            };
            _context.orders.Add(newOrder);

            foreach (var product in products)
            {
                var count = user.CartItems.FirstOrDefault(id => id.ProductId == product.ProductId).Count;

                _context.orderLists.Add(new OrderList
                {
                    Product = product,
                    Order = newOrder,
                    Count = count,
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


            var resOk = new Response<string>()
            {
                IsError = false,
                ErrorMessage = " ",
                Data = $"The order was successful create"
            };

            return Ok(resOk);

        }

        [Authorize]
        [HttpGet("ShowBuyList")]
        public IActionResult ShowBuyList()
        {
            _context.orders.Load();
            _context.orderLists.Load();
            if (!_context.orders.Any(x => x.UserId == UserId)) return Ok(Enumerable.Empty<Order>());

            var orderedProductIds = _context.orders.Where(id => id.UserId == UserId).Include(x=> x.OrderLists).ToList().OrderByDescending(x=> x.OrderTime);

            return Ok(orderedProductIds);
        }
    }
}





