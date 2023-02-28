using System;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebShop.Main.Conext;
using WebShop.Main.DBContext;
using WebShop.Models;
using Microsoft.AspNetCore.Authorization;
using WebShop.Main.Interfaces;
using WebShop.Main.BusinessLogic;
using WebShop.Main.Context;

namespace WebShop.Main.Actions
{
    [ApiController]
    [Route("MoneyToBalance")]
    public class MoneyOnBalanceActions : ControllerBase
    {
        private IMoneyOnBalanceActionsBL _moneyOnBalanceActionsBL;

        private readonly ILoggerBL _loggerBL;

        public MoneyOnBalanceActions(IMoneyOnBalanceActionsBL moneyOnBalanceActionsBL, ILoggerBL loggerBL)
        {
            _moneyOnBalanceActionsBL = moneyOnBalanceActionsBL;
            _loggerBL = loggerBL;
        }
        private Guid UserId => Guid.Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        [Authorize]
        [HttpPost("AddMoney")]
        public async Task<IActionResult> AddMoney([FromBody] TopUpModel model)
        {
            try
            {
                var card = await _moneyOnBalanceActionsBL.GetCartd(model.CardNumber);

                if (card != null)
                {
                    if (card.CardNumber == model.CardNumber && card.Cvv == model.Cvv && card.ExpiredDate == model.ExpiredDate)
                    {
                        if (card.Balance >= model.RequestedAmount)
                        {
                            var user = await _moneyOnBalanceActionsBL.GetUser(UserId);

                            await _moneyOnBalanceActionsBL.TopUpBalance(user, card, model.RequestedAmount);

                            var resOk = new Response<string>()
                            {
                                IsError = false,
                                ErrorMessage = "",
                                Data = "Your balance successful top up"
                            };

                            _loggerBL.AddLog(LoggerLevel.Info, $"User:'{user.UserId}' top up account balance using card(CardNumber:'{card.CardNumber}', Amount:'{model.RequestedAmount}')");
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

                            _loggerBL.AddLog(LoggerLevel.Warn, $"User:'{UserId}' wanted top up account balance using card(CardNumber:'{card.CardNumber}', Amount:'{model.RequestedAmount}', CardBalance:'{card.Balance}')");
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

                        _loggerBL.AddLog(LoggerLevel.Warn, $"User:'{UserId}' enter incorrect card information(CardNumber:'{model.CardNumber}', Date:'{model.ExpiredDate}', CVV:'{model.Cvv}')");
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

                    _loggerBL.AddLog(LoggerLevel.Warn, $"User:'{UserId}' enter incorrect card(CardNumber:'{model.CardNumber}', Date:'{model.ExpiredDate}', CVV:'{model.Cvv}')");
                    return NotFound(resError1);
                }
            }
            catch (Exception ex)
            {
                _loggerBL.AddLog(LoggerLevel.Error, ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("GetAccountBalance")]
        [Authorize]
        public async Task<IActionResult> GetAccountBalance()
        {
            try {
                var user = await _moneyOnBalanceActionsBL.GetUser(UserId);

                if (user != null)
                {
                    var res = new Response<int>()
                    {
                        IsError = false,
                        ErrorMessage = "",
                        Data = user.AccountBalance,
                    };
                    return Ok(res);
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

