﻿using System.ComponentModel.DataAnnotations;

namespace MyBlogAdminService.Models.dtos
{
    public class PostUpdateDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "Content is required.")]
        public string? Content { get; set; }
        public ICollection<int>? CategoryIds { get; set; } = new List<int>();
        public ICollection<int>? TagIds { get; set; } = new List<int>();
    }
}
