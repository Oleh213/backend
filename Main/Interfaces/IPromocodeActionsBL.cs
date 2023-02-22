using System;
using WebShop.Main.Conext;
using WebShop.Models;

namespace WebShop.Main.Interfaces
{
	public interface IPromocodeActionsBL
	{
        Task<User> GetUser(Guid userId);

        Task<Promocode> GetPromocode(string code);
    }
}

