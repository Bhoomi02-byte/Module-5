using System.ComponentModel.DataAnnotations.Schema;

namespace Module_5.Models.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public required string Text { get; set; }

    
        [ForeignKey("User")]
        public int UserId { get; set; }

        [ForeignKey("Post")]
        public int PostId { get; set; }
        public User? User { get; set; }
        public Post? Post { get; set; }

    }
}
