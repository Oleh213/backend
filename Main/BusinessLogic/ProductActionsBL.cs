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


namespace WebShop.Main.BusinessLogic
{
	public class ProductActionsBL : IProductActionsBL
    {
        private ShopContext _context;

        public ProductActionsBL(ShopContext context)
        {
            _context = context;
        }

        public async Task<User> GetUser(Guid userId)
        {
            return await _context.users.FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public async Task<Product> GetProduct(Guid productId)
        {
            return await _context.products.FirstOrDefaultAsync(x => x.ProductId == productId);
        }

        public async Task<bool> CheckCategory(Guid categoryId)
        {
            return await _context.categories.AnyAsync(x => x.CatId == categoryId);
        }

        public async Task<string> AddProduct(ProductModel model)
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

            await _context.SaveChangesAsync();

            return "Ok";
        }

        public async Task<string> UpdateProduct(UpdateProductModel model, Product product)
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

            await _context.SaveChangesAsync();

            return "Ok";
        }

        public List<ProductDTO> AllProductsDTO()
        {
            _context.products.Load();
            _context.categories.Load();
            _context.characteristics.Load();


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
                    Characteristics = item.Characteristics,
                });
            }
            return productDPOs;
        }

        public async Task<Product> GetOneProductWithAll(Guid productId)
        {
            return  await _context.products.
                Where(x => x.ProductId == productId).
                Include(x => x.Characteristics).
                Include(x => x.Category).
                Include(x => x.Images).
                FirstOrDefaultAsync();
        }

        public ProductDTO OneProductsDTO(Product product)
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
                Rating = product.Rating,
                Characteristics = product.Characteristics,
            };

            return productDTO;
        }

        public List<ProductDTO> GetFavouriteProducts()
        {
            _context.products.Load();
            _context.categories.Load();
            _context.cartItems.Load();

            var products = _context.orderLists.GroupBy(ol => ol.ProductId);

            var productDPOs = new List<ProductDTO>();

            Dictionary<Guid, int> topProducts = new Dictionary<Guid, int>();

            foreach (var item in products)
            {
                int count = 0;

                item.ToList().ForEach(x =>
                {
                    count++;
                });
                topProducts.Add(item.Key, count);
            }

            var list = topProducts.OrderByDescending(c => c.Value).Take(6);
            foreach (var item in list)
            {
                var prod = _context.products.FirstOrDefault(x => x.ProductId == item.Key);

                productDPOs.Add(new ProductDTO
                {
                    ProductId = prod.ProductId,
                    Price = prod.Price,
                    CategoryName = prod.Category.Name,
                    CategoryId = prod.CategorytId,
                    ProductName = prod.Name,
                    Available = prod.Available,
                    Discount = prod.Discount,
                    Description = prod.Description,
                    Img = prod.Img
                });
            }
            return productDPOs;

        }


    }
}

