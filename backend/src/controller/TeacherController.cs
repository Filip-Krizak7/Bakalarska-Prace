using System.Net;
using System.IO;
using TeacherPractise.Model;
using TeacherPractise.Service;
using TeacherPractise.Dto.Request;
using TeacherPractise.Dto.Response;
using TeacherPractise.Service.FileService;
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
        private readonly FileService fileService;

        public TeacherController(
        [FromServices] TeacherService teacherService,
        [FromServices] AppUserService appUserService,
        [FromServices] FileService fileService)
        {
            this.teacherService = teacherService;
            this.appUserService = appUserService;
            this.fileService = fileService;
        }

        [HttpPost("practice")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public IActionResult addPractice([FromBody] NewPracticeDto newPracticeDto) 
        {
            string currentEmail = appUserService.getCurrentUserEmail();
            return Ok(teacherService.addPractice(currentEmail, newPracticeDto));
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
            return Ok(teacherService.getPracticesList(currentEmail, date, subjectId, pageNumber, pageSize));
        }

        [HttpGet("practices-list-past")]
        [ProducesResponseType(typeof(List<StudentPracticeDto>), 200)]
        public IActionResult getPracticesListPast(
            [FromQuery(Name = "date")] DateTime date,
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

        [HttpGet("getReview/{email}/{practiceId}")]
        public IActionResult getReviews([FromRoute] string email, [FromRoute] long practiceId)
        {
            return Ok(appUserService.getStudentReview(email, practiceId));
        }

        [HttpPost("file/delete/{teacherEmail}/{fileName}")]
        public IActionResult deleteFileFromLocal([FromRoute] string teacherEmail, [FromRoute] string fileName)
        {
            string filePath = fileService.figureOutFileNameFor(teacherEmail, fileName);
            try
            {
                System.IO.File.Delete(filePath);
                return Ok("Soubor smazán.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Chyba při mazání souboru: " + ex.Message);
            }
        }
    }
}