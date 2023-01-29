using Microsoft.AspNetCore.Mvc;
using WebShop.Main.DBContext;
using WebShop.Main.Conext;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using WebShop.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using WebShop.Main.DTO;

namespace WebShop.Controllers;

[ApiController]
[Route("[controller]")]
public class CartItem : ControllerBase
{
    private readonly ShopContext _context;

    public CartItem(ShopContext context) => _context = context;

    private Guid UserId => Guid.Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

    [HttpPost(Name = "CartItem")]
    [Authorize]
    public IActionResult AddToCart([FromBody] CartItemModel model)
    {

        if (!_context.cartItems.Any(x => x.ProductId == model.ProductId && x.UserId == UserId))
        {
            var prod = _context.products.FirstOrDefault(x => x.ProductId == model.ProductId);

            if (prod.Available >= model.Count)
            {
                int itemPrise = model.Count * prod.Price;

                _context.cartItems.Add(new CartItems
                {
                    ProductId = model.ProductId,
                    Count = model.Count,
                    UserId = UserId,

                });
                _context.SaveChanges();

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
    [HttpGet("Show")]
    [Authorize]
    public IActionResult ShowCart()
    {
        _context.cartItems.Load();
        _context.products.Load();

        var cartOfUser = _context.cartItems.Where(x => x.UserId == UserId);

        if (cartOfUser != null)
        {
            var newCartOfUsers = new List<CartItemDTO>();

            foreach(var item in cartOfUser)
            {
                newCartOfUsers.Add(new CartItemDTO
                {
                    UserId = item.UserId,
                    ProductId = item.ProductId,
                    Count = item.Count,
                    ProductName = item.Product.Name,
                    Img = item.Product.Img,
                    Price = item.Product.Price,
                    Available = item.Product.Available,
                });
            }

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
    [HttpDelete("DellFromCart")]
    [Authorize]
    public IActionResult DellFromCart([FromQuery] CartItemForDell model)
    {
        _context.users.Load();
        _context.cartItems.Load();

        var cartOfUser = _context.cartItems.FirstOrDefault(x=> x.ProductId == model.ProductId && x.UserId == UserId );

        if (cartOfUser != null)
        {
            _context.cartItems.Remove(cartOfUser);

            _context.SaveChanges();

            return Ok();
        }
        else
        {
            return NotFound();
        }
    }
}

