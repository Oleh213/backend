using System;
using System.Collections.Generic;
// using System.Data.Entity;
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

namespace Shop.Main.Actions
{
    [ApiController]
    [Route("OrderActions")]
    public class OrderActions : ControllerBase
    {
        private IOrderActionsBL _orderActionsBL;

        private readonly ILoggerBL _loggerBL;

        public OrderActions(IOrderActionsBL orderActionsBL, ILoggerBL loggerBL)
        {
            _orderActionsBL = orderActionsBL;
            _loggerBL = loggerBL;
        }

        private Guid UserId => Guid.Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        [HttpPost("Buy")]
        [Authorize]
        public async Task<IActionResult> MakeOrder([FromBody] OrderModel model)
        {
            try
            {
                var user = await _orderActionsBL.GetUser(UserId);

                var totalPrice = await _orderActionsBL.GetTotalPrice(user, model.Promocode);

                if (model.Card != null)
                {
                    var card = await _orderActionsBL.GetCartd(model.Card.CardNumber, model.Card.ExpiredDate, model.Card.Cvv);

                    if (card != null)
                    {
                        if (card.Balance >= totalPrice)
                        {
                            card.Balance -= totalPrice;
                        }
                        else
                        {
                            var resError = new Response<string>()
                            {
                                IsError = true,
                                ErrorMessage = "Error ",
                                Data = "You don't have enough amount on your card's balance"
                            };
                            _loggerBL.AddLog(LoggerLevel.Warn, $"User:'{user.UserId}' wanted buy items!(Total price:'{totalPrice}', Card balance:'{card.Balance}')");
                            return NotFound(resError);
                        }
                    }
                    else
                    {
                        var resError = new Response<string>()
                        {
                            IsError = true,
                            ErrorMessage = "Error ",
                            Data = "Check your credit card information!"
                        };
                        _loggerBL.AddLog(LoggerLevel.Warn, $"User:'{UserId}' enter incorrect card (CardNumber:'{model.Card.CardNumber}', Date:'{model.Card.ExpiredDate}', CVV:'{model.Card.Cvv}')");
                        return NotFound(resError);
                    }
                }
                else
                {
                    if (user.AccountBalance >= totalPrice)
                    {
                        user.AccountBalance -= totalPrice;
                    }
                    else
                    {
                        var resError = new Response<string>()
                        {
                            IsError = true,
                            ErrorMessage = "Error ",
                            Data = "You don't have enough amount on your account balance"
                        };
                        _loggerBL.AddLog(LoggerLevel.Warn, $"User:'{UserId}' wanted top up account balance using account balamce (Amount:'{totalPrice}', Acount Balance:'{user.AccountBalance}')");
                        return NotFound(resError);
                    }
                }
                var products = user.CartItems.Select(ci => ci.Product).ToList();

                if (!await _orderActionsBL.CheckCountOfProducts(products, user))
                {
                    var resError = new Response<string>()
                    {
                        IsError = true,
                        ErrorMessage = "Error ",
                        Data = $"Error, count of product is no avialable!"
                    };

                    _loggerBL.AddLog(LoggerLevel.Warn, $"User:'{user.UserId}' wanted buy items!(Count of product is no avialable!)");
                    return NotFound(resError);
                }

                await _orderActionsBL.CreateNewOrder(products, user, model, totalPrice);

                var resOk = new Response<string>()
                {
                    IsError = false,
                    ErrorMessage = " ",
                    Data = $"The order was successful create"
                };

                _loggerBL.AddLog(LoggerLevel.Info, $"User:'{UserId}' bought products!");
                return Ok(resOk);
            }
            catch (Exception ex)
            {
                _loggerBL.AddLog(LoggerLevel.Error, ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize]
        [HttpGet("ShowBuyList")]
        public async Task<IActionResult> ShowBuyList()
        {
            var orderedProductIds = await _orderActionsBL.ShowOrders(UserId);

            return Ok(orderedProductIds);
        }


        [HttpPost("ChangeOrderStatus")]
        public async Task<IActionResult> ChangeOrderStatus([FromBody] ChangeOrderStatusModel model)
        {
            try
            {
                var user = await _orderActionsBL.GetUser(UserId);

                if (user != null)
                {
                    if (user.Role == UserRole.Admin || user.Role == UserRole.Manager || model.orderStatus == OrderStatus.Canceled)
                    {
                        var order = await _orderActionsBL.GetOrder(model.OrderId);

                        if (order.OrderStatus != OrderStatus.AwaitingConfirm && model.orderStatus == OrderStatus.Canceled)
                        {
                            _loggerBL.AddLog(LoggerLevel.Warn, $"User:'{UserId}' wanted changed order status Order:'{order.OrderId}'(From:{model.orderStatus} to:'{order.OrderStatus}')");
                            return NotFound();
                        }
                        if (order != null)
                        {
                            await _orderActionsBL.ChangeOrderStatus(order, model.orderStatus);

                            _loggerBL.AddLog(LoggerLevel.Info, $"User:'{UserId}' changed order status Order:'{order.OrderId}'(From:{model.orderStatus} to:'{order.OrderStatus}')");
                            return Ok();
                        }
                    }
                    else
                    {
                        _loggerBL.AddLog(LoggerLevel.Warn, $"User:'{UserId}' wanted changed order status(Permission denied)");
                        return NotFound();
                    }
                    return NotFound();
                }
                else return NotFound();
            }
            catch (Exception ex)
            {
                _loggerBL.AddLog(LoggerLevel.Error, ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("GetNewOrders")]
        public async Task<IActionResult> GetNewOrders()
        {
            try
            {
                var user = await _orderActionsBL.GetUser(UserId);

                if (user != null)
                {
                    if (user.Role == UserRole.Admin || user.Role == UserRole.Manager)
                    {
                        _loggerBL.AddLog(LoggerLevel.Info, $"User:'{UserId}' get new orders");
                        return Ok(await _orderActionsBL.GetNewOrders());

                    }
                    else
                    {
                        _loggerBL.AddLog(LoggerLevel.Warn, $"User:'{UserId}' wanted get new orders(Permission denied)");
                        return NotFound();
                    }
                }
                else return NotFound();
            }
            catch (Exception ex)
            {
                _loggerBL.AddLog(LoggerLevel.Error, ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}





