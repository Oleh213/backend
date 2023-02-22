using System;
using WebShop.Main.Conext;
using WebShop.Main.Context;

namespace WebShop.Main.Interfaces
{
	public interface IMoneyOnBalanceActionsBL
	{
        Task<Cards> GetCartd(string cardNumber);

        Task<User> GetUser(Guid userId);

        Task<string> TopUpBalance(User user, Cards card, int requestedAmount);
    }
}

