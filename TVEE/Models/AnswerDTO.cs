using System.ComponentModel.DataAnnotations;

namespace Tvee.Models
{
    public class AnswerDto
    {
        [Required]
        public string Content { get; set; }
    }
}