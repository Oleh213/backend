using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using WebShop.Main.Conext;

namespace WebShop.Main.Context
{
	public class OrderList
	{
        public Guid OrderListId { get; set; }

        public Guid OrderId { get; set; }

        public Guid ProductId { get; set; }

        public string Name { get; set; }

        public string Img { get; set; }

        public int Price { get; set; }

        public int Count { get; set; }

        public Product Product { get; set; }

        [JsonIgnore]
        public Order Order { get; set; }

    }
}

