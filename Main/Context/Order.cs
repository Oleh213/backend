using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Text.Json.Serialization;
using WebShop.Main.Context;
using WebShop.Models;

namespace WebShop.Main.Conext
{
    public class Order 
    {
        public Guid OrderId { get; set; }

        public Guid UserId { get; set; }

        public int TotalPrice { get; set; }

        public Info Info { get; set; }

        public string? UsedPromocode { get; set; }

        public ICollection<OrderList> OrderLists { get; set; }
    }

    public enum OrderStatus
    {
        AwaitingConfirm,
        AwaitingShipping,
        Shipped,
        Delivered,
        Completed,
        Declined,
        Refunded,
        Canceled,
    }


    public class Info
    {
        public Guid InfoId { get; set; }

        public Guid OrderId { get; set; }

        public string Country { get; set; }

        public string Region { get; set; }

        public string City { get; set; }

        public string Address { get; set; }

        public string? Address2 { get; set; }

        public string ZipCode { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        [JsonIgnore]
        public Order Order { get; set; }
    }
}





