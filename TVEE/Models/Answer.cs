using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Tvee.Models
{
    public class Answer
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }  // Unique ID for the answer

        public string Content { get; set; }  // Content/body of the answer
        public string UserId { get; set; }  // ID of the user who posted the answer
        public DateTime Timestamp { get; set; }  // Timestamp of when the answer was posted
    }
}