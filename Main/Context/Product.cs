using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using WebShop.Main.Context;

namespace WebShop.Main.Conext
{
    public class Product
    {
        public Guid ProductId { get; set; }

        public int Price { get; set; }

        public Guid CategorytId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Available { get; set; }

        public int Discount { get; set; }

        public Category Category { get; set; }

        public string Img { get; set; }

        public DiscountType DiscountType { get; set; }

        [JsonIgnore]
        public ICollection<CartItems> CartItems { get; set; }
    }
    public enum DiscountType
    {
        None,
        Percent,
        Money
    }
}