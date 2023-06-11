using TeacherPractise.Service;
using TeacherPractise.Dto.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using TeacherPractise.Config;
using TeacherPractise.Model;
using TeacherPractise.Service.Email;
using TeacherPractise.Service.Token.RegistrationToken;
using TeacherPractise.Service.Token.PasswordResetToken;
using Newtonsoft.Json;
using System.Net.Mail;
using Microsoft.AspNetCore.Cors;

namespace TeacherPractise.Controller
{
    [Route("forgotPassword")]
    [AllowAnonymous]
    [ApiController]
    public class ForgotPasswordController : ControllerBase
    {
        private readonly SecurityService securityService;
        private readonly AppUserService appUserService;
        private readonly EmailService emailService;
        private readonly ForgotPasswordService forgotPasswordService;

        public ForgotPasswordController(
        [FromServices] AppUserService appUserService,
        [FromServices] SecurityService securityService,
        [FromServices] EmailService emailService,
        [FromServices] ForgotPasswordService forgotPasswordService)
        {
            this.appUserService = appUserService;
            this.securityService = securityService;
            this.emailService = emailService;
            this.forgotPasswordService = forgotPasswordService;
        }

        [HttpPost("reset")]
        public IActionResult resetPassword([FromBody] ResetPasswordRequestDto request)
        {
            using (var ctx = new Context())
	        {
		        string userEmail = request.email.Replace("\"", "");
                User user = ctx.Users.Where(q => q.Username == userEmail.ToLower()).FirstOrDefault();

                if (user == null)
                {
                    return Ok("Na zadaný e-mail (pokud existuje) byl poslán odkaz pro obnovu hesla");
                }
                string token = Guid.NewGuid().ToString();
                appUserService.createPasswordResetTokenForUser(user.Id, token);
                string link = AppConfig.BASE_DNS_PRODUCTION + "/login?forgotPasswordToken=" + token;

                emailService.sendForgotPasswordMail(
                    userEmail, 
                    forgotPasswordService.buildEmail(user.FirstName, link));

                return Ok("Na zadaný e-mail (pokud existuje) byl poslán odkaz pro obnovu hesla");
            }
        }

        [HttpPost("save")]
        public IActionResult savePassword([FromBody] ForgotPasswordDto passwordDto)
        {
            bool result = securityService.ValidatePasswordResetToken(passwordDto.token);

            if (!result)
            {
                return BadRequest("Neplatný ověřovací odkaz.");
            }

            var user = appUserService.getUserByPasswordResetToken(passwordDto.token);

            if (user != null)
            {
                appUserService.changeUserPassword(user, passwordDto.newPassword);
                appUserService.deleteByToken(passwordDto.token);
                return Ok("Heslo bylo změněno úspěšně");
            }
            else
            {
                return BadRequest("Došlo k chybě při změně hesla");
            }
        }

        private bool IsTokenFound(ConfirmationToken passToken)
        {
            return passToken != null;
        }

        private bool IsTokenExpired(ConfirmationToken passToken)
        {
            return passToken.ExpiresAt < DateTime.Now;
        }

        private MailMessage constructResetTokenEmail(string contextPath, string token, User user)
        {
            string url = contextPath + "/user/changePassword?token=" + token;
            string message = "Změna hesla:";
            return constructEmail("Reset Password", message + " \r\n" + url, user);
        }

        private MailMessage constructEmail(string subject, string body, User user)
        {
            MailMessage email = new MailMessage();
            email.Subject = subject;
            email.Body = body;
            email.To.Add(user.Username);
            email.From = new MailAddress("support");
            return email;
        }
    }
}