using System;
using Microsoft.EntityFrameworkCore;
using WebShop.Main.Conext;
using WebShop.Main.Context;
using WebShop.Main.DBContext;
using WebShop.Main.DTO;
using WebShop.Main.Interfaces;

namespace WebShop.Main.BusinessLogic
{
    public class MessageActionsBl : IMessageActionsBl
    {
        private ShopContext _context;

        public MessageActionsBl(ShopContext context)
        {
            _context = context;
        }

        public async Task<User> GetUser(Guid userId)
        {
            return await _context.users.FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public async Task<string> AddMessage(Guid UserId, string Message, Guid ProductId)
        {
            _context.messages.Add(new Context.Message { UserId = UserId, MessageId = Guid.NewGuid(), MessageText = Message, ProductId = ProductId, DateTime = DateTime.Now });

            await _context.SaveChangesAsync();

            return "Ok";
        }

        public async Task<List<Message>> GetMessages(Guid ProductId)
        {
            return await _context.messages.Where(x => x.ProductId == ProductId).OrderBy(x => x.DateTime).ToListAsync();
        }

        public List<MessagesDTO> MessageDTO(List<Message> messages)
        {
            var messagesDTO = new List<MessagesDTO>();

            var users = _context.users;

            foreach (var item in messages)
            {
                var userName = users.FirstOrDefault(x=> x.UserId == item.UserId).Name;

                messagesDTO.Add(new MessagesDTO { Message = item.MessageText, UserName = userName, DataTime = item.DateTime });
            }

            return messagesDTO;

        }
    }
}

