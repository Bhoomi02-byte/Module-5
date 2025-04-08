namespace Module_5.Models.Entities
{
    public enum UserRole
    {
        Author,
        User
    }
    public class User
    {

        public  int Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string HashPassword { get; set; }
        public UserRole UserRole { get; set; }

        public ICollection<Post>? Posts { get; set; }
        public ICollection<Category>? Category { get; set; }
        
        //public ICollection<Subscription> Subscriptions { get; set; } 
    }
}
