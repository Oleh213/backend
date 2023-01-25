using System;
using System.Text.Json.Serialization;
using WebShop.Main.Conext;

namespace WebShop.Main.DTO
{
    public class CartItemDTO
    {
        public Guid ProductId { get; set; }

        public int Count { get; set; }

        public Guid UserId { get; set; }

        public string ProductName { get; set; }

        public string Img { get; set; }

        public int Price { get; set; }

        public int Available { get; set; }

    }
}