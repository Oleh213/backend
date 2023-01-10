using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using WebShop.Main.Context;

namespace WebShop.Main.Conext
{
    public class Order 
    {
        [Key]
        public Guid? OrderId { get; set; }

        public Guid? UserId { get; set; }

        public int? TotalPrice { get; set; }

        public string DeliveryOptions {get;set;}

        public ICollection<OrderList> OrderLists { get; set; }
    }
}





