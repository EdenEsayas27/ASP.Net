using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Tvee.Models;
using System;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using BCrypt.Net;
using MongoDB.Bson;

namespace Tvee.Services
{
    public class AuthService
    {
        private readonly IMongoCollection<User> _users;
        private readonly IConfiguration _configuration;

        public AuthService(IMongoDatabase database, IConfiguration configuration)
        {
            _users = database.GetCollection<User>("Users");
            _configuration = configuration;
        }

        // ✅ Login method
        public async Task<string?> Login(LoginDTO loginDTO)
        {
            var user = await _users.Find(u => u.Username == loginDTO.Username).FirstOrDefaultAsync();
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDTO.Password, user.PasswordHash))
            {
                return null; // Invalid credentials
            }

            return GenerateJwtToken(user);
        }

        // ✅ Register method
        public async Task<User?> Register(RegisterDTO registerDTO)
        {
            var existingUser = await _users.Find(u => u.Username == registerDTO.Username).FirstOrDefaultAsync();
            if (existingUser != null)
            {
                return null; // Username already exists
            }

            var user = new User
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Username = registerDTO.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDTO.Password),
                Role = registerDTO.Role, // Assign the Role
                Email = registerDTO.Email, // Assign the Email
                Bio = registerDTO.Bio // Assign the Bio
            };

            await _users.InsertOneAsync(user);
            return user;
        }

        // ✅ Get user profile
        public async Task<User?> GetUserProfile(string userId)
        {
            var user = await _users.Find(u => u.Id == userId).FirstOrDefaultAsync();
            return user; // Return the user or null if not found
        }

               private string GenerateJwtToken(User user)
{
    if (user.Id == null || user.Username == null || user.Role == null)
    {
        throw new ArgumentNullException("User ID, Username, or Role is null");
    }

    var claims = new[]
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id),
        new Claim(ClaimTypes.Name, user.Username),
        new Claim(ClaimTypes.Role, user.Role) // Include the user's role in the token
    };

    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    var token = new JwtSecurityToken(
        _configuration["Jwt:Issuer"],
        _configuration["Jwt:Audience"],
        claims,
        expires: DateTime.UtcNow.AddHours(2),
        signingCredentials: creds);

    return new JwtSecurityTokenHandler().WriteToken(token);
}
    }
}
