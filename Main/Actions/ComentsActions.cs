using System;
using System.Collections.Generic;
//using System.Data.Entity;
using System.Linq;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using WebShop.Main.Conext;
using WebShop.Main.Context;
using Microsoft.EntityFrameworkCore;
using WebShop.Main.DBContext;
using WebShop.Main.Interfaces;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using WebShop.Models;
using WebShop.Main.DTO;
using WebShop.Main.BusinessLogic;


namespace WebShop.Main.Actions
{
    [ApiController]
    [Route("ComentsActions")]
    public class ComentsActions : ControllerBase
    {
        private IComentsActionsBL _comentsActionsBL;

        private readonly ILoggerBL _loggerBL;

        public ComentsActions(IComentsActionsBL comentsActionsBL, ILoggerBL loggerBL)
        {
            _comentsActionsBL = comentsActionsBL;
            _loggerBL = loggerBL;
        }

        private Guid UserId => Guid.Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        [HttpPost("AddComent")]
        [Authorize]
        public async Task<IActionResult> AddComent([FromBody] ComentsModel model)
        {
            try
            {
                var user = await _comentsActionsBL.GetUser(UserId);

                var product = await _comentsActionsBL.GetProduct(model.ProductId);

                if (user != null)
                {
                    var id = await _comentsActionsBL.AddComent(model, UserId);

                    await _comentsActionsBL.CountRating(product);

                    var ok = new Response<string>()
                    {
                        IsError = false,
                        ErrorMessage = "",
                        Data = "Coment was successfully added"
                    };

                    _loggerBL.AddLog(LoggerLevel.Info, $"UserId:'{UserId}' added new ComentId:'{id}'");
                    return Ok(ok);
                }
                else
                {
                    var resError = new Response<string>()
                    {
                        IsError = true,
                        ErrorMessage = "",
                        Data = "Please log in to add coment!"
                    };
                    _loggerBL.AddLog(LoggerLevel.Warn, $"UserId:'{UserId}' not login to write comment!");
                    return NotFound(resError);
                }
            }
            catch (Exception ex)
            {
                _loggerBL.AddLog(LoggerLevel.Error, ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("GetComent")]
        public async Task<IActionResult> GetComent([FromQuery] Guid ProductId)
        {
            try
            {
                var coments = await _comentsActionsBL.GetComents(ProductId);

                if (coments != null)
                {
                    var comentsDTO = _comentsActionsBL.ComentsDTO(coments);

                    _loggerBL.AddLog(LoggerLevel.Info, $"UserId:'{UserId}' get coment of ProductId:'{ProductId}'");
                    return Ok(comentsDTO);
                }
                else
                {
                    _loggerBL.AddLog(LoggerLevel.Warn, $"UserId:'{UserId}' wanted get coments ProductId:'{ProductId}'(Coments don't found!)");
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _loggerBL.AddLog(LoggerLevel.Error, ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("UpdateComent")]
        [Authorize]
        public async Task<IActionResult> UpdateComent([FromBody] PatchModel model)
        {
            try
            {
                var coment = await _comentsActionsBL.GetComent(model.ComentId);

                if (coment != null)
                {
                    await _comentsActionsBL.ChangeComent(coment, model.Body);

                    _loggerBL.AddLog(LoggerLevel.Info, $"UserId:'{UserId}' eddit coment of ComentId:'{model.ComentId}'");
                    return Ok();
                }
                else
                {
                    _loggerBL.AddLog(LoggerLevel.Warn, $"UserId:'{UserId}' wanted eddit coment ComentId:'{model.ComentId}'(ComentId don't found!)");
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _loggerBL.AddLog(LoggerLevel.Error, ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("DeleteComent")]
        [Authorize]
        public async Task<IActionResult> DeleteComent([FromQuery] Guid ComentId)
        {
            try
            {
                var coment = await _comentsActionsBL.GetComent(ComentId);

                if (coment != null)
                {
                    var child = await _comentsActionsBL.GetChildComents(ComentId);

                    await _comentsActionsBL.RemoveComent(coment, child);

                    _loggerBL.AddLog(LoggerLevel.Info, $"UserId:'{UserId}' delet coment ComentId:'{ComentId}'");
                    return Ok();
                }
                else
                {
                    _loggerBL.AddLog(LoggerLevel.Warn, $"UserId:'{UserId}' wanted delet coment ComentId:'{ComentId}'(ComentId don't found!)");
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

