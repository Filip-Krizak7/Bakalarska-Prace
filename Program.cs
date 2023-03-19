﻿using TeacherPractise.Service;
using TeacherPractise.Model;
using TeacherPractise.Dto.Request;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System;

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

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1",
                new() { Title="Teacher practice API", Version="v1"});
            });

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
            
            using (var ctx = new Context())
            {
                RegistrationService regService = new RegistrationService();

                RegistrationDto request = new RegistrationDto("r20382@student.osu.cz", "Filip", "Křižák", 1, "123456789", "secret_passwd123", "teacher"); 
                //regService.register(request);

                List<User> us = ctx.Users.ToList();         //error ------------ vyhazuje chybu u enum role
                foreach(User usobj in us)
                {
                    System.Console.WriteLine("{0} {1}", usobj.Username, usobj.FirstName);
                }       

                List<School> sch = ctx.Schools.ToList();
                foreach(School schobj in sch)
                {
                    System.Console.WriteLine("{0} {1}", schobj.Id, schobj.Name);
                }  
            }
            
            var app = builder.Build();

            app.UseRouting();

            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseDeveloperExceptionPage();
            
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
