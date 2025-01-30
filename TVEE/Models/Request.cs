using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Tvee.Models
{
    public class Request
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("senderId")]
        public string SenderId { get; set; } // Student who sends the request

        [BsonElement("receiverId")]
        public string ReceiverId { get; set; } // Teacher who receives the request

        [BsonElement("title")]
        public string Title { get; set; } // Request title

        [BsonElement("description")]
        public string Description { get; set; } // Request details

        [BsonElement("status")]
        public string Status { get; set; } = "Pending"; // Default status

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Timestamp
    }
}
