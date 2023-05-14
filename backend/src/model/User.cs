using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeacherPractise.Model
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string Username { get; set; } //osu email
        [Required]
        public string Password { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string SecondName { get; set; }
        public string? PhoneNumber { get; set; }
        public Roles Role { get; set; }
        public bool Locked { get; set; }
        public bool Enabled { get; set; }

        public int? SchoolId { get; set; }
        [ForeignKey("SchoolId")]
        public School School { get; set; }

        public int? Teacher_PracticeId { get; set; }
        [ForeignKey("Teacher_PracticeId")]
        public Practice Practice { get; set; }


        public int? Student_PracticeId { get; set; }
        [ForeignKey("Student_PracticeId")]
        public UserPractice UserPractice { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<Practice> Practices { get; set; }

        public User(string username, string firstName, string lastName, int schoolId, string phoneNumber, string password, Roles role)
        {
            this.Username = username;
            this.Password = password;
            this.FirstName = firstName;
            this.SecondName = lastName;
            this.PhoneNumber = phoneNumber;
            this.SchoolId = schoolId;
            this.Role = role;
        }

        public User(string username, string password, string firstName, string lastName, string phoneNumber, Roles role, bool locked, bool enabled)
        {
            this.Username = username;
            this.Password = password;
            this.FirstName = firstName;
            this.SecondName = lastName;
            this.PhoneNumber = phoneNumber;
            this.Role = role;
            this.Locked = locked;
            this.Enabled = enabled;
        }

        public User(string username, string password, string firstName, string lastName, int schoolId, string phoneNumber, Roles role, bool locked, bool enabled)
        {
            this.Username = username;
            this.Password = password;
            this.FirstName = firstName;
            this.SecondName = lastName;
            this.SchoolId = schoolId;
            this.PhoneNumber = phoneNumber;
            this.Role = role;
            this.Locked = locked;
            this.Enabled = enabled;
        }

        public User(string username, string password, string firstName, string lastName, int schoolId, string phoneNumber, Roles role, bool locked, 
            bool enabled, int teacher_practiceId, int student_practiceId)
        {
            this.Username = username;
            this.Password = password;
            this.FirstName = firstName;
            this.SecondName = lastName;
            this.SchoolId = schoolId;
            this.PhoneNumber = phoneNumber;
            this.Role = role;
            this.Locked = locked;
            this.Enabled = enabled;
            this.Teacher_PracticeId = teacher_practiceId;
            this.Student_PracticeId = student_practiceId;
        }

        public User()
        {
        }
    }
}