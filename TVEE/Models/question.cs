using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Tvee.Models
{
    public class Question
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }  // Unique ID for the question

        public string Title { get; set; }  // Title of the question
        public string Content { get; set; }  // Content/body of the question

        [BsonIgnore]  // Ignore when serializing input
        public string UserId { get; set; }  // ID of the user who posted the question (this will be set in the controller)

        [BsonIgnore]  // Ignore when serializing input
        public DateTime Timestamp { get; set; }  // Timestamp of when the question was posted (this will be set in the controller)

        [BsonIgnore]  // Ignore when serializing input
        public List<Answer> Answers { get; set; } = new List<Answer>();  // List of answers to the question
    }
}