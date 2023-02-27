using System;
namespace WebShop.Models
{
	public class OrderModel
	{
		public Card? Card { get; set; } 

		public string? Promocode { get; set; }

		public DeliveryOptionsModel DeliveryOptions { get; set; }

		public ContactInfo contactInfo { get; set; }
	}

	public class Card
	{
        public string CardNumber { get; set; }

        public string ExpiredDate { get; set; }

        public string Cvv { get; set; }
    }
	public class ContactInfo
	{
		public string Name { get; set; }

		public string LastName { get; set; }

		public string Email { get; set; }

		public string PhoneNumber { get; set; }
	}
}

