namespace TeacherPractise.Dto.Response
{
    public class UserDto {
        public long id { get; set; }
        public string username { get; set; }
        public string firstName { get; set; }
        public string secondName { get; set; }
        public SchoolDto? school { get; set; }
    }
}