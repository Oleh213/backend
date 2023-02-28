using System;
using WebShop.Main.Conext;
using WebShop.Main.Context;
using WebShop.Main.DTO;
using WebShop.Models;

namespace WebShop.Main.Interfaces
{
	public interface IComentsActionsBL
	{
        Task<User> GetUser(Guid userId);

        Task<Product> GetProduct(Guid productId);

        Task<Guid> AddComent(ComentsModel model, Guid userId);

        Task<string> CountRating(Product product);

        Task<List<Coments>> GetComents(Guid productId);

        List<ComentsDTO> ComentsDTO(List<Coments> coments);

        Task<Coments> GetComent(Guid comentId);

        Task<string> ChangeComent(Coments coment, string body);

        Task<List<Coments>> GetChildComents(Guid comentId);

        Task<string> RemoveComent(Coments coment, List<Coments> child);
    }
}

