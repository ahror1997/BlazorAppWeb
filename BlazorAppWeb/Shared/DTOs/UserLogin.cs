using System.ComponentModel.DataAnnotations;

namespace BlazorAppWeb.Shared.DTOs
{
    public class UserLogin
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
