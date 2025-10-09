using System.ComponentModel.DataAnnotations;

namespace MyBlogAdminService.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string? Title { get; set; }
        //public byte[] Image { get; set; }

        [Required(ErrorMessage = "Paragraph is required")]
        public string? Paragraph { get; set; }

        public DateTime? Created { get; set; }
    }
}
