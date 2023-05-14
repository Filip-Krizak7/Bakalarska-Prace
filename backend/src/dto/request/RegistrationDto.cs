namespace TeacherPractise.Dto.Request
{
    public class RegistrationDto {
        public string email { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public long? school { get; set; }
        public string? phoneNumber { get; set; }
        public string password { get; set; }
        public string role { get; set; }

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