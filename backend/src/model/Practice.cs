using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace TeacherPractise.Model
{
    public class Practice
    {
        public int PracticeId { get; set; }
        public DateOnly Date { get; set; }
        public TimeSpan Start { get; set; }
        public TimeSpan End { get; set; }
        public string Note { get; set; }
        public int Capacity { get; set; }

        //subjectID is fake key to subject.Id
        public int? SubjectId { get; set; }
        [ForeignKey("SubjectId")]
        public Subject Subject { get; set; }

        public int? TeacherId { get; set; }
        [ForeignKey("TeacherId")]
        public User User { get; set; }


        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<User> UsersOnPractice { get; set; } 
    }
}