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
            [FromQuery(Name = "date")] DateTime date,
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

        [HttpPut("practices/{id}/make-reservation")]
        public IActionResult makeReservation([FromRoute] long id)
        {
            string currentEmail = appUserService.getCurrentUserEmail();
            studentService.makeReservation(currentEmail, id);
            return NoContent();
        }

        [HttpPut("practices/{id}/cancel-reservation")]
        public IActionResult cancelReservation([FromRoute] long id)
        {
            string currentEmail = appUserService.getCurrentUserEmail();
            studentService.cancelReservation(currentEmail, id);
            return NoContent();
        }

        [HttpPost("practices/{id}/submitReview")]
        public IActionResult submitReview([FromRoute] long id, [FromBody] ReviesDto reviewDto)
        {
            string currentEmail = appUserService.getCurrentUserEmail();
            return Ok(studentService.submitReview(currentEmail, id, reviewDto.text));
        }

        [HttpGet("practices-slice")]
        public IActionResult getPracticesSlice(
            [FromQuery(Name = "date")] DateTime? date,
            [FromQuery(Name = "subjectId")] long? subjectId,
            [FromQuery(Name = "pageNumber")] int pageNumber,
            [FromQuery(Name = "pageSize")] int pageSize)
        {
            string currentEmail = appUserService.getCurrentUserEmail();
            DateTime localDate = date.GetValueOrDefault();

            return Ok(studentService.getPracticesSlice(currentEmail, localDate, subjectId, pageNumber, pageSize));
        }
    }
}