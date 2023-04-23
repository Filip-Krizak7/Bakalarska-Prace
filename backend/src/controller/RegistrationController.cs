using TeacherPractise.Service;
using TeacherPractise.Model;
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
        private readonly SchoolService schoolService;

        public RegistrationController(
        [FromServices] AppUserService appUserService,
        [FromServices] SecurityService securityService,
        [FromServices] RegistrationService regService,
        [FromServices] SchoolService schoolService)
        {
            this.appUserService = appUserService;
            this.securityService = securityService;
            this.regService = regService;
            this.schoolService = schoolService;
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult register(RegistrationDto request)
        {
            string ret;
            try
            {
                ret = regService.register(request);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(ex.Message);
            }
            return Ok(ret);
        }

        [HttpGet("schools")]
        [AllowAnonymous]
        public IActionResult getAll()
        {
            List<School> ret = schoolService.getSchools();
            return Ok(ret);
        }
    }
}