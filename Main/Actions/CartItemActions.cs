using Microsoft.AspNetCore.Mvc;
using WebShop.Main.DBContext;
using WebShop.Main.Conext;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace WebShop.Controllers;

[ApiController]
[Route("[controller]")]
public class CartItem : ControllerBase
{
    private readonly ShopContext _context;

    public CartItem(ShopContext context) => _context = context;

    [HttpGet(Name = "CartItem")]
    public void Get(int _count, Guid _productId, Guid _userId)
    {
        var user = _context.users.Include(x => x.CartItems).FirstOrDefault(x => x.UserId == _userId);

        if (user != null)
        {
            if (user.Online && _context.products.Any(x => x.ProductId == _productId))
            {
                var prod = _context.products.FirstOrDefault(x => x.ProductId == _productId);

                if (prod.Available >= _count)
                {
                    int itemPrise = _count * prod.Price;

                    _context.cartItems.Add(new CartItems
                    {
                        ProductId = _productId,
                        Count = _count,
                        Price = itemPrise,
                        UserId = _userId
                    });

                    foreach (var i in _context.products)
                    {
                        if (i.ProductId == _productId)
                        {
                            i.Available -= _count;
                        }
                    }

                    _context.SaveChanges();
                }
            }
        }
    }

    [HttpGet("Show")]
    public void Gets(Guid _userId)
    {

        var user = _context.users.FirstOrDefault(x => x.UserId == _userId);

        _context.users.Load();
        _context.cartItems.Load();
        var cartOfUsers = _context.users.Where(x => x.UserId == _userId).Include(u => u.CartItems).FirstOrDefault();

        Console.WriteLine();
    }
}

