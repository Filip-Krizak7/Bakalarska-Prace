using TeacherPractise.Model;

namespace TeacherPractise.Dto.Response
{
    public class StudentPracticeDto { 
        public long id { get; set; }
        public DateTime date { get; set; }
        public TimeSpan start { get; set; }
        public TimeSpan end { get; set; }
        public string note { get; set; }
        public int capacity { get; set; }
        public SubjectDto subject { get; set; }
        public UserDto teacher { get; set; }
        
        public string report { get; set; }
        public List<string> fileNames { get; set; }
        public List<string> studentNames { get; set; }
        public List<string> studentEmails { get; set; }
        public List<ReviewDto> reviews { get; set; }


        public int numberOfReservedStudents { get; set; }
        public bool isCurrentStudentReserved { get; set; }
    }
}