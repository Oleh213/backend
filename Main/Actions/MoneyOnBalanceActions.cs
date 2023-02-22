using System;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebShop.Main.Conext;
using WebShop.Main.DBContext;
using WebShop.Models;
using Microsoft.AspNetCore.Authorization;
using WebShop.Main.Interfaces;

namespace WebShop.Main.Actions
{
    [ApiController]
    [Route("MoneyToBalance")]
    public class MoneyOnBalanceActions : ControllerBase
    {
        private ShopContext _context;

        private IMoneyOnBalanceActionsBL _moneyOnBalanceActionsBL;

        public MoneyOnBalanceActions(ShopContext context, IMoneyOnBalanceActionsBL moneyOnBalanceActionsBL)
        {
            _context = context;

            _moneyOnBalanceActionsBL = moneyOnBalanceActionsBL;
        }
        private Guid UserId => Guid.Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        [Authorize]
        [HttpPost("AddMoney")]
        public async Task<IActionResult> AddMoney([FromBody] TopUpModel model)
        {
            var card = await _moneyOnBalanceActionsBL.GetCartd(model.CardNumber);

            if (card != null)
            {
                if (card.CardNumber == model.CardNumber && card.Cvv == model.Cvv && card.ExpiredDate == model.ExpiredDate)
                {
                    if (card.Balance >= model.RequestedAmount)
                    {
                        var user = await _moneyOnBalanceActionsBL.GetUser(UserId);

                        await _moneyOnBalanceActionsBL.TopUpBalance(user,card,model.RequestedAmount);

                        var resOk = new Response<string>()
                        {
                            IsError = false,
                            ErrorMessage = "",
                            Data = "Your balance successful top up"
                        };
                        return Ok(resOk);
                    }
                    else
                    {
                        var resError2 = new Response<string>()
                        {
                            IsError = true,
                            ErrorMessage = "No ammont",
                            Data = "You don't have enough amount on your card!"
                        };

                        return NotFound(resError2);
                    }
                }
                else
                {
                    var resError1 = new Response<string>()
                    {
                        IsError = true,
                        ErrorMessage = "Card don't found",
                        Data = "Card information is not correct"
                    };
                    return NotFound(resError1);
                }
                    
            }
            else
            {
                var resError1 = new Response<string>()
                {
                    IsError = true,
                    ErrorMessage = "Card don't found",
                    Data = "Card don't found"
                };
                return NotFound(resError1);
            }
        }
        [HttpGet("GetAccountBalance")]
        [Authorize]
        public async  Task<IActionResult> GetAccountBalance()
        {
            var user = await _moneyOnBalanceActionsBL.GetUser(UserId);

            if (user != null)
            {
                var res = new Response<int>()
                {
                    IsError = true,
                    ErrorMessage = "Item in cart ",
                    Data = user.AccountBalance,
                };
                return Ok(res);
            }

            else
                return NotFound();
        }
    }
}

