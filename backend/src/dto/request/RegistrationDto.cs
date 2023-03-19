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

        public RegistrationDto(string username, string firstN, string lastN, long sch, string phoneNum, string passwd, string rl)
        {
            this.email = username;
            this.firstName = firstN;
            this.lastName = lastN;
            this.school = sch;
            this.phoneNumber = phoneNum;
            this.password = passwd;
            this.role = rl;
        }

        public RegistrationDto()
        {
            
        }
    }
}