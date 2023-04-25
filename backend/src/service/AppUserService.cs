using TeacherPractise.Model;
using TeacherPractise.Config;
using TeacherPractise.Service.FileManagement;
using TeacherPractise.Dto.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace TeacherPractise.Service
{
    public class AppUserService
    {
        private readonly SecurityService securityService;
    
        public AppUserService([FromServices] SecurityService securityService)
        {
            this.securityService = securityService;
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

            String token = this.securityService.BuildJwtToken(user);

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
            ensureNotNull(username, nameof(username));

            using (var ctx = new Context())
	        {
		        User appUser = ctx.Users.ToList().FirstOrDefault(q => q.Username == username.ToLower())
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
                var practice = ctx.Practices.FirstOrDefault(p => p.Id == id);  //neexistuje practices? 

                if (practice != null)
                {
                    var folderPath = $"{FileUtil.reportsFolderPath}{id}";
                    var files = Directory.GetFiles(folderPath);

                    if (files.Length > 0)
                    {
                        return Path.GetFileName(files[0]);
                    }

                    return null;
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
		        long id = ctx.Users.ToList().FirstOrDefault(q => q.Username == teacherEmail.ToLower()).Id

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
    }
}