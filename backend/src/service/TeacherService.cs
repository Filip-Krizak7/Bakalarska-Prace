using AutoMapper;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using TeacherPractise.Dto.Request;
using TeacherPractise.Config;
using TeacherPractise.Mapper;
using TeacherPractise.Model;

namespace TeacherPractise.Service
{
    public class TeacherService
    {
        private readonly AppUserService appUserService;
        private readonly SchoolService schoolService;
        private readonly IMapper mapper;

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

                Practice practice = mapper.Map<NewPracticeDto, Practice>(newPracticeDto);
                practice.TeacherId = teacher.Id;

                return practice.Id;
            }
        }
    }
}    