using System.ComponentModel.DataAnnotations;

namespace teacherpractise.model
{
    public class Role
    {
        [Required]
        private string? code { get; set; }
    }
}