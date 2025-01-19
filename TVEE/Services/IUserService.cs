public interface IUserService
{
    Task<User> RegisterAsync(string name, string email, string password);
    Task<User> GetByIdAsync(string userId);
    Task<User> UpdateProfileAsync(string userId, UserProfileUpdateDto updateDto);
    Task<bool> DeleteAccountAsync(string userId);
    Task<List<User>> GetAllUsersAsync();
}
