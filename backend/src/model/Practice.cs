using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeacherPractise.Model
{
    public class Practice
    {
        public int Id { get; set; }
        [ForeignKey("Id")]
        public UserPractice UserPractice { get; set; }

        public DateTime Date { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        [Required]
        public string Note { get; set; }
        public int Capacity { get; set; }

        //subjectID is fake key to subject.Id
        public int SubjectId { get; set; }
        [ForeignKey("SubjectId")]
        public Subject Subject { get; set; }

        public int TeacherId { get; set; }
        [ForeignKey("TeacherId")]
        public User User { get; set; }


        public virtual ICollection<Review> Reviews { get; set; }

        /*public Practice(int id, string firstName, string lastName, School school, string phoneNumber, string password, Roles role)
        {
            this.Username = username;
            this.Password = password;
            this.FirstName = firstName;
            this.SecondName = lastName;
            this.PhoneNumber = phoneNumber;
            this.School = school;
            this.Role = role;
        }*/
    }
}