using System.ComponentModel.DataAnnotations;

namespace teacherpractise.model
{
    public class User
    {
        private int Id { get; set; }
        [Required]
        private string? username { get; set; }
        [Required]
        private string? password { get; set; }
        [Required]
        private string? firstName { get; set; }
        [Required]
        private string? secondName { get; set; }
        [Required]
        private string? phoneNumber { get; set; }
        private int role_id { get; set; }
        private bool locked { get; set; }
        private bool enabled { get; set; }

        //schoolID is fake key to school.Id
        private int schoolId { get; set; }
        [Required]
        public School School { get; set; }

        private int teacher_practiceId { get; set; }
        [Required]
        public Practice Practice { get; set; }


        private int student_practiceId { get; set; }
        [Required]
        public UserPractice UserPractice { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<Practice> Practices { get; set; }
    }
}