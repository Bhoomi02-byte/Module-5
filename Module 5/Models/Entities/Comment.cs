using System.ComponentModel.DataAnnotations.Schema;

namespace Module_5.Models.Entities
{
    public class Comment
    {
        public int CommentId { get; set; }
        public string Text { get; set; }


        // Foreign Keys & 
        [ForeignKey("User")]
        public int UserId { get; set; }

        //Navigation Properties
        public User? User { get; set; }



        [ForeignKey("Post")]
        public int PostId { get; set; }
        public Post? Post { get; set; }

    }
}
