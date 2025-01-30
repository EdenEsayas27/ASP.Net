using System.ComponentModel.DataAnnotations;

namespace Tvee.Models
{
    public class QuestionDto
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }
    }
}