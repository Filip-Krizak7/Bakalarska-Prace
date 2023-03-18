using TeacherPractise.Service;
using TeacherPractise.Dto.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TeacherPractise.Controller
{
    [Route("api/register")]
    [AllowAnonymous]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly RegistrationService regService;
        private readonly AppUserService appUserService;

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Register(RegistrationDto request)
        {
            string ret = regService.register(request);
            Console.WriteLine(ret);
            return Ok(ret);
        }
    }
}