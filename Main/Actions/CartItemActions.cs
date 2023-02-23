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
    private readonly ShopContext _context;

    private ICartItemActionsBL _cartItemActionsBL;

    public CartItemActions(ShopContext context, ICartItemActionsBL cartItemActionsBL)
    {
        _context = context;
        _cartItemActionsBL = cartItemActionsBL;
    }

    private Guid UserId => Guid.Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

    [HttpPost("AddToCart")]
    [Authorize]
    public async Task<IActionResult> AddToCart([FromBody] CartItemModel model)
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
            return BadRequest(resError2);
        }
    
    }
    [HttpGet("ShowCart")]
    [Authorize]
    public async Task<IActionResult> ShowCart()
    {
        var cartOfUser = await _cartItemActionsBL.GetCart(UserId);

        if (cartOfUser != null)
        {
            var newCartOfUsers =  _cartItemActionsBL.CartItemsDTO(cartOfUser);

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

    [HttpDelete("DelItemlFromCart")]
    [Authorize]
    public async Task<IActionResult> DelItemlFromCart([FromQuery] CartItemForDell model)
    {

        var cartOfUser = await _cartItemActionsBL.GetCartItem(model.ProductId, UserId);

        if (cartOfUser != null)
        {
            await _cartItemActionsBL.DellCartItem(cartOfUser);

            return Ok();
        }
        else
        {
            return NotFound();
        }
    }

    [HttpPost("ChangeCountOfItemInCart")]
    [Authorize]
    public async Task<IActionResult> ChangeCountOfItemInCart([FromBody] GetCountModel model)
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

                return NotFound(resError);
            }
        }
        else
            return NotFound();
    }

    [HttpGet("GetCartItemCount")]
    [Authorize]
    public async Task<int> GetCartItemCount()
    {
        var cartOfUser = await _cartItemActionsBL.GetCart(UserId);

        if (cartOfUser != null)
        {
            return cartOfUser.Count();
        }
        else
        {
            return 0;
        }
    }
}

