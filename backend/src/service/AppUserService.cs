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

        public String SignUpUser(User user)
        {
            Create(user);

            String token = this.securityService.BuildJwtToken(user);

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

        public static ServiceException CreateException(string message, Exception? innerException = null) =>
            new(typeof(AppUserService), message, innerException);
    }
}