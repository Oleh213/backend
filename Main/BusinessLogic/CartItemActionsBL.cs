using System;
using Microsoft.EntityFrameworkCore;
using WebShop.Main.Conext;
using WebShop.Main.DBContext;
using WebShop.Main.DTO;
using WebShop.Main.Interfaces;

namespace WebShop.Main.BusinessLogic
{
    public class CartItemActionsBL : ICartItemActionsBL
    {
        private readonly ShopContext _context;

        public CartItemActionsBL(ShopContext context)
        {
            _context = context;
        }

        public async Task<bool> CheckIfCartInProduct(Guid productId, Guid userId)
        {
            return await _context.cartItems.AnyAsync(x => x.ProductId == productId && x.UserId == userId);
        }

        public async Task<Product> GetProduct(Guid productId)
        {
            return await _context.products.FirstOrDefaultAsync(x => x.ProductId == productId);
        }

        public async Task<string> AddProductToCart(Guid productId, int count, Guid userId)
        {
            _context.cartItems.Add(new CartItems
            {
                ProductId = productId,
                Count = count,
                UserId = userId,
            });

            await _context.SaveChangesAsync();

            return "Ok";
        }

        public async Task<CartItems> GetCartItem(Guid productId, Guid userId)
        {
            _context.cartItems.Load();
            _context.products.Load();

            return await _context.cartItems.FirstOrDefaultAsync(x => x.ProductId == productId && x.UserId == userId);
        }

        public async Task<string> DellCartItem(CartItems cartItem)
        {
            _context.cartItems.Remove(cartItem);

            await _context.SaveChangesAsync();
            return "Ok";
        }

        public async Task<List<CartItems>> GetCart(Guid userId)
        {

            _context.cartItems.Load();
            _context.products.Load();

            return await _context.cartItems.Where(x => x.UserId == userId).ToListAsync();
        }

        public List<CartItemDTO> CartItemsDTO(List<CartItems> cartOfUser)
        {
            if (cartOfUser != null)
            {
                var newCartOfUsers = new List<CartItemDTO>();
                foreach (var item in cartOfUser)
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
                return (newCartOfUsers);
            }
            else
                return null;
        }

        public async Task<string> ChangeCoutOfCartItem(CartItems cartItem , int count)
        {
            cartItem.Count = count;

            await _context.SaveChangesAsync();
            return "Ok";
        }

    }
}

