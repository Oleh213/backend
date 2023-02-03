using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebShop.Main.Conext;
using WebShop.Main.DBContext;
using WebShop.Models;
using WebShop.Main.Context;

namespace WebShop.Reguests
{

    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class GetUserDeliveryOptions : ControllerBase
    {
        private readonly ShopContext _context;

        private Guid UserId => Guid.Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public GetUserDeliveryOptions(ShopContext context) => _context = context;


        [HttpGet(Name = "GetDeliveryInfo")]
        [Authorize]
        public IActionResult GetDeliveryInfo()
        {
            var user = _context.deliveryOptions.FirstOrDefault(x => x.UserId == UserId);

            if (user != null)
            {
                if (user.Country == null)
                {
                    user.Country = "Not specified";
                }
                if (user.Region == null)
                {
                    user.Region = "Not specified";
                }
                if (user.City == null)
                {
                    user.City = "Not specified";
                }
                if (user.Address == null)
                {
                    user.Address = "Not specified";
                }
                if (user.Address2 == null)
                {
                    user.Address2 = "Not specified";
                }
                if (user.ZipCode == null)
                {
                    user.ZipCode = "Not specified";
                }

                var outUser = new DeliveryOptionsModel()
                {
                    Country = user.Country,
                    Region = user.Region,
                    City = user.City,
                    Address = user.Address,
                    Address2 = user.Address2,
                    ZipCode = user.ZipCode
                };
                return Ok(outUser);
            }
            else
                return NotFound();
        }
    }
}

