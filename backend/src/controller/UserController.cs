using TeacherPractise.Model;
using TeacherPractise.Service;
using TeacherPractise.Mapper;
using TeacherPractise.Dto.Response;
using TeacherPractise.Dto.Request;
using TeacherPractise.Service.FileService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TeacherPractise.Controller
{
    [Route("user")]
    [Authorize]
    [ApiController]
    public class UserController : ControllerBase
    {
      private readonly RegistrationService registrationService;
      private readonly AppUserService appUserService;
      private readonly FileService fileService;
      private readonly CustomMapper mapper;

      public UserController(
        [FromServices] AppUserService appUserService,
        [FromServices] RegistrationService registrationService,
        [FromServices] FileService fileService,
        [FromServices] CustomMapper mapper)
      {
        this.appUserService = appUserService;
        this.registrationService = registrationService;
        this.fileService = fileService;
        this.mapper = mapper;
      }

      [HttpPost("/inicialize")]
      [AllowAnonymous]
      public IActionResult inicializeDb() 
      {
        registrationService.inicializeDb();
        return Ok();
      }

      [HttpPost("/check")]
      [AllowAnonymous]
      public IActionResult statusCheck()
      {
          return Ok("Hello World!");
      }

        [HttpGet("all")]
      public IActionResult getAll()
      {
        List<User> ret = appUserService.getUsers();
        return Ok(ret);
      }

      [HttpGet("data")]
      public IActionResult getUserData() 
      {
        string currentEmail = appUserService.getCurrentUserEmail();
        User user = appUserService.getUserByUsername(currentEmail);
        UserDto ret = mapper.userToUserDto(user);
        return Ok(ret);
      }

      [HttpGet("info")]
      public IDictionary<string, string> getBasicInfo()
      {
        string currentEmail = appUserService.getCurrentUserEmail();
        User user = appUserService.getUserByUsername(currentEmail);

        string firstName = user.FirstName;
        string secondName = user.SecondName;
        string role = user.Role.ToString();
        return new Dictionary<string, string>
        {
            { "firstName", firstName },
            { "secondName", secondName },
            { "role", role }
        };
      }

      [HttpGet("subjects")]
      public IList<SubjectDto> getSubjects()
      {
          return appUserService.getSubjects();
      }

      [HttpGet("schools")]
      public IList<SchoolDto> getSchools()
      {
          return appUserService.getSchools();
      }

      [HttpGet("teachers")]
      public IList<UserDto> getTeachers()
      {
          return appUserService.getTeachers();
      }

      [HttpGet("students")]
      public IList<UserDto> getStudents()
      {
          return appUserService.getStudents();
      }

      [HttpGet("coordinators")]
      public IList<UserDto> getCoordinators()
      {
          return appUserService.getCoordinators();
      }

      [HttpGet("teacherFiles/{teacherMail}")]
      [ProducesResponseType(typeof(List<string>), 200)]
      public IActionResult getTeacherFiles([FromRoute] string teacherMail)
      {
          var teacherFiles = appUserService.getTeacherFiles(teacherMail);
          return Ok(teacherFiles);
      }

      [HttpPost("changePassword")]
      public IActionResult changePassword([FromBody] PasswordDto passwordDto)
      {
          string currentEmail = appUserService.getCurrentUserEmail();

          if (appUserService.changePassword(currentEmail, passwordDto))
          {
              return Ok("Heslo bylo změněno");
          }
          else
          {
              return BadRequest("Heslo se nepodařilo změnit");
          }
      }

      [HttpGet("download/{teacherEmail}/{fileName}")]
      [AllowAnonymous]
      public IActionResult downloadFileFromLocal([FromRoute] string teacherEmail, [FromRoute] string fileName)
      {
          string filePath = fileService.figureOutFileNameFor(teacherEmail, fileName);
          var file = new FileInfo(filePath);
          if (!file.Exists)
          {
              return NotFound();
          }

          var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
          var contentType = "application/octet-stream";

          return File(fileStream, contentType, file.Name);
      }

      [HttpGet("report/download/{id}")]
      [AllowAnonymous]
      public IActionResult downloadReportFromLocal([FromRoute] string id)
      {
          string name = fileService.figureOutReportNameFor(Convert.ToInt64(id));
          var file = new FileInfo(name);
          if (!file.Exists)
          {
              return NotFound();
          }

          var fileStream = new FileStream(name, FileMode.Open, FileAccess.Read, FileShare.Read);
          var contentType = "application/octet-stream";

          return File(fileStream, contentType, file.Name);
      }
    }
}