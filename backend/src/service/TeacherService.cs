using AutoMapper;
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
        private readonly IMapper mapper;

        public TeacherService([FromServices] AppUserService appUserService, [FromServices] SchoolService schoolService, IMapper mapper)
        {
            this.appUserService = appUserService;
            this.schoolService = schoolService;
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

                Practice practice = new Practice(newPracticeDto.date, newPracticeDto.start, newPracticeDto.end, 
                    newPracticeDto.note, newPracticeDto.capacity, (int)newPracticeDto.subject.id, teacher.Id);
                    
                ctx.practices.Add(practice);
                ctx.SaveChanges();

                return practice.Id;
            }
        }

        /*public List<StudentPracticeDto> getPracticesList(string teacherUsername, DateTime date, long subjectId, int pageNumber, int pageSize)
        {
            using (var ctx = new Context())
	        {
                User teacher = ctx.Users.ToList().FirstOrDefault(q => q.Username == teacherUsername.ToLower())
                	?? throw AppUserService.CreateException($"Učitel {teacherUsername} nenalezen.");

                var practices = ctx.Practices.ToList().Where(q => q.Date == date || q.SubjectId == subjectId || q.TeacherId == teacher.Id); //date, subjectId, teacher.Id, pageNumber, pageSize
                practices.Sort((p1, p2) => p1.Date.CompareTo(p2.Date));

                var practicesDomain = mapper.Map<List<PracticeDomain>>(practices);
                var toDelete = new List<PracticeDomain>();

                foreach (var practiceDomain in practicesDomain)
                {
                    practiceDomain.numberOfReservedStudents = practiceDomain.SetNumberOfReservedStudents(); //chyby
                    practiceDomain.studentNames = getStudentNamesByPractice(practiceDomain, pageNumber, pageSize);
                    practiceDomain.studentEmails = getStudentEmailsByPractice(practiceDomain, pageNumber, pageSize);
                    practiceDomain.fileNames = appUserService.getTeacherFiles(practiceDomain.Teacher.Username);

                    if (practiceDomain.RemoveNotPassedPractices())
                    {
                        toDelete.Add(practiceDomain);
                    }
                }

                foreach (var practiceDomain in toDelete)
                {
                    practicesDomain.Remove(practiceDomain);
                }

                return mapper.Map<List<StudentPracticeDto>>(practicesDomain);
            }           
        }

        /*public List<String> getStudentEmailsByPractice()
        {

        }

        public List<String> getStudentNamesByPractice()
        {

        }*/
    }
}    