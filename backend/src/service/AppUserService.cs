using TeacherPractise.Model;
using TeacherPractise.Config;
using Microsoft.AspNetCore.Mvc;

namespace TeacherPractise.Service
{
    public class AppUserService
    {
        private readonly List<User> inner = new();
        private readonly SecurityService securityService;

        public AppUserService(SecurityService securityService)
        {
            this.securityService = securityService;
        }

        public User Create(string username, string firstName, string lastName, School school,
            string phoneNumber, string password, Roles role)
        {
            EnsureNotNull(username, nameof(username));
            EnsureNotNull(username, nameof(username));

            username = username.ToLower();

            User temp = new User(username, firstName, lastName, school, phoneNumber, password, role);

            string hash = this.securityService.HashPassword(password);
            temp.Password = hash;

            using (var ctx = new Context())
            {
                if (ctx.Users.ToList().Any(q => q.Username == username))
                throw CreateException($"Username {username} already exists.", null);

                ctx.Users.Add(temp);
                ctx.SaveChanges();
            }

            return temp;
        }

        public List<User> GetUsers()
        {
            using (var ctx = new Context())
            {
                return ctx.Users.ToList();     
            }
            //return this.inner.ToList();
        }

        public User GetUserByCredentials(string username, string password)
        {
            EnsureNotNull(username, nameof(username));
            EnsureNotNull(password, nameof(password));

            User appUser = inner.FirstOrDefault(q => q.Username == username.ToLower()) // -----------------------predelat na cteni z databaze
                ?? throw CreateException($"Username {username} does not exist.");

            if (!this.securityService.VerifyPassword(password, appUser.Password))
                throw CreateException($"Credentials are not valid.");

            return appUser;
        }

        private static void EnsureNotNull(string value, string parameterName)
        {
            if (value == null)
                throw CreateException($"Parameter {parameterName} cannot be null.");
        }

        private static ServiceException CreateException(string message, Exception? innerException = null) =>
            new(typeof(AppUserService), message, innerException);
    }
}