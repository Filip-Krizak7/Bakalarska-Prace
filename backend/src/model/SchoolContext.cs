using System.Data.Entity;

namespace teacherpractise.model
{
    public class Context : DbContext
    {
        public Context(): base()
        {

        }

       /* protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder.Entity<>().Property(t => t.Name).IsRequired());
        }*/
    }
}