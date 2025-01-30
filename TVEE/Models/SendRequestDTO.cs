using System.ComponentModel.DataAnnotations; // For Required and StringLength attributes

namespace Tvee.Models
{
    public class SendRequestDTO
    {
        [Required(ErrorMessage = "ReceiverId is required.")]
        public Guid ReceiverId { get; set; }  // Teacher's ID

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(100, ErrorMessage = "Title must be less than 100 characters.")]
        public string? Title { get; set; }  // Title of the request

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(500, ErrorMessage = "Description must be less than 500 characters.")]
        public string? Description { get; set; }  // Description of the request
    }
}
