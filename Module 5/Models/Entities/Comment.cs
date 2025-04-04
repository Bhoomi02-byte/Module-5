using System.ComponentModel.DataAnnotations.Schema;

namespace Module_5.Models.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int UserId { get; set; }
        public int PostId { get; set; }
        public User User { get; set; }
        public Post Post { get; set; }

    }
}
