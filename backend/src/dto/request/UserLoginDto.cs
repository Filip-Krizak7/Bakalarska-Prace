namespace TeacherPractise.Dto.Request
{
    public class UserLoginDto {
        public String username { get; set; }
        public String password { get; set; }

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