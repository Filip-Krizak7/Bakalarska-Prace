using System.Net;
using TeacherPractise.Model;
using TeacherPractise.Service;
using TeacherPractise.Dto.Request;
using TeacherPractise.Dto.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace TeacherPractise.Controller
{
    [Route("teacher")]
    [Authorize]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        private readonly TeacherService teacherService;

        public TeacherController(
        [FromServices] TeacherService teacherService)
        {
            this.teacherService = teacherService;
        }

        [HttpPost("practice")]
        [ProducesResponseType(typeof(long), (int)HttpStatusCode.Created)]
        public IActionResult addPractice([FromBody] NewPracticeDto newPracticeDto) 
        {
            var identity = HttpContext.User.Identity;
            return Ok(teacherService.addPractice(identity.Name, newPracticeDto));
        }

        /*[HttpGet("/practices-list")]
        [ProducesResponseType(typeof(List<StudentPracticeDto>), 200)]
        public IActionResult getPracticesList(
            [FromQuery(Name = "date")] [DataType(DataType.Date)] DateTime date,
            [FromQuery(Name = "subjectId")] long subjectId,
            [FromQuery(Name = "pageNumber")] int pageNumber,
            [FromQuery(Name = "pageSize")] int pageSize)
        {
            var identity = HttpContext.User.Identity;
            return Ok(teacherService.getPracticesList(identity.Name, date, subjectId, pageNumber, pageSize));
        }

        [HttpGet("/practices-list-past")]
        [ProducesResponseType(typeof(List<StudentPracticeDto>), 200)]
        public IActionResult getPracticesListPast(
            [FromQuery(Name = "date")] [DataType(DataType.Date)] DateTime date,
            [FromQuery(Name = "subjectId")] long subjectId,
            [FromQuery(Name = "pageNumber")] int pageNumber,
            [FromQuery(Name = "pageSize")] int pageSize)
        {
            var identity = HttpContext.User.Identity;
            return Ok(teacherService.getPracticesListPast(identity.Name, date, subjectId, pageNumber, pageSize));
        }*/
    }
}