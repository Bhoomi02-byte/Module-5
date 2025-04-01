using System.ComponentModel.DataAnnotations;

namespace Module_5.DTO
{
    public class PostDto
    {
        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Content is required.")]
        public string Content { get; set; }
        public string? ImageUrl { get; set; }
       
      
    }
}
