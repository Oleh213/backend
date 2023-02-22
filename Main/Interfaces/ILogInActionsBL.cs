using System;
using WebShop.Main.Conext;

namespace WebShop.Main.Interfaces
{
	public interface ILogInActionsBL
	{
        string GenerateJWT(User user);

        Task<User> AuthenticateUser(string name, string password);
    }
}

