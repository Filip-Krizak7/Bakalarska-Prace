using TeacherPractise.Model;
using Microsoft.AspNetCore.Mvc;

namespace TeacherPractise.Service
{
    public class SchoolService
    {
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

        public void checkSchoolById(long id)
        {
            using (var ctx = new Context())
            {
                if (!(ctx.Schools.ToList().Exists(q => q.Id == (int)id)))
                throw AppUserService.CreateException($"School with ID: {id} does not exists.", null);
            }
        }
    }
}