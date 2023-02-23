using System;
using System.ComponentModel.DataAnnotations;
using WebShop.Main.Conext;

namespace WebShop.Main.Context
{
    public class Coments
    {
        [Key]
        public Guid ComentId { get; set; }

        public Guid UserId { get; set; }

        public string Body { get; set; }

        public Guid? ParentId { get; set; }

        public Guid ProductId { get; set; }

        public byte? Rating { get; set; }

        public DateTime CreatedAt { get; set; }

        public User User { get; set; }
    }
}

