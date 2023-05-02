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

        public TeacherService([FromServices] AppUserService appUserService, [FromServices] SchoolService schoolService)
        {
            this.appUserService = appUserService;
            this.schoolService = schoolService;
        }

        public long addPractice(string teacherUsername, NewPracticeDto newPracticeDto)
        {
            using (var ctx = new Context())
	        {
		        User teacher = ctx.Users.ToList().FirstOrDefault(q => q.Username == teacherUsername.ToLower())
                	?? throw AppUserService.CreateException($"Učitel {teacherUsername} nenalezen.");

            	Subject subject = ctx.Subjects.ToList().FirstOrDefault(q => q.Id == newPracticeDto.subject.id)
                	?? throw AppUserService.CreateException($"Předmět {newPracticeDto.subject.name} nenalezen.");

                Practice practice = new Practice(newPracticeDto.date, newPracticeDto.start, newPracticeDto.end, 
                    newPracticeDto.note, newPracticeDto.capacity, (int)newPracticeDto.subject.id, teacher.Id);
                    
                ctx.practices.Add(practice);
                ctx.SaveChanges();

                return practice.Id;
            }
        }

        public List<long> findAllStudentIdsByStudentPracticeIds(long id, int pageNumber, int pageSize)
        {
            using (var ctx = new Context())
	        {
                var studentIds = ctx.UserPractices
                    .Where(up => up.PracticeId == id)
                    .OrderBy(up => up.UserPracticeId)
                    .Take(pageSize)
                    .Select(up => up.UserId)
                    .ToList();

                return studentIds.ConvertAll<long>(x => (long)x);
            }
        }

        public List<StudentPracticeDto> getPracticesList(string teacherUsername, DateTime date, long subjectId, int pageNumber, int pageSize)
        {
            using (var ctx = new Context())
	        {
                User teacher = ctx.Users.ToList().FirstOrDefault(q => q.Username == teacherUsername.ToLower())
                	?? throw AppUserService.CreateException($"Učitel {teacherUsername} nenalezen.");

                var practices = ctx.practices.ToList().Where(q => q.Date == date || q.SubjectId == subjectId || q.TeacherId == teacher.Id); //date, subjectId, teacher.Id, pageNumber, pageSize
                practices.OrderBy(p => p.Date);

                var practicesDomain = mapper.practicesToPracticesDomain(practices.ToList());
                var toDelete = new List<PracticeDomain>();

                foreach (PracticeDomain p in practicesDomain)
                {
                    p.SetNumberOfReservedStudents();
                    p.studentNames = getStudentNamesByPractice(p, pageNumber, pageSize);
                    p.studentEmails = getStudentEmailsByPractice(p, pageNumber, pageSize);
                    p.fileNames = appUserService.getTeacherFiles(p.teacher.username);

                    if (p.RemoveNotPassedPractices())
                    {
                        toDelete.Add(p);
                    }
                }

                foreach (PracticeDomain p in toDelete)
                {
                    practicesDomain.Remove(p);
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