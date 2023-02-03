using System;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebShop.Main.Conext;
using WebShop.Main.DBContext;
using WebShop.Models;
using Microsoft.AspNetCore.Authorization;
using WebShop.Main.Context;

namespace WebShop.Main.Actions
{
    [ApiController]
    [Route("AddDeliveryInfo")]
    public class AddDeliveryInfo : ControllerBase
    {
        private ShopContext _context;
        public AddDeliveryInfo(ShopContext context)
        {
            _context = context;
        }
        private Guid UserId => Guid.Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);


        [HttpPost(Name = "AddInfo")]
        public IActionResult AddInfo([FromBody] DeliveryOptionsModel model)
        {
            var user = _context.deliveryOptions.FirstOrDefault(user => user.UserId == UserId);

            if (user != null)
            {
                user.Country = model.Country;
                user.Region = model.Region;
                user.City = model.City;
                user.Address = model.Address;
                user.Address2 = model.Address2;
                user.ZipCode = model.ZipCode;

                _context.SaveChanges();

                var res = new Response<string>()
                {
                    IsError = false,
                    ErrorMessage = "",
                    Data = "Your deliery information successful update"
                };

                return Ok(res);
            }
            else
                return Unauthorized();
        }

    }
}

