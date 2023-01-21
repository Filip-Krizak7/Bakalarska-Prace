using System.Data.Entity;

namespace TeacherPractise.Model
{
    public class DbInitializer : CreateDatabaseIfNotExists<Context>
    {
        protected override void Seed(Context context)
        {
            base.Seed(context);
        }
    }
}