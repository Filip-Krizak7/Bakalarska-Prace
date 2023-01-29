using System.Data.Entity;

namespace TeacherPractise.Model
{
    public class Context : DbContext
    {
        //sql server - localhost/mssqlserver_bp
        public Context(): base("SchoolDB")
        {
            Database.SetInitializer<Context>(new CreateDatabaseIfNotExists<Context>());
        }

        public DbSet<School> Schools { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Practice> practices { get; set; }
        public DbSet<UserPractice> UserPractices { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //setting composite primary key
            modelBuilder.Entity<UserPractice>()
                .HasKey(u => new { u.PracticeId, u.UserId });

            /*modelBuilder.Entity<User>()
                .HasRequired(u => u.Practice)
                .WithMany(s => s.Users)
                .HasForeignKey();*/
        }
    }
}