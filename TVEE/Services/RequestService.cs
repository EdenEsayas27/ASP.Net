using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tvee.Models;

namespace Tvee.Services
{
    public class RequestService
    {
        private readonly IMongoCollection<Request> _requests;

        public RequestService(IMongoDatabase database)
        {
            _requests = database.GetCollection<Request>("Requests");
        }

        // Create a request
        public async Task<Request> SendRequest(string senderId, string receiverId, string title, string description)
        {
            var request = new Request
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                Title = title,
                Description = description
            };

            await _requests.InsertOneAsync(request);
            return request;
        }

        // Get all requests for a specific teacher
        public async Task<List<Request>> GetRequestsForTeacher(string teacherId)
        {
            return await _requests.Find(r => r.ReceiverId == teacherId).ToListAsync();
        }

        // Mark a request as read
        public async Task<bool> MarkRequestAsRead(string requestId)
        {
            var update = Builders<Request>.Update.Set(r => r.Status, "Read");
            var result = await _requests.UpdateOneAsync(r => r.Id == requestId, update);
            return result.ModifiedCount > 0;
        }
    }
}
