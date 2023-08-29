using System.ComponentModel.DataAnnotations;

namespace TeacherPractise.Model
{
    public class School
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public virtual ICollection<User> Users { get; set; }

        public School(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        public School()
        {}
    }
}