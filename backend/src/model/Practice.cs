using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeacherPractise.Model
{
    public class Practice
    {
        private int Id { get; set; }
        [ForeignKey("Id")]
        public UserPractice UserPractice { get; set; }

        private DateTime Date { get; set; }
        private DateTime Start { get; set; }
        private DateTime End { get; set; }
        [Required]
        private string? Note { get; set; }
        private int Capacity { get; set; }

        //subjectID is fake key to subject.Id
        private int SubjectId { get; set; }
        [ForeignKey("SubjectId")]
        public Subject Subject { get; set; }

        private int TeacherId { get; set; }
        [ForeignKey("TeacherId")]
        public User User { get; set; }


        public virtual ICollection<Review> Reviews { get; set; }
    }
}