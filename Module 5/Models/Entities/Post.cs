﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Module_5.Models.Entities
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? ImageUrl { get; set; }
        public string Content { get; set; }

        [Required]
        public bool IsPublished { get; set; } = false;
        public int AuthorId { get; set; }
        public User Author { get; set; }

        public int? CategoryId { get; set; }
        public Category? Category { get; set; }

        public ICollection<Comment>? Comments { get; set; }
        public ICollection<Like>? Likes { get; set; }
    }
}

