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

        Task<string> CreateNewOrder(List<Product> products, User user, OrderModel model);

        Task<List<Order>> ShowOrders(Guid userId);
    }
}

