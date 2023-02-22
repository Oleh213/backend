using System;
namespace WebShop.Main.Interfaces
{
	public interface IRegistActionsBL
	{
        Task<bool> CheckName(string name);

        Task<bool> CheckEmail(string name);

        Task<string> Regist(string name, string email, string password);
    }
}

