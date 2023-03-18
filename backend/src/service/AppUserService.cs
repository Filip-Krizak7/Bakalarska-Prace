using TeacherPractise.Model;
using TeacherPractise.Service;
using TeacherPractise.Config;
using Microsoft.AspNetCore.Mvc;

namespace TeacherPractise.Service
{
    public class AppUserService
    {
        //private readonly List<User> inner = new();
        private readonly SecurityService securityService;
    
        public AppUserService(SecurityService securityService)
        {
            this.securityService = securityService;
        }

        public User Create(User user)
        {
            EnsureNotNull(user.Username, nameof(user.Username));
            EnsureNotNull(user.Username, nameof(user.Username));

            String username = user.Username.ToLower();

            //User temp = new User(username, firstName, lastName, school, phoneNumber, password, role);

            /*string hash = this.securityService.HashPassword(password);
            temp.Password = hash;*/

            using (var ctx = new Context())
            {
                if (ctx.Users.ToList().Any(q => q.Username == username))
                throw CreateException($"Username {username} already exists.", null);

                ctx.Users.Add(user);
                ctx.SaveChanges();
            }

            return user;
        }

        public String SignUpUser(User user)
        {
            Create(user);

            String token = securityService.BuildJwtToken(user); //dodelat --------------------------------------

            return token;
        }

        public List<User> GetUsers()
        {
            using (var ctx = new Context())
            {
                return ctx.Users.ToList();     
            }
        }

        public User GetUserByCredentials(string username, string password)
        {
            EnsureNotNull(username, nameof(username));
            EnsureNotNull(password, nameof(password));

            using (var ctx = new Context())
	        {
		        User appUser = ctx.Users.ToList().FirstOrDefault(q => q.Username == username.ToLower())
                	?? throw CreateException($"Username {username} does not exist.");

            	if (!this.securityService.VerifyPassword(password, appUser.Password))
                	throw CreateException($"Credentials are not valid.");

            	return appUser;
            }
        }

        public static void EnsureNotNull(string value, string parameterName)
        {
            if (value == null)
                throw CreateException($"Parameter {parameterName} cannot be null.");
        }

        private static ServiceException CreateException(string message, Exception? innerException = null) =>
            new(typeof(AppUserService), message, innerException);
    }
}