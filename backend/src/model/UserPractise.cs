using System.ComponentModel.DataAnnotations;

namespace TeacherPractise.Model
{
    public class UserPractice
    {
        [Key]
        public int UserPracticeId { get; set; }
        public int PracticeId { get; set; }
        public int UserId { get; set; }

        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<Practice> Practices { get; set; }

        public UserPractice(int practiceId, int userId)
        {
            this.PracticeId = practiceId;
            this.UserId = userId;
        }
    }
}