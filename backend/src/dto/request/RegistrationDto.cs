namespace TeacherPractise.Dto.Request
{
    public class RegistrationDto {
        public String email { get; set; }
        public String firstName { get; set; }
        public String lastName { get; set; }
        public long school { get; set; }
        public String phoneNumber { get; set; }
        public String password { get; set; }
        public String role { get; set; }
    }
}