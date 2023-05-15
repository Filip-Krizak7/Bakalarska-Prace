using System;
using System.Linq;
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

        public List<StudentPracticeDto> getPracticesList(string studentUsername, DateOnly date, long subjectId, int pageNumber, int pageSize)
        {
            using (var ctx = new Context())
	        {
                User teacher = ctx.Users.ToList().FirstOrDefault(q => q.Username == studentUsername.ToLower())
                	?? throw AppUserService.CreateException($"Student {studentUsername} nenalezen.");

                var practices = ctx.Practices.ToList().Where(q => q.Date == date || q.SubjectId == subjectId || q.TeacherId == teacher.Id); //date, subjectId, teacher.Id, pageNumber, pageSize
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
                    if (practiceDomain.RemovePassedPractices())
                    {
                        practicesDomain.Remove(practiceDomain);
                    }
                }

                return mapper.practicesDomainToStudentPracticesDto(practicesDomain);
            }           
        }

        //getStudentReservedPractices

        //getStudentPassedPractices

        //getPracticesSlice

        /*public void makeReservation(string studentUsername, long practiceId)
        {
            using (var ctx = new Context())
	        {
                User student = ctx.Users.ToList().FirstOrDefault(q => q.Username == studentUsername.ToLower())
                	?? throw AppUserService.CreateException($"Student {studentUsername} nenalezen.");

                Practice practice = ctx.Practices.ToList().FirstOrDefault(q => q.Id == practiceId)
                	?? throw AppUserService.CreateException($"Požadovaná praxe nebyla nalezena.");

                List<User> registeredStudents = ctx.Users.Where(q => q.Student_PracticeId == practiceId).ToList(); //--------------------------------- zmenit

                if (registeredStudents.Contains(student))
                {
                    throw new UserErrorException("Na tuto praxi jste již přihlášen/á.");
                }
                if (registeredStudents.Count >= practice.Capacity)
                {
                    throw new UserErrorException("Na tuto praxi se již více studentů přihlásit nemůže. V případě potřeby kontaktujte, prosím, vyučujícího.");
                }
                if (practice.Date.AddDays(-AppConfig.MAKE_RESERVATION_DAYS_LEFT) <= DateOnly.FromDateTime(DateTime.Now))
                {
                    throw new UserErrorException($"Na praxi je možné se přihlásit nejpozději {AppConfig.MAKE_RESERVATION_DAYS_LEFT} dní předem.");
                }

                registeredStudents.Add(student);
                practice.Users = registeredStudents;

                ctx.SaveChanges();
            }
        }

        public void cancelReservation(string studentUsername, long practiceId)
        {
            using (var ctx = new Context())
	        {
                User student = ctx.Users.ToList().FirstOrDefault(q => q.Username == studentUsername.ToLower())
                	?? throw AppUserService.CreateException($"Student {studentUsername} nenalezen.");

                Practice practice = ctx.Practices.ToList().FirstOrDefault(q => q.Id == practiceId)
                	?? throw AppUserService.CreateException($"Požadovaná praxe nebyla nalezena.");

                List<User> registeredStudents = ctx.Users.Where(q => q.Student_PracticeId == practiceId).ToList();

                if (!registeredStudents.Contains(student))
                {
                    throw new UserErrorException("Na tuto praxi nejste přihlášen/á.");
                }
                if (practice.Date.AddDays(-AppConfig.CANCEL_RESERVATION_DAYS_LEFT) <= DateOnly.FromDateTime(DateTime.Now)) 
                {
                    throw new UserErrorException($"Z praxe je možné se odhlásit nejpozději {AppConfig.CANCEL_RESERVATION_DAYS_LEFT} dní předem.");
                }

                registeredStudents.Remove(student);
                practice.Users = registeredStudents;

                ctx.SaveChanges();
            }
        }*/

        public string SubmitReview(string name, long practiceId, string text) 
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
