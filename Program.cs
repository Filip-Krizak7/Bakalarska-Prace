// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");
namespace TeacherPractise.Model
{
    class Demo
    {
        static void Main(string[] args)
        {
            using (var ctx = new Context())
            {
                var school1 = new School() { Id = 1, Name = "School1" }; //Id is autoincrementing --> no need to enter here
                var school2 = new School() { Id = 2, Name = "School2" };
        
                ctx.Schools.Add(school1);
                ctx.Schools.Add(school2);
                ctx.SaveChanges();  
            }

            using (var ctx = new Context())
            {
                List<School> sch = ctx.Schools.ToList();
                foreach(School schobj in sch)
                {
                    System.Console.WriteLine("{0} {1}", schobj.Id, schobj.Name);
                }            
            }
        }
    }
}
