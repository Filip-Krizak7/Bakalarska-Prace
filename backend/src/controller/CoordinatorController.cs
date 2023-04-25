using System.Net;
using TeacherPractise.Model;
using TeacherPractise.Service;
using TeacherPractise.Dto.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TeacherPractise.Controller
{
    [Route("cordinator")]
    //[Authorize]
    [ApiController]
    public class CoordinatorController : ControllerBase
    {
        private readonly CoordinatorService coordinatorService;

        public CoordinatorController(
        [FromServices] CoordinatorService coordinatorService)
        {
            this.coordinatorService = coordinatorService;
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
    }
}