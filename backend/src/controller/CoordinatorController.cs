using System.Net;
using Microsoft.AspNetCore.Http;
using TeacherPractise.Model;
using TeacherPractise.Service;
using TeacherPractise.Dto.Response;
using TeacherPractise.Dto.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
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

        [HttpGet("waitingList")]
        public IList<UserDto> getLockedUsers() 
        {
            return coordinatorService.getWaitingList();
        }
        
        [HttpPost("addSchool")]
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
            [FromQuery(Name = "date")] [DataType(DataType.Date)] DateTime date,
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

        [HttpPost("removeUser")]
        public IActionResult removeUser([FromBody] string request)
        {
            //string result = request.Substring(1, request.Length - 2);
            return Ok(appUserService.removeUser(request));
        }
        
        [HttpPost("removeSchool")]
        public IActionResult removeSchool([FromBody] string request)
        {
            string result = request.Replace("\"", "");
            return Ok(coordinatorService.removeSchool(result));
        }

        [HttpPost("removeSubject")]
        public IActionResult removeSubject([FromBody] string request)
        {
            string result = request.Replace("\"", "");
            return Ok(coordinatorService.removeSubject(result));
        }

        [HttpPost("editSubject")]
        public IActionResult editSubject([FromBody] EditSubjectDto request)
        {
            return Ok(coordinatorService.editSubject(request.originalSubject, request.newSubject));
        }

        [HttpPost("editSchool")]
        public IActionResult editSchool([FromBody] EditSchoolDto request)
        {
            return Ok(coordinatorService.editSchool(request.originalSchool, request.newSchool));
        }

        [HttpPost("assignSchool")]
        public IActionResult assignSchool([FromBody] AssignSchoolDto request)
        {
            return Ok(coordinatorService.assignSchool(request));
        }

        [HttpPost("unlockUser")]
        public IActionResult unlockUser([FromBody] string request)
        {
            //string result = request.Substring(1, request.Length - 2);
            return Ok(appUserService.unlockUser(request));
        }

        [HttpGet("getTeachersWithoutSchool")]
        public IActionResult getTeachersWithoutSchool()
        {
            return Ok(coordinatorService.getTeachersWithoutSchool());   
        }

        [HttpPost("changePhoneNumber")]
        public IActionResult changePhoneNumber([Required][FromBody] NewNumberDto phoneNumber)
        {
            string result = phoneNumber.phoneNew.Replace("\"", "");
            if (!Regex.IsMatch(result, @"^(\\+420)? ?[1-9][0-9]{2} ?[0-9]{3} ?[0-9]{3}$"))
            {
                return BadRequest("The phone number must be in Czech format.");
            }
            var username = appUserService.getCurrentUserEmail();
            return Ok(coordinatorService.changePhoneNumber(username, result));
        }
    }
}