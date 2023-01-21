using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeacherPractise.Model
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string SecondName { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        public int Role_id { get; set; }
        public bool Locked { get; set; }
        public bool Enabled { get; set; }

        public int SchoolId { get; set; }
        [ForeignKey("SchoolId")]
        public School School { get; set; }

        public int Teacher_PracticeId { get; set; }
        [ForeignKey("Teacher_PracticeId")]
        public Practice Practice { get; set; }


        public int Student_PracticeId { get; set; }
        [ForeignKey("Student_PracticeId")]
        public UserPractice UserPractice { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<Practice> Practices { get; set; }
    }
}