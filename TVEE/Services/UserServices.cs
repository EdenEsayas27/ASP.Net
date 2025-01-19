using MongoDB.Driver;
using TVEEAPI.Data;
using TVEEAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

public class UserService : IUserService
{
    private readonly IMongoCollection<User> _users;

    public UserService(IConfiguration config)
    {
        var client = new MongoClient(config.GetConnectionString("MongoDB"));
        var database = client.GetDatabase("TVEE");
        _users = database.GetCollection<User>("Users");
    }

    public async Task<User> RegisterAsync(string name, string email, string password)
    {
        var user = new User
        {
            Name = name,
            Email = email,
            PasswordHash = password, // Hash password properly
            Role = "Student" // Default role
        };

        await _users.InsertOneAsync(user);
        return user;
    }

    public async Task<User> GetByIdAsync(string userId)
    {
        return await _users.Find(u => u.Id == userId).FirstOrDefaultAsync();
    }

    public async Task<User> UpdateProfileAsync(string userId, UserProfileUpdateDto updateDto)
    {
        var update = Builders<User>.Update
            .Set(u => u.Name, updateDto.Name)
            .Set(u => u.ProfilePictureUrl, updateDto.ProfilePictureUrl);

        return await _users.FindOneAndUpdateAsync(u => u.Id == userId, update);
    }

    public async Task<bool> DeleteAccountAsync(string userId)
    {
        var result = await _users.DeleteOneAsync(u => u.Id == userId);
        return result.DeletedCount > 0;
    }

    public async Task<List<User>> GetAllUsersAsync()
    {
        return await _users.Find(_ => true).ToListAsync();
    }
}
