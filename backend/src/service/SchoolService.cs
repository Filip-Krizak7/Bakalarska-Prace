using TeacherPractise.Model;
using Microsoft.AspNetCore.Mvc;
using TeacherPractise.Dto.Response;

namespace TeacherPractise.Service
{
    public class SchoolService
    {
        public SchoolService()
        {
        }

        public School getSchoolById(long id)
        {
            using (var ctx = new Context())
            {
                if (!(ctx.Schools.ToList().Exists(q => q.Id == (int)id)))
                throw AppUserService.CreateException($"School with ID: {id} does not exists.", null);
                
                School sch = ctx.Schools.ToList().Find(q => q.Id == (int)id);

                return sch; 
            }
        }

        public Subject getSubjectById(long id)
        {
            using (var ctx = new Context())
            {
                if (!(ctx.Subjects.ToList().Exists(q => q.Id == (int)id)))
                throw AppUserService.CreateException($"Subject with ID: {id} does not exists.", null);
                
                Subject sbj = ctx.Subjects.ToList().Find(q => q.Id == (int)id);

                return sbj; 
            }
        }

        public User getTeacherById(long id)
        {
            using (var ctx = new Context())
            {
                if (!(ctx.Users.ToList().Exists(q => q.Id == (int)id)))
                throw AppUserService.CreateException($"Teacher with ID: {id} does not exists.", null);
                
                User user = ctx.Users.ToList().Find(q => q.Id == (int)id);
                if (user.Role != Roles.ROLE_TEACHER)
                throw AppUserService.CreateException($"User with ID: {id} is not a teacher.", null);

                return user; 
            }
        }

        public void checkSchoolById(long id)
        {
            using (var ctx = new Context())
            {
                if (!(ctx.Schools.ToList().Exists(q => q.Id == (int)id)))
                throw AppUserService.CreateException($"School with ID: {id} does not exists.", null);
            }
        }

        public List<School> getSchools()
        {
            using (var ctx = new Context())
            {
                return ctx.Schools.ToList();     
            }
        }

        public User getStudentById(long id)
        {
            using (var ctx = new Context())
	        {
		        User appUser = ctx.Users.ToList().FirstOrDefault(q => q.Id == (int)id)
                	?? throw AppUserService.CreateException($"Uživatel s ID {id} neexistuje.");

            	return appUser;
            }
        }
    }
}