using AutoMapper;
using TeacherPractise.Dto.Request;
using TeacherPractise.Dto.Response;
using TeacherPractise.Domain;
using TeacherPractise.Model;

namespace TeacherPractise.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            /*

            CreateMap<List<Subject>, List<SubjectDto>>();

            CreateMap<List<School>, List<SchoolDto>>();

            CreateMap<List<Practice>, List<PracticeDomain>>();
            CreateMap<List<User>, List<UserDto>>();

            CreateMap<List<PracticeDomain>, List<StudentPracticeDto>>();*/

            //CreateMap<List<Review>, List<ReviewDto>>();
        }
    }
}