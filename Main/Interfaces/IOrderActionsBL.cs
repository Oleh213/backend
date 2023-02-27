using System;
using WebShop.Main.Conext;
using WebShop.Main.Context;
using WebShop.Models;

namespace WebShop.Main.Interfaces
{
	public interface IOrderActionsBL
	{
        Task<User> GetUser(Guid userId);

        Task<Cards> GetCartd(string cardNumber, string expiredDate, string cvv);

        Task<bool> CheckCountOfProducts(List<Product> products, User user);

        Task<string> CreateNewOrder(List<Product> products, User user, OrderModel model, int totalPrice);

        Task<List<Order>> ShowOrders(Guid userId);

        Task<Order> GetOrder(Guid orderId);

        Task<string> ChangeOrderStatus(Order order, OrderStatus orderStatus);

        Task<List<Order>> GetNewOrders();

        Task<int> GetTotalPrice(User user, string promocode);
    }
}

