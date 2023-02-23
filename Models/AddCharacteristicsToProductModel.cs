using System;
namespace WebShop.Models
{
	public class AddCharacteristicsToProductModel
	{
		public Guid ProductId { get; set; }

		public string CharacteristicName { get; set; }

        public string CharacteristicValue { get; set; }
    }
}

