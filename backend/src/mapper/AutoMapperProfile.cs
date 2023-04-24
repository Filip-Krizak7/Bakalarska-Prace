using AutoMapper;
using TeacherPractise.Dto.Request;
using TeacherPractise.Dto.Response;
using TeacherPractise.Model;

namespace TeacherPractise.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<NewPracticeDto, Practice>();
            CreateMap<SubjectDto, Subject>();
            CreateMap<Subject, SubjectDto>();
            CreateMap<List<Subject>, List<SubjectDto>>();
            CreateMap<SchoolDto, School>();
            CreateMap<School, SchoolDto>();
            CreateMap<List<School>, List<SchoolDto>>();
            //CreateMap<Practice, PracticeDomain>();
            //CreateMap<List<Practice>, List<PracticeDomain>>();
            CreateMap<List<User>, List<UserDto>>();
            CreateMap<UserDto, User>();
            CreateMap<User, UserDto>();
            //CreateMap<PracticeDomain, StudentPracticeDto>();
            //CreateMap<List<PracticeDomain>, List<StudentPracticeDto>>();
            //CreateMap<ReviewDto, Review>();
            //CreateMap<Review, ReviewDto>();
            //CreateMap<List<Review>, List<ReviewDto>>();
        }
    }
}