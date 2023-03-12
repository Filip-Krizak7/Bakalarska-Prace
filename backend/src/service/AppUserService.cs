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

        public User Create(User user)
        {
            EnsureNotNull(user.Username, nameof(user.Username));
            EnsureNotNull(user.Password, nameof(user.Password));

            user.Username = user.Username.ToLower();

            if (inner.Any(q => q.Username == user.Username))
                throw CreateException($"Username {user.Username} already exists.", null);

            string hash = this.securityService.HashPassword(user.Password);
            user.Password = hash;

            this.inner.Add(user);

            return user;
        }

        public List<User> GetUsers()
        {
            return this.inner.ToList();
        }

        public User GetUserByCredentials(string username, string password)
        {
            EnsureNotNull(username, nameof(username));
            EnsureNotNull(password, nameof(password));

            User appUser = inner.FirstOrDefault(q => q.Username == username.ToLower())
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