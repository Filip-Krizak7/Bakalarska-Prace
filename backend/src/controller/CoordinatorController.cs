using System.Net;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using TeacherPractise.Model;
using TeacherPractise.Service;
using TeacherPractise.Service.CsvReport;
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
        private readonly AppUserService appUserService;
        private readonly CsvReport csvReport;

        public CoordinatorController(
        [FromServices] CoordinatorService coordinatorService,  
        [FromServices] AppUserService appUserService,
        [FromServices] CsvReport csvReport)
        {
            this.coordinatorService = coordinatorService;
            this.appUserService = appUserService;
            this.csvReport = csvReport;
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

        [HttpGet("practices-list")]
        [ProducesResponseType(typeof(List<StudentPracticeDto>), 200)]
        public IActionResult getPracticesList(
            [FromQuery(Name = "date")] DateTime date,
            [FromQuery(Name = "subjectId")] long subjectId,
            [FromQuery(Name = "pageNumber")] int pageNumber,
            [FromQuery(Name = "pageSize")] int pageSize)
        {
            return Ok(coordinatorService.getPracticesList(date, subjectId, pageNumber, pageSize));
        }

        [HttpGet("practices-list-past")]
        [ProducesResponseType(typeof(List<StudentPracticeDto>), 200)]
        public IActionResult getPracticesListPast(
            [FromQuery(Name = "date")] DateTime date,
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

        [HttpPost("registerCoordinator")]
        public IActionResult register([FromBody] RegistrationDto request)
        {
            return Ok(coordinatorService.register(request));
        }
        

        [HttpPost("deleteCoordinator")]
        public IActionResult deleteCoordinator([FromBody] long id)
        {        
            return Ok(coordinatorService.deleteCoordinator(id));
        }

        [HttpGet("getReview/{email}/{practiceId}")]
        public IActionResult getReviews([FromRoute] string email, [FromRoute] long practiceId)
        {
            return Ok(appUserService.getStudentReview(email, practiceId));
        }

        [HttpPost("export")]
        public IActionResult getExport([FromBody] ExportDatesDto request)
        {
            DateTime start = new DateTime(request.startYear, request.startMonth, request.startDay);
            DateTime end = new DateTime(request.endYear, request.endMonth, request.endDay);
            string filePath = "/home/student/project/myproject/backend/export.csv";
            csvReport.createReport(filePath, start, end);

            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            var fileContentResult = new FileContentResult(fileBytes, "application/octet-stream")
            {
                FileDownloadName = Path.GetFileName(filePath)
            };
            return Ok(fileContentResult);
        }
    }
}