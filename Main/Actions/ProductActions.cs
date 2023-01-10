using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using WebShop.Main.Conext;
using WebShop.Main.DBContext;
using WebShop.Main.Interfaces;

namespace Shop.Main.Actions
{

    [ApiController]
    [Route("[controller]")]
    public class ProductAction : ControllerBase
    {
        private ShopContext _context;

        public ProductAction(ShopContext context)
        {
            _context = context;
        }

        [HttpGet(Name = "AddProductr")]
        public IActionResult AddProduct(string _name, int _available, int _price, string _description, Guid _categoryId, Guid _userId)
        {
            var user = _context.users.FirstOrDefault(x => x.UserId == _userId);

            if (user.Online == true && user.Role == UserRole.Admin && _context.categories.Any(x => x.CatId == _categoryId))
            {
                _context.products.Add(new Product()
                {
                    Name = _name,
                    Available = _available,
                    Price = _price,
                    CategorytId = _categoryId,
                    ProductId = Guid.NewGuid(),
                    Description = _description,
                });
                _context.SaveChanges();
                return Ok($"Product '{_name}' seccusful added by {user.Name}");
            }
            else
                return Unauthorized($"Error! {user.Name}, You cann't do it!");
        }

        [HttpGet("ShowProducts")]
        public IActionResult ShowProducts(Guid _userId, Guid _productId)
        {
            var user = _context.users.FirstOrDefault(x => x.UserId == _userId);

            if (user.Online == true && _context.products.Any(x => x.ProductId == _productId))
            {
                var product = _context.products.FirstOrDefault(x => x.ProductId == _productId);

                return Ok($"Name: {product.Name} \n Category id of this product: {product.CategorytId} \n Price: {product.Price} \n Now available: {product.Available} pcs. \n Product id: {product.ProductId} \n Descriptions: {product.Description}");
            }
            else
                return Unauthorized($"Error! {user.Name}, You cann't do it!");
        }

    }
}




