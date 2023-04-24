namespace TeacherPractise.Dto.Response
{
    public class SubjectDto {
        public long id { get; set; }
        public String name { get; set; }

        public SubjectDto(long id, string name)
        {
            this.id = id;
            this.name = name;
        }

        public SubjectDto()
        {
            
        }
    }
}