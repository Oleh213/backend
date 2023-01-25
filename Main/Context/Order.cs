using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Text.Json.Serialization;
using WebShop.Main.Context;

namespace WebShop.Main.Conext
{
    public class Order 
    {
        [Key]
        public Guid OrderId { get; set; }

        public Guid UserId { get; set; }

        public int TotalPrice { get; set; }

        public string DeliveryOptions {get;set;}

        public string? UsedPromocode { get; set; }

        public DateTime OrderTime { get; set; }

        public ICollection<OrderList> OrderLists { get; set; }
    }
}





