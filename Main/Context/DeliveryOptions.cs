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

        public string? City {get;set;}

        public string? Street {get;set;}

        public string? House {get;set;}

        public string? ZipCode {get;set;}

        public User User { get; set; }
    }
}

