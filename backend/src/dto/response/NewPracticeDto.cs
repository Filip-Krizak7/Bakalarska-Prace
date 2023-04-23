using TeacherPractise.Dto.Request;

namespace TeacherPractise.Dto.Response
{
    public class NewPracticeDto {
        public DateTime date;
        public DateTime start;
        public DateTime end;
        public String note;
        public int capacity;
        public SubjectDto subject;
    }
}