using System;
using System.ComponentModel.DataAnnotations;

namespace WebShop.Models
{
    public class CartItemModel
    {
        public int Count { get; set; }

        public Guid ProductId { get; set; }
    }
}

