using MongoDB.Driver;
using Tvee.Models;
using System.Threading.Tasks;

namespace Tvee.Services
{
    public class UserService : IUserService
    {
       

        // Constructor to inject MongoDB database context
        public UserService(IMongoDatabase database)
        {
            _users = database.GetCollection<User>("Users");
        }

        // Method to get a user by their ID
        public async Task<User> GetUserById(string userId)
        {
            // Create a filter to find the user by their ID
            

            // Execute the query and return the user
            return await _users.Find(filter).FirstOrDefaultAsync();
        }
    }
}