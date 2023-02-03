using System;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebShop.Main.Conext;
using WebShop.Main.DBContext;
using WebShop.Models;

namespace WebShop.Main.Actions
{
    [ApiController]
    [Route("AddMoneyToBalance")]
    public class AddMoneyToBalance : ControllerBase
    {
        private ShopContext _context;
        public AddMoneyToBalance(ShopContext context)
        {
            _context = context;
        }
        private Guid UserId => Guid.Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);


        [HttpPost(Name = "AddMoney")]
        public IActionResult AddMoney([FromBody] TopUpModel model)
        {
            var card = _context.cards.FirstOrDefault(number => number.CardNumber == model.CardNumber);

            if (card != null)
            {
                if (card.CardNumber == model.CardNumber && card.Cvv == model.Cvv && card.ExpiredDate == model.ExpiredDate)
                {
                    if (card.Balance >= model.RequestedAmount)
                    {
                        var user = _context.users.FirstOrDefault(id => id.UserId == UserId);

                        user.AccountBalance += model.RequestedAmount;

                        card.Balance -= model.RequestedAmount;

                        _context.SaveChanges();

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

    }
}

