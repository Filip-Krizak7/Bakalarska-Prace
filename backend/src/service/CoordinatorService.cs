using AutoMapper;
using System;
using Microsoft.AspNetCore.Mvc;
using TeacherPractise.Dto.Response;
using TeacherPractise.Dto.Request;
using TeacherPractise.Config;
using TeacherPractise.Mapper;
using TeacherPractise.Model;

namespace TeacherPractise.Service
{
    public class CoordinatorService
    {
        private readonly AppUserService appUserService;
        private readonly IMapper mapper;

        public CoordinatorService([FromServices] AppUserService appUserService,[FromServices] IMapper mapper)
        {
            this.appUserService = appUserService;
            this.mapper = mapper;
        }

        public string addSubject(SubjectDto subjectDto)
        {
            string subjectName = subjectDto.name;

            using (var ctx = new Context())
	        {
                if (ctx.Subjects.ToList().Any(q => q.Name == subjectName.ToLower()))
                {
                    throw AppUserService.CreateException($"Předmět {subjectName} již existuje.");
                }
                else
                {
                    Subject subject = new Subject((int)subjectDto.id, subjectDto.name);
                    //Subject subject = mapper.Map<Subject>(subjectDto);
                    ctx.Subjects.Add(subject);
                    ctx.SaveChanges();
                }
            }

            return "Předmět byl zapsán.";
        }

        public string addSchool(SchoolDto schoolDto)
        {
            string schoolName = schoolDto.name;

            using (var ctx = new Context())
	        {
                if (ctx.Schools.ToList().Any(q => q.Name == schoolName.ToLower()))
                {
                    throw AppUserService.CreateException($"Škola {schoolName} již existuje.");
                }
                else
                {
                    School school = new School((int)schoolDto.id, schoolDto.name);
                    //School school = mapper.Map<School>(schoolDto);
                    ctx.Schools.Add(school);
                    ctx.SaveChanges();
                }
            }

            return "Škola byla přidána.";
        }
    }
}    