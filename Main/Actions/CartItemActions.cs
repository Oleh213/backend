using Microsoft.AspNetCore.Mvc;
using WebShop.Main.DBContext;
using WebShop.Main.Conext;
using System.ComponentModel.DataAnnotations;
using WebShop.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using WebShop.Main.DTO;
using WebShop.Main.Actions;
using WebShop.Main.Interfaces;
using WebShop.Main.BusinessLogic;
using Microsoft.EntityFrameworkCore;

namespace WebShop.Controllers;

[ApiController]
[Route("[controller]")]
public class CartItemActions : ControllerBase
{

    private ICartItemActionsBL _cartItemActionsBL;

    private readonly ILoggerBL _loggerBL;


    public CartItemActions(ICartItemActionsBL cartItemActionsBL, ILoggerBL loggerBL)
    {
        _cartItemActionsBL = cartItemActionsBL;
        _loggerBL = loggerBL;

    }

    private Guid UserId => Guid.Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

    [HttpPost("AddToCart")]
    [Authorize]
    public async Task<IActionResult> AddToCart([FromBody] CartItemModel model)
    {
        try
        {
            if (!await _cartItemActionsBL.CheckIfCartInProduct(model.ProductId, UserId))
            {
                var product = await _cartItemActionsBL.GetProduct(model.ProductId);

                if (product != null)
                {
                    if (product.Available >= model.Count)
                    {
                        await _cartItemActionsBL.AddProductToCart(model.ProductId, model.Count, UserId);

                        var res = new Response<string>()
                        {
                            IsError = false,
                            ErrorMessage = "",
                            Data = "The item successful added to cart!"
                        };

                        _loggerBL.AddLog(Main.Context.LoggerLevel.Info, $"UserId:'{UserId}' add Product:'{product.ProductId}' to cart!");
                        return Ok(res);
                    }
                    else
                    {
                        var resError1 = new Response<string>()
                        {
                            IsError = true,
                            ErrorMessage = "Item is unavailabel ",
                            Data = "Sorry, the item is  unavailable."
                        };

                        _loggerBL.AddLog(Main.Context.LoggerLevel.Warn, $"UserId:'{UserId}' can't add Product:'{product.ProductId}' to cart!(count of product is unavailable)");
                        return BadRequest(resError1);
                    }
                }
                else
                    return NotFound();
            }
            else
            {
                var resError2 = new Response<string>()
                {
                    IsError = true,
                    ErrorMessage = "Item in cart ",
                    Data = "The item is already in your cart!"
                };

                _loggerBL.AddLog(Main.Context.LoggerLevel.Warn, $"UserId:'{UserId}' can't add product to cart!(product already in cart)");
                return BadRequest(resError2);
            }
        }
        catch(Exception ex)
        {
            _loggerBL.AddLog(Main.Context.LoggerLevel.Error, ex.Message);

            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }


    [HttpGet("ShowCart")]
    [Authorize]
    public async Task<IActionResult> ShowCart()
    {
        try
        {
            var cartOfUser = await _cartItemActionsBL.GetCart(UserId);

            if (cartOfUser != null)
            {
                var newCartOfUsers = _cartItemActionsBL.CartItemsDTO(cartOfUser);

                _loggerBL.AddLog(Main.Context.LoggerLevel.Info, $"UserId:'{UserId}' reguest to show cart!");

                return Ok(newCartOfUsers);
            }
            else
            {
                var resEr = new Response<string>()
                {
                    IsError = true,
                    ErrorMessage = "401",
                    Data = $"* The cart is empy *"
                };
                return NotFound(resEr);
            }
        }
        catch(Exception ex)
        {
            _loggerBL.AddLog(Main.Context.LoggerLevel.Error, ex.Message);

            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpDelete("DelItemlFromCart")]
    [Authorize]
    public async Task<IActionResult> DelItemlFromCart([FromQuery] CartItemForDell model)
    {
        try
        {
            var itemOfCart = await _cartItemActionsBL.GetCartItem(model.ProductId, UserId);

            if (itemOfCart != null)
            {
                await _cartItemActionsBL.DellCartItem(itemOfCart);

                _loggerBL.AddLog(Main.Context.LoggerLevel.Info, $"UserId:'{UserId}' delet Product:'{model.ProductId}' from cart");

                return Ok();
            }
            else
            {
                _loggerBL.AddLog(Main.Context.LoggerLevel.Warn, $"UserId:'{UserId}' wanted delet Product:'{model.ProductId}' from cart!(Product don't found in cart)");

                return NotFound();
            }
        }
        catch(Exception ex)
        {
            _loggerBL.AddLog(Main.Context.LoggerLevel.Error, ex.Message);

            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpPost("ChangeCountOfItemInCart")]
    [Authorize]
    public async Task<IActionResult> ChangeCountOfItemInCart([FromBody] GetCountModel model)
    {
        try
        {
            var count = await _cartItemActionsBL.GetCartItem(model.ProductId, UserId);

            if (count != null)
            {
                var product = await _cartItemActionsBL.GetProduct(model.ProductId);

                if (product.Available >= model.Count)
                {
                    var res = new Response<string>()
                    {
                        IsError = false,
                        ErrorMessage = "",
                        Data = "Your information successful update"
                    };
                    await _cartItemActionsBL.ChangeCoutOfCartItem(count, model.Count);

                    _loggerBL.AddLog(Main.Context.LoggerLevel.Info, $"UserId:'{UserId}' canged count of Product:'{model.ProductId}' in cart");
                    return Ok(res);
                }
                else
                {
                    var resError = new Response<string>()
                    {
                        IsError = true,
                        ErrorMessage = "CountUnAvailable",
                        Data = "Sorry, we don't have available count of this product"
                    };

                    _loggerBL.AddLog(Main.Context.LoggerLevel.Warn, $"UserId:'{UserId}' wanted change count of Product:'{model.ProductId}' in cart!(Don't available count of this product)");

                    return NotFound(resError);
                }
            }
            else
                return NotFound();
        }
        catch(Exception ex)
        {
            _loggerBL.AddLog(Main.Context.LoggerLevel.Error, ex.Message);

            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpGet("GetCartItemCount")]
    [Authorize]
    public async Task<IActionResult> GetCartItemCount()
    {
        try
        {
            var cartOfUser = await _cartItemActionsBL.GetCart(UserId);

            if (cartOfUser != null)
            {
                var resOk = new Response<int>()
                {
                    IsError = false,
                    ErrorMessage = "",
                    Data = cartOfUser.Count(),
                };

                //_loggerBL.AddLog(Main.Context.LoggerLevel.Info, $"UserId:'{UserId}' get count of products in cart");
                return Ok(resOk);
            }
            else
            {
                var resOk = new Response<int>()
                {
                    IsError = false,
                    ErrorMessage = "",
                    Data = 0,
                };

                //_loggerBL.AddLog(Main.Context.LoggerLevel.Info, $"UserId:'{UserId}' get count of products in cart");
                return Ok(resOk);
            }

        }
        catch (Exception ex)
        {
            _loggerBL.AddLog(Main.Context.LoggerLevel.Error, ex.Message);

            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}

