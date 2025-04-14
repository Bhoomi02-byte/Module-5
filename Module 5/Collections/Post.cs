using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Module_5.Collections
{
    public class Post
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("title")]
        public string Title { get; set; } 

        [BsonElement("imageUrl")]
        public string? ImageUrl { get; set; }

        [BsonElement("content")]
        public string Content { get; set; } 

        [BsonElement("isPublished")]
        public bool IsPublished { get; set; } = false;

        [BsonElement("authorId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string AuthorId { get; set; }

        [BsonElement("categoryId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string CategoryId { get; set; }
    }
}
