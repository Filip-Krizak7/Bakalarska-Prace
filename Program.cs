﻿using JWTCoreDemo.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

/*namespace TeacherPractise.Model
{
    class Demo
    {
        static void Main(string[] args)
        {*/
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
            SecurityService securityService = new(builder.Configuration);
            builder.Services.AddSingleton<SecurityService>(securityService);
            builder.Services.AddSingleton<AppUserService>();

            builder.Services.AddControllers();
            builder.Services.AddAuthorization();
            builder.Services.AddAuthentication(opt =>
                {
                    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(opt =>
                {
                // key from config
                //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]));
                // or random key each app-start
                    var key = new SymmetricSecurityKey(securityService.Key);
                    opt.TokenValidationParameters = new TokenValidationParameters()
                    {
                        IssuerSigningKey = key,
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });

            var app = builder.Build();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
            /*using (var ctx = new Context())
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
            }*/
        /*}
    }
}*/
