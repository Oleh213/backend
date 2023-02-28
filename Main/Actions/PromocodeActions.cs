using System;
using System.Linq;
using System.Security.Claims;
using System.Xml.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebShop.Main.BusinessLogic;
using WebShop.Main.Conext;
using WebShop.Main.Context;
using WebShop.Main.DBContext;
using WebShop.Main.Interfaces;
using WebShop.Models;

namespace Shop.Main.Actions
{
    [ApiController]
    [Route("[controller]")]
    public class PromocodeActions : ControllerBase
    {
        private IPromocodeActionsBL _promocodeActionsBL;

        private readonly ILoggerBL _loggerBL;

        public PromocodeActions(IPromocodeActionsBL promocodeActionsBL, ILoggerBL loggerBL)
        {
            _promocodeActionsBL = promocodeActionsBL;
            _loggerBL = loggerBL;
        }

        private Guid UserId => Guid.Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        //[HttpPost("AddPromocode")]
        //public IActionResult AddPromocode(int _discount, string _code, Guid _userId)
        //{
        //    var user = _context.users.FirstOrDefault(z => z.UserId == _userId);

        //    if (user != null)
        //    {
        //        if (user.Role == UserRole.Admin)
        //        {
        //            _context.promocodes.Add(new Promocode()
        //            {
        //                PromocodetId = Guid.NewGuid(),
        //                Code = _code,
        //                Discount = _discount
        //            });
        //            _context.SaveChanges();
        //            return Ok($"Promocode: {_code}, added to promolist");
        //        }
        //        else
        //            return Unauthorized($"Error {user.Name}!");
        //    }
        //    else
        //        return Unauthorized($"Error {user.Name}!");
        //}

        [HttpPost("UsePromocode")]
        public async Task<IActionResult> UsePromocode([FromBody] PromocodeModel model)
        {
            try
            {
                var promo = await _promocodeActionsBL.GetPromocode(model.Code);

                if (promo != null)
                {
                    var resOk = new Response<int>()
                    {
                        IsError = true,
                        ErrorMessage = "",
                        Data = promo.Discount,
                    };

                    _loggerBL.AddLog(LoggerLevel.Info, $"User:'{UserId}' used Promocode:'{promo.Code}'");
                    return Ok(resOk);
                }
                else
                {
                    var resError = new Response<int>()
                    {
                        IsError = true,
                        ErrorMessage = "Promocode don't found",
                        Data = 0,
                    };

                    _loggerBL.AddLog(LoggerLevel.Warn, $"User:'{UserId}' wanted to use Promocode:'{promo.Code}'");
                    return Ok(resError);
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

