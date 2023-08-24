using TeacherPractise.Service;
using TeacherPractise.Dto.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using TeacherPractise.Config;
using TeacherPractise.Model;
using Newtonsoft.Json;

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
        public async Task<IActionResult> login(UserLoginDto request)
        {
            if (request.password == null || request.username == null) {
                throw new UnauthorizedAccessException($"Uživatelské jméno nebo heslo nevyplněno.");
            }
            User appUser = appUserService.login(request);
            string token = securityService.BuildJwtToken(appUser);

            HttpContext.Response.Cookies.Append(SecurityConfig.COOKIE_NAME, token, new CookieOptions
            {
                Expires = DateTime.Now.AddSeconds(SecurityConfig.COOKIE_EXPIRATION_SECONDS),
                HttpOnly = SecurityConfig.COOKIE_HTTP_ONLY,
                Secure = SecurityConfig.COOKIE_SECURE,
            });

            HttpContext.Response.StatusCode = 200;
            HttpContext.Response.ContentType = "application/json";

            var responseJson = JsonConvert.SerializeObject(new Dictionary<string, string> { { "role", appUser.Role.ToString() } });

            await HttpContext.Response.WriteAsync(responseJson);

            return new EmptyResult();
        }
    }
}