using TeacherPractise.Dto.Response;

namespace TeacherPractise.Dto.Request
{
    public class NewPracticeDto {
        public DateTime date { get; set; }
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public string note { get; set; }
        public int capacity { get; set; }
        public SubjectDto subject { get; set; }

        public NewPracticeDto(DateTime date, DateTime start, DateTime end, String note, int capacity, SubjectDto subject)
        {
            this.date = date;
            this.start = start;
            this.end = end;
            this.note = note;
            this.capacity = capacity;
            this.subject = subject;
        }

        public NewPracticeDto()
        {
            
        }
    }
}