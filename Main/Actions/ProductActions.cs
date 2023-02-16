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
using WebShop.Main.DTO;

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
        public IActionResult AddProduct([FromBody] ProductModel model)
        {
            _context.users.Load();
            _context.products.Load();
            _context.characteristics.Load();
            
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
            _context.characteristics.Load();
            _context.cartItems.Load();

            var productDPOs = new List<ProductDTO>();

            foreach (var item in _context.products.Include(x => x.Characteristics).ToList())
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
                    Img = item.Img,
                    Characteristics= item.Characteristics,                   
                });
            }
            return Ok(productDPOs);
        }
    }
}