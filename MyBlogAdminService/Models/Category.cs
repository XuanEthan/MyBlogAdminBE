using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MyBlogAdminService.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required .")]
        public string? Name { get; set; }

        public string? Description { get; set; }

        [JsonIgnore]
        public ICollection<Post>? Posts { get; set; }
    }
}
