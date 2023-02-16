using System;
using System.ComponentModel.DataAnnotations;

namespace WebShop.Models
{
	public class ComentsModel
	{
        public Guid ComentId { get; set; }

        public string Body { get; set; }

        public Guid? ParentId { get; set; }

        public Guid UserId { get; set; }

        public Guid ProductId { get; set; }

        public byte? Rating { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}

