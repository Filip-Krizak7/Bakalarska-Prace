using TeacherPractise.Service;
using TeacherPractise.Dto.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace TeacherPractise.Controller
{
    [Route("login")]
    [AllowAnonymous]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly SecurityService securityService;
        private readonly RegistrationService regService;
        private readonly AppUserService appUserService;

        public AuthController(
        [FromServices] AppUserService appUserService,
        [FromServices] SecurityService securityService,
        [FromServices] RegistrationService regService)
        {
            this.appUserService = appUserService;
            this.securityService = securityService;
            this.regService = regService;
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login(UserLoginDto request)
        {
            try
            {
                appUserService.login(request, HttpContext);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(ex.Message);
            }
            return Ok();
        }
    }
}