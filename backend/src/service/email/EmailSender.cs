namespace TeacherPractise.Service.Email
{
    public interface EmailSender
    {
        Task send(string to, string email);
        Task sendForgotPasswordMail(string to, string email);
    }
}