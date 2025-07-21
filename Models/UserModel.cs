using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace S3AdvancedV2.Models
{
    public class UserModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }  // id for the user 

        public string Username { get; set; }  // username for the user
        public string Password { get; set; }  // password for the user
        public string Role { get; set; }  // role of the user (e.g., admin, user, etc.)
    }
}
