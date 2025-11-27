using System.ComponentModel.DataAnnotations;

namespace DeveloperAgent.Web.Models
{
    public class CreateTodoRequest
    {
        [Required]
        [StringLength(200, MinimumLength = 1)]
        public string Title { get; set; } = string.Empty;

        public bool IsComplete { get; set; }
    }

    public class UpdateTodoRequest
    {
        [Required]
        [StringLength(200, MinimumLength = 1)]
        public string Title { get; set; } = string.Empty;

        public bool IsComplete { get; set; }
    }
}
