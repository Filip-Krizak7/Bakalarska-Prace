using System.Net;
using TeacherPractise.Model;
using TeacherPractise.Service;
using TeacherPractise.Dto.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace TeacherPractise.Controller
{
    [Route("coordinator")]
    [Authorize]
    [ApiController]
    public class CoordinatorController : ControllerBase
    {
        private readonly CoordinatorService coordinatorService;
        private readonly TeacherService teacherService;
        private readonly AppUserService appUserService;

        public CoordinatorController(
        [FromServices] CoordinatorService coordinatorService, [FromServices] TeacherService teacherService, [FromServices] AppUserService appUserService)
        {
            this.coordinatorService = coordinatorService;
            this.teacherService = teacherService;
            this.appUserService = appUserService;
        }

        [HttpPost("addSchoool")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Created)]
        public IActionResult addSchool([FromBody] SchoolDto newSchoolDto) 
        {
            return Ok(coordinatorService.addSchool(newSchoolDto));
        }

        [HttpPost("addSubject")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Created)]
        public IActionResult addSubject([FromBody] SubjectDto newSubjectDto) 
        {
            return Ok(coordinatorService.addSubject(newSubjectDto));
        }

        [HttpGet("practices-list")] //v originale pageable misto pageNumber a pageSize
        [ProducesResponseType(typeof(List<StudentPracticeDto>), 200)]
        public IActionResult getPracticesList(
            [FromQuery(Name = "date")] [DataType(DataType.Date)] DateTime date,         //upravit podle teacherController
            [FromQuery(Name = "subjectId")] long subjectId,
            [FromQuery(Name = "pageNumber")] int pageNumber,
            [FromQuery(Name = "pageSize")] int pageSize)
        {
            return Ok(coordinatorService.getPracticesList(date, subjectId, pageNumber, pageSize));
        }

        [HttpGet("practices-list-past")]
        [ProducesResponseType(typeof(List<StudentPracticeDto>), 200)]
        public IActionResult getPracticesListPast(
            [FromQuery(Name = "date")] [DataType(DataType.Date)] DateTime date,
            [FromQuery(Name = "subjectId")] long subjectId,
            [FromQuery(Name = "pageNumber")] int pageNumber,
            [FromQuery(Name = "pageSize")] int pageSize)
        {
            return Ok(coordinatorService.getPracticesListPast(date, subjectId, pageNumber, pageSize));
        }

        [HttpGet("getAllReviews")]
        public IDictionary<long, string> getAllReviews()
        {
            return appUserService.getAllReviews();
        }
    }
}