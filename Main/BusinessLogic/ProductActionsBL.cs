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
            return await _context.categories.AnyAsync(x => x.CategoryId == categoryId);
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
                CategoryId = model.CategoryId,
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
            product.CategoryId = model.CategoryId;
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
                    CategoryId = item.CategoryId,
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
                CategoryId = product.CategoryId,
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
                    CategoryId = prod.CategoryId,
                    ProductName = prod.Name,
                    Available = prod.Available,
                    Discount = prod.Discount,
                    Description = prod.Description,
                    Img = prod.Img
                });
            }
            return productDPOs;

        }

        public async Task<Characteristics> CheckCharacteristic(string name, string value)
        {
            return await _context.characteristics.FirstOrDefaultAsync(x => x.CharacteristicName == name && x.CharacteristicValue == value);
        }

        public async Task<string> AddCharacteristicToProduct(Characteristics characteristics, Guid productId)
        {
            var product = await GetOneProductWithAll(productId);

            if(product!=null)
            {
                if (product.Characteristics.Any(x => x.CharacteristicName == characteristics.CharacteristicName))
                {
                    var characteristic = product.Characteristics.FirstOrDefault(x => x.CharacteristicName == characteristics.CharacteristicName);

                    product.Characteristics.Remove(characteristic);
                    product.Characteristics.Add(characteristics);
                }
                else
                {
                    product.Characteristics.Add(characteristics);
                }
                await _context.SaveChangesAsync();
            }
            return "Ok";
        }

        public Dictionary<string, List<Characteristics>> GetChatacteristics(Product product)
        {
            return product.Characteristics.GroupBy(x => x.CharacteristicName).ToDictionary(g => g.Key, g => g.ToList());
        }

        public Dictionary<string, List<string>> FirlterDTO(Dictionary<string, List<Characteristics>> chatacteristics)
        {
            Dictionary<string, List<string>> characteristicsDTO = new Dictionary<string, List<string>>();

            foreach (var item in chatacteristics)
            {
                List<string> list = new List<string>();

                item.Value.ToList().ForEach(x => { list.Add(x.CharacteristicValue); });

                characteristicsDTO.Add(item.Key, list);
            }
            return characteristicsDTO;
        }

        public async Task<string> AddNewCharacteristic(string name, string value)
        {
            _context.characteristics.Add(new Characteristics { CharacteristicName = name,CharacteristicValue = value});

            await _context.SaveChangesAsync();

            return "Ok";
        }
    }
}

