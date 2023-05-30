using System;
using System.Linq;
using System.Data.Entity;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using TeacherPractise.Dto.Request;
using TeacherPractise.Dto.Response;
using TeacherPractise.Config;
using TeacherPractise.Mapper;
using TeacherPractise.Model;
using TeacherPractise.Domain;
using TeacherPractise.Exceptions;

namespace TeacherPractise.Service
{
    public class StudentService
    {
        private readonly AppUserService appUserService;
        private readonly SchoolService schoolService;
        private readonly CustomMapper mapper;

        public StudentService([FromServices] AppUserService appUserService, [FromServices] SchoolService schoolService, [FromServices] CustomMapper mapper)
        {
            this.appUserService = appUserService;
            this.schoolService = schoolService;
            this.mapper = mapper;
        }

        public List<StudentPracticeDto> getPracticesList(string studentUsername, DateTime date, long subjectId, int pageNumber, int pageSize)
        {
            using (var ctx = new Context())
	        {
                var practices = ctx.Practices.ToList().Where(q => q.Date == date || q.SubjectId == subjectId);
                if(!practices.Any()) 
                {
                    practices = ctx.Practices.ToList();
                }
                practices.OrderBy(p => p.Date);

                var practicesDomain = mapper.practicesToPracticesDomain(practices.ToList());
                var toDelete = new List<PracticeDomain>();

                foreach (PracticeDomain p in practicesDomain)
                {
                    p.SetNumberOfReservedStudents();
                    p.SetIsCurrentStudentReserved(studentUsername);
                    p.SetFileNames(appUserService.getTeacherFiles(p.teacher.username));
                    toDelete.Add(p);
                }

                foreach (PracticeDomain practiceDomain in toDelete)
                {
                    if (practiceDomain.RemoveNotPassedPractices())
                    {
                        practicesDomain.Remove(practiceDomain);
                    }
                }

                return mapper.practicesDomainToStudentPracticesDto(practicesDomain);
            }           
        }

        public List<StudentPracticeDto> getStudentReservedPractices(string studentUsername, int pageNumber, int pageSize)
        {
            using (var ctx = new Context())
	        {
                User student = ctx.Users.Include(u => u.AttendedPractices).SingleOrDefault(u => u.Username == studentUsername)
                	?? throw AppUserService.CreateException($"Student {studentUsername} nenalezen.");

                var practices = student.AttendedPractices;
                practices.OrderBy(p => p.Date);

                var practicesDomain = mapper.practicesToPracticesDomain(practices.ToList());

                var toDelete = new List<PracticeDomain>();

                foreach (PracticeDomain p in practicesDomain)
                {
                    p.SetNumberOfReservedStudents();
                    p.SetIsCurrentStudentReserved(studentUsername);
                    p.SetFileNames(appUserService.getTeacherFiles(p.teacher.username));
                    toDelete.Add(p);
                }

                foreach (PracticeDomain practiceDomain in toDelete)
                {
                    if (practiceDomain.RemoveNotPassedPractices())
                    {
                        practicesDomain.Remove(practiceDomain);
                    }
                }

                return mapper.practicesDomainToStudentPracticesDto(practicesDomain);
            }
        }

        public List<StudentPracticeDto> getStudentPassedPractices(string studentUsername, int pageNumber, int pageSize)
        {
            using (var ctx = new Context())
	        {
                User student = ctx.Users.Include(u => u.AttendedPractices).SingleOrDefault(u => u.Username == studentUsername)
                	?? throw AppUserService.CreateException($"Student {studentUsername} nenalezen.");

                var practices = student.AttendedPractices;
                practices.OrderBy(p => p.Date);

                var practicesDomain = mapper.practicesToPracticesDomain(practices.ToList());
                var toDelete = new List<PracticeDomain>();

                foreach (PracticeDomain p in practicesDomain)
                {
                    p.SetNumberOfReservedStudents();
                    p.SetIsCurrentStudentReserved(studentUsername);
                    p.SetFileNames(appUserService.getTeacherFiles(p.teacher.username));

                    string report = appUserService.getPracticeReport(p.id);
                    p.SetReport(report);
                    List<ReviewDto> reviewList = new List<ReviewDto>();
                    ReviewDto studentRev = appUserService.getStudentReview(studentUsername, p.id);
                    reviewList.Add(studentRev);
                    p.SetReviews(studentRev == null ? null : reviewList);

                    toDelete.Add(p);
                }

                foreach (PracticeDomain practiceDomain in toDelete)
                {
                    if (practiceDomain.RemovePassedPractices())
                    {
                        practicesDomain.Remove(practiceDomain);
                    }
                }

                return mapper.practicesDomainToStudentPracticesDto(practicesDomain);
            }
        }

