
using System;
using System.Globalization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData;
using WebShop.Main.Conext;
using WebShop.Main.Context;
using WebShop.Main.DBContext;
using WebShop.Main.Hubs;
using WebShop.Main.Interfaces;
using WebShop.Models;

namespace WebShop.Main.BusinessLogic
{
	public class OrderActionsBL : IOrderActionsBL
    {
        private ShopContext _context;

        private readonly IHubContext<OrderHub> _hubContext;


        public OrderActionsBL(ShopContext context, IHubContext<OrderHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        public async Task<User> GetUser(Guid userId)
        {
            return await _context.users
                .Where(x => x.UserId == userId)
                .Include(u => u.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(); 
        }

        public async Task<List<Product>> GetUserProducts(User user)
        {
           
            return user.CartItems.Select(x => x.Product).ToList();
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

        public async Task<bool> ChangeOrderInformation(Guid orderId, ChangeOrderInformationModel model)
        {
            var order = await _context.orders.
                Where(x => x.OrderId == orderId)
                .Include(x=>x.Info).
                FirstOrDefaultAsync();

            if (order != null)
            {
                order.Info.Name = model.Name;
                order.Info.Address = model.Address;
                order.Info.Address2 = model.Address2;
                order.Info.City = model.City;
                order.Info.Country = model.Country;
                order.Info.Email = model.Email;
                order.Info.LastName = model.LastName;
                order.Info.PhoneNumber = model.PhoneNumber;
                order.Info.Region = model.Region;
                order.Info.ZipCode = model.ZipCode;

                await _context.SaveChangesAsync();

                await _hubContext.Clients.All.SendAsync("MakeOrder", order);

                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<Order> CreateNewOrder(List<Product> products, User user, OrderModel model, int totalPrice)
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
                    Name = model.ContactInfo.Name,
                    LastName = model.ContactInfo.LastName,
                    Email = model.ContactInfo.Email,
                    PhoneNumber = model.ContactInfo.PhoneNumber,
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

            await _hubContext.Clients.All.SendAsync("MakeOrder", newOrder);

            return newOrder;
        }

        public async Task<List<Order>> ShowOrders(Guid userId)
        {
            return await _context.orders.Where(id => id.UserId == userId).Include(x => x.OrderLists).Include(x => x.Info).OrderByDescending(x => x.OrderTime).ToListAsync();
        }

        public async Task<Order> GetOrder(Guid orderId)
        {
            return await _context.orders.Where(x => x.OrderId == orderId).Include(x=> x.Info).FirstOrDefaultAsync();
        }

        public async Task<string> ChangeOrderStatus(Order order, OrderStatus orderStatus)
        {
            order.OrderStatus = orderStatus;

            var orderDTO = _context.orders
                .Where(x => x.OrderId == order.OrderId)
                .Include(x => x.OrderLists)
                .Include(x => x.Info)
                .FirstOrDefault();

            await _context.SaveChangesAsync();

            await _hubContext.Clients.All.SendAsync("MakeOrder", orderDTO);

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

