using TeacherPractise.Model;
using TeacherPractise.Config;
using TeacherPractise.Service.FileManagement;
using TeacherPractise.Service.Token.RegistrationToken;
using TeacherPractise.Dto.Request;
using TeacherPractise.Dto.Response;
using TeacherPractise.Mapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Newtonsoft.Json;

using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IdentityModel.Tokens.Jwt;

namespace TeacherPractise.Service
{
    public class AppUserService
    {
        private readonly SecurityService securityService;
        private readonly CustomMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ConfirmationTokenService confirmationTokenService;
    
        public AppUserService(
            [FromServices] SecurityService securityService, 
            [FromServices] CustomMapper mapper, 
            [FromServices] IHttpContextAccessor httpContextAccessor,
            [FromServices] ConfirmationTokenService confirmationTokenService)
        {
            this.securityService = securityService;
            this.mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
            this.confirmationTokenService = confirmationTokenService;
        }

        public User create(User user)
        {
            ensureNotNull(user.Username, nameof(user.Username));
            ensureNotNull(user.Password, nameof(user.Password));

            String username = user.Username.ToLower();

            using (var ctx = new Context())
            {
                if (ctx.Users.ToList().Any(q => q.Username == username))
                throw CreateException($"Username {username} already exists.", null);

                string hash = this.securityService.HashPassword(user.Password);
                user.Password = hash;

                ctx.Users.Add(user);
                ctx.SaveChanges();
            }

            return user;
        }

        public String signUpUser(User user)
        {
            create(user);

            string token = this.securityService.BuildJwtToken(user);

            ConfirmationToken confirmationToken = new ConfirmationToken(
                user.Id,
                token,
                DateTime.Now,
                DateTime.Now.AddMinutes(AppConfig.REGISTRATION_CONFIRMATION_TOKEN_EXPIRY_TIME)
            );
            confirmationTokenService.saveConfirmationToken(confirmationToken);

            return token;
        }

        public List<User> getUsers()
        {
            using (var ctx = new Context())
            {
                return ctx.Users.ToList();     
            }
        }

        public User getUserByUsername(string username)
        {
            using (var ctx = new Context())
	        {
		        User appUser = ctx.Users.Where(q => q.Username == username.ToLower()).FirstOrDefault()
                	?? throw CreateException($"Username {username} does not exist.");

            	return appUser;
            }
        }

        public User getUserByCredentials(string username, string password)
        {
            ensureNotNull(username, nameof(username));
            ensureNotNull(password, nameof(password));

            using (var ctx = new Context())
	        {
		        User appUser = ctx.Users.ToList().FirstOrDefault(q => q.Username == username.ToLower())
                	?? throw CreateException($"Username {username} does not exist.");

            	if (!this.securityService.VerifyPassword(password, appUser.Password))
                	throw CreateException($"Credentials are not valid.");

            	return appUser;
            }
        }

        public static void ensureNotNull(string value, string parameterName)
        {
            if (value == null)
                throw CreateException($"Parameter {parameterName} cannot be null.");
        }

        public static ServiceException CreateException(string message, Exception? innerException = null) =>
            new(typeof(AppUserService), message, innerException);

        public bool checkEmail(string email, Roles role)
        {
            string patternStudent = @"^[A-Za-z0-9._%+-]+@student.osu.cz$";
            string patternTeacher = @"^(.+)@(.+)$";
            
            if(role == Roles.ROLE_STUDENT)
            {
                Match m = Regex.Match(email, patternStudent, RegexOptions.IgnoreCase);
                return m.Success;
            }
            else
            {
                Match m = Regex.Match(email, patternTeacher, RegexOptions.IgnoreCase);
                return m.Success;
            }
        }

