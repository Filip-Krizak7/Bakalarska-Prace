using System;
using Microsoft.AspNetCore.Mvc;
using TeacherPractise.Dto.Response;
using TeacherPractise.Dto.Request;
using TeacherPractise.Config;
using TeacherPractise.Mapper;
using TeacherPractise.Domain;
using TeacherPractise.Model;

namespace TeacherPractise.Service
{
    public class CoordinatorService
    {
        private readonly AppUserService appUserService;
        private readonly TeacherService teacherService;
        private readonly CustomMapper mapper;

        public CoordinatorService([FromServices] AppUserService appUserService, [FromServices] TeacherService teacherService, [FromServices] CustomMapper mapper)
        {
            this.appUserService = appUserService;
            this.teacherService = teacherService;
            this.mapper = mapper;
        }

        public string addSubject(SubjectDto subjectDto)
        {
            string subjectName = subjectDto.name;

            using (var ctx = new Context())
	        {
                if (ctx.Subjects.ToList().Any(q => q.Name == subjectName.ToLower()))
                {
                    throw AppUserService.CreateException($"Předmět {subjectName} již existuje.");
                }
                else
                {
                    Subject subject = new Subject((int)subjectDto.id, subjectDto.name);
                    //Subject subject = mapper.Map<Subject>(subjectDto);
                    ctx.Subjects.Add(subject);
                    ctx.SaveChanges();
                }
            }

            return "Předmět byl zapsán.";
        }

        public string addSchool(SchoolDto schoolDto)
        {
            string schoolName = schoolDto.name;

            using (var ctx = new Context())
	        {
                if (ctx.Schools.ToList().Any(q => q.Name == schoolName.ToLower()))
                {
                    throw AppUserService.CreateException($"Škola {schoolName} již existuje.");
                }
                else
                {
                    School school = new School((int)schoolDto.id, schoolDto.name);
                    //School school = mapper.Map<School>(schoolDto);
                    ctx.Schools.Add(school);
                    ctx.SaveChanges();
                }
            }

            return "Škola byla přidána.";
        }

        public List<StudentPracticeDto> getPracticesList(DateTime date, long subjectId, int pageNumber, int pageSize)
        {
            using (var ctx = new Context())
	        {
                var practices = ctx.practices.ToList().Where(q => q.Date == date || q.SubjectId == subjectId);
                practices.OrderBy(p => p.Date);

                List<PracticeDomain> practicesDomain = mapper.practicesToPracticesDomain(practices.ToList());
                List<PracticeDomain> toDelete = new List<PracticeDomain>();

                foreach (PracticeDomain p in practicesDomain)
                {
                    p.SetNumberOfReservedStudents();
                    p.SetStudentNames(teacherService.getStudentNamesByPractice(p, pageNumber, pageSize));
                    p.SetFileNames(appUserService.getTeacherFiles(p.teacher.username));
                    p.SetStudentEmails(teacherService.getStudentEmailsByPractice(p, pageNumber, pageSize));
                    toDelete.Add(p);
                }

                foreach (PracticeDomain practiceDomain in toDelete)
                {
                    if (practiceDomain.RemovePassedPractices())
                    {
                        practicesDomain.Remove(practiceDomain);
                    }
                }

                return mapper.practicesDomainToStudentPracticesDto(practicesDomain);
            }
        }

        public List<StudentPracticeDto> getPracticesListPast(DateTime date, long subjectId, int pageNumber, int pageSize)
        {
            using (var ctx = new Context())
	        {
                var practices = ctx.practices.ToList().Where(q => q.Date == date || q.SubjectId == subjectId);
                practices.OrderBy(p => p.Date);

                List<PracticeDomain> practicesDomain = mapper.practicesToPracticesDomain(practices.ToList());
                List<PracticeDomain> toDelete = new List<PracticeDomain>();

                foreach (PracticeDomain p in practicesDomain)
                {
                    p.SetNumberOfReservedStudents();
                    p.SetStudentNames(teacherService.getStudentNamesByPractice(p, pageNumber, pageSize));
                    p.SetFileNames(appUserService.getTeacherFiles(p.teacher.username));
                    p.SetStudentEmails(teacherService.getStudentEmailsByPractice(p, pageNumber, pageSize));
                    string report = appUserService.getPracticeReport(p.id);
                    p.SetReport(report);
                    toDelete.Add(p);
                }

                foreach (PracticeDomain practiceDomain in toDelete)
                {
                    if (practiceDomain.RemovePassedPractices())
                    {
                        practicesDomain.Remove(practiceDomain);
                    }
                }

                return mapper.practicesDomainToStudentPracticesDto(practicesDomain);
            }
        }
    }
}    