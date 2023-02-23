using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebShop.Main.Conext
{
    public class Category
    {
        [Key]
        public Guid CategoryId { get; set; }

        public string Name { get; set; }

        [JsonIgnore]
        public ICollection<Product> Products { get; set; }
    }
}



