using TeacherPractise.Dto.Request;
using TeacherPractise.Dto.Response;
using TeacherPractise.Domain;
using TeacherPractise.Model;
using TeacherPractise.Service;

namespace TeacherPractise.Mapper
{
    public class Mapper
    {
        private readonly SchoolService schoolService;

        public SchoolDto schoolToSchoolDto(School school)
        {
            SchoolDto schoolDto = new SchoolDto();
            schoolDto.id = (long)school.Id;
            schoolDto.name = school.Name;

            return schoolDto;
        }

        public School schoolDtoToSchool(SchoolDto schoolDto)
        {
            School school = new School();
            school.Id = (int)schoolDto.id;
            school.Name = schoolDto.name;

            return school;
        }

        public SubjectDto subjectToSubjectDto(Subject subject)
        {
            SubjectDto subjectDto = new SubjectDto();
            subjectDto.id = (long)subject.Id;
            subjectDto.name = subject.Name;

            return subjectDto;
        }

        public Subject subjectDtoToSubject(SubjectDto subjectDto)
        {
            Subject subject = new Subject();
            subject.Id = (int)subjectDto.id;
            subject.Name = subjectDto.name;

            return subject;
        }

        public ReviewDto reviewToReviewDto(Review review)
        {
            ReviewDto reviewDto = new ReviewDto();
            reviewDto.practiceId = (long)review.PracticeId;
            //reviewDto.name = review.;
            reviewDto.reviewText = review.Text;

            return reviewDto;
        }

        public Review reviewDtoToReview(ReviewDto reviewDto)
        {
            Review review = new Review();
            review.PracticeId = (int)reviewDto.practiceId;
            //review.name = review.;
            review.Text = reviewDto.reviewText;

            return review;
        }

        public UserDto userToUserDto(User user)
        {
            UserDto userDto = new UserDto();
            userDto.id = (long)user.Id;
            userDto.username = user.Username;
            userDto.firstName = user.FirstName;
            userDto.secondName = user.SecondName;
            userDto.school = schoolToSchoolDto(schoolService.getSchoolById((long)user.SchoolId));
            
            return userDto;
        }

        public User userDtoToUser(UserDto userDto)
        {
            User user = new User();
            user.Id = (int)userDto.id;
            user.Username = userDto.username;
            user.FirstName = userDto.firstName;
            user.SecondName = userDto.secondName;
            user.SchoolId = (int)userDto.school.id;
            
            return user;
        }

        public Practice practiceDtoToPractice(NewPracticeDto practiceDto)
        {
            Practice practice = new Practice();
            practice.Date = practiceDto.date;
            practice.Start = practiceDto.start;
            practice.End = practiceDto.end;
            practice.Note = practiceDto.note;
            practice.Capacity = practiceDto.capacity;
            practice.SubjectId = (int)practiceDto.subject.id;

            return practice;
        }

        public PracticeDomain practiceToPracticeDomain(Practice practice)
        {
            PracticeDomain practiceDomain = new PracticeDomain();
            practiceDomain.id = (long)practice.Id;
            practiceDomain.date = practice.Date;
            practiceDomain.start = practice.Start.TimeOfDay;
            practiceDomain.end = practice.End.TimeOfDay;
            practiceDomain.note = practice.Note;
            practiceDomain.capacity = practice.Capacity;
            practiceDomain.subject = subjectToSubjectDto(schoolService.getSubjectById((long)practice.SubjectId));
            practiceDomain.teacher = userToUserDto(schoolService.getTeacherById((long)practice.TeacherId));
            
            return practiceDomain;
        }

        public StudentPracticeDto practiceDomainToStudentPracticeDto(PracticeDomain practiceDomain)
        {
            StudentPracticeDto studentPracticeDto = new StudentPracticeDto();
            studentPracticeDto.id = practiceDomain.id;
            studentPracticeDto.date = practiceDomain.date;
            studentPracticeDto.start = practiceDomain.start;
            studentPracticeDto.end = practiceDomain.end;
            studentPracticeDto.note = practiceDomain.note;
            studentPracticeDto.capacity = practiceDomain.capacity;
            studentPracticeDto.subject = practiceDomain.subject;
            studentPracticeDto.teacher = practiceDomain.teacher;
            //reviews

            return studentPracticeDto;
        }
    }
}