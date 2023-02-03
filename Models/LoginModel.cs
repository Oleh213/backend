using System;
using System.ComponentModel.DataAnnotations;

namespace WebShop.Models
{
    public class LoginModule
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}