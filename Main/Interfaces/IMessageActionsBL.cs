using System;
using WebShop.Main.Conext;
using WebShop.Main.Context;
using WebShop.Main.DTO;

namespace WebShop.Main.Interfaces
{
	public interface IMessageActionsBl
    {
        Task<User> GetUser(Guid userId);

        Task <string> AddMessage(Guid UserId, string Message, Guid ProductId, string Name);

        Task<List<Message>> GetMessages(Guid ProductId);

        List<MessagesDTO> MessageDTO(List<Message> messages);

    }
}

