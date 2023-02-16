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

namespace WebShop.Reguests
{
    [ApiController]
    [Route("Reguests")]
    public class GetProduct : ControllerBase
    {
        private readonly ShopContext _context;

        public GetProduct(ShopContext context) => _context = context;

        private Guid UserId => Guid.Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        [HttpGet("GetOneProduct")]
        public IActionResult GetOneProduct([FromQuery] GetProductModel model)
        {
            _context.products.Load();
            _context.characteristics.Load();
            _context.categories.Load();
            _context.productImages.Load();

            var product = _context.products.Where(x=> x.ProductId == model.ProductId).Include(x => x.Characteristics).Include(x=> x.Category).Include(x=> x.Images).ToList().FirstOrDefault();

            if (product != null)
            {
                var images = new List<ImageDTO>();

                foreach(var item in product.Images)
                {
                    images.Add(new ImageDTO
                    {
                        Image = item.ImageLink,
                        ThumbImage = item.ImageLink,
                    });
                }

                var productDTO = new ProductDTO
                {
                    ProductId = product.ProductId,
                    Price = product.Price,
                    CategoryName = product.Category.Name,
                    CategoryId = product.CategorytId,
                    ProductName = product.Name,
                    Available = product.Available,
                    Discount = product.Discount,
                    Description = product.Description,
                    Img = product.Img,
                    Images = images,
                    Rating = product.Rating,
                    Characteristics = product.Characteristics,
                };

                return Ok(productDTO);
            }
            else  return NotFound();

        }
    }


    public class GetProductModel
    {
        public Guid ProductId { get; set; }
    }
}

