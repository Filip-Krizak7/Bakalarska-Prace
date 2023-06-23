namespace TeacherPractise.Dto.Request
{
    public class UserLoginDto {
        public string username { get; set; }
        public string password { get; set; }

        public UserLoginDto(string usern, string passwd)
        {
            this.username = usern;
            this.password = passwd;
        }

        public UserLoginDto()
        {
            
        }
    }
}