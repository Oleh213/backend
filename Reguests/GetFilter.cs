using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebShop.Main.Conext;
using WebShop.Main.DBContext;
using System.Data.Entity;

namespace WebShop.Reguests
{
    [ApiController]
    [Route("[controller]")]
    public class GetFilter : ControllerBase
    {
        private ShopContext _context;

        public GetFilter(ShopContext context)
        {
            _context = context;
        }

        private Guid UserId => Guid.Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        [HttpGet("GetFilter")]
        public IActionResult GetFilterOfCharacteristics()
        {
            var chatacteristics = _context.characteristics.GroupBy(x=> x.CharacteristicName).ToDictionary(g => g.Key, g => g.ToList());

            Dictionary<string, List<string>> characteristicsDTO = new Dictionary<string, List<string>>();

            foreach(var item in chatacteristics)
            {
                List<string> list = new List<string>();

                item.Value.ToList().ForEach(x => { list.Add(x.CharacteristicValue); });

                characteristicsDTO.Add(item.Key, list);
            }
            return Ok(characteristicsDTO.ToList());
          
        }
    }
}



    
    