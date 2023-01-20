using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeacherPractise.Model
{
    public class User
    {
        private int Id { get; set; }
        [Required]
        private string? Username { get; set; }
        [Required]
        private string? Password { get; set; }
        [Required]
        private string? FirstName { get; set; }
        [Required]
        private string? SecondName { get; set; }
        [Required]
        private string? PhoneNumber { get; set; }
        private int Role_id { get; set; }
        private bool Locked { get; set; }
        private bool Enabled { get; set; }

        private int SchoolId { get; set; }
        [ForeignKey("SchoolId")]
        public School School { get; set; }

        private int Teacher_PracticeId { get; set; }
        [ForeignKey("Teacher_PracticeId")]
        public Practice Practice { get; set; }


        private int Student_PracticeId { get; set; }
        [ForeignKey("Student_PracticeId")]
        public UserPractice UserPractice { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<Practice> Practices { get; set; }
    }
}