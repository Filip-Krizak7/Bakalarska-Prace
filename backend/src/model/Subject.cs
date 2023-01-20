using System.ComponentModel.DataAnnotations;

namespace TeacherPractise.Model
{
    public class Subject
    {
        private int Id { get; set; }
        [Required]
        private string? Name { get; set; }

        public virtual ICollection<Practice> Practices { get; set; }
    }
}