using System.ComponentModel.DataAnnotations;

namespace Module_5.DTO
{
    public class CommentDto
    {
        [Required(ErrorMessage ="Text is Required")]
        public string text { get; set; }
    }
}
