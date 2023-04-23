using TeacherPractise.Model;
using TeacherPractise.Service;
using TeacherPractise.Config;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
    //[Authorize(Role = AppConfig.ADMIN_ROLE_NAME)] 
    [AllowAnonymous]
    public IActionResult getAll()
    {
      List<User> ret = appUserService.getUsers();
      return Ok(ret);
    }
  }
}