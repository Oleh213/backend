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
        private ShopContext _context;

        private IOrderActionsBL _orderActionsBL;

        public OrderActions(ShopContext context, IOrderActionsBL orderActionsBL)
        {
            _context = context;
            _orderActionsBL = orderActionsBL;
        }

        private Guid UserId => Guid.Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        [HttpPost("Buy")]
        [Authorize]
        public async Task<IActionResult> MakeOrder([FromBody] OrderModel model)
        {
            var user =  await _orderActionsBL.GetUser(UserId);

            if (model.Card != null)
            {
                var card = await _orderActionsBL.GetCartd(model.Card.CardNumber, model.Card.ExpiredDate, model.Card.Cvv);

                if (card != null)
                {
                    if (card.Balance >= model.TotalPrice)
                    {
                        card.Balance -= model.TotalPrice;
                    }
                    else
                    {
                        var resError = new Response<string>()
                        {
                            IsError = true,
                            ErrorMessage = "Error ",
                            Data = "You don't have enough amount on your card's balance"
                        };
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
                    return NotFound(resError);
                }
            }
            else
            {
                if (user.AccountBalance >= model.TotalPrice)
                {
                    user.AccountBalance -= model.TotalPrice;
                }
                else
                {
                    var resError = new Response<string>()
                    {
                        IsError = true,
                        ErrorMessage = "Error ",
                        Data = "You don't have enough amount on your account balance"
                    };
                    return NotFound(resError);
                }
            }
            var products = user.CartItems.Select(ci => ci.Product).ToList();

            if (! await _orderActionsBL.CheckCountOfProducts(products, user))
            {
                var resError = new Response<string>()
                {
                    IsError = true,
                    ErrorMessage = "Error ",
                    Data = $"Error, count of product is no avialable!"
                };
                return NotFound(resError);
            }

            await _orderActionsBL.CreateNewOrder(products, user, model);

            var resOk = new Response<string>()
            {
                IsError = false,
                ErrorMessage = " ",
                Data = $"The order was successful create"
            };
            return Ok(resOk);
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
            var user = await _orderActionsBL.GetUser(UserId);

            if (user != null)
            {
                if (user.Role == UserRole.Admin || user.Role == UserRole.Manager || model.orderStatus == OrderStatus.Canceled)
                {
                    var order = await _orderActionsBL.GetOrder(model.OrderId);

                    if (order.OrderStatus != OrderStatus.AwaitingConfirm && model.orderStatus == OrderStatus.Canceled)
                    {
                        return NotFound();
                    }
                    if (order != null)
                    {
                        await _orderActionsBL.ChangeOrderStatus(order, model.orderStatus);

                        return Ok();

                    }
                    else return NotFound();
                }
                else return NotFound();
            }
            else return NotFound();
        }

        [HttpGet("GetNewOrders")]
        public async Task<IActionResult> GetNewOrders()
        {
            var user = await _orderActionsBL.GetUser(UserId);

            if (user != null)
            {
                if (user.Role == UserRole.Admin || user.Role == UserRole.Manager)
                {
                    return Ok(await _orderActionsBL.GetNewOrders());

                }
                else return NotFound();
            }
            else return NotFound();
        }
    }
}





