using TeacherPractise.Model;
using TeacherPractise.Service;
using TeacherPractise.Config;
using TeacherPractise.Dto.Response;
using TeacherPractise.Dto.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Linq;

namespace TeacherPractise.Controller
{
  [Route("user")]
  [Authorize]
  [ApiController]
  public class UserController : ControllerBase
  {
    private readonly AppUserService appUserService;
    private readonly SecurityService securityService;

    public UserController(
      [FromServices] AppUserService appUserService,
      [FromServices] SecurityService securityService)
    {
      this.appUserService = appUserService;
      this.securityService = securityService;
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult getUserNames() 
    {
      List<string> ret = appUserService.getUsers()
        .Select(q => q.Username[..q.Username.IndexOf("@")])
        .ToList();
      return Ok(ret);
    }

    [Authorize]
    [HttpGet("data")]
    public IActionResult getUserData() 
    {
      string username = User.Identity.Name;
      User ret = appUserService.getUserByUsername(username);
      return Ok(ret);
    }

    [HttpGet("all")]
    [Authorize] 
    public IActionResult getAll()
    {
      List<User> ret = appUserService.getUsers();
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

    [HttpPost("loginTest")]
    [AllowAnonymous]
    public IActionResult loginTwo(UserLoginDto request) 
    {
      User appUser = appUserService.login(request);
      string token = securityService.BuildJwtToken(appUser);
      return Ok(token);
    }

  }
}