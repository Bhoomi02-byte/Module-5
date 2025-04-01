using System.ComponentModel.DataAnnotations.Schema;

namespace Module_5.Models.Entities
{
    public class Subscription
    {
        public int Id { get; set; }

        [ForeignKey("Viewer")]
        public int UserId { get; set; }

        [ForeignKey("Author")]
        public int AuthorId { get; set; }
        public required User Viewer { get; set; }
        public required User Author { get; set; }
    }

}
