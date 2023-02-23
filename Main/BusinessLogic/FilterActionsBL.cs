using System;
using System.Reflection.PortableExecutable;
using Microsoft.EntityFrameworkCore;
using WebShop.Main.Conext;
using WebShop.Main.Context;
using WebShop.Main.DBContext;
using WebShop.Main.Interfaces;
using Characteristics = WebShop.Main.Context.Characteristics;

namespace WebShop.Main.BusinessLogic
{
    public class FilterActionsBL : IFilterActionsBL
    {
        private ShopContext _context;

        public FilterActionsBL(ShopContext context)
        {
            _context = context;
        }

        public async Task<Dictionary<string, List<Characteristics>>> GetChatacteristics()
        {
            return await _context.characteristics.GroupBy(x => x.CharacteristicName).ToDictionaryAsync(g => g.Key, g => g.ToList());
        }

        public Dictionary<string, List<string>> FirlterDTO(Dictionary<string, List<Characteristics>> chatacteristics)
        {
            Dictionary<string, List<string>> characteristicsDTO = new Dictionary<string, List<string>>();

            foreach (var item in chatacteristics)
            {
                List<string> list = new List<string>();

                item.Value.ToList().ForEach(x => { list.Add(x.CharacteristicValue); });

                characteristicsDTO.Add(item.Key, list);
            }
            return characteristicsDTO;
        }


    }
}

