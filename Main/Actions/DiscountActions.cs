using System;
using Microsoft.AspNetCore.Mvc;
using WebShop.Main.Conext;
using WebShop.Main.DBContext;
using WebShop.Main.Interfaces;
using Microsoft.IdentityModel.Tokens;
using WebShop.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using WebShop.Main.Context;

namespace WebShop.Main.Actions
{

    [ApiController]
    [Route("[controller]")]
    public class DiscountActions : ControllerBase
    {
        private IDiscountActionsBL _discountActionsBL;

        private readonly ILoggerBL _loggerBL;

        public DiscountActions(IDiscountActionsBL discountActionsBL, ILoggerBL loggerBL)
        {
            _discountActionsBL = discountActionsBL;
            _loggerBL = loggerBL;
        }

        private Guid UserId => Guid.Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        [HttpPost("AddDiscount")]
        [Authorize]
        public async Task<IActionResult> AddDiscount([FromBody] AddProductDiscountModel model)
        {
            try
            {
                var user = await _discountActionsBL.GetUser(UserId);

                if (user != null)
                {
                    if (user.Role == UserRole.Admin)
                    {
                        var product = await _discountActionsBL.GetProduct(model.ProductId);

                        if (product != null)
                        {
                            if (await _discountActionsBL.UsePromocode(product, model.DiscountType, model.Discount))
                            {
                                var resOk = new Response<string>()
                                {
                                    IsError = false,
                                    ErrorMessage = "",
                                    Data = "Discount successfully added!"
                                };

                                _loggerBL.AddLog(LoggerLevel.Info, $"UserId:'{UserId}' added discount to ProductId:'{model.ProductId}'(Discount: {model.Discount} DiscountType: {model.DiscountType})");
                                return Ok(resOk);
                            }
                            else
                            {
                                _loggerBL.AddLog(LoggerLevel.Warn, $"UserId:'{UserId}' want add discount to ProductId:'{model.ProductId}'(Discount: {model.Discount} DiscountType: {model.DiscountType})");
                                return NotFound();
                            }
                        }
                        else
                            return NotFound();
                    }
                    else
                    {
                        var resEr = new Response<string>()
                        {
                            IsError = true,
                            ErrorMessage = "401",
                            Data = $"* Error *, You can't do it!"
                        };

                        _loggerBL.AddLog(LoggerLevel.Warn, $"UserId:'{UserId}' want add discount to ProductId:'{model.ProductId}'(Permission denied!)");
                        return Unauthorized(resEr);
                    }
                }
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                _loggerBL.AddLog(LoggerLevel.Error, ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpPost("ClearDiscount")]
        [Authorize]
        public async Task<IActionResult> ClearDiscount([FromBody] ClearDiscountProduct model)
        {
            try
            {
                var user = await _discountActionsBL.GetUser(UserId);

                if (user != null)
                {
                    if (user.Role == UserRole.Admin)
                    {
                        var product = await _discountActionsBL.GetProduct(model.ProductId);

                        if (product != null)
                        {
                            if (product.Discount != 0)
                            {
                                await _discountActionsBL.ClearDiscount(product);

                                var resOk = new Response<string>()
                                {
                                    IsError = false,
                                    ErrorMessage = "",
                                    Data = "Discount successfully cleaned!"
                                };

                                _loggerBL.AddLog(LoggerLevel.Info, $"UserId:'{UserId}' clean discount to ProductId:'{model.ProductId}'");
                                return Ok(resOk);
                            }
                            else
                            {
                                _loggerBL.AddLog(LoggerLevel.Warn, $"UserId:'{UserId}' wanted clean discount to ProductId:'{model.ProductId}'(Product discount != 0)");
                                return NotFound();
                            }
                        }
                        else
                        {
                            _loggerBL.AddLog(LoggerLevel.Warn, $"UserId:'{UserId}' wanted clean discount to ProductId:'{model.ProductId}'(ProductId:'{model.ProductId}' don't found)");
                            return NotFound();
                        }
                    }
                    else
                    {
                        var resEr = new Response<string>()
                        {
                            IsError = true,
                            ErrorMessage = "401",
                            Data = $"* Error *, You can't do it!"
                        };

                        _loggerBL.AddLog(LoggerLevel.Warn, $"UserId:'{UserId}' wanted clean discount to ProductId:'{model.ProductId}'(Permission denied)");
                        return Unauthorized(resEr);
                    }
                }
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                _loggerBL.AddLog(LoggerLevel.Error, ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}


