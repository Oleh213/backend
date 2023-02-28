using System;
using WebShop.Main.Conext;
using WebShop.Main.DBContext;
using WebShop.Main.Interfaces;
using System.Data.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Shop.Main.BusinessLogic;
using WebShop.Main.BusinessLogic;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Shop.Main.Actions
{
    [ApiController]
    [Route("AdminActions")]
    public class AddAdminActions : ControllerBase
    {

        private readonly IAdminActionsBL _addAdminActionsBL;

        private readonly ILoggerBL _loggerBL; 

        public AddAdminActions(IAdminActionsBL addAdminActionsBL, ILoggerBL loggerBL)
        {
            _addAdminActionsBL = addAdminActionsBL;
            _loggerBL = loggerBL;
        }

        private Guid UserId => Guid.Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);


        [HttpPut("AddAdmin")]
        [Authorize]
        public async Task<IActionResult> AddAdmin(Guid _userId)
        {
            try
            {
                var name = await _addAdminActionsBL.AddAdmin(_userId);

                _loggerBL.AddLog(WebShop.Main.Context.LoggerLevel.Info, $"{_userId}, was added to admins team by {UserId}");
                return Ok($"User name is {name}");
            }
            catch (Exception ex)
            {
                _loggerBL.AddLog(WebShop.Main.Context.LoggerLevel.Error, ex.Message);
            
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}

