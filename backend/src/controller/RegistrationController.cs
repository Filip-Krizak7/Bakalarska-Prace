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
        private readonly RegistrationService regService;
        private readonly SchoolService schoolService;

        public RegistrationController(
        [FromServices] RegistrationService regService,
        [FromServices] SchoolService schoolService)
        {
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

        [HttpGet("confirm")]
        [AllowAnonymous]
        public IActionResult confirmUser(string token)
        {
            return Ok(regService.confirmToken(token));
        }
    }
}