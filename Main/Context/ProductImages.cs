using System;
using WebShop.Main.Conext;

namespace WebShop.Main.Context
{
	public class ProductImages
	{
		public Guid ImageId { get; set;}

		public string ImageLink { get; set; }

		public ICollection<Product> Products { get; set; }

	}
}

