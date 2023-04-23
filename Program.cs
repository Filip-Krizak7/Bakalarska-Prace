using TeacherPractise.Service;
using TeacherPractise.Model;
using TeacherPractise.Dto.Request;
using TeacherPractise.Dto.Response;
using System.Data.Entity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System;

using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

            //var  MyAllowSpecificOrigins = "_myAllowSpecificOrigins";  
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
            SecurityService securityService = new(builder.Configuration);
            builder.Services.AddSingleton<SecurityService>(securityService);
            builder.Services.AddSingleton<AppUserService>();
            builder.Services.AddSingleton<RegistrationService>();
            builder.Services.AddSingleton<SchoolService>();
            builder.Services.AddControllers();


            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1",
                new() { Title="Teacher practice API", Version="v1"});
            });
            
            builder.Services.AddCors();
            builder.Services.AddMvc(options => options.EnableEndpointRouting = false);

            builder.Services.AddAuthorization();
            builder.Services.AddAuthentication(opt =>
                {
                    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(opt =>
                {
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
                //private readonly RegistrationService regService;
                /*RegistrationService regService;
                SchoolService schService;
                AppUserService appService;*/

                /*var school1 = new School() { Id = 2, Name = "ZŠ Dobrá" };
                ctx.Schools.Add(school1);
                ctx.SaveChanges();*/

                //RegistrationDto request = new RegistrationDto("r73123@student.osu.cz", "Testing", "Tester", 2, "123456789", "secret_passwd123", "teacher");
                //UserLoginDto req = new UserLoginDto("r93452@student.osu.cz", "secret_passwd123");
                //appService.login(req, HttpContext);
                //regService.register(request);
                /*User appUser = new User("r31252@student.osu.cz", "secret_passwd123", "Testing", "Tester", 2, "123456789", Roles.ROLE_TEACHER, false, true);

                List<string> roleList = new List<string>();
                roleList.Add(appUser.Role.ToString());

                Dictionary<string, object> roleClaims = roleList.ToDictionary(
                    q => ClaimTypes.Role,
                    q => (object)q.ToUpper());

                foreach (var item in roleList)
                {
                    Console.WriteLine(item);
                }

                foreach (var item in roleClaims)
                {
                    Console.WriteLine("Key: {0}, Value: {1}", item.Key, item.Value);
                }*/

                /*List<User> us = ctx.Users.ToList();
                foreach(User usobj in us)
                {
                    System.Console.WriteLine("{0} | {1} | {2}", usobj.Id, usobj.Username, usobj.SchoolId);
                }      

                System.Console.WriteLine("-------------------------------------");

                List<School> sch = ctx.Schools.ToList();
                foreach(School schobj in sch)
                {
                    System.Console.WriteLine("{0} {1}", schobj.Id, schobj.Name);
                }  */
            }
            
            var app = builder.Build();

            app.UseCors(
                options => options.WithOrigins("http://localhost:5000").AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin()
            );
            app.UseRouting(); //.AllowCredentials()

            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseDeveloperExceptionPage();

            //app.UseCors(MyAllowSpecificOrigins); 
            app.UseMvc();

            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        
