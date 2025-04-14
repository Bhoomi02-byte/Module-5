using MongoDB.Driver;
using Module_5.Collections;

namespace Module_5.Data
{
    public class MongoDbContext : IMongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IMongoDatabase database)
        {
            _database = database;
        }

        public IMongoCollection<User> Users => _database.GetCollection<User>("Users");
        public IMongoCollection<Category> Categories => _database.GetCollection<Category>("Categories");
        public IMongoCollection<Post> Posts => _database.GetCollection<Post>("Posts");
        public IMongoCollection<Comment> Comments => _database.GetCollection<Comment>("Comments");
        public IMongoCollection<Like> Likes => _database.GetCollection<Like>("Likes");
        public IMongoCollection<Subscription> Subscriptions => _database.GetCollection<Subscription>("Subscriptions");
    }
}
