namespace Tvee.Models
{
    public class UpdateUserDTO
    {
        public string? Username { get; set; }
         public string? OldPassword { get; set; }
        public string? NewPassword { get; set; }
         public string? Email { get; set; } // New field
        public string? Bio { get; set; }
    }
}