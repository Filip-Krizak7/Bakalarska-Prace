namespace TeacherPractise.Dto.Request
{
    public class ForgotPasswordDto {
        public string token { get; set; }
        public string newPassword { get; set; }
    }
}