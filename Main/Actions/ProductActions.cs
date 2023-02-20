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
            
            if (!_context.users.Any(x => x.UserId == UserId)) return Unauthorized();
            {
                var user = _context.users.FirstOrDefault(x => x.UserId == UserId);

                if (user.Role == UserRole.Admin)
                {
                    if(_context.categories.Any(x => x.CatId == model.CategoryId))
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

                        var resOk = new Response<string>()
                        {
                            IsError = false,
                            ErrorMessage = "401",
                            Data = $"Product successfully added!"
                        };
                        return Ok(resOk);
                    }
                    else
                    {
                        var resEr = new Response<string>()
                        {
                            IsError = true,
                            ErrorMessage = "401",
                            Data = $"* Error, category dont't found *"
                        };
                        return NotFound(resEr);
                    }
                    
                }
                else
                {
                    var resEr = new Response<string>()
                    {
                        IsError = true,
                        ErrorMessage = "401",
                        Data = $"* Error, you dont have permissions! *"
                    };
                    return Unauthorized(resEr);
                }    
            }
        }

        [HttpPatch("UpdateProduct")]
        public IActionResult UpdateProduct([FromBody] UpdateProductModel model)
        {
            _context.users.Load();
            _context.products.Load();

            if (!_context.users.Any(x => x.UserId == UserId)) return Unauthorized();
            {
                var user = _context.users.FirstOrDefault(x => x.UserId == UserId);

                if (user.Role == UserRole.Admin)
                {
                    if(_context.categories.Any(x => x.CatId.ToString() == model.CategoryId.ToString()))
                    {
                        var product = _context.products.FirstOrDefault(x => x.ProductId == model.ProductId);

                        if (product != null)
                        {
                            product.Name = model.Name;
                            product.Img = model.Img;
                            product.Available = model.Available;
                            product.CategorytId = model.CategoryId;
                            product.Description = model.Description;

                            if (product.Discount > 0)
                            {
                                product.Price = model.Price - product.Discount;
                            }
                            else
                            {
                                product.Price = model.Price;
                            }

                            _context.SaveChanges();

                            var resOk = new Response<string>()
                            {
                                IsError = false,
                                ErrorMessage = "401",
                                Data = $"Information successfully updated!"
                            };
                            return Ok(resOk);

                        }
                        else
                        {
                            var resEr = new Response<string>()
                            {
                                IsError = true,
                                ErrorMessage = "401",
                                Data = $"* Error, product dont't found *"
                            };
                            return NotFound(resEr);
                        }

                    }
                    else
                    {
                        var resEr = new Response<string>()
                        {
                            IsError = true,
                            ErrorMessage = "401",
                            Data = $"* Error, category dont't found *"
                        };
                        return NotFound(resEr);
                    }
                }
                else
                {
                    var resEr = new Response<string>()
                    {
                        IsError = true,
                        ErrorMessage = "401",
                        Data = $"* Error, you dont have permissions! *"
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