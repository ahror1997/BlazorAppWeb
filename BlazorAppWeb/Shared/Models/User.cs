using System.ComponentModel.DataAnnotations;

namespace BlazorAppWeb.Shared.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }

        [Required]
        [StringLength(10)]
        public string? LastName { get; set; }
    }
}
