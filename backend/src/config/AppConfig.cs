namespace TeacherPractise.Config
{
    public class AppConfig
    {
        public const int MAKE_RESERVATION_DAYS_LEFT = 6;
        public const int CANCEL_RESERVATION_DAYS_LEFT = 6;
        public const int CREATE_PRACTICE_DAYS_LEFT = 7;
        public const int PRACTICE_MIN_CAPACITY = 1;
        public const int PRACTICE_MAX_CAPACITY = 10;
        public const int PRACTICE_NOTE_MAX_LENGTH = 250;
        public const int REGISTRATION_CONFIRMATION_TOKEN_EXPIRY_TIME = 60;
        public const int FORGOT_PASSWORD_TOKEN_EXPIRY_TIME = 60;
        //public const string CONFIRMATION_EMAIL_ADDRESS = "ucitelske.praxe.osu@seznam.cz";
        public const string CONFIRMATION_EMAIL_ADDRESS = "ucitelske.praxe.osu@gmail.com";
        public const string COORDINATOR_EMAIL_PASSWORD = "secret_passwd123";
        public const string CONFIRMATION_EMAIL_PASSWORD = "grvrwsfdmihpycke";
        public const int MAXIMUM_FILE_NUMBER_PER_USER = 3;
        public const int MAXIMUM_NUMBER_OF_REPORTS = 1;
        public const int TOKEN_EXPIRATION_IN_SECONDS = 600;
        public const string ADMIN_ROLE_NAME = "ROLE_ADMIN";
        public const string BASE_URL_DEVELOPMENT = "https://localhost:80";
        public const string BASE_URL_PRODUCTION = "https://172.16.101.118:80";
        public const string BASE_DNS_PRODUCTION = "http://localhost:80";
        //public const string BASE_DNS_PRODUCTION = "https://rezervace.prf.osu.cz/";
        public const string EMAIL_SMTP_SERVER = "smtp.gmail.com";
        public const int SMTP_SERVER_PORT = 465;

        public const string FOLDER_PATH = "/home/student/project/myproject/backend/user-files/";
        public const string REPORTS_FOLDER_PATH = "/home/student/project/myproject/backend/report-files/";
    }
}