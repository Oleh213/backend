using System;
using System.ComponentModel.DataAnnotations;
using WebShop.Main.Conext;

namespace WebShop.Main.Context
{
	public class OrderList
	{
        public Guid OrderListId { get; set; }

        public Guid OrderId { get; set; }

        public Guid ProductId { get; set; }

        public Product Product { get; set; }

        public Order Order { get; set; }
    }
}

