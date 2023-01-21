﻿// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");
namespace TeacherPractise.Model
{
    class Demo
    {
        static void Main(string[] args)
        {
            using (var ctx = new Context())
            {
                var school = new School() { Id = 1, Name = "School1" };
        
                ctx.Schools.Add(school);
                ctx.SaveChanges();                
            }
        }
    }
}
