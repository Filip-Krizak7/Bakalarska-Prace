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

namespace TeacherPractise.Service
{
    public class TeacherService
    {
        private readonly AppUserService appUserService;
        private readonly SchoolService schoolService;
        private readonly CustomMapper mapper;

        public TeacherService([FromServices] AppUserService appUserService, [FromServices] CustomMapper mapper)
        {
            this.appUserService = appUserService;
            this.mapper = mapper;
        }

        public long addPractice(string teacherUsername, NewPracticeDto newPracticeDto)
        {
            using (var ctx = new Context())
	        {
		        User teacher = ctx.Users.ToList().FirstOrDefault(q => q.Username == teacherUsername.ToLower())
                	?? throw AppUserService.CreateException($"Učitel {teacherUsername} nenalezen.");

            	Subject subject = ctx.Subjects.ToList().FirstOrDefault(q => q.Id == newPracticeDto.subject.id)
                	?? throw AppUserService.CreateException($"Předmět {newPracticeDto.subject.name} nenalezen.");

                Practice practice = mapper.practiceDtoToPractice(newPracticeDto);
                practice.TeacherId = teacher.Id;

                ctx.Practices.Add(practice);
                ctx.SaveChanges();

                return practice.PracticeId;
            }
        }

        public List<long> findAllStudentIdsByStudentPracticeIds(long id, int pageNumber, int pageSize)
        {
            using (var ctx = new Context())
	        {
                var studentIds = ctx.Practices
                    .Where(p => p.PracticeId == id)
                    .SelectMany(p => p.UsersOnPractice.Select(s => s.Id))
                    .ToList();

                var paginatedStudentIds = studentIds
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                return paginatedStudentIds.ConvertAll<long>(x => (long)x);
            }
        }

        public List<StudentPracticeDto> getPracticesList(string teacherUsername, DateOnly date, long subjectId, int pageNumber, int pageSize)
        {
            using (var ctx = new Context())
	        {
                User teacher = ctx.Users.ToList().FirstOrDefault(q => q.Username == teacherUsername.ToLower())
                	?? throw AppUserService.CreateException($"Učitel {teacherUsername} nenalezen.");

                var practices = ctx.Practices.ToList().Where(q => q.Date == date || q.SubjectId == subjectId || q.TeacherId == teacher.Id);
                practices.OrderBy(p => p.Date);

                var practicesDomain = mapper.practicesToPracticesDomain(practices.ToList());
                var toDelete = new List<PracticeDomain>();

                foreach (PracticeDomain p in practicesDomain)
                {
                    p.SetNumberOfReservedStudents();
                    p.SetStudentNames(getStudentNamesByPractice(p, pageNumber, pageSize));
                    p.SetFileNames(appUserService.getTeacherFiles(p.teacher.username));
                    p.SetStudentEmails(getStudentEmailsByPractice(p, pageNumber, pageSize));
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

        public List<StudentPracticeDto> getPracticesListPast(string teacherUsername, DateOnly date, long subjectId, int pageNumber, int pageSize)
        {
            using (var ctx = new Context())
	        {
                User teacher = ctx.Users.ToList().FirstOrDefault(q => q.Username == teacherUsername.ToLower())
                	?? throw AppUserService.CreateException($"Učitel {teacherUsername} nenalezen.");

                var practices = ctx.Practices.ToList().Where(q => q.Date == date || q.SubjectId == subjectId || q.TeacherId == teacher.Id); //date, subjectId, teacher.Id, pageNumber, pageSize
                practices.OrderBy(p => p.Date);

                var practicesDomain = mapper.practicesToPracticesDomain(practices.ToList());
                var toDelete = new List<PracticeDomain>();

                foreach (PracticeDomain p in practicesDomain)
                {
                    p.SetNumberOfReservedStudents();
                    p.SetStudentNames(getStudentNamesByPractice(p, pageNumber, pageSize));
                    p.SetFileNames(appUserService.getTeacherFiles(p.teacher.username));
                    p.SetStudentEmails(getStudentEmailsByPractice(p, pageNumber, pageSize));
                    string report = appUserService.getPracticeReport(p.id);
                    p.SetReport(report);
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

        public List<string> getStudentEmailsByPractice(PracticeDomain p, int pageNumber, int pageSize)
        {
            List<long> ids = findAllStudentIdsByStudentPracticeIds(p.id, pageNumber, pageSize);
            List<string> emails = new List<string>();

            foreach (long id in ids)
            {
                User u = appUserService.getUserById(id);
                string email = u.Username;
                emails.Add(email);
            }

            string[] arr = emails.ToArray();
            Array.Sort(arr, (str1, str2) =>
            {
                string substr1 = appUserService.getUserByUsername(str1).SecondName;
                string substr2 = appUserService.getUserByUsername(str2).SecondName;

                return substr1.CompareTo(substr2);
            });

            return new List<string>(arr);
        }

        public List<String> getStudentNamesByPractice(PracticeDomain p, int pageNumber, int pageSize)
        {
            List<long> ids = findAllStudentIdsByStudentPracticeIds(p.id, pageNumber, pageSize);
            List<string> names = new List<string>();

            foreach (long id in ids)
            {
                User u = appUserService.getUserById(id);
                string name = u.FirstName + " " + u.SecondName;
                names.Add(name);
            }

            string[] arr = names.ToArray();
            Array.Sort(arr, (str1, str2) =>
            {
                string[] temp1 = str1.Split(' ');
                string substr1 = temp1[temp1.Length - 1];
                string[] temp2 = str2.Split(' ');
                string substr2 = temp2[temp2.Length - 1];

                return substr1.CompareTo(substr2);
            });

            return new List<string>(arr);
        }
    }
}    