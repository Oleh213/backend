using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using WebShop.Main.DBContext;
using WebShop.Main.Interfaces;
using Microsoft.EntityFrameworkCore;
using WebShop.Main.BusinessLogic;
using WebShop.Main.Context;

namespace WebShop.Main.Actions
{
    [ApiController]
    [Route("[controller]")]
    public class FilterActions : ControllerBase
    {
        public IFilterActionsBL _filterActionsBL;

        private readonly ILoggerBL _loggerBL;

        public FilterActions(IFilterActionsBL filterActionsBL, ILoggerBL loggerBL)
        {
            _filterActionsBL = filterActionsBL;
            _loggerBL = loggerBL;
        }

        private Guid UserId => Guid.Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        [HttpGet("GetFilter")]
        public async Task<IActionResult> GetFilterOfCharacteristics()
        {
            try
            {
                var chatacteristics = await _filterActionsBL.GetChatacteristics();

                var characteristicsDTO = _filterActionsBL.FirlterDTO(chatacteristics);

                return Ok(characteristicsDTO.ToList());
            }
            catch (Exception ex)
            {
                _loggerBL.AddLog(LoggerLevel.Error, ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}

