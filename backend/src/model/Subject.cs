using System.ComponentModel.DataAnnotations;

namespace TeacherPractise.Model
{
    public class Subject
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public virtual ICollection<Practice> Practices { get; set; }
    }
}