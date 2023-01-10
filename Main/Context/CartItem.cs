using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using WebShop.Main.Context;

namespace WebShop.Main.Conext
{
    public class CartItems
    {
        public Guid ProductId { get; set; }

        public int Count { get; set; }

        public Guid UserId { get; set; }

        public int Price { get; set; }

        public User User { get; set; }

        public Product Product { get; set; }

    }
}