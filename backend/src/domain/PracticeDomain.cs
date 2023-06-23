using TeacherPractise.Dto.Response;
using TeacherPractise.Model;

namespace TeacherPractise.Domain
{
    public class PracticeDomain
    {
        public long id { get; set; }
        public DateTime date { get; set; }
        public TimeSpan start { get; set; }
        public TimeSpan end { get; set; }
        public string note { get; set; }
        public int capacity { get; set; }
        public SubjectDto subject { get; set; }
        public UserDto teacher { get; set; }

        public List<UserDto> students { get; set; }
        public string report { get; set; }
        public List<ReviewDto> reviews { get; set; }
        public List<string> fileNames { get; set; }
        public List<string> studentNames { get; set; }
        public List<string> studentEmails { get; set; }

        public int numberOfReservedStudents { get; private set; }
        public bool isCurrentStudentReserved { get; private set; }

        public void SetNumberOfReservedStudents()
        {
            if(students != null) this.numberOfReservedStudents = students.Count();
            else this.numberOfReservedStudents = 0;
        }

        public void SetIsCurrentStudentReserved(string currentStudentUsername)
        {
            UserDto currentStudent = new UserDto();
            
            if(students != null)
            {
                currentStudent = students.FirstOrDefault(student => currentStudentUsername.Equals(student.username));
            }
            
            this.isCurrentStudentReserved = currentStudent?.username != null;
        }

        public bool RemovePassedPractices()
        {
            DateTime dateAndEnd = date;

            if (DateTime.Now > dateAndEnd)
            {
                return false;
            }
            return true;
        }

        public bool RemoveNotPassedPractices()
        {
            DateTime dateAndEnd = date;

            if (DateTime.Now > dateAndEnd)
            {
                return true;
            }
            return false;
        }

        public void SetFileNames(List<string> list)
        {
            this.fileNames = list;
        }

        public void SetReport(string report)
        {
            this.report = report;
        }

        public void SetStudentNames(List<string> list)
        {
            this.studentNames = list;
        }

        public void SetStudentEmails(List<string> list)
        {
            this.studentEmails = list;
        }

        public void SetReviews(List<ReviewDto> list)
        {
            this.reviews = list;
        }

        public PracticeDomain(long id, DateTime date, TimeSpan start, TimeSpan end, string note, int capacity,
                       SubjectDto subject, UserDto teacher, List<UserDto> students, string report,
                       List<ReviewDto> reviews, List<string> fileNames, List<string> studentNames,
                       List<string> studentEmails)
        {
            this.id = id;
            this.date = date;
            this.start = start;
            this.end = end;
            this.note = note;
            this.capacity = capacity;
            this.subject = subject;
            this.teacher = teacher;
            this.students = students;
            this.report = report;
            this.reviews = reviews;
            this.fileNames = fileNames;
            this.studentNames = studentNames;
            this.studentEmails = studentEmails;
        }

        public PracticeDomain()
        {}
    }
}