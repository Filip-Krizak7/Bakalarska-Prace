using TeacherPractise.Config;
using TeacherPractise.Dto.Response;
using TeacherPractise.Dto.Request;
using TeacherPractise.Service;
using TeacherPractise.Model;
using Microsoft.AspNetCore.Mvc;

namespace TeacherPractise.Service
{
    public class RegistrationService
    {
        private AppUserService appUserService = new AppUserService();
        private SchoolService schoolService = new SchoolService();
        
        public String register(RegistrationDto request)
        {
            AppUserService.EnsureNotNull(request.email, nameof(request.email));
            AppUserService.EnsureNotNull(request.firstName, nameof(request.firstName));
            AppUserService.EnsureNotNull(request.lastName, nameof(request.lastName));
            AppUserService.EnsureNotNull(request.school.ToString(), nameof(request.school));
            AppUserService.EnsureNotNull(request.phoneNumber, nameof(request.phoneNumber));
            AppUserService.EnsureNotNull(request.password, nameof(request.password));
            AppUserService.EnsureNotNull(request.role, nameof(request.role));

            String email, password, firstName, lastName, phoneNumber;
            School sch;
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
                    sch = null;
                    role = Roles.ROLE_STUDENT;
                    locked = false;
                    enabled = true;
                    break;
                case "teacher":
                    email = request.email;
                    password = request.password;
                    firstName = request.firstName;
                    lastName = request.lastName;
                    phoneNumber = request.phoneNumber;
                    sch = this.schoolService.getSchoolById(request.school); 
                    role = Roles.ROLE_TEACHER;
                    locked = false;
                    enabled = true;
                    break;
                case "coordinator":
                    email = request.email;
                    password = request.password;
                    firstName = request.firstName;
                    lastName = request.lastName;
                    phoneNumber = request.phoneNumber;
                    sch = null; 
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
                    sch = null;
                    role = Roles.ROLE_ADMIN;
                    locked = false;
                    enabled = true;
                    break;
                default:
                    throw new Exception("Incorrect role that cannot be converted to enum.");
            }

            String token = appUserService.SignUpUser(new User(email, password, firstName, lastName, sch, phoneNumber, role, locked, enabled));

            /*
            poslani emailu s potvrzenim
            */
            
            return "Success";
        }
    }
}