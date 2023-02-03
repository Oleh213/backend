using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebShop.Main.DBContext;
using WebShop.Models;

namespace WebShop.Reguests
{
    [ApiController]
    [Route("[controller]")]
    public class GetAccountBalance : ControllerBase
    {
        private ShopContext _context;

        public GetAccountBalance(ShopContext context)
        {
            _context = context;
        }

        private Guid UserId => Guid.Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        [HttpGet("AccountBalance")]
        [Authorize]
        public IActionResult AccountBalance()
        {

            var user = _context.users.FirstOrDefault(x => x.UserId == UserId);

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

