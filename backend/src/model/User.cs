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
        [Required]
        public string PhoneNumber { get; set; }
        public Roles Role { get; set; }
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

        public User(string username, string firstName, string lastName, School school, string phoneNumber, string password, Roles role)
        {
            this.Username = username;
            this.Password = password;
            this.FirstName = firstName;
            this.SecondName = lastName;
            this.PhoneNumber = phoneNumber;
            this.School = school;
            this.Role = role;
        }

        public User()
        {
            
        }
    }
}