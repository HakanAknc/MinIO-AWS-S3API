using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace S3AdvancedV2.Models
{
    public class UserModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Username { get; set; }
        public string Password { get; set; } // Demo için plain-text
        public string Role { get; set; } // admin | user
    }
}
