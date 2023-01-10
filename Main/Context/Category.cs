using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebShop.Main.Conext
{
    public class Category
    {
        [Key]
        public Guid? CatId { get; set; }

        public string? Name { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}



