using System.ComponentModel.DataAnnotations;

namespace Tvee.Models
{
    public class EditQuestionDto
    {
        [Required]
        public string NewContent { get; set; }
    }
}