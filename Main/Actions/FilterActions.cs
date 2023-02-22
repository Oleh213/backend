using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using WebShop.Main.DBContext;
using WebShop.Main.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace WebShop.Main.Actions
{
    [ApiController]
    [Route("[controller]")]
    public class FilterActions : ControllerBase
    {
        private ShopContext _context;

        public IFilterActionsBL _filterActionsBL;

        public FilterActions(ShopContext context, IFilterActionsBL filterActionsBL)
        {
            _context = context;
            _filterActionsBL = filterActionsBL;
        }

        private Guid UserId => Guid.Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        [HttpGet("GetFilter")]
        public async Task<IActionResult> GetFilterOfCharacteristics()
        {
            var chatacteristics = await _filterActionsBL.GetChatacteristics();

            var characteristicsDTO = _filterActionsBL.FirlterDTO(chatacteristics);

            return Ok(characteristicsDTO.ToList());
        }
    }
}

