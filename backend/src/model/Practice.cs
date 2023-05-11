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
        public int? SubjectId { get; set; }
        [ForeignKey("SubjectId")]
        public Subject Subject { get; set; }

        public int? TeacherId { get; set; }
        [ForeignKey("TeacherId")]
        public User User { get; set; }


        public virtual ICollection<Review> Reviews { get; set; }

        public Practice(DateTime date, DateTime start, DateTime end, string note, int capacity, int subjectId, int teacherId)
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