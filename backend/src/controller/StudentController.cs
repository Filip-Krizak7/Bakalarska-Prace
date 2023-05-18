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

        [HttpGet("practices-list")]
        [ProducesResponseType(typeof(List<StudentPracticeDto>), 200)]
        public IActionResult getPracticesList(
            [FromQuery(Name = "date")] DateOnly date,
            [FromQuery(Name = "subjectId")] long subjectId,
            [FromQuery(Name = "pageNumber")] int pageNumber,
            [FromQuery(Name = "pageSize")] int pageSize)
        {
            string currentEmail = appUserService.getCurrentUserEmail();
            return Ok(studentService.getPracticesList(currentEmail, date, subjectId, pageNumber, pageSize));
        }

        [HttpGet("reserved-practices-list")]
        [ProducesResponseType(typeof(List<StudentPracticeDto>), 200)]
        public IActionResult getPracticesListPast(
            [FromQuery(Name = "pageNumber")] int pageNumber,
            [FromQuery(Name = "pageSize")] int pageSize)
        {
            string currentEmail = appUserService.getCurrentUserEmail();
            return Ok(studentService.getStudentReservedPractices(currentEmail, pageNumber, pageSize));
        }

        [HttpGet("passed-practices-list")]
        [ProducesResponseType(typeof(List<StudentPracticeDto>), 200)]
        public IActionResult getPassedPractices(
            [FromQuery(Name = "pageNumber")] int pageNumber,
            [FromQuery(Name = "pageSize")] int pageSize)
        {
            string currentEmail = appUserService.getCurrentUserEmail();
            return Ok(studentService.getStudentPassedPractices(currentEmail, pageNumber, pageSize));
        }
    }
}