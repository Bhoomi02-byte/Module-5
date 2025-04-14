using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Module_5.Collections
{
    public enum UserRole
    {
        Author,
        User
    }

    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } 

        [BsonElement("name")]
        public string Name { get; set; } 

        [BsonElement("email")]
        public string Email { get; set; } 

        [BsonElement("hashPassword")]
        public string HashPassword { get; set; } 

        [BsonElement("userRole")]
        public UserRole UserRole { get; set; }

        [BsonElement("tokenVersion")]
        public int TokenVersion { get; set; } = 0;
    }
}
