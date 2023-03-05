using TeacherPractise.Model;
using Microsoft.AspNetCore.Mvc;

namespace TeacherPractise.Service
{
    public class AppUserService
    {
        private readonly List<User> inner = new();
        private readonly SecurityService securityService;

        public AppUserService([Fromservice] SecurityService securityService)
        {
            this.securityService = securityService;
        }

        public AppUser Create(User user)
        {
            EnsureNotNull(user.email, nameof(user.email));
            EnsureNotNull(user.password, nameof(user.password));

            user.email = user.email.ToLower();

            if (inner.Any(q => q.Email == user.email))
                throw CreateException($"Email {user.email} already exists.", null);

            string hash = this.securityService.HashPassword(user.password);

            AppUser ret = new(user.email, hash);
            if (user.Role_id == 1) ret.Roles.Add(AppUser.ADMIN_ROLE_NAME);
            this.inner.Add(ret);

            return ret;
        }

        public List<AppUser> GetUsers()
        {
            return this.inner.ToList();
        }

        public AppUser GetUserByCredentials(string email, string password)
        {
            EnsureNotNull(email, nameof(email));
            EnsureNotNull(password, nameof(password));

            AppUser appUser = inner.FirstOrDefault(q => q.Email == email.ToLower())
                ?? throw CreateException($"Email {email} does not exist.");

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