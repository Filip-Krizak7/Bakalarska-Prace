using TeacherPractise.Model;
using TeacherPractise.Config;
using Microsoft.AspNetCore.Mvc;

namespace TeacherPractise.Service
{
    public class AppUserService
    {
        private readonly List<AppUser> inner = new();
        private readonly SecurityService securityService;

        public AppUserService(SecurityService securityService)
        {
            this.securityService = securityService;
        }

        public AppUser Create(User user)
        {
            EnsureNotNull(user.Username, nameof(user.Username));
            EnsureNotNull(user.Password, nameof(user.Password));

            user.Username = user.Username.ToLower();

            if (inner.Any(q => q.Username == user.Username))
                throw CreateException($"Username {user.Username} already exists.", null);

            string hash = this.securityService.HashPassword(user.Password);

            AppUser ret = new(user.Username, hash);
            if (user.Role == Roles.ROLE_TEACHER) ret.Roles.Add(AppConfig.ADMIN_ROLE_NAME);
            this.inner.Add(ret);

            return ret;
        }

        public List<AppUser> GetUsers()
        {
            return this.inner.ToList();
        }

        public AppUser GetUserByCredentials(string username, string password)
        {
            EnsureNotNull(username, nameof(username));
            EnsureNotNull(password, nameof(password));

            AppUser appUser = inner.FirstOrDefault(q => q.Username == username.ToLower())
                ?? throw CreateException($"Username {username} does not exist.");

            if (!this.securityService.VerifyPassword(password, appUser.PasswordHash))
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