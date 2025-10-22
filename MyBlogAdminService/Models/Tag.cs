using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MyBlogAdminService.Models
{
    public class Tag
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tag name is required.")]
        public string? Name { get; set; }

        public string? Description { get; set; }

        [JsonIgnore]
        public ICollection<Post>? Posts { get; set; }
    }
}
