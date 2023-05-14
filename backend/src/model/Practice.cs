using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace TeacherPractise.Model
{
    public class Practice
    {
        [Key] //---
        public int Id { get; set; }
        [ForeignKey("Id")]
        public UserPractice UserPractice { get; set; }

        public DateOnly Date { get; set; }
        public TimeSpan Start { get; set; }
        public TimeSpan End { get; set; }
        [Required]
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
        public virtual ICollection<User> Users { get; set; } //---

        public Practice(DateOnly date, TimeSpan start, TimeSpan end, string note, int capacity, int subjectId, int teacherId)
        {
            this.Date = date;
            this.Start = start;
            this.End = end;
            this.Note = note;
            this.Capacity = capacity;
            this.SubjectId = subjectId;
            this.TeacherId = teacherId;
        }

        public Practice()
        {}
    }
}