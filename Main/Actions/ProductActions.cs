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
using WebShop.Main.BusinessLogic;

namespace Shop.Main.Actions
{

    [ApiController]
    [Route("[controller]")]
    public class ProductActions : ControllerBase
    {
        private ShopContext _context;

        private IProductActionsBL _productActionsBL;

        private Guid UserId => Guid.Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public ProductActions(ShopContext context, IProductActionsBL productActionsBL)
        {
            _context = context;
            _productActionsBL = productActionsBL;
        }

        [HttpPost("AddProduct")]
        [Authorize]
        public async Task<IActionResult> AddProduct([FromBody] ProductModel model)
        {
            var user = await _productActionsBL.GetUser(UserId);

            if (user!=null)
            {
                if (user.Role == UserRole.Admin)
                {
                    if(await _productActionsBL.CheckCategory(model.CategoryId))
                    {
                        await _productActionsBL.AddProduct(model);

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
            return Unauthorized();
        }

        [HttpPatch("UpdateProduct")]
        [Authorize]
        public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductModel model)
        {
            var user = await _productActionsBL.GetUser(UserId);

            if (user != null)
            {
                if (user.Role == UserRole.Admin)
                {
                    if(await _productActionsBL.CheckCategory(model.CategoryId))
                    {
                        var product = await _productActionsBL.GetProduct(model.ProductId);

                        if (product != null)
                        {
                            await _productActionsBL.UpdateProduct(model, product);

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
            else return Unauthorized();
    }

        [HttpGet("ShowProducts")]
        public IActionResult ShowProducts()
        {

            var productDPOs = _productActionsBL.AllProductsDTO();

            return Ok(productDPOs);
        }

        [HttpGet("GetOneProduct")]
        public async Task<IActionResult> GetOneProduct([FromQuery] GetProductModel model)
        {
            var product = await _productActionsBL.GetOneProductWithAll(model.ProductId);

            if (product != null)
            {
                var productDTO = _productActionsBL.OneProductsDTO(product);

                return Ok(productDTO);
            }
            else return NotFound();
        }


        [HttpGet("FavouriteProductReguest")]
        public IActionResult FavouriteProductReguest()
        {
            var productDPOs = _productActionsBL.GetFavouriteProducts();

            return Ok(productDPOs);
        }

        [HttpPost("AddCharacteristicsToProduct")]
        public async Task<IActionResult> AddCharacteristicsToProduct([FromBody] AddCharacteristicsToProductModel model)
        {
            var user = await _productActionsBL.GetUser(UserId);

            if (user != null)
            {
                if (user.Role == UserRole.Admin)
                {
                    var characteristic = await _productActionsBL.CheckCharacteristic(model.CharacteristicName, model.CharacteristicValue);

                    if(characteristic !=null )
                    {
                        await _productActionsBL.AddCharacteristicToProduct(characteristic, model.ProductId);

                        var resOk = new Response<string>()
                        {
                            IsError = false,
                            ErrorMessage = "401",
                            Data = $"Characteristic successfully added to this product!"
                        };
                        return Ok(resOk);
                    }
                    else
                    {
                        var resEr = new Response<string>()
                        {
                            IsError = true,
                            ErrorMessage = "401",
                            Data = $"* Error, Characteristic dont't found *"
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
            return Unauthorized();
        }

        [HttpGet("GetCharacteristicsOfProduct")]
        public async Task<IActionResult> GetCharacteristicsOfProduct([FromQuery] GetProductModel model)
        {
            var product = await _productActionsBL.GetOneProductWithAll(model.ProductId);

            if (product != null)
            {
                var chatacteristics = _productActionsBL.GetChatacteristics(product);

                var characteristicsDTO = _productActionsBL.FirlterDTO(chatacteristics);

                return Ok(characteristicsDTO);
            }
            else return NotFound();
        }

        [HttpPost("AddNewCharacteristic")]
        public async Task<IActionResult> AddNewCharacteristic([FromBody] AddNewCharacteristicModel model)
        {
            var user = await _productActionsBL.GetUser(UserId);

            if (user != null)
            {
                if (user.Role == UserRole.Admin)
                {
                    var characteristic = await _productActionsBL.CheckCharacteristic(model.CharacteristicName, model.CharacteristicValue);

                    if (characteristic == null)
                    {
                        await _productActionsBL.AddNewCharacteristic(model.CharacteristicName, model.CharacteristicValue);

                        var resOk = new Response<string>()
                        {
                            IsError = false,
                            ErrorMessage = "401",
                            Data = $"New characteristic successfully added!"
                        };
                        return Ok(resOk);
                    }
                    else
                    {
                        var resEr = new Response<string>()
                        {
                            IsError = true,
                            ErrorMessage = "401",
                            Data = $"* Error, change another value! *"
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
            return Unauthorized();
        }
    }
}