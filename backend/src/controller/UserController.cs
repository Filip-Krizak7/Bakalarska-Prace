using TeacherPractise.Model;
using TeacherPractise.Service;
using TeacherPractise.Config;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TeacherPractise.Controller
{
  [Route("api/user")]
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
    public IActionResult GetUserNames() 
    {
      List<string> ret = appUserService.GetUsers()
        .Select(q => q.Username[..q.Username.IndexOf("@")])
        .ToList();
      return Ok(ret);
    }

    [HttpGet("emails")]
    public IActionResult GetUserEmails() 
    {
      List<string> ret = appUserService.GetUsers()
        .Select(q => q.Username)
        .ToList();
      return Ok(ret);
    }

    [HttpGet("all")]
    //[Authorize(Role = AppConfig.ADMIN_ROLE_NAME)] 
    [AllowAnonymous]
    public IActionResult GetAll()
    {
      List<User> ret = appUserService.GetUsers();
      return Ok(ret);
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public IActionResult Login(string username, string password)
    {
      User appUser;
      try
      {
        appUser = this.appUserService.GetUserByCredentials(username, password);
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }

      string token = securityService.BuildJwtToken(appUser);
      return Ok(token);
    }
  }
}