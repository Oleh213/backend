using System;
namespace WebShop.Main.BusinessLogic
{
	public interface IAdminActionsBL
	{
        Task<string> AddAdmin(Guid _userId);
    }
}

