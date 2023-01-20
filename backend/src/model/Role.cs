using System.ComponentModel.DataAnnotations;

namespace TeacherPractise.Model
{
    public class Role
    {
        [Required]
        private string? Code { get; set; }
    }
}