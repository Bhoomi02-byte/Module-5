using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Module_5.Collections
{
    public class Category
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("categoryName")]
        public string CategoryName { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("authorId")]
        public string AuthorId { get; set; }



    }
}

