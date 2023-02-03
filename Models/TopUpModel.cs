using System;
namespace WebShop.Models
{
	public class TopUpModel
	{
		public string CardNumber { get; set; }

		public string ExpiredDate { get; set; }

		public string Cvv { get; set; }

		public int RequestedAmount { get; set; }
	}
}

