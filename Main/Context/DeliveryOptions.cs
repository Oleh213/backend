using System;
using System.ComponentModel.DataAnnotations;
using WebShop.Main.Conext;

namespace WebShop.Main.Context
{
	public class DeliveryOptions
	{
        public Guid UserId { get; set; }

        public Guid DeliveryOptionsId { get; set; }

		public string? Country { get; set; }

        public string? Region { get; set; }

        public string? City {get;set;}

        public string? Address {get;set;}

        public string? Address2 {get;set;}

        public string? ZipCode {get;set;}

        public User User { get; set; }
    }
}

