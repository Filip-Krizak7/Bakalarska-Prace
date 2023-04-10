using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace TeacherPractise.Model
{
    public class Context : DbContext
    {
        //sql server - localhost/mssqlserver_bp
        public Context(): base("SchoolDB")
        {
            Database.SetInitializer<Context>(new DropCreateDatabaseIfModelChanges<Context>());
            Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<School> Schools { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Practice> practices { get; set; }
        public DbSet<UserPractice> UserPractices { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }
    }
}