using System;
using WebShop.Main.Conext;

namespace WebShop.Main.Context
{
	public class ChangeOrderStatusModel
	{
		public Guid OrderId { get; set; }

		public OrderStatus orderStatus  { get; set; }
	}
}

