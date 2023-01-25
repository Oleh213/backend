using System;
namespace WebShop.Models
{
	public class OrderModel
	{
		public List<Item> Cart { get; set; } 

		public int TotalPrice { get; set; }
	}

	public class Item
	{
		public int Count { get; set; }

		public Guid ProductId { get; set; }
	}
}

