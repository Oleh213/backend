using System;
namespace WebShop.Models
{
	public class Response<T>
	{
        public bool IsError { get; set; }
        public string? ErrorMessage { get; set; }

        public T? Data { get; set; }
	}
}

