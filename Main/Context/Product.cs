using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebShop.Main.Context;

namespace WebShop.Main.Conext
{
    public class Product
    {
        [Key]
        public Guid? ProductId { get; set; }

        public int Price { get; set; }

        public Guid? CategorytId { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public int? Available { get; set; }

        public Category Category { get; set; }
    }
}