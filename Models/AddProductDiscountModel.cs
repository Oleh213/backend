using System;
namespace WebShop.Models
{
	public class AddProductDiscountModel
	{
        public Guid ProductId { get; set; }

        public int Discount { get; set; }

        public int DiscountType { get; set; }

    }
}

