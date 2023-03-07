using System;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Mvc;
using WebShop.Main.Conext;
using WebShop.Main.Context;
using System.Security.Claims;
using WebShop.Main.DBContext;
using WebShop.Main.Hubs;
using Microsoft.AspNetCore.Authorization;
using WebShop.Main.Interfaces;
using WebShop.Models;
using WebShop.Main.BusinessLogic;

namespace WebShop.Main.Actions
{
    [ApiController]
    [Route("MessageActions")]
    public class MessageActions : ControllerBase
    {
        private readonly IHubContext<Hubs.ChatHub> _hubContext;

        private readonly IMessageActionsBl _messageActionsBl;

        private readonly ILoggerBL _loggerBL;

        public MessageActions(IHubContext<Hubs.ChatHub> hubContext, IMessageActionsBl messageActionsBl, ILoggerBL loggerBL)
        {
            _hubContext = hubContext;
            _messageActionsBl = messageActionsBl;
            _loggerBL = loggerBL;
        }

        private Guid UserId => Guid.Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);


        [Authorize]
        [Route("SendMessage")]
        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody] SendMessageModel model)
        {
            try
            {
                var user = await _messageActionsBl.GetUser(UserId);

                if (user != null)
                {
                    await _messageActionsBl.AddMessage(UserId, model.Message, model.ProductId);

                    await _hubContext.Clients.All.SendAsync("ReceiveOne", model.Message, user.Name);

                    _loggerBL.AddLog(LoggerLevel.Info, $"User:'{user.UserId}' write new coment to ProductId:{model.ProductId} ");
                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _loggerBL.AddLog(LoggerLevel.Error, ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Route("GetMessages")]
        [HttpGet]
        public async Task<IActionResult> GetMessages([FromQuery] GetMessagesModel model)
        {
            try
            {
                var messages = await _messageActionsBl.GetMessages(model.ProductId);

                if (messages != null)
                {
                    var messagesDTO = _messageActionsBl.MessageDTO(messages);

                    return Ok(messagesDTO);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _loggerBL.AddLog(LoggerLevel.Error, ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

	}
}

