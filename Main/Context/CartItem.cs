using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Xml.Linq;
using WebShop.Main.Context;

namespace WebShop.Main.Conext
{
    public class CartItems
    {
        public Guid CartItemsId { get; set; }

        public Guid ProductId { get; set; }

        public int Count { get; set; }

        public Guid UserId { get; set; }

        [JsonIgnore]
        public User User { get; set; }

        public Product Product { get; set; }

    }
}