using System.Data.Entity;

namespace teacherpractise.model
{
    public class Context : DbContext
    {
        public DbSet<School> Schools { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Practice> practices { get; set; }
        public DbSet<UserPractice> UserPractices { get; set; }

        /*protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<School>().HasMany(s => s.Users).WithOne(u => u.School).IsRequired();
        }*/
    }
}