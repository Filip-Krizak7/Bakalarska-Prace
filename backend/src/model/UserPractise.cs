using System.ComponentModel.DataAnnotations;

namespace TeacherPractise.Model
{
    public class UserPractice
    {
        public int PracticeId { get; set; }
        public int UserId { get; set; }

        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<Practice> Practices { get; set; }
    }
}