using System.ComponentModel.DataAnnotations.Schema;

namespace Module_5.Models.Entities
{
    public class Like
    {
        public int Id { get; set; }

         
        [ForeignKey("User")]
        public int UserId { get; set; }

        
        public User? User { get; set; }



        [ForeignKey("Post")]
        public int PostId { get; set; }
        public Post? Post { get; set; }
    }
}
