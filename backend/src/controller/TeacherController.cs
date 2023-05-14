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
    [Route("teacher")]
    [Authorize]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        private readonly TeacherService teacherService;
        private readonly AppUserService appUserService;

        public TeacherController(
        [FromServices] TeacherService teacherService,
        [FromServices] AppUserService appUserService)
        {
            this.teacherService = teacherService;
            this.appUserService = appUserService;
        }

        [HttpPost("practice")]
        //[ProducesResponseType(StatusCodes.Status201Created)]
        public IActionResult addPractice([FromBody] NewPracticeDto newPracticeDto) 
        {
            Console.WriteLine("vypis -----------------------------------------");
            Console.WriteLine(newPracticeDto._dateString);
            Console.WriteLine(newPracticeDto.date);
            Console.WriteLine(newPracticeDto.start);
            Console.WriteLine(newPracticeDto.end);
            Console.WriteLine(newPracticeDto.subject.name);
            string currentEmail = appUserService.getCurrentUserEmail();
            return Ok(teacherService.addPractice(currentEmail, newPracticeDto));
        }

        [HttpGet("practices-list")]
        [ProducesResponseType(typeof(List<StudentPracticeDto>), 200)]
        public IActionResult getPracticesList(
            [FromQuery(Name = "date")] [DataType(DataType.Date)] DateOnly date,
            [FromQuery(Name = "subjectId")] long subjectId,
            [FromQuery(Name = "pageNumber")] int pageNumber,
            [FromQuery(Name = "pageSize")] int pageSize)
        {
            string currentEmail = appUserService.getCurrentUserEmail();
            return Ok(teacherService.getPracticesList(currentEmail, date, subjectId, pageNumber, pageSize));
        }

        [HttpGet("practices-list-past")]
        [ProducesResponseType(typeof(List<StudentPracticeDto>), 200)]
        public IActionResult getPracticesListPast(
            [FromQuery(Name = "date")] [DataType(DataType.Date)] DateOnly date,
            [FromQuery(Name = "subjectId")] long subjectId,
            [FromQuery(Name = "pageNumber")] int pageNumber,
            [FromQuery(Name = "pageSize")] int pageSize)
        {
            string currentEmail = appUserService.getCurrentUserEmail();
            return Ok(teacherService.getPracticesListPast(currentEmail, date, subjectId, pageNumber, pageSize));
        }

        [HttpGet("getAllReviews")]
        public IDictionary<long, string> getAllReviews()
        {
            return appUserService.getAllReviews();
        }
    }
}