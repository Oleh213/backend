using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Security.Claims;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using WebShop.Main.Conext;
using WebShop.Main.DTO;
using WebShop.Main.DBContext;
using WebShop.Models;
using Microsoft.AspNetCore.Authorization;

namespace Shop.Main.Actions
{

    [ApiController]
    [Route("[controller]")]
    public class ProductAction : ControllerBase
    {
        private ShopContext _context;

        private Guid UserId => Guid.Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public ProductAction(ShopContext context )
        {
            _context = context;
        }

        [HttpPost(Name = "AddProductr")]
        [Authorize]
        public IActionResult AddProduct([FromBody] ProductModel model)
        {
            _context.users.Load();
            _context.products.Load();
            
            if (!_context.orders.Any(x => x.UserId == UserId)) return Unauthorized();
            {
                var user = _context.users.FirstOrDefault(x => x.UserId == UserId);

                if (user.Role == UserRole.Admin && _context.categories.Any(x => x.CatId == model.CategoryId))
                {

                    _context.products.Add(new Product()
                    {
                        Name = model.Name,
                        Available = model.Available,
                        Price = model.Price,
                        ProductId = Guid.NewGuid(),
                        Description = model.Description,
                        CategorytId = model.CategoryId,
                        Img = model.Img,
                    });
                    _context.SaveChanges();

                    return Ok();
                }
                else
                {
                    var resEr = new Response<string>()
                    {
                        IsError = true,
                        ErrorMessage = "401",
                        Data = $"* Error *"
                    };

                    return Unauthorized(resEr);
                }    
            }
        }

        [HttpGet("ShowProducts")]
        public IActionResult ShowProducts()
        {
            _context.products.Load();
            _context.categories.Load();
            _context.cartItems.Load();

            var productDPOs = new  List<ProductDTO>();

            foreach (var item in _context.products)
            {
                productDPOs.Add(new ProductDTO
                {
                    ProductId = item.ProductId,
                    Price = item.Price,
                    CategoryName = item.Category.Name,
                    CategoryId = item.CategorytId,
                    ProductName = item.Name,
                    Available = item.Available,
                    Discount = item.Discount,
                    Description = item.Description,
                    Img = item.Img
                });
            }
            return Ok(productDPOs);
        }
    }
}