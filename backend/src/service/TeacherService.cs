using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using TeacherPractise.Dto.Response;
using TeacherPractise.Config;

namespace TeacherPractise.Service
{
    public class TeacherService
    {
        private readonly AppUserService appUserService;
        private readonly SchoolService schoolService;

        public TeacherService([FromServices] AppUserService appUserService, [FromServices] SchoolService schoolService)
        {
            this.appUserService = appUserService;
            this.schoolService = schoolService;
        }

        public long addPractice(string teacherUsername, NewPracticeDto newPracticeDto)
        {
            long id = 1;

            return id;
        }
    }
}    