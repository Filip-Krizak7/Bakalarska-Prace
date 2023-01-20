using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace TeacherPractise.Model
{
    public class School
    {
        private int Id { get; set; }
        [Required]
        private string? Name { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}