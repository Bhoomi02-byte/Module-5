namespace Module_5.Models.Entities
{
    public class User
    {
           public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string HashPassword { get; set; }
        public string Role { get; set; }

        public ICollection<Post> Posts { get; set; }
        public ICollection<Category> Category { get; set; }
        public ICollection<Subscriptions> Subscriptions { get; set; } // If user is a viewer
    }
}
