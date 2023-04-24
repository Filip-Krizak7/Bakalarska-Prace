namespace TeacherPractise.Dto.Response
{
    public class UserDto {
        public long id { get; set; }
        public String username { get; set; }
        public String firstName { get; set; }
        public String secondName { get; set; }
        public SchoolDto school { get; set; }
    }
}