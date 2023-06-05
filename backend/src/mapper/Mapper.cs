using TeacherPractise.Dto.Request;
using TeacherPractise.Dto.Response;
using TeacherPractise.Domain;
using TeacherPractise.Model;
using TeacherPractise.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace TeacherPractise.Mapper
{
    public class CustomMapper
    {
        private readonly SchoolService schoolService;

        public CustomMapper([FromServices] SchoolService schoolService)
        {
            this.schoolService = schoolService;
        }

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
            User temp = schoolService.getStudentById((long)review.UserId);
            ReviewDto reviewDto = new ReviewDto();
            reviewDto.practiceId = (long)review.PracticeId;
            using (var ctx = new Context())
	        {
                User usr = ctx.Users.Where(q => q.Id == review.UserId).FirstOrDefault();
                reviewDto.name = $"{usr.FirstName} {usr.SecondName}";
            }
            reviewDto.reviewText = review.Text;

            return reviewDto;
        }

        /*public Review reviewDtoToReview(ReviewDto reviewDto)
        {
            Review review = new Review();
            review.PracticeId = (int)reviewDto.practiceId;
            //review.UserId = review.;
            review.Text = reviewDto.reviewText;

            return review;
        }*/

        public UserDto userToUserDto(User user)
        {
            UserDto userDto = new UserDto();
            userDto.id = (long)user.Id;
            userDto.username = user.Username;
            userDto.firstName = user.FirstName;
            userDto.secondName = user.SecondName;
            if (user.SchoolId != null)
            {
                userDto.school = schoolToSchoolDto(schoolService.getSchoolById((long)user.SchoolId));
            }
            
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
            practiceDomain.id = (long)practice.PracticeId;
            practiceDomain.date = practice.Date;
            practiceDomain.start = practice.Start;
            practiceDomain.end = practice.End;
            practiceDomain.note = practice.Note;
            practiceDomain.capacity = practice.Capacity;
            practiceDomain.students = usersToUserDtos(practice.UsersOnPractice.ToList());
            using (var ctx = new Context())
	        {
                Subject sbj = ctx.Subjects.Where(q => q.Id == practice.SubjectId).FirstOrDefault();
                User usr = ctx.Users.Where(q => q.Id == practice.TeacherId).FirstOrDefault();
                practiceDomain.subject = subjectToSubjectDto(sbj);
                practiceDomain.teacher = userToUserDto(usr);
            }
            
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

            studentPracticeDto.report = practiceDomain.report;
            studentPracticeDto.reviews = practiceDomain.reviews;
            studentPracticeDto.fileNames = practiceDomain.fileNames;
            studentPracticeDto.studentNames = practiceDomain.studentNames;
            studentPracticeDto.studentEmails = practiceDomain.studentEmails;

            studentPracticeDto.numberOfReservedStudents = practiceDomain.numberOfReservedStudents;
            studentPracticeDto.isCurrentStudentReserved = practiceDomain.isCurrentStudentReserved;

            return studentPracticeDto;
        }

        public List<PracticeDomain> practicesToPracticesDomain(List<Practice> practices)
        {
            List<PracticeDomain> practiceDomains = new List<PracticeDomain>();

            foreach (Practice practice in practices)
            {
                PracticeDomain practiceDomain = practiceToPracticeDomain(practice);
                practiceDomains.Add(practiceDomain);
            }

            return practiceDomains;
        }

        public List<StudentPracticeDto> practicesDomainToStudentPracticesDto(List<PracticeDomain> practiceDomains)
        {
            List<StudentPracticeDto> studentPracticeDtos = new List<StudentPracticeDto>();

            foreach (PracticeDomain practiceDomain in practiceDomains)
            {
                StudentPracticeDto studentPracticeDto = practiceDomainToStudentPracticeDto(practiceDomain);
                studentPracticeDtos.Add(studentPracticeDto);
            }

            return studentPracticeDtos;
        }

        public List<ReviewDto> reviewsToReviewDtos(List<Review> reviews)
        {
            List<ReviewDto> reviewDtos = new List<ReviewDto>();

            foreach (Review review in reviews)
            {
                ReviewDto reviewDto = reviewToReviewDto(review);
                reviewDtos.Add(reviewDto);
            }

            return reviewDtos;
        }

        public List<SubjectDto> subjectsToSubjectDtos(List<Subject> subjects)
        {
            List<SubjectDto> subjectDtos = new List<SubjectDto>();

            foreach (Subject subject in subjects)
            {
                SubjectDto subjectDto = subjectToSubjectDto(subject);
                subjectDtos.Add(subjectDto);
            }

            return subjectDtos;
        }

        public List<SchoolDto> schoolsToSchoolDtos(List<School> schools)
        {
            List<SchoolDto> subjectDtos = new List<SchoolDto>();

            foreach (School school in schools)
            {
                SchoolDto schoolDto = schoolToSchoolDto(school);
                subjectDtos.Add(schoolDto);
            }

            return subjectDtos;
        }

        public List<UserDto> usersToUserDtos(List<User> users)
        {
            List<UserDto> userDtos = new List<UserDto>();

            foreach (User user in users)
            {
                UserDto userDto = userToUserDto(user);
                userDtos.Add(userDto);
            }

            return userDtos;
        }
    }
}