using System.Text.RegularExpressions;

namespace TeacherPractise.Service.Email
{
    public class EmailValidator
    {
        private static readonly Regex VALID_EMAIL_ADDRESS_REGEX_STUDENT =
            new Regex("^[A-Za-z0-9._%+-]+@student.osu.cz$", RegexOptions.IgnoreCase);
        private static readonly Regex VALID_EMAIL_ADDRESS_REGEX_TEACHER =
            new Regex("^(.+)@(.+)$", RegexOptions.IgnoreCase);

        public bool checkEmail(string s, string role)
        {
            Console.WriteLine(s + " " + role);
            Match matcher;
            if (role.Equals("student", StringComparison.OrdinalIgnoreCase))
            {
                matcher = VALID_EMAIL_ADDRESS_REGEX_STUDENT.Match(s);
            }
            else
            {
                matcher = VALID_EMAIL_ADDRESS_REGEX_TEACHER.Match(s);
            }
            return matcher.Success;
        }
    }
}