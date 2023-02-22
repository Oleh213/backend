using System;
using WebShop.Main.Context;

namespace WebShop.Main.Interfaces
{
	public interface IFilterActionsBL
	{
        Task<Dictionary<string, List<Characteristics>>> GetChatacteristics();

        Dictionary<string, List<string>> FirlterDTO(Dictionary<string, List<Characteristics>> chatacteristics);

    }
}

