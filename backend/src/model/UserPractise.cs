using System.ComponentModel.DataAnnotations;

namespace TeacherPractise.Model
{
    public class UserPractice
    {
        private int PracticeId { get; set; }
        private int UserId { get; set; }

        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<Practice> Practices { get; set; }
    }
}