        public User login(UserLoginDto request)
        {
            User appUser;

            try
            {
                appUser = getUserByCredentials(request.username, request.password);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

            return appUser;
        }

        public string getPracticeReport(long id)
        {
            using (var ctx = new Context())
            {
                var practice = ctx.Practices.FirstOrDefault(p => p.PracticeId == id);

                if (practice != null)
                {
                    var folderPath = $"{FileUtil.reportsFolderPath}{id}";

                    if (!Directory.Exists(folderPath))
                    {
                        try
                        {
                            Directory.CreateDirectory(folderPath);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Nepodařilo se vytvořit adresář: {ex.Message}");
                            return null;
                        }
                    }
                    var files = Directory.GetFiles(folderPath);

                    if (files == null || files.Length == 0)
                        return null;

                    return Path.GetFileName(files[0]);
                }
                else
                {
                    throw CreateException("Praxe nebyla nalezena.");
                } 
            }
        }

        public List<string> getTeacherFiles(string teacherEmail)
        {
            using (var ctx = new Context())
	        {
		        long id = ctx.Users.ToList().FirstOrDefault(q => q.Username == teacherEmail.ToLower()).Id;

                if (id != null)
                {
                    string folderPath = Path.Combine(FileUtil.folderPath, id.ToString());
                    DirectoryInfo folder = new DirectoryInfo(folderPath);

                    if (!folder.Exists)
                        return new List<string>();

                    var files = folder.GetFiles();

                    List<string> list = new List<string>();
                    foreach (var file in files)
                    {
                        if (file.Exists)
                        {
                            list.Add(file.Name);
                        }
                    }

                    return list;
                }           
                else
                {
                    throw CreateException($"Učitel s mailem {teacherEmail} nenalezen.");
                }
            }
        }

        public User getUserById(long id)
        {
            using (var ctx = new Context())
	        {
		        User appUser = ctx.Users.ToList().FirstOrDefault(q => q.Id == (int)id)
                	?? throw CreateException($"Uživatel s ID {id} neexistuje.");

            	return appUser;
            }
        }

        public List<SubjectDto> getSubjects()
        {
            using (var ctx = new Context())
	        {
                var subjects = ctx.Subjects.ToList();
                
                return mapper.subjectsToSubjectDtos(subjects);
            }
        }

        public List<UserDto> getCoordinators()
        {
            using (var ctx = new Context())
	        {
                var coordinators = ctx.Users.Where(q => q.Role == Roles.ROLE_COORDINATOR).ToList();
                return mapper.usersToUserDtos(coordinators);
            }
        }

        public List<SchoolDto> getSchools()
        {
            using (var ctx = new Context())
	        {
                var schools = ctx.Schools.ToList();
                return mapper.schoolsToSchoolDtos(schools);
            }
        }

        public List<UserDto> getTeachers()
        {
            using (var ctx = new Context())
	        {
                var teachers = ctx.Users.Where(q => q.Role == Roles.ROLE_TEACHER).ToList();
                return mapper.usersToUserDtos(teachers);
            }
        }

        public List<UserDto> getStudents()
        {
            using (var ctx = new Context())
	        {
                var students = ctx.Users.Where(q => q.Role == Roles.ROLE_STUDENT).ToList();
                return mapper.usersToUserDtos(students);
            }
        }

        public Dictionary<long, string> getAllReviews()
        {
            List<ReviewDto> reviews = new List<ReviewDto>();
            Dictionary<long, string> practicesAndNames = new Dictionary<long, string>();

            using (var ctx = new Context())
	        {
                List<Review> allReviews = ctx.Reviews.ToList();

                foreach (Review r in allReviews)
                {
                    ReviewDto revDto = mapper.reviewToReviewDto(r);
                    reviews.Add(revDto);

                    User temp = getUserById((long)r.UserId);
                    practicesAndNames.Add(r.PracticeId, $"{temp.FirstName} {temp.SecondName}");
                }
                return practicesAndNames;
            }        
        }

        public string getCurrentUserEmail()
        {
            string authHeader = httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();

            if (string.IsNullOrEmpty(authHeader))
            {
                return "No one is logged in!";
            }
            else{
                string token = authHeader.Split(' ').Last().Replace("Bearer ", "");

                string currentEmail = new JwtSecurityTokenHandler()
                    .ReadJwtToken(token)
                    .Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

                return currentEmail;
            }
        }

        public string removeUser(string username)
        {
            using (var ctx = new Context())
	        {
                User user = ctx.Users.Where(q => q.Username == username.ToLower()).FirstOrDefault();
                if (user != null)
                {
                    //Console.WriteLine("before token removal" + " " + username);
                    confirmationTokenService.deleteConfirmationTokenById(user.Id);
                    ctx.Users.Remove(user);
                    int rowsAffected = ctx.SaveChanges();
                    if (rowsAffected == 1) return "User deleted";
                    else return "Something went wrong";
                }
                else return "User was not deleted";
            }
        }

        public string unlockUser(string username) 
        {
            using (var ctx = new Context())
	        {
                User user = ctx.Users.Where(q => q.Username == username.ToLower()).FirstOrDefault();
                if (user != null) {
                    user.Locked = false;
                    ctx.SaveChanges();
                    return "User unlocked";
                }
                return "Email not found";
            }
        }

        public string signUpCoordinator(User user)
        {
            create(user);
            return "Koordinátor byl vytvořen";
        }

        public int enableAppUser(string email)
        {
            using (var ctx = new Context())
	        {
                User user = ctx.Users.Where(q => q.Username == email.ToLower()).FirstOrDefault();
                user.Enabled = true;
                return ctx.SaveChanges();
            }
        }

        public ReviewDto getStudentReview(string username, long practiceId){
            using (var ctx = new Context())
	        {
                User student = ctx.Users.Where(q => q.Username == username.ToLower()).FirstOrDefault();
                if(student != null)
                {
                    Review rev = ctx.Reviews.Where(q => q.UserId == student.Id && q.PracticeId == (int)practiceId).FirstOrDefault();
                    if(rev != null) 
                    {
                        ReviewDto revDto = new ReviewDto();
                        revDto.practiceId = practiceId;
                        revDto.name = student.FirstName + " " + student.SecondName;
                        revDto.reviewText = rev.Text;
                        return revDto;
                    }
                }
                return null;
            }
        }
    }
}