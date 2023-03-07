using System;
using WebShop.Main.Conext;

namespace WebShop.Main.Context
{
	public class Message
	{
		public Guid MessageId { get; set; }

		public Guid UserId { get; set; }

		public Guid ProductId { get; set; }

		public string MessageText { get; set; }

		public DateTime DateTime { get; set; }

		public Product Product { get; set; }

		public User User { get; set; }
	}
}

