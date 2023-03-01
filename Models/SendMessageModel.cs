using System;
namespace WebShop.Models
{
    public class SendMessageModel
    {
        public string Message { get; set; }

        public Guid ProductId { get; set; }
    }
}

