using System.ComponentModel.DataAnnotations;

namespace TeacherPractise.Model
{
    public class Role
    {
        [Required]
        public string Code { get; set; }
    }
}