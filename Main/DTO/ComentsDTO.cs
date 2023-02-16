using System;
namespace WebShop.Main.DTO
{
	public class ComentsDTO
	{
        public Guid ComentId { get; set; }

        public string Body { get; set; }

        public Guid? ParentId { get; set; }

        public Guid UserId { get; set; }

        public Guid ProductId { get; set; }

        public string Username { get; set; }

        public byte? Rating { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}

