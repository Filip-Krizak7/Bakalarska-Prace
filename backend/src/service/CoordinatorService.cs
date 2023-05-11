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

        public List<UserDto> getWaitingList() {
            using (var ctx = new Context())
	        {
                var users = ctx.Users.Where(q => q.Locked == true).ToList();
                return mapper.usersToUserDtos(users);
            }
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
                    Subject subject = mapper.subjectDtoToSubject(subjectDto);
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
                    School school = mapper.schoolDtoToSchool(schoolDto);
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
                var practices = ctx.Practices.ToList().Where(q => q.Date == date || q.SubjectId == subjectId);
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
                var practices = ctx.Practices.ToList().Where(q => q.Date == date || q.SubjectId == subjectId);
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

        public string removeSchool(string schoolName)
        {
            using (var ctx = new Context())
	        {
                School school = ctx.Schools.Where(q => q.Name == schoolName.ToLower()).FirstOrDefault();
                if (school != null)
                {
                    ctx.Users.Where(u => u.SchoolId == school.Id).ToList().ForEach(u => {
                        u.School = null;
                        u.SchoolId = null;
                    });
                    ctx.Schools.Remove(school);
                    int rowsAffected = ctx.SaveChanges();
                    if (rowsAffected >= 1) return "School deleted";
                    else return "Something went wrong";
                }
                else return "School was not deleted";
            }
        }

        public string removeSubject(string subjectName)
        {
            using (var ctx = new Context())
	        {
                Subject subject = ctx.Subjects.Where(q => q.Name == subjectName.ToLower()).FirstOrDefault();
                if (subject != null)
                {
                    ctx.Practices.Where(p => p.SubjectId == subject.Id).ToList().ForEach(p => {
                        p.Subject = null;
                        p.SubjectId = null;
                    });
                    ctx.Subjects.Remove(subject);
                    int rowsAffected = ctx.SaveChanges();
                    if (rowsAffected >= 1) return "Subject deleted";
                    else return "Something went wrong";
                }
                else return "Subject was not deleted";
            }
        }

        public string editSubject(string originalSubject, string newSubject)
        {
            using (var ctx = new Context())
	        {
                Subject subject = ctx.Subjects.Where(q => q.Name == originalSubject.ToLower()).FirstOrDefault();
                if (subject != null)
                {
                    subject.Name = newSubject;
                    int rowsAffected = ctx.SaveChanges();
                    if (rowsAffected >= 1) return "Subject changed";
                    else return "Something went wrong";
                }
                else return "Subject was not changed";
            }
        }

        public string editSchool(string originalSchool, string newSchool)
        {
            using (var ctx = new Context())
	        {
                School school = ctx.Schools.Where(q => q.Name == originalSchool.ToLower()).FirstOrDefault();
                if (school != null)
                {
                    school.Name = newSchool;
                    int rowsAffected = ctx.SaveChanges();
                    if (rowsAffected >= 1) return "School changed";
                    else return "Something went wrong";
                }
                else return "School was not changed";
            }
        }

        public string assignSchool(AssignSchoolDto request)
        {
            using (var ctx = new Context())
	        {
                School school = ctx.Schools.Where(q => q.Name == request.school.ToLower()).FirstOrDefault();
                if (school != null)
                {
                    User user = ctx.Users.Where(q => q.Username == request.username.ToLower()).FirstOrDefault();
                    if (user != null)
                    {
                        user.SchoolId = school.Id;
                        user.School = school;
                        ctx.SaveChanges();
                        return "School assigned.";
                    }
                }
                return "School was not assigned.";
            }
        }

        public List<UserDto> getTeachersWithoutSchool()
        {
            using (var ctx = new Context())
	        {
                var users = ctx.Users.Where(q => q.SchoolId == null && q.Role == Roles.ROLE_TEACHER).ToList();
                return mapper.usersToUserDtos(users);
            }
        }

        public string changePhoneNumber(string username, string phoneNumber)
        {
            using (var ctx = new Context())
	        {
                User user = ctx.Users.Where(q => q.Username == username.ToLower()).FirstOrDefault();
                if (user != null)
                {
                    user.PhoneNumber = phoneNumber;
                    ctx.SaveChanges();
                    return "Phone number changed.";
                }
            }
            return "Phone number was not changed.";
        }
    }
}    