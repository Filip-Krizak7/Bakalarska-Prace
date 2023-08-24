using TeacherPractise.Config;
using TeacherPractise.Dto.Request;
using TeacherPractise.Service.Email;
using TeacherPractise.Model;
using TeacherPractise.Service.Token.RegistrationToken;
using Microsoft.AspNetCore.Mvc;

namespace TeacherPractise.Service
{
    public class RegistrationService
    {
        private readonly AppUserService appUserService;
        private readonly SchoolService schoolService;
        private readonly EmailService emailService;
        private readonly ConfirmationTokenService confirmationTokenService;

        public RegistrationService(
            [FromServices] AppUserService appUserService, 
            [FromServices] SchoolService schoolService,
            [FromServices] EmailService emailService,
            [FromServices] ConfirmationTokenService confirmationTokenService)
        {
            this.appUserService = appUserService;
            this.schoolService = schoolService;
            this.confirmationTokenService = confirmationTokenService;
            this.emailService = emailService;
        }
        
        public string register(RegistrationDto request)
        {
            AppUserService.ensureNotNull(request.email, nameof(request.email));
            AppUserService.ensureNotNull(request.firstName, nameof(request.firstName));
            AppUserService.ensureNotNull(request.lastName, nameof(request.lastName));
            AppUserService.ensureNotNull(request.password, nameof(request.password));

            string email, password, firstName, lastName, phoneNumber;
            int schId = (int)request.school;
            Roles role;
            bool locked, enabled;

            switch (request.role)
            {
                case "student":
                    email = request.email;
                    password = request.password;
                    firstName = request.firstName;
                    lastName = request.lastName;
                    phoneNumber = null;
                    role = Roles.ROLE_STUDENT;
                    locked = true;
                    enabled = false;
                    break;
                case "teacher":
                    email = request.email;
                    password = request.password;
                    firstName = request.firstName;
                    lastName = request.lastName;
                    phoneNumber = request.phoneNumber;
                    schId = (int)request.school;
                    role = Roles.ROLE_TEACHER;
                    locked = true;
                    enabled = false;
                    break;
                case "coordinator":
                    email = request.email;
                    password = request.password;
                    firstName = request.firstName;
                    lastName = request.lastName;
                    phoneNumber = request.phoneNumber;
                    role = Roles.ROLE_COORDINATOR;
                    locked = false;
                    enabled = true;
                    break;
                case "admin":
                    email = request.email;
                    password = request.password;
                    firstName = request.firstName;
                    lastName = request.lastName;
                    phoneNumber = request.phoneNumber;
                    role = Roles.ROLE_ADMIN;
                    locked = false;
                    enabled = true;
                    break;
                default:
                    throw new Exception("Incorrect role that cannot be converted to enum.");
            }

            if(!(appUserService.checkEmail(email, role)))
            throw AppUserService.CreateException($"Email is in the wrong format.", null);

            string token;
            
            if(role == Roles.ROLE_STUDENT)
            {
                token = appUserService.signUpUser(new User(email, password, firstName, lastName, phoneNumber, role, locked, enabled));
            }
            else 
            {
                token = appUserService.signUpUser(new User(email, password, firstName, lastName, schId, phoneNumber, role, locked, enabled));
            }
                
            string link = AppConfig.BASE_DNS_PRODUCTION + "/login?token=" + token;
            emailService.send( 
                request.email,
                buildEmail(request.firstName, link));
            
            return "Success";
        }

        public string confirmToken(string token)
        {
            ConfirmationToken confirmationToken = confirmationTokenService.getToken(token);

            if (confirmationToken == null)
            {
                throw new InvalidOperationException("Token nenalezen.");
            }

            if (confirmationToken.ConfirmedAt != null)
            {
                throw new InvalidOperationException("Účet již byl potvrzen.");
            }

            if (confirmationToken.ExpiresAt < DateTime.Now)
            {
                throw new InvalidOperationException("Potvrzovací odkaz vypršel.");
            }

            confirmationTokenService.setConfirmedAt(token);

            using (var ctx = new Context())
            {
                var user = ctx.Users.Where(q => q.Id == confirmationToken.ConfirmationTokenId).FirstOrDefault()
                    ?? throw AppUserService.CreateException($"User with ID {confirmationToken.ConfirmationTokenId} does not exist.");

                appUserService.enableAppUser(user.Username);
            }

            return "E-mail byl úspěšně ověřen.";
        }

