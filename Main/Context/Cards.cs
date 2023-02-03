using System;
namespace WebShop.Main.Context
{
	public class Cards
	{
		public Guid CardId { get; set; }

		public string CardNumber { get; set; }

		public string ExpiredDate { get; set; }

		public string Cvv { get; set; }

		public int Balance { get; set; }
	}
}