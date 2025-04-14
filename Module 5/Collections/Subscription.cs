using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Module_5.Collections
{
    public class Subscription
    {
        [BsonRepresentation(BsonType.ObjectId)]  
        public string Id { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("userId")]
        public string UserId { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("authorId")]
        public string AuthorId { get; set; }
      
    }

}