        private string buildEmail(string name, string link) 
        {
            return "<div style=\"font-family:Helvetica,Arial,sans-serif;font-size:16px;margin:0;color:#0b0c0c\">\n" +
                    "\n" +
                    "<span style=\"display:none;font-size:1px;color:#fff;max-height:0\"></span>\n" +
                    "\n" +
                    "  <table role=\"presentation\" width=\"100%\" style=\"border-collapse:collapse;min-width:100%;width:100%!important\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\">\n" +
                    "    <tbody><tr>\n" +
                    "      <td width=\"100%\" height=\"53\" bgcolor=\"#0b0c0c\">\n" +
                    "        \n" +
                    "        <table role=\"presentation\" width=\"100%\" style=\"border-collapse:collapse;max-width:580px\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\" align=\"center\">\n" +
                    "          <tbody><tr>\n" +
                    "            <td width=\"70\" bgcolor=\"#0b0c0c\" valign=\"middle\">\n" +
                    "                <table role=\"presentation\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\" style=\"border-collapse:collapse\">\n" +
                    "                  <tbody><tr>\n" +
                    "                    <td style=\"padding-left:10px\">\n" +
                    "                  \n" +
                    "                    </td>\n" +
                    "                    <td style=\"font-size:28px;line-height:1.315789474;Margin-top:4px;padding-left:10px\">\n" +
                    "                      <span style=\"font-family:Helvetica,Arial,sans-serif;font-weight:700;color:#ffffff;text-decoration:none;vertical-align:top;display:inline-block\">Potvrďte svou registraci</span>\n" +
                    "                    </td>\n" +
                    "                  </tr>\n" +
                    "                </tbody></table>\n" +
                    "              </a>\n" +
                    "            </td>\n" +
                    "          </tr>\n" +
                    "        </tbody></table>\n" +
                    "        \n" +
                    "      </td>\n" +
                    "    </tr>\n" +
                    "  </tbody></table>\n" +
                    "  <table role=\"presentation\" class=\"m_-6186904992287805515content\" align=\"center\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\" style=\"border-collapse:collapse;max-width:580px;width:100%!important\" width=\"100%\">\n" +
                    "    <tbody><tr>\n" +
                    "      <td width=\"10\" height=\"10\" valign=\"middle\"></td>\n" +
                    "      <td>\n" +
                    "        \n" +
                    "                <table role=\"presentation\" width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\" style=\"border-collapse:collapse\">\n" +
                    "                  <tbody><tr>\n" +
                    "                    <td bgcolor=\"#1D70B8\" width=\"100%\" height=\"10\"></td>\n" +
                    "                  </tr>\n" +
                    "                </tbody></table>\n" +
                    "        \n" +
                    "      </td>\n" +
                    "      <td width=\"10\" valign=\"middle\" height=\"10\"></td>\n" +
                    "    </tr>\n" +
                    "  </tbody></table>\n" +
                    "\n" +
                    "\n" +
                    "\n" +
                    "  <table role=\"presentation\" class=\"m_-6186904992287805515content\" align=\"center\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\" style=\"border-collapse:collapse;max-width:580px;width:100%!important\" width=\"100%\">\n" +
                    "    <tbody><tr>\n" +
                    "      <td height=\"30\"><br></td>\n" +
                    "    </tr>\n" +
                    "    <tr>\n" +
                    "      <td width=\"10\" valign=\"middle\"><br></td>\n" +
                    "      <td style=\"font-family:Helvetica,Arial,sans-serif;font-size:19px;line-height:1.315789474;max-width:560px\">\n" +
                    "        \n" +
                    "            <p style=\"Margin:0 0 20px 0;font-size:19px;line-height:25px;color:#0b0c0c\">Dobrý den, " + name + ",</p><p style=\"Margin:0 0 20px 0;font-size:19px;line-height:25px;color:#0b0c0c\"> Děkujeme za registraci. Otevřete prosím následující odkaz k potvrzení e-mailu: </p><blockquote style=\"Margin:0 0 20px 0;border-left:10px solid #b1b4b6;padding:15px 0 0.1px 15px;font-size:19px;line-height:25px\"><p style=\"Margin:0 0 20px 0;font-size:19px;line-height:25px;color:#0b0c0c\"> <a href=\"" + link + "\">Potvrzovací odkaz</a> </p></blockquote>\n Potvrzovací odkaz vyprší za " + AppConfig.REGISTRATION_CONFIRMATION_TOKEN_EXPIRY_TIME + " minut." +
                    "        \n" +
                    "      </td>\n" +
                    "      <td width=\"10\" valign=\"middle\"><br></td>\n" +
                    "    </tr>\n" +
                    "    <tr>\n" +
                    "      <td height=\"30\"><br></td>\n" +
                    "    </tr>\n" +
                    "  </tbody></table><div class=\"yj6qo\"></div><div class=\"adL\">\n" +
                    "\n" +
                    "</div></div>";
        }
    }
}