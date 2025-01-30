namespace Tvee.Models
{
    public class RegisterDTO
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; } // New field
        public string? Email { get; set; } // New field
        public string? Bio { get; set; } // New field
    }
}
