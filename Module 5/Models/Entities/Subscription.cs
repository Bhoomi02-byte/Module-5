using System.ComponentModel.DataAnnotations.Schema;

namespace Module_5.Models.Entities
{
    public class Subscription
    {
        
        public int UserId { get; set; }
        public int AuthorId { get; set; }
        public User User { get; set; }
        public User Author { get; set; }
    }

}
