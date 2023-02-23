using System;
using WebShop.Main.Conext;
using WebShop.Main.Context;
using WebShop.Main.DBContext;
using WebShop.Main.DTO;
using WebShop.Main.Interfaces;
using WebShop.Models;
using Microsoft.EntityFrameworkCore;


namespace WebShop.Main.BusinessLogic
{
	public class ComentsActionsBL : IComentsActionsBL
    {
        private readonly ShopContext _context;

        public ComentsActionsBL(ShopContext context)
        {
            _context = context;
        }

        public async Task<User> GetUser(Guid userId)
        {
            return await _context.users.FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public async Task<Product> GetProduct(Guid productId)
        {
            return await _context.products.FirstOrDefaultAsync(x => x.ProductId == productId);
        }

        public async Task<string> AddComent(ComentsModel model, Guid userId)
        {
            _context.coments.Add(new Coments
            {
                ComentId = Guid.NewGuid(),
                Body = model.Body,
                ParentId = model.ParentId,
                CreatedAt = DateTime.Now,
                ProductId = model.ProductId,
                UserId = userId,
                Rating = model.Rating,
            });

            await _context.SaveChangesAsync();

            return "Ok";
        }

        public async Task<string> CountRating(Product product)
        {
            _context.users.Load();
            _context.coments.Load();
            _context.products.Load();

            int reting = 0;
            int count = 0;

            var comments = _context.coments.Where(x => x.ProductId == product.ProductId);

            foreach (var item in comments)
            {
                if (item.Rating != null)
                {
                    reting += (int)item.Rating;
                    count++;
                }
            }
            product.Rating = reting / count;

            await _context.SaveChangesAsync();

            return "Ok";
        }

        public async Task<List<Coments>> GetComents(Guid productId)
        {
            return  await _context.coments.Where(x => x.ProductId == productId).ToListAsync();
        }

        public List<ComentsDTO> ComentsDTO(List<Coments> coments)
        {
            var comentsDTO = new List<ComentsDTO>();

            foreach (var item in coments)
            {
                var user = _context.users.FirstOrDefault(x => x.UserId == item.UserId);

                if(user!=null)
                {
                    comentsDTO.Add(new ComentsDTO
                    {
                        Body = item.Body,
                        UserId = item.UserId,
                        Username = user.Name,
                        ComentId = item.ComentId,
                        CreatedAt = item.CreatedAt,
                        ParentId = item.ParentId,
                        ProductId = item.ProductId,
                        Rating = item.Rating,
                    });
                }                
            }

            return comentsDTO;

        }
        public async Task<Coments> GetComent(Guid comentId)
        {
            return await _context.coments.FirstOrDefaultAsync(x => x.ComentId == comentId);
        }

        public async Task<string> ChangeComent(Coments coment, string body)
        {
            coment.Body = body;

            await _context.SaveChangesAsync();

            return "Ok";
        }

        public async Task<List<Coments>> GetChildComents(Guid comentId)
        {
            return await _context.coments.Where(x => x.ParentId == comentId).ToListAsync();
        }

        public async Task<string> RemoveComent(Coments coment, List<Coments> child)
        {
            foreach (var item in _context.coments)
            {
                if (item.ParentId == coment.ComentId)
                {
                    _context.coments.Remove(item);
                }
            }
            _context.coments.RemoveRange(child);

            _context.coments.Remove(coment);

            await _context.SaveChangesAsync();

            return "Ok";
        }

    }
}

