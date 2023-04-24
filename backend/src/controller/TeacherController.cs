using System.Net;
using TeacherPractise.Model;
using TeacherPractise.Service;
using TeacherPractise.Dto.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
    }
}