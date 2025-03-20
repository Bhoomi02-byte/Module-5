using System.ComponentModel.DataAnnotations.Schema;

namespace Module_5.Models.Entities
{
    public class Subscriptions
    {
        public int SubscriptionsId { get; set; }

        [ForeignKey("Viewer")]
        public int ViewerId { get; set; }
        public User? Viewer { get; set; }

        [ForeignKey("Author")]
        public int AuthorId { get; set; }
        public User? Author { get; set; }
    }
}