        public List<StudentPracticeDto> getPracticesSlice(string studentUsername, DateTime date, long? subjectId, int pageNumber, int pageSize)
        {
            using (var ctx = new Context())
	        {
                List<Practice> practices = ctx.Practices.Where(q => q.Date == date || q.SubjectId == subjectId).ToList();
                if(!practices.Any()) 
                {
                    practices = ctx.Practices.ToList();
                }

                List<PracticeDomain> practicesDomain = mapper.practicesToPracticesDomain(practices);

                foreach (var p in practicesDomain)
                {
                    p.SetNumberOfReservedStudents();
                    p.SetIsCurrentStudentReserved(studentUsername);
                }

                return practicesDomain.Select(mapper.practiceDomainToStudentPracticeDto).ToList();
            }
        }

        public void makeReservation(string studentUsername, long practiceId)
        {
            using (var ctx = new Context())
	        {
                User student = ctx.Users.ToList().FirstOrDefault(q => q.Username == studentUsername.ToLower())
                	?? throw AppUserService.CreateException($"Student {studentUsername} nenalezen.");

                Practice practice = ctx.Practices.ToList().FirstOrDefault(q => q.PracticeId == practiceId)
                	?? throw AppUserService.CreateException($"Požadovaná praxe nebyla nalezena.");

                List<User> registeredStudents = ctx.Users
                    .Include(u => u.AttendedPractices)
                    .Where(u => u.AttendedPractices.Any(p => p.PracticeId == practiceId))
                    .ToList();

                if (registeredStudents.Contains(student))
                {
                    throw new UserErrorException("Na tuto praxi jste již přihlášen/á.");
                }
                if (registeredStudents.Count >= practice.Capacity)
                {
                    throw new UserErrorException("Na tuto praxi se již více studentů přihlásit nemůže. V případě potřeby kontaktujte, prosím, vyučujícího.");
                }
                if (practice.Date.AddDays(-AppConfig.MAKE_RESERVATION_DAYS_LEFT) <= DateTime.Now)
                {
                    throw new UserErrorException($"Na praxi je možné se přihlásit nejpozději {AppConfig.MAKE_RESERVATION_DAYS_LEFT} dní předem.");
                }

                registeredStudents.Add(student);
                practice.UsersOnPractice = registeredStudents;
                //student.AttendedPractices.Add(practice);

                ctx.SaveChanges();
            }
        }

        public void cancelReservation(string studentUsername, long practiceId)
        {
            using (var ctx = new Context())
	        {
                User student = ctx.Users.ToList().FirstOrDefault(q => q.Username == studentUsername.ToLower())
                	?? throw AppUserService.CreateException($"Student {studentUsername} nenalezen.");

                Practice practice = ctx.Practices.ToList().FirstOrDefault(q => q.PracticeId == practiceId)
                	?? throw AppUserService.CreateException($"Požadovaná praxe nebyla nalezena.");

                List<User> registeredStudents = ctx.Users
                    .Include(u => u.AttendedPractices)
                    .Where(u => u.AttendedPractices.Any(p => p.PracticeId == practiceId))
                    .ToList();

                if (!registeredStudents.Contains(student))
                {
                    throw new UserErrorException("Na tuto praxi nejste přihlášen/á.");
                }
                if (practice.Date.AddDays(-AppConfig.CANCEL_RESERVATION_DAYS_LEFT) <= DateTime.Now) 
                {
                    throw new UserErrorException($"Z praxe je možné se odhlásit nejpozději {AppConfig.CANCEL_RESERVATION_DAYS_LEFT} dní předem.");
                }

                registeredStudents.Remove(student);
                practice.UsersOnPractice = registeredStudents;
                //student.AttendedPractices.Remove(practice);

                ctx.SaveChanges();
            }
        }

        public string submitReview(string name, long practiceId, string text) 
        {
            using (var ctx = new Context())
	        {
                Practice practice = ctx.Practices.ToList().FirstOrDefault(q => q.PracticeId == practiceId)
                	?? throw AppUserService.CreateException($"Požadovaná praxe nebyla nalezena.");

                User student = ctx.Users.ToList().FirstOrDefault(q => q.Username == name.ToLower())
                	?? throw AppUserService.CreateException($"Student {name} nenalezen.");

                if(student != null) {
                    var existingReview = ctx.Reviews.ToList().Where(q => q.UserId == student.Id && q.PracticeId == practiceId);
                    if(existingReview != null) {
                        return "Již jste jedno hodnocení této praxi napsal.";
                    }

                    Review review = new Review(student.Id, (int)practiceId, text);

                    ctx.Reviews.Add(review);
                    ctx.SaveChanges();

                    return "Hodnocení bylo úspěšně uloženo.";
                }

                return "Chyba při ukládání hodnocení.";
            }
        }
    }
}
