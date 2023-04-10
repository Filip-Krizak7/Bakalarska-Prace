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
        private readonly AppUserService appUserService;

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login([FromBody] UserLoginDto request)
        {
            try
            {
                appUserService.login(request, HttpContext);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();
        }
    }
}