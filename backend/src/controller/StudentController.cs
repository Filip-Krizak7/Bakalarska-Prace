using System.Net;
using TeacherPractise.Model;
using TeacherPractise.Service;
using TeacherPractise.Dto.Request;
using TeacherPractise.Dto.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace TeacherPractise.Controller
{
    [Route("student")]
    [Authorize]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly AppUserService appUserService;
        private readonly StudentService studentService;

        public StudentController(
        [FromServices] StudentService studentService,
        [FromServices] AppUserService appUserService)
        {
            this.studentService = studentService;
            this.appUserService = appUserService;
        }


    }
}