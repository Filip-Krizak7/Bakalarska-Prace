using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace teacherpractise.model
{
    public class Practice
    {
        private int Id { get; set; }
        [ForeignKey("Id")]
        public UserPractice UserPractice { get; set; }

        private DateTime date { get; set; }
        private DateTime start { get; set; }
        private DateTime end { get; set; }
        [Required]
        private string? note { get; set; }
        private int capacity { get; set; }

        //subjectID is fake key to subject.Id
        private int subjectId { get; set; }
        [ForeignKey("subjectId")]
        public Subject Subject { get; set; }

        private int teacherId { get; set; }
        [ForeignKey("teacherId")]
        public User User { get; set; }


        public virtual ICollection<Review> Reviews { get; set; }
    }
}