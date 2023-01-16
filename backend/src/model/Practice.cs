using System.ComponentModel.DataAnnotations;

namespace teacherpractise.model
{
    public class Practice
    {
        private int Id { get; set; }
        [Required]
        public UserPractice UserPractice { get; set; }

        private DateTime date { get; set; }
        private DateTime start { get; set; }
        private DateTime end { get; set; }
        [Required]
        private string? note { get; set; }
        private int capacity { get; set; }

        //subjectID is fake key to subject.Id
        private int subjectId { get; set; }
        [Required]
        public Subject Subject { get; set; }

        private int teacherId { get; set; }
        [Required]
        public User User { get; set; }


        public virtual ICollection<Review> Reviews { get; set; }
    }
}