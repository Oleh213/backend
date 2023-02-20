using System;
using System.ComponentModel.DataAnnotations;

namespace WebShop.Models
{
    public class ProductModel
    {
        public string Name { get; set; }

        public int Available { get; set; }

        public int Price { get; set; }

        public string Description { get; set; }

        public Guid CategoryId { get; set; }

        public string Img { get; set; }
    }

    public class UpdateProductModel
    {
        public Guid ProductId { get; set; }

        public string Name { get; set; }

        public int Available { get; set; }

        public int Price { get; set; }

        public string Description { get; set; }

        public Guid CategoryId { get; set; }

        public string Img { get; set; }
    }
}

