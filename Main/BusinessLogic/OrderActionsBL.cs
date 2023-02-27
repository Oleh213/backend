
using System;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData;
using WebShop.Main.Conext;
using WebShop.Main.Context;
using WebShop.Main.DBContext;
using WebShop.Main.Interfaces;
using WebShop.Models;

namespace WebShop.Main.BusinessLogic
{
	public class OrderActionsBL : IOrderActionsBL
    {
        private ShopContext _context;

        public OrderActionsBL(ShopContext context)
        {
            _context = context;
        }

        public async Task<User> GetUser(Guid userId)
        {
            return await _context.users
                .Where(x => x.UserId == userId)
                .Include(u => u.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(); 
        }

        public async Task<Cards> GetCartd(string cardNumber, string expiredDate, string cvv)
        {
            return await _context.cards.FirstOrDefaultAsync(x => x.CardNumber == cardNumber && x.Cvv == cvv && x.ExpiredDate == expiredDate);
        }

        public async Task<bool> CheckCountOfProducts(List<Product> products, User user)
        {
            foreach (var item in products)
            {
                var count = user.CartItems.FirstOrDefault(id => id.ProductId == item.ProductId).Count;

                if (item.Available <= count)
                {
                    return false;
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
                    await _context.SaveChangesAsync();
                    return true;
                }
            }
            return false;
        }

        public async Task<string> CreateNewOrder(List<Product> products, User user, OrderModel model, int totalPrice)
        {
            var id = Guid.NewGuid();

            DateTime newDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd h:mm:ss tt"));

            var newOrder = new Order()
            {
                OrderId = id,
                UserId = user.UserId,
                TotalPrice = totalPrice,
                OrderTime = newDate,
                OrderStatus = OrderStatus.AwaitingConfirm,
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
                    if (i.UserId == user.UserId)
                    {
                        _context.cartItems.Remove(i);
                    }
                }
            }
            await _context.SaveChangesAsync();

            return "Ok";
        }

        public async Task<List<Order>> ShowOrders(Guid userId)
        {
            return await _context.orders.Where(id => id.UserId == userId).Include(x => x.OrderLists).OrderByDescending(x => x.OrderTime).ToListAsync();
        }

        public async Task<Order> GetOrder(Guid orderId)
        {
            return await _context.orders.FirstOrDefaultAsync(x => x.OrderId == orderId);
        }

        public async Task<string> ChangeOrderStatus(Order order, OrderStatus orderStatus)
        {
            order.OrderStatus = orderStatus;

            await _context.SaveChangesAsync();

            return "Ok";
        }

        public async Task<List<Order>> GetNewOrders()
        {
            return await _context.orders.Where(x => x.OrderStatus != OrderStatus.Completed && x.OrderStatus != OrderStatus.Declined && x.OrderStatus != OrderStatus.Canceled)
                .Include(x=> x.OrderLists)
                .Include(x=> x.Info)
                .OrderByDescending(x => x.OrderTime)
                .ToListAsync();
        }

        public async Task<int> GetTotalPrice(User user, string promocode)
        {
            var cart = await _context.cartItems.Where(x => x.UserId == user.UserId).ToListAsync();

            if (cart!=null)
            {
                var promo = await _context.promocodes.FirstOrDefaultAsync(x=> x.Code == promocode);

                int totalPrice = 0;
                foreach(var item in cart)
                {
                    totalPrice += item.Product.Price;
                }

                if(promo!=null)
                {
                    if (promo.Discount > totalPrice)
                    {
                        return 0;
                    }
                    else
                    {
                        return totalPrice -= promo.Discount;
                    }
                }
                else
                {
                    return totalPrice;
                }
            }
            return 0;
        }

    }
}

