using MongoDB.Driver;
using Module_5.Collections;

namespace Module_5.Data
{
    public interface IMongoDbContext
    {
        IMongoCollection<User> Users { get; }
        IMongoCollection<Category> Categories { get; }
        IMongoCollection<Post> Posts { get; }
        IMongoCollection<Comment> Comments { get; }
        IMongoCollection<Like> Likes { get; }
        IMongoCollection<Subscription> Subscriptions { get; }
    }
}
