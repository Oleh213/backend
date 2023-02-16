using System;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebShop.Main.Conext;
using WebShop.Main.DBContext;
using WebShop.Main.DTO;
using System.Data.Entity;

namespace WebShop.Reguests
{
    [ApiController]
    [Route("Reguests")]
    public class GetProductCharacteristics : ControllerBase
    {
        private readonly ShopContext _context;

        public GetProductCharacteristics(ShopContext context) => _context = context;

        private Guid UserId => Guid.Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        [HttpGet("GetProduct")]
        public IActionResult GetOneProduct([FromQuery] GetProductModel model)
        {
            _context.products.Load();
            _context.characteristics.Load();
            _context.categories.Load();
            _context.productImages.Load();

            var product = _context.products.Where(x => x.ProductId == model.ProductId).Include(x => x.Characteristics).Include(x => x.Category).Include(x => x.Images).ToList().FirstOrDefault();

            if (product != null)
            {
                var images = new List<ImageDTO>();

                foreach (var item in product.Images)
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
                    Characteristics = product.Characteristics,
                };

                return Ok(productDTO);
            }
            else return NotFound();

        }
    }
}

