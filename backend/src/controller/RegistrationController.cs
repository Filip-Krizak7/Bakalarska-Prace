using TeacherPractise.Service;
using TeacherPractise.Dto.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TeacherPractise.Controller
{
    [Route("register")]
    [AllowAnonymous]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly SecurityService securityService;
        private readonly RegistrationService regService;
        private readonly AppUserService appUserService;

        public RegistrationController(
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
        public IActionResult Register(RegistrationDto request)
        {
            string ret;
            try
            {
                ret = regService.register(request);
                Console.WriteLine(ret);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(ex.Message);
            }
            return Ok(ret);
        }
    }
}