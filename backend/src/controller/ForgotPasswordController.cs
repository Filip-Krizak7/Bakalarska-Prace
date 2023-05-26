using TeacherPractise.Service;
using TeacherPractise.Dto.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using TeacherPractise.Config;
using TeacherPractise.Model;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Cors;

namespace TeacherPractise.Controller
{
    [Route("forgotPassword")]
    [AllowAnonymous]
    [ApiController]
    public class ForgotPasswordController : ControllerBase
    {
        private readonly SecurityService securityService;
        private readonly RegistrationService regService;
        private readonly AppUserService appUserService;
        private readonly ForgotPasswordService forgotPasswordService;

        public ForgotPasswordController(
        [FromServices] AppUserService appUserService,
        [FromServices] SecurityService securityService,
        [FromServices] RegistrationService regService,
        [FromServices] ForgotPasswordService forgotPasswordService)
        {
            this.appUserService = appUserService;
            this.securityService = securityService;
            this.regService = regService;
            this.forgotPasswordService = forgotPasswordService;
        }


    }
}