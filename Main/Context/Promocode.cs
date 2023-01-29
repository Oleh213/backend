using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebShop.Main.Conext
{
    public class Promocode
    {
        [Key]
        public Guid PromocodetId { get; set; }

        public int Discount { get; set; }
        
        public string Code { get; set; }

    }
}