//using System;
//using System.Collections.Generic;
//using Shop.Main.Actions;
//using System.Xml.Linq;
//using System.Linq;
//using WebShop.Main.Conext;
//using WebShop.Main.DBContext;
//using WebShop.Main.Interfaces;
//using Microsoft.AspNetCore.Mvc;
//using System.Data.Entity;

//namespace Shop.Main.Actions
//{

//    [ApiController]
//    [Route("CartAction")]
//    public class CartAction : ControllerBase
//    {
//        private ShopContext _context;
//        public CartAction(ShopContext context)
//        {
//            _context = context;
//        }
//        [HttpGet(Name = "AddToCart")]
//        public IActionResult AddToCart(int _count, Guid _productId, Guid _userId)
//        {
//            var user = _context.users.FirstOrDefault(x => x.UserId == _userId);

//            if (user?.Online && _context.products.Any(x => x.ProductId == _productId))
//            {
//                var prod = _context.products.FirstOrDefault(x => x.ProductId == _productId);

//                var cartOfUsers = _context.users.Where(x => x.UserId == _userId).Include(u => u.CartItems).FirstOrDefault();
//                var items = user.CartItems;

//                if (prod.Available >= _count)
//                {
//                    var cart = _context.carts.FirstOrDefault(x => x.CartId == _userId);
//                    if (cart != null)
//                    {
//                        var itemPrise = _count * prod.Price;
//                        cart.TotalPrice += itemPrise;

//                        cart.CartItems.Add(new CartItems
//                        {
//                            ProductId = _productId,
//                            Count = _count,

//                        }
//                            );

//                        cart.CartItems.Add(new CartItems
//                        {
//                            ProductId = _productId,
//                            Count = _count,
//                            Price = itemPrise
//                        });

//                        foreach (var i in _context.products)
//                        {
//                            if (i.ProductId == _productId)
//                            {
//                                i.Available -= _count;
//                            }
//                        }
//                        _context.SaveChanges();
//                        return Ok("Product seccessfully added to your cart!");
//                    }
//                    else
//                        return Unauthorized($"Error! {user.Name}, You cann't do it!");
//                }
//                else
//                    return Unauthorized($"Error! {user.Name}, You cann't do it!");
//            }
//            else
//                return Unauthorized($"Error! {user.Name}, You cann't do it!");
//        }




//        //[HttpGet("ShowCart")]
//        //public IActionResult ShowCart(Guid _userId)
//        //{
//        //    var user = _context.carts.FirstOrDefault(x => x.CartId == _userId && _context.users.Any(x => x.Online));

//        //    var cartList = _context.carts.FirstOrDefault(x => x.CartId == _userId);

//        //    if (cartList != null)
//        //    {
//        //       return Ok($"User id: {cartList.CartId}, Total Price: {cartList.TotalPrice}");

//        //    }
//        //    else
//        //        return Unauthorized("Error!");
//        //}
//    }
//}
