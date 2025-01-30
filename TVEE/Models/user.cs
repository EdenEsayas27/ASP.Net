using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Tvee.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }  // MongoDB will auto-generate this

        public string? Username { get; set; }
        public string? PasswordHash { get; set; }
        public string? Role { get; set; } // New field
        public string? Email { get; set; } // New field
        public string? Bio { get; set; } // New field
    }
}
