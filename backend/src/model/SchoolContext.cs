using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using TeacherPractise.Service.Token.RegistrationToken;

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
        public DbSet<Practice> Practices { get; set; }
        public DbSet<ConfirmationToken> ConfirmationTokens { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {         
            modelBuilder.Entity<User>()
                .HasMany<Practice>(s => s.AttendedPractices)
                .WithMany(c => c.UsersOnPractice)
                .Map(cs =>
                        {
                            cs.MapLeftKey("UserRefId");
                            cs.MapRightKey("PracticeRefId");
                            cs.ToTable("UserPractice");
                        });

            /*modelBuilder.Entity<User>()
                .HasOne(u => u.ConfirmationToken)
                .WithOne(ct => ct.User)
                .HasForeignKey<ConfirmationToken>(ct => ct.UserId);*/
                
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }
    }
}