namespace TeacherPractise.Config
{
    public class SecurityConfig
    {
        public const string JWT_SECRET_KEY = "secret-key";
        public const int JWT_TOKEN_EXPIRATION_DAYS = 14;

        // cookie containing jwt token
        public const string COOKIE_NAME = "access_token";
        public const bool COOKIE_HTTP_ONLY = false;
        public const bool COOKIE_SECURE = false;
        public const int COOKIE_EXPIRATION_SECONDS = JWT_TOKEN_EXPIRATION_DAYS * 24 * 60 * 60;
    }
}